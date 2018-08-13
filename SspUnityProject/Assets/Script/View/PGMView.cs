using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PGMView : IView {
    OutputBuffer outputBuffer;
    Texture pgmBuffer;
    RenderProcessManager renderProcessManager;

    void Awake()
    {
        //outputBuffer = gameObject.AddComponent<OutputBuffer>();
        //outputBuffer.InitFromConfig();
        //renderProcessManager = RenderProcessFactory.CreateProcessManager(Vector3.left * 500);
        //renderProcessManager.transform.parent = transform;
        ViewImage = GetComponent<RawImage>();
    }

    public void StartPush()
    {
        outputBuffer.StartPush(pgmBuffer);
    }

    public void AttachUILayer()
    {
        renderProcessManager.EarlyProcess.SetOverlay(ResourceDisplayList.Instance.PGMPreRenderPipeLineCamera.targetTexture);
        renderProcessManager.PostProcess.SetOverlay(ResourceDisplayList.Instance.PGMPostRenderPipeLineCamera.targetTexture);
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
