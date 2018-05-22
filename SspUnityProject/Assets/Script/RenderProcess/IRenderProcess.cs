using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IRenderProcess  {

    protected List<IResourceRenderer> ResourceOverlays = new List<IResourceRenderer>();

	protected Camera renderCamera;

	public Action ProcessBegin;

	public Action ProcessEnd;

	public Texture ProcessResult
	{ get; protected set;}

	public virtual IRenderProcess CreateRenderProcess ()
	{
		var width = OutputBuffer.Instance.Config.Width;
		var height = OutputBuffer.Instance.Config.Height;
		ProcessResult = new RenderTexture (height, width, 0);
		GameObject obj = new GameObject ("Renderer");
		renderCamera = obj.AddComponent<Camera> ();
		renderCamera.targetTexture = ProcessResult;
		renderCamera.aspect = (float)width / (float)height;
	}

    public abstract void DoRenderProcess();
}
