using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour {

    public PGMView PGM;
    public PVWView PVW;
    public List<VCamView> Vcams = new List<VCamView>();

    public VCamMappingTable MappingTable;

    public void InitView()
    {
        MappingTable.BindVcam(VcamList.Instance.GetList()[0], PGM);
        MappingTable.BindVcam(VcamList.Instance.GetList()[1], PVW);
        for (int i = 0; i < VcamList.Instance.GetList().Count; i++)
        {
            MappingTable.BindVcam(VcamList.Instance.GetList()[i], Vcams[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnChangeBiding()
    {

    }

    public void OnPushToPVW(IView view)
    {
        VCam cam = null;
        if (MappingTable.GetVcamByView(view, ref cam))
        {
            MappingTable.BindVcam(cam, PVW);
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
