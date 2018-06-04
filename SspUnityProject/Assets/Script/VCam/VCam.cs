using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCam 
{
    public InputSource source { get; protected set; }
    public VCamController controller { get; protected set; }

    public System.Action<Texture> OnSetTexture;

    protected List<VCamRender> renders = new List<VCamRender>();

    protected void SetRenders(Texture y, Texture u, Texture v)
    {
        for (int i = 0; i < renders.Count; ++i)
        {
            renders[i].SetTextures(y,u,v);
        }


        if (null != OnSetTexture && null != source.GetResult())
        {
            OnSetTexture(source.GetResult());
        }
    }
    public VCam(InputSource s)
    {
        source = s;
        source.OnSetTextures = SetRenders;
        controller = new VCamController(this);
        VcamList.Instance.Add(this);
    }

    public void AddRender(VCamRender render)
    {
        if(!renders.Contains(render))
            renders.Add(render);
    }

    public void ClearRender()
    {
        var num_render = renders.Count;
        renders.RemoveRange(1, num_render - 1);
    }

    public bool ExistRender(VCamRender render)
    {
        return renders.Contains(render);
    }

    public void RemoveRender(VCamRender render)
    {
        if (renders.Contains(render))
            renders.Remove(render);
    }
}


public class VCamFactory
{
    public static VCam Create(string url, GameObject o)
    {
        Debug.LogWarning("URL is "+url);
        var s = InputSourceProvider.Create(url,o);
        var r = InputRenderProvide.Create(o);
        var cam =  new VCam(s);
        if (null == r)
        {
            throw new System.NullReferenceException("Renderer return null");
        }
        cam.AddRender(r);

        return cam;
    }
}