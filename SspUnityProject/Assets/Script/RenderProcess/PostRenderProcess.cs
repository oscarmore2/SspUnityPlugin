using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRenderProcess : CommonRenderProcess
{
    [SerializeField]
    private RenderTexture Overlay;

	public Camera OverlayCamera { get; private set;}
    public override void SetupProcess(Texture inputTex)
    {
        processShader = Shader.Find("RenderProcess/PostProcess");
        GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Renderer"), this.gameObject.transform);
        var mesh = obj.GetComponentInChildren<MeshRenderer>();
        procressMaterial = new Material(processShader);
        mesh.material = procressMaterial;
        renderCamera = obj.GetComponent<Camera>();
        Overlay = new RenderTexture(OutputBuffer.Instance.OutputConf.Width, OutputBuffer.Instance.OutputConf.Height, 0, RenderTextureFormat.ARGB32);
        procressMaterial.SetTexture("_Overlay", inputTex);
        base.SetupProcess(inputTex);
    }

    public void SetOverlay(Camera cam)
    {
		OverlayCamera = cam;
        cam.targetTexture = Overlay;
        procressMaterial.SetTexture("_Overlay", Overlay);
    }

    public override void DoRenderProcess(Texture newTex = null)
    {
        if (null != ProcessBegin)
            ProcessBegin();

        if (newTex != null)
            procressMaterial.mainTexture = newTex;

        if (null != ProcessEnd)
            ProcessEnd();
    }

}
