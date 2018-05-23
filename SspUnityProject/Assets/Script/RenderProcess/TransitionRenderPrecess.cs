using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransitionRenderPrecess : IRenderProcess
{
    public override void SetupProcess(Texture inputTex)
    {
        processShader = Shader.Find("RenderProcess/TransitionProcess");
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
