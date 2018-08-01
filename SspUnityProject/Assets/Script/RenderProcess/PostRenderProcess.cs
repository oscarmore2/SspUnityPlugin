using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRenderProcess : CommonRenderProcess
{
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

    public override void DoRenderProcess()
    {
        if (null != ProcessBegin)
            ProcessBegin();

        if (null != ProcessEnd)
            ProcessEnd();
    }

}
