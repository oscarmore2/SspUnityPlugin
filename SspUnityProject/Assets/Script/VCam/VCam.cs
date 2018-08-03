using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityPlugin.Decoder;

public class VCam
{
    public IInputSource source { get; protected set; }

    public VCamController controller { get; protected set; }

	public Surface currentSurface{ get; set;}

    public VCam(IInputSource s)
    {
        source = s;
        controller = new VCamController(this);
    }
}


public class VCamFactory
{
    public static VCam Create(string url, GameObject o)
    {
        var s = InputSourceProvider.Create(url, o);
        var cam = new VCam(s);
        return cam;
    }
}