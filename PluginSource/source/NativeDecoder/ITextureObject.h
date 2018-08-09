#pragma once

extern "C" {
#include <libavutil/pixfmt.h>
}
class ITextureObject {
public:
	virtual ~ITextureObject() {}
	virtual void create(void* handler, unsigned int width, unsigned int height) = 0;
	virtual void getResourcePointers(void*& ptry, void*& ptru, void*& ptrv) = 0;
	virtual void upload(unsigned char* ych, unsigned char* uch, unsigned char* vch) = 0;
	virtual void destroy() = 0;

	virtual void getResourcePointer(void*& ptr) {}
	virtual void uploadOnce(unsigned char* data) {}

	static const unsigned int CPU_ALIGMENT = 64;
	static const unsigned int TEXTURE_NUM = 3;


	virtual void create(AVPixelFormat format, void* handler, unsigned int width, unsigned int height) {}
};