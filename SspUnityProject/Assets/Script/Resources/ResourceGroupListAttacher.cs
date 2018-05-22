using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourcesListContainor<ResourceGroup>))]
public class ResourceGroupListAttacher : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        gameObject.AddComponent<ResourcesListContainor<ResourceGroup>>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
