using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRenderProcess : CommonRenderProcess
{
    private Texture Overlay;
    public override void SetupProcess(Texture inputTex)
    {
        processShader = Shader.Find("RenderProcess/PostProcess");
        GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Renderer"), this.gameObject.transform);
        var mesh = obj.GetComponentInChildren<MeshRenderer>();
        procressMaterial = new Material(processShader);
        mesh.material = procressMaterial;
        renderCamera = obj.GetComponent<Camera>();
        base.SetupProcess(inputTex);
    }

    public void SetOverlay(Texture txd)
    {
        Overlay = txd;
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
