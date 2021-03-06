﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRenderProcess : CommonRenderProcess
{
	public Texture InputTexture { get; private set;}
    public override void SetupProcess(Texture inputTex)
    {
		InputTexture = inputTex;
        processShader = Shader.Find("RenderProcess/EffectProcess");
        GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Renderer"), this.gameObject.transform);
        var mesh = obj.GetComponentInChildren<MeshRenderer>();
        procressMaterial = new Material(processShader);
        mesh.material = procressMaterial;
        renderCamera = obj.GetComponent<Camera>();
        base.SetupProcess(inputTex);
    }

    public override void DoRenderProcess(Texture newTex = null)
    {
        if (null != ProcessBegin)
            ProcessBegin();

        if (newTex != null)
        {
            InputTexture = newTex;
            procressMaterial.mainTexture = newTex;
        }
            

        if (null != ProcessEnd)
            ProcessEnd();
    }
}
