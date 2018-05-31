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
        ViewImage = GetComponent<RawImage>();
    }

    public void OnUpdate()
    {
        if (ViewImage.mainTexture && !pvwBuffer)
        {

            renderProcessManager.StartRender(ViewImage.mainTexture);
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
