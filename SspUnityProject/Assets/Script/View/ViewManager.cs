using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour {

    public PGMView PGM;
    public PVWView PVW;
    public List<VCamView> VCamView = new List<VCamView>();
    public Texture2D DefaultImg;
    

    public VCamMappingTable MappingTable;

    public void InitView()
    {
        for (int i = 0; i < VcamList.Instance.GetList().Count; i++)
        {
            VCamView[i].InitView(this);
            MappingTable.BindVcam(VcamList.Instance.GetList()[i], VCamView[i], new Rect(1, 1, 1, 1));
        }
        PGM.InitView(this);
        PVW.InitView(this);
    }

    void OnChangeBiding()
    {

    }

    public void OnPushToPVW(IView view)
    {
        var vcam = (VCamView)view;
        if (vcam.BufferTexture != null)
            PVW.OnUpdateTexture(vcam.BufferTexture);
    }

    void OnTransitionToPGM()
    {

    }

    public void ExpendCameraView()
    {

    }

    public void ShinkCameraView()
    {

    }
}
