//========= Copyright 2015-2018, HTC Corporation. All rights reserved. ===========

#pragma once
#include "IUnityInterface.h"

extern "C" {

	//Log
	UNITY_INTERFACE_EXPORT void nativeRegistLogHandler(UnityLog fp);

	UNITY_INTERFACE_EXPORT int nativeCreateDecoder(const char* filePath, int& id);
	UNITY_INTERFACE_EXPORT int nativeCreateDecoderAsync(const char* filePath, int& id);
	UNITY_INTERFACE_EXPORT int nativeGetDecoderState(int id);
	UNITY_INTERFACE_EXPORT void nativeCreateTexture(int id, void*& tex0, void*& tex1, void*& tex2);
	UNITY_INTERFACE_EXPORT bool nativeStartDecoding(int id);
	UNITY_INTERFACE_EXPORT void nativeDestroyDecoder(int id);
	UNITY_INTERFACE_EXPORT bool nativeIsEOF(int id);

	UNITY_INTERFACE_EXPORT bool nativeIsVideoEnabled(int id);
	UNITY_INTERFACE_EXPORT void nativeSetVideoEnable(int id, bool isEnable);
	UNITY_INTERFACE_EXPORT void nativeGetVideoFormat(int id, int& width, int& height, float& totalTime);
	UNITY_INTERFACE_EXPORT void nativeSetVideoTime(int id, float currentTime);
	UNITY_INTERFACE_EXPORT bool nativeIsContentReady(int id);
	UNITY_INTERFACE_EXPORT bool nativeIsVideoBufferFull(int id);
	UNITY_INTERFACE_EXPORT bool nativeIsVideoBufferEmpty(int id);

	UNITY_INTERFACE_EXPORT bool nativeIsAudioEnabled(int id);
	UNITY_INTERFACE_EXPORT void nativeSetAudioEnable(int id, bool isEnable);
	UNITY_INTERFACE_EXPORT void nativeSetAudioAllChDataEnable(int id, bool isEnable);
	UNITY_INTERFACE_EXPORT void nativeGetAudioFormat(int id, int& channel, int& frequency, float& totalTime);
	UNITY_INTERFACE_EXPORT float nativeGetAudioData(int id, unsigned char** audioData, int& frameSize);
	UNITY_INTERFACE_EXPORT void nativeFreeAudioData(int id);

	UNITY_INTERFACE_EXPORT void nativeSetSeekTime(int id, float sec);
	UNITY_INTERFACE_EXPORT bool nativeIsSeekOver(int id);

	UNITY_INTERFACE_EXPORT int nativeGetMetaData(const char* filePath, char*** key, char*** value);
}