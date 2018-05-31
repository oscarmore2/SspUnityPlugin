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
        outputBuffer = gameObject.AddComponent<OutputBuffer>();
        renderProcessManager = RenderProcessFactory.CreateProcessManager(Vector3.left * 500);
        ViewImage = GetComponent<RawImage>();
    }

    public void StartPush()
    {
        outputBuffer.StartPush(pgmBuffer);
    }

    void Update()
    {
        if (ViewImage.mainTexture && !pgmBuffer)
        {
            renderProcessManager.StartRender(ViewImage.mainTexture);
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
