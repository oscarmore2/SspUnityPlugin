using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IRenderProcess : MonoBehaviour {

    protected List<IResourceRenderer> ResourceOverlays = new List<IResourceRenderer>();

    protected RenderTexture renderOutput;

    public abstract void DoRenderProcess();
}
