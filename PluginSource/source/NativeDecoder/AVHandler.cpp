#include "AVHandler.h"
#include "DecoderFFmpeg.h"
#include "Logger.h"
#include "DecoderSsp.h"

const std::string AVHandler::PROTOCOL_SSP = "ssp://";
const std::string AVHandler::PROTOCOL_HTTP = "http://";
const std::string AVHandler::PROTOCOL_FILE = "file://";

AVHandler::AVHandler(const char* path) {
	mDecoderState = UNINITIALIZED;
	mSeekTime = 0.0;
	if (strncmp(path, PROTOCOL_SSP.c_str(),PROTOCOL_SSP.size())== 0)
	{
		mIDecoder = new DecoderSsp();
	}
	else
	{
		mIDecoder = new DecoderFFmpeg();
	}
}

void AVHandler::init(const char* filePath) {
	this->filePath.assign(filePath);
	if (mIDecoder == NULL || !mIDecoder->init(filePath)) {
		mDecoderState = INIT_FAIL;
	} else {
		mDecoderState = INITIALIZED;
	}
}

AVHandler::DecoderState AVHandler::getDecoderState() {
	return mDecoderState;
}

void AVHandler::stopDecoding() {
	mDecoderState = STOP;
	if (mDecodeThread.joinable()) {
		mDecodeThread.join();
	}

	if (mIDecoder != NULL) {
		delete mIDecoder;
		mIDecoder = NULL;
	}
	mDecoderState = UNINITIALIZED;
}

double AVHandler::getVideoFrame(uint8_t** outputY, uint8_t** outputU, uint8_t** outputV) {
	if (mIDecoder == NULL || !mIDecoder->getVideoInfo().isEnabled || mDecoderState == SEEK) {
		LOG("Video is not available. \n");
		*outputY = *outputU = *outputV = NULL;
		return -1;
	}

	return mIDecoder->getVideoFrame(outputY, outputU, outputV);
}

double AVHandler::getVideoFrameNV12(uint8_t** output1, int& out1Size, uint8_t** output2, int& out2Size) {
	if (mIDecoder == NULL || !mIDecoder->getVideoInfo().isEnabled || mDecoderState == SEEK) {
		LOG("Video is not available. \n");
		*output1 = *output2= NULL;
		return -1;
	}

	return mIDecoder->getVideoFrameNV12(output1, out1Size, output2, out2Size);
}

double AVHandler::getAudioFrame(uint8_t** outputFrame, int& frameSize) {
	if (mIDecoder == NULL || !mIDecoder->getAudioInfo().isEnabled || mDecoderState == SEEK) {
		LOG("Audio is not available. \n");
		*outputFrame = NULL;
		return -1;
	}
	
	return mIDecoder->getAudioFrame(outputFrame, frameSize);
}

void AVHandler::freeVideoFrame() {
	if (mIDecoder == NULL || !mIDecoder->getVideoInfo().isEnabled || mDecoderState == SEEK) {
		LOG("Video is not available. \n");
		return;
	}

	mIDecoder->freeVideoFrame();
}

void AVHandler::freeAudioFrame() {
	if (mIDecoder == NULL || !mIDecoder->getAudioInfo().isEnabled || mDecoderState == SEEK) {
		LOG("Audio is not available. \n");
		return;
	}

	mIDecoder->freeAudioFrame();
}

void AVHandler::startDecoding() {
	if (mIDecoder == NULL || mDecoderState != INITIALIZED) {
		LOG("Not initialized, decode thread would not start. \n");
		return;
	}
	mDecodeThread = std::thread([&]() {

		//if (!(mIDecoder->getVideoInfo().isEnabled || mIDecoder->getAudioInfo().isEnabled)) {
		//	LOG("No stream enabled. \n");
		//	LOG("Decode thread would not start. \n");
		//	return;
		//}

		mDecoderState = DECODING;
		while (mDecoderState != STOP) {
			switch (mDecoderState) {
			case DECODING:
				if (!mIDecoder->decode()) {
					mDecoderState = DECODE_EOF;
				}
				break;
			case SEEK:
				mIDecoder->seek(mSeekTime);
				mDecoderState = DECODING;
				break;
			case DECODE_EOF:
				break;
			}
		}
	});
}

AVHandler::~AVHandler() {
	stopDecoding();
}

void AVHandler::setSeekTime(float sec) {
	if (mDecoderState < INITIALIZED || mDecoderState == SEEK) {
		LOG("Seek unavaiable.");
		return;
	} 

	mSeekTime = sec;
	mDecoderState = SEEK;
}

bool AVHandler::isContentReady()
{
	return mIDecoder->isContentReady();
}

IDecoder::VideoInfo AVHandler::getVideoInfo() {
	return mIDecoder->getVideoInfo();
}

IDecoder::AudioInfo AVHandler::getAudioInfo() {
	return mIDecoder->getAudioInfo();
}

bool AVHandler::isVideoBufferEmpty() {
	IDecoder::VideoInfo* videoInfo = &(mIDecoder->getVideoInfo());
	IDecoder::BufferState EMPTY = IDecoder::BufferState::EMPTY;
	return videoInfo->isEnabled && videoInfo->bufferState == EMPTY;
}

bool AVHandler::isVideoBufferFull() {
	IDecoder::VideoInfo* videoInfo = &(mIDecoder->getVideoInfo());
	IDecoder::BufferState FULL = IDecoder::BufferState::FULL;
	return videoInfo->isEnabled && videoInfo->bufferState == FULL;
}

int AVHandler::getMetaData(char**& key, char**& value) {
	if (mIDecoder == NULL ||mDecoderState <= UNINITIALIZED) {
		return 0;
	}
	
	return mIDecoder->getMetaData(key, value);
}

void AVHandler::setVideoEnable(bool isEnable) {
	if (mIDecoder == NULL) {
		return;
	}

	mIDecoder->setVideoEnable(isEnable);
}

void AVHandler::setAudioEnable(bool isEnable) {
	if (mIDecoder == NULL) {
		return;
	}

	mIDecoder->setAudioEnable(isEnable);
}

void AVHandler::setAudioAllChDataEnable(bool isEnable) {
	if (mIDecoder == NULL) {
		return;
	}

	mIDecoder->setAudioAllChDataEnable(isEnable);
}