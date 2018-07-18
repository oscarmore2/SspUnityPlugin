using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResTest : MonoBehaviour {

    [SerializeField]
    ResourcesManager manager;

	// Use this for initialization
	void Start () {
        manager.OnInit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
