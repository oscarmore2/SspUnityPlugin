using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCam 
{
    public InputSource source { get; protected set; }
    public VCamRender render { get; protected set; }
    public VCamController controller { get; protected set; }

    public VCam()
    {
        controller = new VCamController(this);
    }

    public VCam(InputSource s, VCamRender r)
    {
        source = s;
        render = r;
        controller = new VCamController(this);
    }
}


public class VCamFactory
{
    public static VCam Create(string url, GameObject o)
    {
        var s = InputSourceProvider.Create(url,o);
        var r = InputRenderProvide.Create(o);
        return new VCam(s,r);
    }
}