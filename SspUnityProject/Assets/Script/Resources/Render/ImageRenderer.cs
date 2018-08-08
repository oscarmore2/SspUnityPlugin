using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ImageRenderer : IResourceRenderer {

    public RawImage Image;

    public override Resource.ResourceType GetType()
    {
        return Resource.ResourceType.Image;
    }

    public override void ApplyAttrs(Dictionary<string, string> _Attrs = null)
    {
        base.ApplyAttrs(_Attrs);
    }

    public override void ChangeContent(Resource.IResource contentData)
    {
        Image.texture = (Texture2D)contentData.GetFile();
    }

    public static class ImageRenderGenerate
    {
        public static ImageRenderer Generate(Resource.IResource res, Transform root)
        {
            GameObject obj = new GameObject(res.Name);
            var render = obj.AddComponent<ImageRenderer>();
            render.transform.parent = root;
            render.rectTranfrom = obj.GetComponent<RectTransform>();
            render.Image = obj.GetComponent<RawImage>();
            render.ChangeContent(res);
            render.Attrs = res.Attrs;
            render.ApplyAttrs();
            return render;
        }
    }
}
