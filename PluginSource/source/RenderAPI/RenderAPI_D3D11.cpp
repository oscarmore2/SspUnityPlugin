#include "RenderAPI.h"
#include "PlatformBase.h"
#include "DX11TextureObject.h"
// Direct3D 11 implementation of RenderAPI.

#if SUPPORT_D3D11

#include <assert.h>
#include <d3d11.h>
#include "IUnityGraphicsD3D11.h"

class RenderAPI_D3D11 : public RenderAPI
{
  public:
	RenderAPI_D3D11(UnityGfxRenderer apiType);
	virtual ~RenderAPI_D3D11() {}

	virtual void ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces *interfaces);

	virtual bool GetUsesReverseZ()
	{
		return (int)m_Device->GetFeatureLevel() >= (int)D3D_FEATURE_LEVEL_10_0;
	}
	virtual ITextureObject *CreateTextureObject(unsigned int width, unsigned int height,
												TextureFormat format = TEXTURE_FORMAT_YUV);

  private:
	ID3D11Device *m_Device;
};

RenderAPI *CreateRenderAPI_D3D11(UnityGfxRenderer apiType)
{
	return new RenderAPI_D3D11(apiType);
}

RenderAPI *RenderAPI_D3D11::RenderAPI_D3D11(UnityGfxRenderer apiType) : RenderAPI(apiType)
{
	return new DX11
}

void RenderAPI_D3D11::ProcessDeviceEvent(UnityGfxDeviceEventType type,
										 IUnityInterfaces *interfaces)
{
	switch (type)
	{
	case kUnityGfxDeviceEventInitialize:
	{
		IUnityGraphicsD3D11 *d3d = interfaces->Get<IUnityGraphicsD3D11>();
		m_Device = d3d->GetDevice();
		break;
	}

	case kUnityGfxDeviceEventShutdown:
		break;
	}
}

ITextureObject *RenderAPI_D3D11::CreateTextureObject(unsigned int width, unsigned int height,
													 TextureFormat format = TEXTURE_FORMAT_YUV)
{
	DX11TextureObject *to = new DX11TextureObject();
	to->create(m_Device, width, height);
	return to;
}
#endif // #if SUPPORT_D3D11
