using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRenderProcess : IRenderProcess
{
    public override void SetupProcess(Texture inputTex)
    {
        processShader = Shader.Find("RenderProcess/PostProcess");
        procressMaterial.shader = processShader;
        base.SetupProcess(inputTex);
    }

    public override IEnumerator DoRenderProcess()
    {
		this.ProcessBegin ();
        yield return new WaitForEndOfFrame();
		this.ProcessEnd ();
    }

}
