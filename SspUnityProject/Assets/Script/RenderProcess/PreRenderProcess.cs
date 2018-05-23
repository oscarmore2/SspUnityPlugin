using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreRenderProcess : IRenderProcess
{
	public override void SetupProcess(Texture inputTex)
	{
        processShader = Shader.Find("RenderProcess/PreProcess");
        procressMaterial.shader = processShader;
        base.SetupProcess(inputTex);
    }

    public override IEnumerator DoRenderProcess()
    {
        ProcessBegin();
        yield return 1;
        ProcessEnd();
    }
}
