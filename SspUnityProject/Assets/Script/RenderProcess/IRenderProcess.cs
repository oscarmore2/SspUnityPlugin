using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



public abstract class IRenderProcess : MonoBehaviour  {

    protected List<IResourceRenderer> ResourceOverlays = new List<IResourceRenderer>();

	public Camera renderCamera { get; protected set; }

    protected Texture inputTexture;

    protected Shader processShader;

    protected Material procressMaterial;

	public Action ProcessBegin;

    public WaitUntil Processing;

	public Action ProcessEnd;

	public Texture ProcessResult
	{ get; protected set;}

    /// <summary>
    /// Call this function afer setup shader
    /// </summary>
	public virtual void SetupProcess (Texture inputTex)
	{
        var width = 1920;
        var height = 1080;
        ProcessResult = new RenderTexture (width, height, 0);
        renderCamera.targetTexture = (RenderTexture)ProcessResult;
        renderCamera.aspect = (float)width / (float)height;
        inputTexture = inputTex;
        procressMaterial.mainTexture = inputTexture;
	}
    
    public abstract void DoRenderProcess();
}
