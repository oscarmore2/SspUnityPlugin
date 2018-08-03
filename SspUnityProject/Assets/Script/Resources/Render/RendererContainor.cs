using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererContainor : Singleton<RendererContainor> {

	Dictionary<string, IResourceRenderer> RenderPathMapping = new Dictionary<string, IResourceRenderer>();

    public override void OnInitialize()
    {
        
    }

    public override void OnUninitialize()
    {
        
    }

    
}
