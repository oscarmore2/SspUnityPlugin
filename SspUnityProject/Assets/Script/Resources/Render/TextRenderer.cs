using System;
using System.Collections;
using System.Collections.Generic;
using Resource;
using UnityEngine;
using UnityEngine.UI;

public class TextRenderer : IResourceRenderer {

    public Text Content;

    public override void ChangeContent(Resource.IResource contentData)
    {
        Content.text = (string)contentData.GetFile();
    }

    public override Resource.ResourceType GetType()
    {
        return Resource.ResourceType.Text;
    }

    public override T AttachRenderTarget<T>(GameObject target)
    {
        base.AttachRenderTarget<T>(target);
        System.Type type = Content.GetType();
        var dst = target.GetComponent(type) as T;
        if (!dst)
            dst = target.AddComponent(type) as T;

        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(Content));
        }

        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(Content, null), null);
        }
        renderTarget.Add(target);
        return dst;
    }

    public static class TextRenderGenerate
    {
        public static TextRenderer Generate(Resource.IResource res, Transform root)
        {
            GameObject obj = new GameObject(res.Name);
            var render = obj.AddComponent<TextRenderer>();
            render.transform.parent = root;
            render.rectTranfrom = obj.GetComponent<RectTransform>();
            render.Content = obj.GetComponent<Text>();
            render.ChangeContent(res);
            render.Attrs = res.Attrs;
            render.ApplyAttrs();
            return render;
        }
    }
}
