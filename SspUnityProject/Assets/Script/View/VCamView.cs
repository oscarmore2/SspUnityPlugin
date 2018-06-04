using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCamView : IView {

    void Awake()
    {
        ViewImage = GetComponent<RawImage>();
    }

    protected override void OnHided()
    {

    }

    protected override void OnShown()
    {

    }

    public override void OnUpdateTexture(Texture tex)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
