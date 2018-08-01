using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppInit : Singleton<AppInit> {

    public WaitUntil OnInitInterface;

    public WaitUntil OnLoadConfigs;

    public WaitUntil OnLoadResouce;

    public WaitUntil OnLoadDevice;

	// Use this for initialization
	void Start () {
        AppInit.Create();
    }

    public override void OnInitialize()
    {
        
    }

    IEnumerator StartUpApp()
    {
        yield return new WaitForEndOfFrame();
    }

    public override void OnUninitialize()
    {
        throw new NotImplementedException();
    }
}
