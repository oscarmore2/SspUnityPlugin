using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of physical camera
/// </summary>
public class VcamList : Singleton<VcamList>, IIniConfigable
{
    List<VCam> Cam = new List<VCam>(); 
    Configuration config;

    public void LoadConfig()
    {
        config = new Configuration(System.IO.Path.Combine(Application.streamingAssetsPath, "config/StreamSetting.ini"));

        if (config == null)
        {
            SetDefaultConfig();
            return;
        }

        foreach (var key in config["Input"])
        {
            if (key.KeyName.Contains("url"))
            {
                InputSource source = new InputSource(key.Value);
				source.CreateSurface (new Rect (1, 1, 1, 1));
                VCam v = new VCam(source);
                Add(v);
            }
        }
    }

    public void SetConfig()
    {
        
    }

    public override void OnInitialize()
    {

    }

    public override void OnUninitialize()
    {
        
    }

    public void Add(VCam v)
    {
        Cam.Add(v);
    }

    public void SetDefaultConfig()
    {
        
    }

    public List<VCam> GetList()
    {
        return Cam;
    }
}

public class VCamMappingTable : MonoBehaviour,  IIniConfigable
{

    private Dictionary<VCam, IView> VCamIViewDictionary = new Dictionary<VCam, IView>();

    public Configuration config { get; private set; }

	public void BindVcam(VCam vcam, VCamView view, Rect rect)
    {
		var source = (InputSource)vcam.source;
		var surface = source.CreateSurface (rect);
        VCamIViewDictionary[vcam] = view;
		view.SetSurface (surface);
        source.Begin();
    }

    public IView GetViewByVcam(VCam cam)
    {
        return VCamIViewDictionary[cam];
    }

    public bool GetVcamByView(IView view, ref VCam vcam)
    {
        if (!VCamIViewDictionary.ContainsValue(view))
            return false;

        foreach (var kp in VCamIViewDictionary)
        {
            if (kp.Value == view)
            {
                vcam = kp.Key;
                return true;
            }
        }
        return false;
    }



    public void LoadConfig()
    {
        if (config == null)
        {
            SetDefaultConfig();
            return;
        }
    }

    public void SetConfig()
    {
        
    }

    public void SetDefaultConfig()
    {
        
    }
}
