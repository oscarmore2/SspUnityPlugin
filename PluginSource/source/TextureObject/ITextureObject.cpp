#include "ITextureObject.h"
#include "PlatformBase.h"
#include "IUnityGraphics.h"
ITextureObject *createTextureObject(UnityGfxRenderer apiType, void *handler, unsigned int width, unsigned int height)
{
#if SUPPORT_D3D11
    if (apiType == kUnityGfxRendererD3D11)
    {
        ITextureObject *textureObj = new DX11TextureObject();
        textureObj->create(handler, width, height);
        return textureObj;
    }
#endif // if SUPPORT_D3D11

#if SUPPORT_D3D9

#endif // if SUPPORT_D3D9

#if SUPPORT_D3D12

#endif // if SUPPORT_D3D9

#if SUPPORT_OPENGL_UNIFIED

#endif // if SUPPORT_OPENGL_UNIFIED

#if SUPPORT_OPENGL_LEGACY

#endif // if SUPPORT_OPENGL_LEGACY

#if SUPPORT_METAL

#endif // if SUPPORT_METAL

    // Unknown or unsupported graphics API
    return nullptr;
}