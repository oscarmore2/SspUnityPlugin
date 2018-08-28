using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ImageRenderer : CommonResourceRenderer {

    public RawImage image;
    

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
        image.texture = (Texture2D)((ImageResource)contentData).GetFile();
    }

    public override T AttachRenderTarget<T>(GameObject target)
    {
        base.AttachRenderTarget<T>(target);
        System.Type type = image.GetType();
        var dst = target.GetComponent(type) as T;
        if (!dst)
            dst = target.AddComponent(type) as T;

        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(image));
        }

        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(image, null), null);
        }
        target.transform.localPosition = Vector3.zero;
        RectTransform rect = target.GetComponent<RectTransform>();
        rect.anchoredPosition = rectTranfrom.anchoredPosition;
        target.transform.localScale = Vector3.one;
        renderTarget.Add(target);
        return dst;
    }

    public static class ImageRenderGenerate
    {
        public static ImageRenderer Generate(Resource.IResource res, Transform root)
        {
            GameObject obj = new GameObject(res.Name);
            var render = obj.AddComponent<ImageRenderer>();
            render.transform.parent = root;
            render.rectTranfrom = obj.GetComponent<RectTransform>();
            render.image = obj.GetComponent<RawImage>();
            render.ChangeContent(res);
            render.Attrs = res.Attrs;
            render.ApplyAttrs(res.Attrs);
            return render;
        }
    }
}
