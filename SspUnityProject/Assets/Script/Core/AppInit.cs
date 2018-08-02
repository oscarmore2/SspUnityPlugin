using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;

public class AppInit : Singleton<AppInit> {

    public event Func<bool> OnInitInterface;

    public event Func<bool> OnLoadResouce;

    public event Func<bool> OnSetUpRenderer;

    public event Func<bool> OnLoadDevice;

	// Use this for initialization
	void Start () {
        OnInitInterface += new Func<bool>(() =>
        {
            Resource.ResourcesManager.Create();
            Resource.ResourceGroupManager.Create();
            Resource.ResourceGroupList.Create();
            GroupTraveller.Create();
            RendererContainor.Create();
            VcamList.Create();
            RenderProcessManager.Create();
            OutputBuffer.Create();
            return true;
        });


        OnLoadResouce += new Func<bool>(()=>{
            Resource.ResourcesManager.Instance.Init();
            Resource.ResourceGroupManager.Instance.OnInit(Resource.ResourcesManager.Instance);
            return true;
        });

        OnSetUpRenderer += new Func<bool>(()=> {
            RenderProcessManager.Instance.CreateBseicRenderProcess();
            return true;
        });

        OnLoadDevice += new Func<bool>(() => {
            VcamList.Instance.LoadConfig();
            OutputBuffer.Instance.LoadConfig();
            return true;
        });
    }

    public override void OnInitialize()
    {
        StartCoroutine(StartUpApp());
    }

    IEnumerator StartUpApp()
    {
        yield return new WaitForEndOfFrame();

        if (OnInitInterface.GetInvocationList().Length > 0)
        {
            WaitUntil init = new WaitUntil(OnInitInterface);
            yield return init;
        }

        if (OnLoadResouce.GetInvocationList().Length > 0)
        {
            WaitUntil init = new WaitUntil(OnLoadResouce);
            yield return init;
        }

        if (OnSetUpRenderer.GetInvocationList().Length > 0)
        {
            WaitUntil init = new WaitUntil(OnSetUpRenderer);
            yield return init;
        }

        if (OnLoadDevice.GetInvocationList().Length > 0)
        {
            WaitUntil init = new WaitUntil(OnLoadDevice);
            yield return init;
        }
    }

    public override void OnUninitialize()
    {
        OnInitInterface = null;
        OnLoadResouce = null;
        OnLoadDevice = null;
        OnSetUpRenderer = null;
    }
}
