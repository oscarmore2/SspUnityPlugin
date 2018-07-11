
#include "RenderAPI.h"
#include "PlatformBase.h"


// Metal implementation of RenderAPI.


#if SUPPORT_METAL

#include "IUnityGraphicsMetal.h"
#import <Metal/Metal.h>


class RenderAPI_Metal : public RenderAPI
{
public:
	RenderAPI_Metal(UnityGfxRenderer apiType);
	virtual ~RenderAPI_Metal() { }
	
	virtual void ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces* interfaces);
	
	virtual bool GetUsesReverseZ() { return true; }
};

RenderAPI* CreateRenderAPI_Metal(UnityGfxRenderer apiType)
{
	return new RenderAPI_Metal(apiType);
}

RenderAPI_Metal::RenderAPI_Metal(UnityGfxRenderer apiType):RenderAPI(apiType)
{
}

void RenderAPI_Metal::ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces* interfaces)
{
}
#endif // #if SUPPORT_METAL
