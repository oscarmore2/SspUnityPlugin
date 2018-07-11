
#include <stdio.h>
#include <stddef.h>
#include <thread>
#include <list>
#include "NativeDecoder.h"

#include "AVHandler.h"
#include "Logger.h"
#include "RenderAPI/RenderAPI.h"
#include "TextureObject/ITextureObject.h"

typedef struct _VideoContext
{
	int id = -1;
	char *path = NULL;
	std::thread initThread;
	AVHandler *avhandler = NULL;
	ITextureObject *textureObj = NULL;
	float progressTime = 0.0f;
	float lastUpdateTime = -1.0f;
	// This flag is used to indicate the period that seek over until first data is got.
	// Usually used for AV sync problem, in pure audio case, it should be discard.
	bool isContentReady = false;

} VideoContext;

void *g_render_device = NULL;
std::list<VideoContext> videoContexts;
typedef std::list<VideoContext>::iterator VideoContextIter;
// --------------------------------------------------------------------------
static IUnityInterfaces *s_unity_interfaces = nullptr;
static IUnityGraphics *s_graphics = nullptr;
static UnityGfxRenderer s_device_type = kUnityGfxRendererNull;
static RenderAPI *s_render_api = nullptr;

static void UNITY_INTERFACE_API OnGraphicsDeviceEvent(UnityGfxDeviceEventType eventType)
{
	if (nullptr != s_render_api)
	{
		s_render_api->ProcessDeviceEvent(eventType, s_unity_interfaces);
	}

	switch (eventType)
	{
	case kUnityGfxDeviceEventInitialize:
	{
		break;
	}

	case kUnityGfxDeviceEventShutdown:
	{
		break;
	}

	case kUnityGfxDeviceEventBeforeReset:
	{
		break;
	}

	case kUnityGfxDeviceEventAfterReset:
	{
		break;
	}

	default:
		break;
	};
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginLoad(
	IUnityInterfaces *unityInterfaces)
{
	s_unity_interfaces = unityInterfaces;
	s_graphics = s_unity_interfaces->Get<IUnityGraphics>();
	s_device_type = s_graphics->GetRenderer();
	s_graphics->RegisterDeviceEventCallback(OnGraphicsDeviceEvent);
	s_render_api = CreateRenderAPI(s_device_type);
	// Run OnGraphicsDeviceEvent(initialize) manually on plugin load
	OnGraphicsDeviceEvent(kUnityGfxDeviceEventInitialize);
}

extern "C" void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload()
{
	s_device_type = kUnityGfxRendererNull;
	s_graphics->UnregisterDeviceEventCallback(OnGraphicsDeviceEvent);
}
// --------------------------------------------------------------------------

bool getVideoContextIter(int id, VideoContextIter *iter)
{
	for (VideoContextIter it = videoContexts.begin(); it != videoContexts.end(); it++)
	{
		if (it->id == id)
		{
			*iter = it;
			return true;
		}
	}

	//LOG("Decoder does not exist. \n");
	return false;
}

void DoRendering(int id);

static void UNITY_INTERFACE_API OnRenderEvent(int eventID)
{
	// Unknown graphics device type? Do nothing.
	if (s_device_type == -1)
	{
		return;
	}

	// Actual functions defined below
	DoRendering(eventID);
}

extern "C" UnityRenderingEvent UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API GetRenderEventFunc()
{
	return OnRenderEvent;
}

void DoRendering(int id)
{
	if (s_device_type == kUnityGfxRendererD3D11 && g_render_device != NULL)
	{
		VideoContextIter iter;

		if (getVideoContextIter(id, &iter))
		{
			VideoContext *localVideoContext = &(*iter);
			AVHandler *localAVHandler = localVideoContext->avhandler;

			if (localAVHandler != NULL && localAVHandler->getDecoderState() >= AVHandler::DecoderState::INITIALIZED && localAVHandler->getVideoInfo().isEnabled)
			{
				clock_t start = clock();

				if (localVideoContext->textureObj == NULL)
				{
					unsigned int width = localAVHandler->getVideoInfo().width;
					unsigned int height = localAVHandler->getVideoInfo().height;
					localVideoContext->textureObj = createTextureObject(s_device_type, g_render_device, width, height);
				}

				double videoDecCurTime = localAVHandler->getVideoInfo().lastTime;

				if (videoDecCurTime <= localVideoContext->progressTime)
				{
					uint8_t *ptrY = NULL;
					uint8_t *ptrU = NULL;
					uint8_t *ptrV = NULL;
					double curFrameTime = localAVHandler->getVideoFrame(&ptrY, &ptrU, &ptrV);

					if (ptrY != NULL && curFrameTime != -1 && localVideoContext->lastUpdateTime != curFrameTime)
					{
						localVideoContext->textureObj->upload(ptrY, ptrU, ptrV);
						localVideoContext->lastUpdateTime = (float)curFrameTime;
						localVideoContext->isContentReady = true;
					}

					localAVHandler->freeVideoFrame();
				}

				LOG("Render video texture = %f\n", (float)(clock() - start) / CLOCKS_PER_SEC);
			}
		}
	}
}

int nativeCreateDecoderAsync(const char *filePath, int &id)
{
	//LOG("Query available decoder id. \n");

	int newID = 0;
	VideoContextIter iter;

	while (getVideoContextIter(newID, &iter))
	{
		newID++;
	}

	VideoContext context;
	context.avhandler = new AVHandler(filePath);
	context.id = newID;
	id = context.id;
	context.path = (char *)malloc(sizeof(char) * (strlen(filePath) + 1));
	strcpy_s(context.path, strlen(filePath) + 1, filePath);
	context.isContentReady = false;
	videoContexts.push_back(std::move(context));

	getVideoContextIter(id, &iter);
	iter->initThread = std::thread([iter]() {
		iter->avhandler->init(iter->path);
	});

	return 0;
}

//  Synchronized init. Used for thumbnail currently.
int nativeCreateDecoder(const char *filePath, int &id)
{
	//LOG("Query available decoder id. \n");

	int newID = 0;
	VideoContextIter iter;

	while (getVideoContextIter(newID, &iter))
	{
		newID++;
	}

	VideoContext context;
	context.avhandler = new AVHandler(filePath);
	context.id = newID;
	id = context.id;
	context.path = NULL;
	context.isContentReady = false;
	context.avhandler->init(filePath);

	videoContexts.push_back(std::move(context));

	return 0;
}

int nativeGetDecoderState(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter) || iter->avhandler == NULL)
	{
		return -1;
	}

	return iter->avhandler->getDecoderState();
}

