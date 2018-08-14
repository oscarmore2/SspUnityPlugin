using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;

public class RendererContainor : Singleton<RendererContainor> {
    

	Dictionary<string, IResourceRenderer> RenderPathMapping = new Dictionary<string, IResourceRenderer>();

    public override void OnInitialize()
    {
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
    }

    public override void OnUninitialize()
    {
        
    }

	public IResourceRenderer PVWRender(IResource resource, bool isAfterTransition)
    {
        var suffix = "@PVW";
        var rend = AddRender(suffix, resource);
        rend.IsAfterTransition = isAfterTransition;
		return rend;
    }

	public IResourceRenderer PGMRender(IResource resource, bool isAfterTransition)
    {
        var suffix = "@PGM";
        var rend = AddRender(suffix, resource);
        rend.IsAfterTransition = isAfterTransition;
		return rend;
    }

    IResourceRenderer AddRender(string suffix, IResource resource)
    {
        IResourceRenderer render = null;
        foreach (var val in RenderPathMapping.Keys)
        {
            if (val.Replace(suffix, "") == resource.GUID)
            {
                render = RenderPathMapping[val];
                RenderPathMapping.Remove(val);
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
        return render;
    }

    public IResourceRenderer this[string path]
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
