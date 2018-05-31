using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputRenderProvide
{
    public static VCamRender Create(RawImage image)
    {
        return image.gameObject.AddComponent<InputRawImageRender>();
    }

    public static VCamRender Create(MeshRenderer filter)
    {
        return filter.gameObject.AddComponent<InputMeshRender>();
    }

    public static VCamRender Create(GameObject g)
    {
        var image = g.GetComponent<RawImage>();
        if (image)
            return Create(image);
        var meshRender = g.GetComponent<MeshRenderer>();
        if (meshRender)
            return Create(meshRender);
        return null;
    }
}
public interface VCamRender
{
    void SetTextures(Texture y, Texture u, Texture v);
}
[RequireComponent(typeof(MeshRenderer))]
public class InputMeshRender :MonoBehaviour,VCamRender
{
    protected MeshRenderer meshRenderer;
    protected Material material;

    public event Action OnSetTexture;

    public void SetTextures(Texture y, Texture u, Texture v)
    {
        if (null == meshRenderer)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        if (null == material)
        {
            material = new Material(Shader.Find("Unlit/YUV2RGBA"));
            meshRenderer.material = material;
        }
        material.SetTexture("_YTex",y);
        material.SetTexture("_UTex", u);
        material.SetTexture("_VTex", v);
    }
}

[RequireComponent(typeof(RawImage))]
public class InputRawImageRender :MonoBehaviour,VCamRender
{
    protected RawImage rawImage;
    protected Material material;

    public event Action OnSetTexture;

    public void SetTextures(Texture y, Texture u, Texture v)
    {
        if (null == rawImage)
        {
            rawImage = GetComponent<RawImage>();
        }
        if (null == material)
        {
            material = new Material(Shader.Find("UI/YUV2RGBA"));
            rawImage.material = material;
        }
        material.SetTexture("_YTex", y);
        material.SetTexture("_UTex", u);
        material.SetTexture("_VTex", v);
    }
}
