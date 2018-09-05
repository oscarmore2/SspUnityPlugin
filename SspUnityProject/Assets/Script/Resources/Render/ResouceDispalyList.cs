using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplayList : Singleton<ResourceDisplayList>
{
    Canvas PVWPreRenderPipeLine;
    Canvas PGMPreRenderPipeLine;

    Canvas PVWPostRenderPipeLine;
    Canvas PGMPostRenderPipeLine;

    public Camera PVWPreRenderPipeLineCamera;
    public Camera PGMPreRenderPipeLineCamera;

    public Camera PVWPostRenderPipeLineCamera;
    public Camera PGMPostRenderPipeLineCamera;

    public Transform PGMPreRender;
    public Transform PGMPostRender;
    public Transform PVWPreRender;
    public Transform PVWPostRender;

    public override void OnInitialize()
    {
        
    }

    public void InitProcess()
    {
        this.initProcess("PVWPreRenderPipeLine", ref PVWPreRenderPipeLine, ref PVWPreRenderPipeLineCamera);
        PVWPreRender = PVWPreRenderPipeLine.transform;
        this.initProcess("PGMPreRenderPipeLine", ref PGMPreRenderPipeLine, ref PGMPreRenderPipeLineCamera);
        PGMPreRender = PGMPreRenderPipeLine.transform;

        this.initProcess("PGMPostRenderPipeLine", ref PVWPostRenderPipeLine, ref PVWPostRenderPipeLineCamera);
        PVWPostRender = PVWPostRenderPipeLine.transform;
        this.initProcess("PGMPostRenderPipeLine", ref PGMPostRenderPipeLine, ref PGMPostRenderPipeLineCamera);
        PGMPostRender = PGMPostRenderPipeLine.transform;
    }

    void initProcess(string name, ref Canvas canvas, ref Camera cam)
    {
        
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        cam = obj.AddComponent<Camera>();
        cam.orthographic = true;
        //cam.clearFlags = CameraClearFlags.Nothing;
        
        cam.aspect = 1.77f;

        GameObject objChild = new GameObject(name);
        objChild.transform.parent = obj.transform;
        objChild.transform.localPosition = Vector3.forward;
        canvas = objChild.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        var scaler = objChild.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(OutputBuffer.Instance.OutputConf.Width, OutputBuffer.Instance.OutputConf.Height);
    }

	public void ReFlushComponent(CommonResourceRenderer renderer, Resource.ResourceGroup rg, Transform trans)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            var target = trans.GetChild(i).GetComponent<ResourceRenderTarget>();
            if (target != null)
            {
				if (target.renderer == renderer && target.resourceGroup == rg)
                    Destroy(trans.GetChild(i).gameObject);
            }
            
        }
    }

    public void ClearComponent()
    {
        for (int i = 0; i < PVWPreRender.childCount; i++)
        {
            Destroy(PVWPreRender.GetChild(i).gameObject);
        }
        for (int i = 0; i < PGMPostRender.childCount; i++)
        {
            Destroy(PGMPostRender.GetChild(i).gameObject);
        }
        for (int i = 0; i < PVWPostRender.childCount; i++)
        {
            Destroy(PVWPostRender.GetChild(i).gameObject);
        }
        for (int i = 0; i < PVWPostRender.childCount; i++)
        {
            Destroy(PVWPostRender.GetChild(i).gameObject);
        }
    }


    public override void OnUninitialize()
    {
        
    }
}
