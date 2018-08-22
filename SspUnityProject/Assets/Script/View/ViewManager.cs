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

	Texture tempBuffer;
	Transition transition;
    [SerializeField]
    TransitionCreater currentTransition;
    public void OnTransitionToPGM()
    {
		TransitionManager.Instance.OnTransitionStart = ((Material obj) => {
            tempBuffer = PVW.renderProcessManager.EffectProcess.InputTexture;
            PVW.renderProcessManager.ChangeSurface(PGM.renderProcessManager.EffectProcess.InputTexture);
			obj.SetTexture("_PVW", tempBuffer);
			obj.SetTexture("_PGM", PGM.renderProcessManager.EarlyProcess.ProcessResult);
			PGM.renderProcessManager.TransitionProcess.SetTransition(obj);
		});

		TransitionManager.Instance.OnTransitionEnd = (() => {
			PGM.renderProcessManager.TransitionProcess.ResetMaterial();
			PGM.renderProcessManager.TransitionProcess.DoRenderProcess(PGM.renderProcessManager.EarlyProcess.ProcessResult);
			PGM.renderProcessManager.ChangeSurface(tempBuffer);
			var temp = PGM.renderProcessManager.EarlyProcess.OverlayCamera;
			PGM.renderProcessManager.EarlyProcess.SetOverlay(PVW.renderProcessManager.EarlyProcess.OverlayCamera);
			PVW.renderProcessManager.EarlyProcess.SetOverlay(temp);
		});

		TransitionManager.Instance.Transition (transition);
    }

	public void SetTransition(TransitionCreater tran)
	{
        currentTransition = tran;
		transition = tran.transition;
	}

    public void ExpendCameraView()
    {

    }

    public void ShinkCameraView()
    {

    }
}
