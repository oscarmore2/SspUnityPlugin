using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// List of physical camera
/// </summary>
public class VcamList : Singleton<VcamList>, IConfigable
{
    List<VCam> Cam = new List<VCam>();

    public void LoadConfig(object config)
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

    public override void OnInitialize()
    {
        LoadConfig(null);
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

public class VCamMappingTable : MonoBehaviour,  IConfigable {

    private Dictionary<VCam, IView> VCamIViewDictionary = new Dictionary<VCam, IView>();
    private Dictionary<IView, VCamRender> IViewRenderDictionary = new Dictionary<IView, VCamRender>();

    public void BindVcam(VCam vcam, IView view)
    {
        VCamIViewDictionary[vcam] = view;
        VCam vcamOld = null;
        if (GetVcamByView(view, ref vcamOld))
        {
            if (vcamOld == vcam)
                return;

            vcamOld.RemoveRender(IViewRenderDictionary[view]);
            vcamOld.OnSetTexture -= view.OnUpdateTexture;
            if (!vcam.ExistRender(IViewRenderDictionary[view]))
            {
                vcam.OnSetTexture += view.OnUpdateTexture;
                IViewRenderDictionary[view] = InputRenderProvide.Create(view.ViewImage);
                vcam.AddRender(IViewRenderDictionary[view]);
            }
        }
        else
        {
            vcam.OnSetTexture += view.OnUpdateTexture;
            IViewRenderDictionary[view] = InputRenderProvide.Create(view.ViewImage);
            vcam.AddRender(IViewRenderDictionary[view]);
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
            if (v.ExistRender(IViewRenderDictionary[view]))
            {
                vcam = v;
                return true;
            }
        }
        return false;
    }



    public void LoadConfig(object config)
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
