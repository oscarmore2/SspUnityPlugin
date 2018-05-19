using System.Collections;
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
        var filter = g.GetComponent<MeshRenderer>();
        if (filter)
            return Create(filter);
        return null;
    }
}
public interface VCamRender  {

}
[RequireComponent(typeof(MeshRenderer))]
public class InputMeshRender :MonoBehaviour,VCamRender
{

}

[RequireComponent(typeof(RawImage))]
public class InputRawImageRender :MonoBehaviour,VCamRender
{

}
