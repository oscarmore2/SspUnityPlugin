using System;
using System.Collections;
using System.Collections.Generic;
using Resource;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextRenderer : CommonResourceRenderer {

    public Text Content;

    public override void ChangeContent(Resource.IResource contentData)
    {
		
        Content.text = (string)((TextResoure)contentData).GetFile();
    }

    public override Resource.ResourceType GetType()
    {
        return Resource.ResourceType.Text;
    }

	public override void ApplyAttrs(Dictionary<string, string> _Attrs = null)
	{
		base.ApplyAttrs(_Attrs);
		var text = GetComponent<Text> ();
		float x = 0;
		float y = 0;
		float r = 0;
		float g = 0;
		float b = 0;
		float a = 0;
		int fontSize = 10;
		float.TryParse (Attrs ["R"], out r);
		float.TryParse (Attrs ["G"], out g);
		float.TryParse (Attrs ["B"], out b);
		float.TryParse (Attrs ["Alpha"], out a);
		float.TryParse (Attrs ["X"], out x);
		float.TryParse (Attrs ["Y"], out y);
		int.TryParse (Attrs ["FontSize"], out fontSize);
		text.color = new Color (r, g, b, a);
		text.fontSize = fontSize;
		text.font = Font.CreateDynamicFontFromOSFont (Attrs ["FontFace"], 10);
		rectTranfrom = GetComponent<RectTransform> ();
		rectTranfrom.anchoredPosition = new Vector2 (x, y);
	}

	public override T AttachRenderTarget<T>(GameObject target, Resource.ResourceGroup rg)
    {
		base.AttachRenderTarget<T>(target,rg);
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
        target.transform.localPosition = Vector3.zero;
        RectTransform rect = target.GetComponent<RectTransform>();
		rect.anchoredPosition = rectTranfrom.anchoredPosition + new Vector2(rg.XAxis, rg.YAxis);
		target.transform.localScale = new Vector3(rg.Scale, rg.Scale, rg.Scale);
		var cont = target.AddComponent<ContentSizeFitter> ();
		cont.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		cont.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        return dst;
    }

    public static class TextRenderGenerate
    {
        public static TextRenderer Generate(Resource.IResource res, Transform root)
        {
            GameObject obj = new GameObject(res.Name);
            var render = obj.AddComponent<TextRenderer>();
            render.transform.parent = root;
			ModifyContent (ref render, res);
            return render;
        }

		public static void ModifyContent(ref TextRenderer render, Resource.IResource res)
		{
			render.rectTranfrom = render.GetComponent<RectTransform>();
			render.Content = render.GetComponent<Text>();
			render.ChangeContent(res);
			render.Attrs = res.Attrs;
			render.ApplyAttrs(res.Attrs);
		}
    }
}
