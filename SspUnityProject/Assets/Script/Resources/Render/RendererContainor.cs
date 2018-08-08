using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;

public class RendererContainor : Singleton<RendererContainor> {
    

	Dictionary<string, IResourceRenderer> RenderPathMapping = new Dictionary<string, IResourceRenderer>();

    public override void OnInitialize()
    {
        
    }

    public override void OnUninitialize()
    {
        
    }

    public void PVWRender(IResource resource, bool isAfterTransition)
    {
        var suffix = "@PVW";
        var rend = AddRender(suffix, resource);
        rend.IsAfterTransition = isAfterTransition;
    }

    public void PGMRender(IResource resource, bool isAfterTransition)
    {
        var suffix = "@PGM";
        var rend = AddRender(suffix, resource);
        rend.IsAfterTransition = isAfterTransition;
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

        RenderPathMapping[resource.GUID + suffix] = render;
        return render;
    }

    public IResourceRenderer this[string path]
    {
        get {
            return RenderPathMapping[path];
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
