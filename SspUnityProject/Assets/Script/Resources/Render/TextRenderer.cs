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
