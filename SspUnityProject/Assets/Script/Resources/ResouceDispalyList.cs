using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplayList : Singleton<ResourceDisplayList>
{
    Canvas PVWPreRenderPipeLine;
    Canvas PGMPreRenderPipeLine;

    Canvas PVWPostRenderPipeLine;
    Canvas PGMPostRenderPipeLine;

    public Camera PVWPreRenderPipeLineCamera;
    public Camera PGMPreRenderPipeLineCamera;

    public Camera PVWPostRenderPipeLineCamera;
    public Camera PGMPostRenderPipeLineCamera;

    public override void OnInitialize()
    {
        this.initProcess("PVWPreRenderPipeLine", ref PVWPreRenderPipeLine, ref PVWPreRenderPipeLineCamera);
        this.initProcess("PGMPreRenderPipeLine", ref PGMPreRenderPipeLine, ref PGMPreRenderPipeLineCamera);

        this.initProcess("PGMPostRenderPipeLine", ref PVWPostRenderPipeLine, ref PVWPostRenderPipeLineCamera);
        this.initProcess("PGMPostRenderPipeLine", ref PGMPostRenderPipeLine, ref PGMPostRenderPipeLineCamera);
    }

    void initProcess(string name, ref Canvas canvas, ref Camera cam)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        canvas = obj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        var scaler = obj.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.referenceResolution = new Vector2(OutputBuffer.Instance.OutputConf.Width, OutputBuffer.Instance.OutputConf.Height);
        cam = obj.AddComponent<Camera>();
        canvas.worldCamera = cam;
        cam.aspect = 1.77f;
    }

    public override void OnUninitialize()
    {
        
    }
}
