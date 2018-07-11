#include "RenderAPI.h"
#include "PlatformBase.h"
#include "IUnityGraphics.h"

RenderAPI *CreateRenderAPI(UnityGfxRenderer apiType)
{
#if SUPPORT_D3D11

	if (apiType == kUnityGfxRendererD3D11)
	{
		extern RenderAPI *CreateRenderAPI_D3D11(UnityGfxRenderer apiType);
		return CreateRenderAPI_D3D11(apiType);
	}

#endif // if SUPPORT_D3D11

#if SUPPORT_D3D9

	if (apiType == kUnityGfxRendererD3D9)
	{
		extern RenderAPI *CreateRenderAPI_D3D9(UnityGfxRenderer apiType);
		return CreateRenderAPI_D3D9(apiType);
	}

#endif // if SUPPORT_D3D9

#if SUPPORT_D3D12

	if (apiType == kUnityGfxRendererD3D12)
	{
		extern RenderAPI *CreateRenderAPI_D3D12(UnityGfxRenderer apiType);
		return CreateRenderAPI_D3D12(apiType);
	}

#endif // if SUPPORT_D3D9

#if SUPPORT_OPENGL_UNIFIED

	if (apiType == kUnityGfxRendererOpenGLCore || apiType == kUnityGfxRendererOpenGLES20 || apiType == kUnityGfxRendererOpenGLES30)
	{
		extern RenderAPI *CreateRenderAPI_OpenGLCoreES(UnityGfxRenderer apiType);
		return CreateRenderAPI_OpenGLCoreES(apiType);
	}

#endif // if SUPPORT_OPENGL_UNIFIED

#if SUPPORT_OPENGL_LEGACY

	if (apiType == kUnityGfxRendererOpenGL)
	{
		extern RenderAPI *CreateRenderAPI_OpenGL2(UnityGfxRenderer apiType);
		return CreateRenderAPI_OpenGL2(apiType);
	}

#endif // if SUPPORT_OPENGL_LEGACY

#if SUPPORT_METAL

	if (apiType == kUnityGfxRendererMetal)
	{
		extern RenderAPI *CreateRenderAPI_Metal(UnityGfxRenderer apiType);
		return CreateRenderAPI_Metal(apiType);
	}

#endif // if SUPPORT_METAL

	// Unknown or unsupported graphics API
	return NULL;
}
