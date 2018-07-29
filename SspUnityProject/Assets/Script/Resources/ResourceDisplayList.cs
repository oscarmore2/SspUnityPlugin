using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;

public class ResourceDisplayList : Singleton<ResourceDisplayList> {

    public List<IResource> PreviewPreRenderList = new List<IResource>();
    public List<IResource> PreviewPostRenderList = new List<IResource>();

    public List<IResource> PGMPreRenderList = new List<IResource>();
    public List<IResource> PGMPostRenderList = new List<IResource>();

    public override void OnInitialize()
    {
        throw new NotImplementedException();
    }

    public override void OnUninitialize()
    {
        throw new NotImplementedException();
    }
}