void nativeCreateTexture(int id, void *&tex0, void *&tex1, void *&tex2)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter) || iter->textureObj == NULL)
	{
		return;
	}

	iter->textureObj->getResourcePointers(tex0, tex1, tex2);
}

bool nativeStartDecoding(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter) || iter->avhandler == NULL)
	{
		return false;
	}

	if (iter->initThread.joinable())
	{
		iter->initThread.join();
	}

	AVHandler *avhandler = iter->avhandler;

	if (avhandler->getDecoderState() >= AVHandler::DecoderState::INITIALIZED)
	{
		avhandler->startDecoding();
	}

	if (!avhandler->getVideoInfo().isEnabled)
	{
		iter->isContentReady = true;
	}

	return true;
}

void nativeDestroyDecoder(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	iter->id = -1;

	if (iter->initThread.joinable())
	{
		iter->initThread.join();
	}

	if (iter->avhandler != NULL)
	{
		delete (iter->avhandler);
		iter->avhandler = NULL;
	}

	if (iter->path != NULL)
	{
		free(iter->path);
		iter->path = NULL;
	}

	iter->progressTime = 0.0f;
	iter->lastUpdateTime = 0.0f;

	if (iter->textureObj != NULL)
	{
		delete (iter->textureObj);
		iter->textureObj = NULL;
	}

	iter->isContentReady = false;

	videoContexts.erase(iter);
}

//  Video
bool nativeIsVideoEnabled(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return false;
	}

	if (iter->avhandler->getDecoderState() < AVHandler::DecoderState::INITIALIZED)
	{
		LOG("Decoder is unavailable currently. \n");
		return false;
	}

	bool ret = iter->avhandler->getVideoInfo().isEnabled;
	LOG("nativeIsVideoEnabled: %s \n", ret ? "true" : "false");
	return ret;
}

void nativeGetVideoFormat(int id, int &width, int &height, float &totalTime)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	if (iter->avhandler->getDecoderState() < AVHandler::DecoderState::INITIALIZED)
	{
		LOG("Decoder is unavailable currently. \n");
		return;
	}

	IDecoder::VideoInfo *videoInfo = &(iter->avhandler->getVideoInfo());
	width = videoInfo->width;
	height = videoInfo->height;
	totalTime = (float)(videoInfo->totalTime);
}

