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
	public void Init () {
        StartCoroutine(StartUpApp());
    }

    public override void OnInitialize()
    {
        OnInitInterface += new Func<bool>(() =>
        {
            Resource.ResourceGroupList.Create();
            GroupTraveller.Create();
            RendererContainor.Create();
            VcamList.Create();
            RendererContainor.Create();
            GroupTraveller.Create();
            OutputBuffer.Create();
            ResourceDisplayList.Create();
			TransitionManager.Create();
            return true;
        });


        //OnLoadResouce += new Func<bool>(() => {
        //    return true;
        //});

        OnSetUpRenderer += new Func<bool>(() => {
            return true;
        });

        OnLoadDevice += new Func<bool>(() => {
            VcamList.Instance.LoadConfig();
            OutputBuffer.Instance.InitFromConfig();
            ResourceDisplayList.Instance.InitProcess();
            GroupTraveller.Instance.OnInit();
            return true;
        });
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
