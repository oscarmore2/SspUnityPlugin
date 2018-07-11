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
                VCam v = VCamFactory.Create(key.Value, obj);
                v.controller.Start();
                Add(v);
            }
        }
    }

    public void SetConfig()
    {
        
    }

    public override void OnInitialize()
    {
        LoadConfig();
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

    public void BindVcam(VCam vcam, IView view)
    {
        VCamIViewDictionary[vcam] = view;
        VCam vcamOld = null;
        if (GetVcamByView(view, ref vcamOld))
        {
            if (vcamOld == vcam)
                return;

//            vcamOld.RemoveRender((InputRawImageRender)IViewRenderDictionary[view]);
//            vcamOld.OnSetTexture -= view.OnUpdateTexture;
//            if (!vcam.ExistRender(IViewRenderDictionary[view]))
//            {
//                vcam.OnSetTexture += view.OnUpdateTexture;
//                IViewRenderDictionary[view] = InputRenderProvide.Create(view.gameObject);
//                vcam.AddRender(IViewRenderDictionary[view]);
//            }
        }
        else
        {
//            vcam.OnSetTexture += view.OnUpdateTexture;
//            IViewRenderDictionary[view] = InputRenderProvide.Create(view.gameObject);
//            vcam.AddRender(IViewRenderDictionary[view]);
        }
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
