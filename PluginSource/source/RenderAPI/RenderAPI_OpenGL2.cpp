#include "RenderAPI.h"
#include "PlatformBase.h"

// OpenGL 2 (legacy, deprecated) implementation of RenderAPI.

#if SUPPORT_OPENGL_LEGACY

#include "GLEW/glew.h"

class RenderAPI_OpenGL2 : public RenderAPI
{
  public:
	RenderAPI_OpenGL2(UnityGfxRenderer apiType);
	virtual ~RenderAPI_OpenGL2() {}

	virtual void ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces *interfaces);

	virtual bool GetUsesReverseZ()
	{
		return false;
	}
};

RenderAPI *CreateRenderAPI_OpenGL2(UnityGfxRenderer apiType)
{
	return new RenderAPI_OpenGL2(apiType);
}

RenderAPI_OpenGL2::RenderAPI_OpenGL2(UnityGfxRenderer apiType) : RenderAPI(apiType)
{
}

void RenderAPI_OpenGL2::ProcessDeviceEvent(UnityGfxDeviceEventType type,
										   IUnityInterfaces *interfaces)
{
	// not much to do :)
}

#endif // #if SUPPORT_OPENGL_LEGACY
