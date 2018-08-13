using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVWView : IView {
    Texture pvwBuffer;

    public RenderProcessManager renderProcessManager { get; private set; }

    void Awake()
    {
        renderProcessManager = RenderProcessFactory.CreateProcessManager(Vector3.zero);
        renderProcessManager.transform.parent = transform;
        renderProcessManager = GetComponent<RenderProcessManager>();
        ViewImage = GetComponent<RawImage>();
    }

    public override void OnUpdateTexture(Texture tex)
    {
        renderProcessManager.StopRender();
        renderProcessManager.ChangeSurface(tex);
        pvwBuffer = renderProcessManager.PostProcess.ProcessResult;
    }

    public void AttachUILayer()
    {
        renderProcessManager.EarlyProcess.SetOverlay(ResourceDisplayList.Instance.PVWPreRenderPipeLineCamera.targetTexture);
        renderProcessManager.PostProcess.SetOverlay(ResourceDisplayList.Instance.PVWPostRenderPipeLineCamera.targetTexture);
    }

    protected override void OnHided()
    {

    }

    protected override void OnShown()
    {

    }
}
