using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of physical camera
/// </summary>
public class VcamList : Singleton<VcamList>, IConfigable
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
                var obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(""));
                obj.name = "DefaultBuffer";
                obj.transform.parent = transform;
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

public class VCamMappingTable : MonoBehaviour,  IConfigable
{

    private Dictionary<VCam, IView> VCamIViewDictionary = new Dictionary<VCam, IView>();
    private Dictionary<IView, VCamRender> IViewRenderDictionary = new Dictionary<IView, VCamRender>();

    public Configuration config { get; private set; }

	public void BindVcam(ref VCam vcam, VCamView view, Rect rect)
    {
		var source = (InputSource)vcam.source;
		var surface = source.CreateSurface (rect);
        VCamIViewDictionary[vcam] = view;
		vcam.currentSurface = surface;
		view.SetImage (surface);
    }

    public IView GetViewByVcam(VCam cam)
    {
        return VCamIViewDictionary[cam];
    }

    public bool GetVcamByView(IView view, ref VCam vcam)
    {
        if (!IViewRenderDictionary.ContainsKey(view))
            return false;

        foreach (var v in VcamList.Instance.GetList())
        {
//            if (v.ExistRender(IViewRenderDictionary[view]))
//            {
//                vcam = v;
//                return true;
//            }
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
