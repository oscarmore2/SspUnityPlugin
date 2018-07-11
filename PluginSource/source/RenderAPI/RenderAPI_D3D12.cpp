#include "RenderAPI.h"
#include "PlatformBase.h"

// Direct3D 12 implementation of RenderAPI.

#if SUPPORT_D3D12

#include <assert.h>
#include <d3d12.h>
#include "IUnityGraphicsD3D12.h"

class RenderAPI_D3D12 : public RenderAPI
{
  public:
	RenderAPI_D3D12();
	virtual ~RenderAPI_D3D12() {}

	virtual void ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces *interfaces);

	virtual bool GetUsesReverseZ() { return true; }
};

RenderAPI *CreateRenderAPI_D3D12()
{
	return new RenderAPI_D3D12();
}

RenderAPI_D3D12::RenderAPI_D3D12()
{
}

void RenderAPI_D3D12::ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces *interfaces)
{
}
#endif // #if SUPPORT_D3D12
