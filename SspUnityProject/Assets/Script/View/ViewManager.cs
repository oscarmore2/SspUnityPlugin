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
			MappingTable.BindVcam(VcamList.Instance.GetList()[i], VCamView[i], new Rect(1, 1, 1, 1));
        }
        PGM.renderProcessManager.CreateBseicRenderProcess();
        PVW.renderProcessManager.CreateBseicRenderProcess();

        PGM.renderProcessManager.StartRender(DefaultImg);
        PVW.renderProcessManager.StartRender(DefaultImg);
        AttachUI();
    }

    void OnChangeBiding()
    {

    }

    public void AttachUI()
    {
        PGM.AttachUILayer();
        PVW.AttachUILayer();
    }

    public void OnPushToPVW(IView view)
    {
        VCam cam = null;
        MappingTable.GetVcamByView(view, ref cam);
        if (cam != null)
        {
            PGM.OnUpdateTexture(null);
        }
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
