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

    public override void OnUpdateTexture(Texture tex)
    {
        if (!pgmBuffer)
        {
            renderProcessManager.StartRender(tex);
            pgmBuffer = renderProcessManager.PostProcess.ProcessResult;
        }
    }

    protected override void OnHided()
    {
        
    }

    protected override void OnShown()
    {
        
    }
}
