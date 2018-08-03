using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour {

    public PGMView PGM;
    public PVWView PVW;
    public List<VCamView> VCamView = new List<VCamView>();

    public VCamMappingTable MappingTable;

    public void InitView()
    {
        for (int i = 0; i < VcamList.Instance.GetList().Count; i++)
        {
			MappingTable.BindVcam(VcamList.Instance.GetList()[i], VCamView[i], new Rect(1, 1, 1, 1));
        }
    }

    void OnChangeBiding()
    {

    }

    public void OnPushToPVW(IView view)
    {
        VCam cam = null;
        
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
