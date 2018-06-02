using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSSP : MonoBehaviour {

    [SerializeField]
    ViewManager cameraManger;

    [SerializeField]
    GameObject buffer0;

    [SerializeField]
    GameObject buffer1;

    // Use this for initialization
    void Start () {
        VcamList.Create();
        cameraManger.InitView();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
