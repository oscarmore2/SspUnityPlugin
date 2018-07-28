using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;

public class resourceGroupTest : MonoBehaviour {

    public ResourcesManager ResManager;
    public ResourceGroupManager ResGroupManager;

	// Use this for initialization
	void Start () {
        ResManager.OnInit();
        ResourceGroupList.Create();
        ResGroupManager.OnInit(ResManager);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
