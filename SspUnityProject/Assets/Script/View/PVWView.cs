using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVWView : IView {
    Texture pvwBuffer;
    RenderProcessManager renderProcessManager;

    void Awake()
    {
        renderProcessManager = RenderProcessFactory.CreateProcessManager(Vector3.zero);
        renderProcessManager.transform.parent = transform;
        renderProcessManager.CreateBseicRenderProcess();
        ViewImage = GetComponent<RawImage>();
    }

    public override void OnUpdateTexture(Texture tex)
    {
        if (!pvwBuffer)
        {
            renderProcessManager.StartRender(tex);
            pvwBuffer = renderProcessManager.PostProcess.ProcessResult;
        }
    }

    protected override void OnHided()
    {

    }

    protected override void OnShown()
    {

    }
}
