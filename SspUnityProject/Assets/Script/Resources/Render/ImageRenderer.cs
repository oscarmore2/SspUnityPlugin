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
		var img = GetComponent<RawImage> ();
		float rotation = 0;
		float alpha = 0;
		float x = 0;
		float y = 0;
		float.TryParse (_Attrs ["Alpha"], out alpha);
		float.TryParse (_Attrs ["Rotation"], out rotation);
		var angel = transform.localEulerAngles;
		angel.z = rotation;
		transform.localEulerAngles = angel;
		var c = img.color;
		c.a = alpha;
		img.color = c;
		rectTranfrom = GetComponent<RectTransform> ();
		rectTranfrom.anchoredPosition = new Vector2 (x, y);
    }

    public override void ChangeContent(Resource.IResource contentData)
    {
        image.texture = (Texture2D)((ImageResource)contentData).GetFile();
    }

	public override T AttachRenderTarget<T>(GameObject target, Resource.ResourceGroup rg)
    {
		base.AttachRenderTarget<T>(target, rg);
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
		rect.anchoredPosition = rectTranfrom.anchoredPosition + new Vector2(rg.XAxis, rg.YAxis);
		target.transform.localScale = new Vector3(rg.Scale, rg.Scale, rg.Scale);
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
			ModifyContent (ref render, res);
            return render;
        }

		public static void ModifyContent(ref ImageRenderer renderer, Resource.IResource res)
		{
			renderer.rectTranfrom = renderer.GetComponent<RectTransform>();
			renderer.image = renderer.GetComponent<RawImage>();
			renderer.ChangeContent(res);
			renderer.Attrs = res.Attrs;
			renderer.ApplyAttrs(res.Attrs);
		}
    }
}
