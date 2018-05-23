using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRenderProcess : IRenderProcess
{
    public override void SetupProcess(Texture inputTex)
    {
        processShader = Shader.Find("RenderProcess/EffectProcess");
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
