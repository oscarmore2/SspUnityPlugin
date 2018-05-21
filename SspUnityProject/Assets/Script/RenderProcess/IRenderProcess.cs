using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IRenderProcess  {

    protected List<IResourceRenderer> ResourceOverlays = new List<IResourceRenderer>();

    protected RenderTexture renderOutput;

	public Action ProcessBegin;

	public Action ProcessEnd;

	public Texture PrecessResult;

    public abstract void DoRenderProcess();
}
