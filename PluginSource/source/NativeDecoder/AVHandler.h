//========= Copyright 2015-2018, HTC Corporation. All rights reserved. ===========

#pragma once
#include "IDecoder.h"
#include <thread>
#include <mutex>
 
class AVHandler {
public:
	enum DecoderState {
		INIT_FAIL = -1, UNINITIALIZED, INITIALIZED, DECODING, SEEK, BUFFERING, DECODE_EOF, STOP
	};

	AVHandler(const char* path);
	~AVHandler();
	
	DecoderState getDecoderState();

	void init(const char* filePath);
	void startDecoding();
	void stopDecoding();
	void setSeekTime(float sec);
	bool isContentReady();
	double getVideoFrame(uint8_t** outputY, uint8_t** outputU, uint8_t** outputV);
	double getVideoFrameNV12(uint8_t** output1, int& out1Size, uint8_t** output2, int& out2Size);
	double getAudioFrame(uint8_t** outputFrame, int& frameSize);
	void freeVideoFrame();
	void freeAudioFrame();
	void setVideoEnable(bool isEnable);
	void setAudioEnable(bool isEnable);
	void setAudioAllChDataEnable(bool isEnable);

	IDecoder::VideoInfo getVideoInfo();
	IDecoder::AudioInfo getAudioInfo();
	IDecoder::VideoFrame getVideoFrame();
	IDecoder::AudioFrame getAudioFrame();

	bool isVideoBufferEmpty();
	bool isVideoBufferFull();

	int getMetaData(char**& key, char**& value);

private:
	DecoderState mDecoderState;
	IDecoder* mIDecoder;
	double mSeekTime;
	std::string filePath;
	std::thread mDecodeThread;

private:

	 static const std::string PROTOCOL_SSP;
	 static const std::string PROTOCOL_HTTP;
	 static const std::string PROTOCOL_FILE;
};

