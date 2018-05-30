using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamController
{
    protected VCam cam;
    public VCamController(VCam c)
    {
        cam = c;
    }

    public float Volume
    {
        get { return 1; }
        set
        {

        }
    }

    public void Start()
    {
        if (cam.source != null)
            cam.source.Begin();
    }

    public void Pause()
    {
        if (cam.source != null)
            cam.source.Pause();
    }

    public void Resume()
    {
        if (cam.source != null)
            cam.source.Resume();
    }

    public void Stop()
    {
        if (cam.source != null)
            cam.source.End();
    }
}
