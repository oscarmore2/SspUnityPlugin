using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resource;

public class resourceGroupTest : MonoBehaviour {

    public ResourcesManager ResManager;
    public ResourceGroupManager ResGroupManager;

	// Use this for initialization
	void Start () {
        ResourcesManager.Create();
        ResourceGroupList.Create();
        ResGroupManager.OnInit(ResourcesManager.Instance);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
