using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;

public class RendererContainor : Singleton<RendererContainor> {
    

	Dictionary<string, CommonResourceRenderer> RenderPathMapping = new Dictionary<string, CommonResourceRenderer>();

    public override void OnInitialize()
    {
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
    }

    public override void OnUninitialize()
    {
        
    }

	public CommonResourceRenderer PVWRender(IResource resource, bool isAfterTransition)
    {
        var suffix = "@PVW";
        var rend = AddRender(suffix, resource);
        rend.IsAfterTransition = isAfterTransition;
		return rend;
    }

	public CommonResourceRenderer PGMRender(IResource resource, bool isAfterTransition)
    {
        var suffix = "@PGM";
        var rend = AddRender(suffix, resource);
        rend.IsAfterTransition = isAfterTransition;
		return rend;
    }

	CommonResourceRenderer AddRender(string suffix, IResource resource)
    {
        CommonResourceRenderer render = null;
        foreach (var val in RenderPathMapping.Keys)
        {
            if (val.Replace(suffix, "") == resource.GUID)
            {
                render = RenderPathMapping[val];
                RenderPathMapping.Remove(val);
				if (resource.GetType() == ResourceType.Image)
				{
					var rend = (ImageRenderer)render;
					ImageRenderer.ImageRenderGenerate.ModifyContent(ref rend, resource);
					render = rend;
				}
				else if (resource.GetType() == ResourceType.Text)
				{
					var rend = (TextRenderer)render;
					TextRenderer.TextRenderGenerate.ModifyContent(ref rend, resource);
					render = rend;
				}
                break;
            }
        }

        if (render == null)
        {
            if (resource.GetType() == ResourceType.Image)
            {
                render = ImageRenderer.ImageRenderGenerate.Generate(resource, transform);
            }
            else if (resource.GetType() == ResourceType.Text)
            {
                render = TextRenderer.TextRenderGenerate.Generate(resource, transform);
            }
        }

        render.GUID = resource.GUID;

        RenderPathMapping[resource.GUID + suffix] = render;
		render.Attrs = resource.Attrs;
        return render;
    }

    public CommonResourceRenderer this[string path]
    {
        get {
            if (RenderPathMapping.ContainsKey(path))
            {
                return RenderPathMapping[path];
            }
            else
            {
                return null;
            }
        }

        set {
            RenderPathMapping[path] = value;
        }
    }

    public int Count
    {
        get { return RenderPathMapping.Count; }
    }
}
