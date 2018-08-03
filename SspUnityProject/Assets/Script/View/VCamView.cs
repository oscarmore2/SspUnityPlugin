using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPlugin.Decoder;

public class VCamView : IView {

    Surface surface;

    protected override void OnHided()
    {

    }

    protected override void OnShown()
    {

    }

    public void SetSurface(Surface _surface)
    {
        surface = _surface;
    }

    public override void OnUpdateTexture(Texture tex)
    {
        SetImage(tex);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (null != surface)
        {
            OnUpdateTexture(surface.GetTexture());
        }
	}

    
}
