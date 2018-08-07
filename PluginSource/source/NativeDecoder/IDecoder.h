//========= Copyright 2015-2018, HTC Corporation. All rights reserved. ===========

#pragma once
#include<cstddef>
class IDecoder
{
  public:
	enum BufferState
	{
		EMPTY,
		NORMAL,
		FULL
	};

	struct VideoInfo
	{
		bool isEnabled;
		int width;
		int height;
		double lastTime;
		double totalTime;
		BufferState bufferState;
	};

	struct VideoFrame
	{
		const unsigned char *yChannel;
		const unsigned char *uChannel;
		const unsigned char *vChannel;
		const int channelSize;

		VideoFrame(unsigned char *y,
				   unsigned char *u,
				   unsigned char *v,
				   int size)
			: yChannel(y),
			  uChannel(u),
			  vChannel(v),
			  channelSize(size) {}
	};

	struct AudioInfo
	{
		bool isEnabled;
		unsigned int channels;
		unsigned int sampleRate;
		double lastTime;
		double totalTime;
		BufferState bufferState;
	};

	struct AudioFrame
	{
		const unsigned char *data;
		const int size;
		AudioFrame(unsigned char* d, int s): data(d), size(s) {}
	};

	virtual ~IDecoder() {}

	virtual VideoFrame getVideoFrame()
	{
		VideoFrame f(NULL, NULL, NULL, 0);
		return f;
	}

	virtual AudioFrame getAudioFrame()
	{
		AudioFrame f(NULL, 0);
		return f;
	}

	virtual bool init(const char *filePath) = 0;
	virtual bool decode() = 0;
	virtual void seek(double time) = 0;
	virtual void destroy() = 0;

	virtual VideoInfo getVideoInfo() = 0;
	virtual AudioInfo getAudioInfo() = 0;
	virtual void setVideoEnable(bool isEnable) = 0;
	virtual void setAudioEnable(bool isEnable) = 0;
	virtual void setAudioAllChDataEnable(bool isEnable) = 0;
	virtual double getVideoFrame(unsigned char **outputY, unsigned char **outputU, unsigned char **outputV) = 0;
	virtual double getAudioFrame(unsigned char **outputFrame, int &frameSize) = 0;
	virtual void freeVideoFrame() = 0;
	virtual void freeAudioFrame() = 0;
	virtual bool isContentReady() = 0;
	virtual int getMetaData(char **&key, char **&value) = 0;
};