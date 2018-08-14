using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRenderTarget : MonoBehaviour{

    public int Prioirty;
    public IResourceRenderer renderer;

    public static ResourceRenderTarget Create(GameObject parent, int _priority)
    {
        var obj = new GameObject("render target");
        var comonent = obj.AddComponent<ResourceRenderTarget>();
        comonent.transform.parent = parent.transform;
        comonent.Prioirty = _priority;
        comonent.transform.SetSiblingIndex(_priority);
        return comonent;
    }
}
