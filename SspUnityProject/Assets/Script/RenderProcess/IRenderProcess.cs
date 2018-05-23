using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IRenderProcess  {

    protected List<IResourceRenderer> ResourceOverlays = new List<IResourceRenderer>();

	protected Camera renderCamera;

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
		var width = OutputBuffer.Instance.Config.Width;
		var height = OutputBuffer.Instance.Config.Height;
        ProcessResult = new RenderTexture (height, width, 0);
        GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Renderer"), Vector3.zero, Quaternion.identity, GameObject.Find("RenderProcess").transform);
        renderCamera = obj.GetComponent<Camera> ();
        renderCamera.targetTexture = (RenderTexture)ProcessResult;
        renderCamera.aspect = (float)width / (float)height;
        inputTexture = inputTex;
        procressMaterial.mainTexture = inputTexture;
	}

    public abstract IEnumerator DoRenderProcess();
}