void nativeSetVideoTime(int id, float currentTime)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	iter->progressTime = currentTime;
}

bool nativeIsAudioEnabled(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return false;
	}

	if (iter->avhandler->getDecoderState() < AVHandler::DecoderState::INITIALIZED)
	{
		LOG("Decoder is unavailable currently. \n");
		return false;
	}

	bool ret = iter->avhandler->getAudioInfo().isEnabled;
	LOG("nativeIsAudioEnabled: %s \n", ret ? "true" : "false");
	return ret;
}

void nativeGetAudioFormat(int id, int &channel, int &frequency, float &totalTime)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	if (iter->avhandler->getDecoderState() < AVHandler::DecoderState::INITIALIZED)
	{
		LOG("Decoder is unavailable currently. \n");
		return;
	}

	IDecoder::AudioInfo *audioInfo = &(iter->avhandler->getAudioInfo());
	channel = audioInfo->channels;
	frequency = audioInfo->sampleRate;
	totalTime = (float)(audioInfo->totalTime);
}

float nativeGetAudioData(int id, unsigned char **audioData, int &frameSize)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return -1.0f;
	}

	return (float)(iter->avhandler->getAudioFrame(audioData, frameSize));
}

void nativeFreeAudioData(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	iter->avhandler->freeAudioFrame();
}

void nativeSetSeekTime(int id, float sec)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	if (iter->avhandler->getDecoderState() < AVHandler::DecoderState::INITIALIZED)
	{
		LOG("Decoder is unavailable currently. \n");
		return;
	}

	LOG("nativeSetSeekTime %f. \n", sec);
	iter->avhandler->setSeekTime(sec);

	if (!iter->avhandler->getVideoInfo().isEnabled)
	{
		iter->isContentReady = true;
	}
	else
	{
		iter->isContentReady = false;
	}
}

bool nativeIsSeekOver(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return false;
	}

	return !(iter->avhandler->getDecoderState() == AVHandler::DecoderState::SEEK);
}

bool nativeIsVideoBufferFull(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return false;
	}

	return iter->avhandler->isVideoBufferFull();
}

bool nativeIsVideoBufferEmpty(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return false;
	}

	return iter->avhandler->isVideoBufferEmpty();
}

int nativeGetMetaData(const char *filePath, char ***key, char ***value)
{
	AVHandler *avhandler = new AVHandler(filePath);
	avhandler->init(filePath);

	char **metaKey = NULL;
	char **metaValue = NULL;
	int metaCount = avhandler->getMetaData(metaKey, metaValue);

	*key = (char **)CoTaskMemAlloc(sizeof(char *) * metaCount);
	*value = (char **)CoTaskMemAlloc(sizeof(char *) * metaCount);

	for (int i = 0; i < metaCount; i++)
	{
		(*key)[i] = (char *)CoTaskMemAlloc(strlen(metaKey[i]) + 1);
		(*value)[i] = (char *)CoTaskMemAlloc(strlen(metaValue[i]) + 1);
		strcpy_s((*key)[i], strlen(metaKey[i]) + 1, metaKey[i]);
		strcpy_s((*value)[i], strlen(metaValue[i]) + 1, metaValue[i]);
	}

	free(metaKey);
	free(metaValue);

	delete avhandler;

	return metaCount;
}

bool nativeIsContentReady(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return false;
	}

	return iter->isContentReady;
}

void nativeSetVideoEnable(int id, bool isEnable)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	iter->avhandler->setVideoEnable(isEnable);
}

void nativeSetAudioEnable(int id, bool isEnable)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	iter->avhandler->setAudioEnable(isEnable);
}

void nativeSetAudioAllChDataEnable(int id, bool isEnable)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter))
	{
		return;
	}

	iter->avhandler->setAudioAllChDataEnable(isEnable);
}

bool nativeIsEOF(int id)
{
	VideoContextIter iter;

	if (!getVideoContextIter(id, &iter) || iter->avhandler == NULL)
	{
		return true;
	}

	return iter->avhandler->getDecoderState() == AVHandler::DecoderState::DECODE_EOF;
}

void nativeRegistLogHandler(UnityLog fp)
{
	//Logger::_unity = fp;
}
