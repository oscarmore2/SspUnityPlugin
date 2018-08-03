using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneSSP : MonoBehaviour {

    [SerializeField]
    Resource.ResourcesManager resourceManager;
    [SerializeField]
    Resource.ResourceGroupManager resourceGroupManager;

    [SerializeField]
    ViewManager viewManager;

    // Use this for initialization
    void Awake () {
        AppInit.Create();
        

        AppInit.Instance.OnLoadResouce += new Func<bool>(() => {
            resourceManager.Init();
            resourceGroupManager.OnInit(resourceManager);
            return true;
        });

        AppInit.Instance.OnLoadDevice += new Func<bool>(() => {
            viewManager.InitView();
            return true;
        });

        AppInit.Instance.Init();
    }
}
