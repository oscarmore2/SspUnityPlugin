#pragma once

#include "IUnityGraphics.h"
#include "ITextureObject.h"
#include <stddef.h>

struct IUnityInterfaces;

// Super-simple "graphics abstraction". This is nothing like how a proper platform abstraction layer would look like;
// all this does is a base interface for whatever our plugin sample needs. Which is only "draw some triangles"
// and "modify a texture" at this point.
//
// There are implementations of this base class for D3D9, D3D11, OpenGL etc.; see individual RenderAPI_* files.
class RenderAPI
{
  public:
	RenderAPI(UnityGfxRenderer r) : unity_renderer(r) {}
	virtual ~RenderAPI() {}

	// Process general event like initialization, shutdown, device loss/reset etc.
	virtual void ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces *interfaces) = 0;

	// Is the API using "reversed" (1.0 at near plane, 0.0 at far plane) depth buffer?
	// Reversed Z is used on modern platforms, and improves depth buffer precision.
	virtual bool GetUsesReverseZ() = 0;

	UnityGfxRenderer GetUnityGfxRenderer()
	{
		return unity_renderer;
	}

	virtual ITextureObject *CreateTextureObject(unsigned int width, unsigned int height,
												TextureFormat format = TEXTURE_FORMAT_YUV) = 0;

  protected:
	UnityGfxRenderer unity_renderer;
};

// Create a graphics API implementation instance for the given API type.
RenderAPI *CreateRenderAPI(UnityGfxRenderer apiType);
