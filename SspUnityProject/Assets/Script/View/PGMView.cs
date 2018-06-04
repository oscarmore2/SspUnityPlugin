using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PGMView : IView {
    OutputBuffer outputBuffer;

    [SerializeField]
    RawImage  _pgmBuffer;

    [SerializeField]
    UnityPlugin.Encoder.MediaEncoder _encoder;

    Texture pgmBuffer;
    RenderProcessManager renderProcessManager;

    public InputField address;

    void Awake()
    {
        outputBuffer = gameObject.AddComponent<OutputBuffer>();
        outputBuffer.encoder = _encoder;
        renderProcessManager = RenderProcessFactory.CreateProcessManager(Vector3.left * 500);
        renderProcessManager.transform.parent = transform;
        renderProcessManager.CreateBseicRenderProcess();
        ViewImage = GetComponent<RawImage>();
    }

    public void StartPush()
    {
        outputBuffer.StartPush(pgmBuffer, address.text);
        _pgmBuffer.material = ViewImage.material;
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
