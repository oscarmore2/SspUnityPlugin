using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PGMView : IView {
    OutputBuffer outputBuffer;
    Texture pgmBuffer;
    public RenderProcessManager renderProcessManager { get; private set; }

    bool isPushing;
    void Awake()
    {
        //outputBuffer = gameObject.AddComponent<OutputBuffer>();
        //outputBuffer.InitFromConfig();
        //renderProcessManager = RenderProcessFactory.CreateProcessManager(Vector3.left * 500);
        //renderProcessManager.transform.parent = transform;
        renderProcessManager = GetComponent<RenderProcessManager>();
        ViewImage = GetComponent<RawImage>();
    }

    public override void InitView(ViewManager _manager)
    {
        manager = _manager;
        renderProcessManager.CreateBseicRenderProcess();
        renderProcessManager.StartRender(manager.DefaultImg);
        pgmBuffer = renderProcessManager.PostProcess.ProcessResult;
        ViewImage.texture = pgmBuffer;
        AttachUILayer();
    }

    public void TogglePush()
    {
        if (!isPushing)
            outputBuffer.StartPush(pgmBuffer);
        else
            outputBuffer.StopPush();
    }

    public void AttachUILayer()
    {
        renderProcessManager.EarlyProcess.SetOverlay(ResourceDisplayList.Instance.PGMPreRenderPipeLineCamera);
        renderProcessManager.PostProcess.SetOverlay(ResourceDisplayList.Instance.PGMPostRenderPipeLineCamera);
    }

    public override void OnUpdateTexture(Texture tex)
    {
        renderProcessManager.StopRender();
        renderProcessManager.ChangeSurface(tex);
        pgmBuffer = renderProcessManager.PostProcess.ProcessResult;
    }

    protected override void OnHided()
    {
        
    }

    protected override void OnShown()
    {
        
    }
}
