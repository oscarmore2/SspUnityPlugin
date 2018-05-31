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
        var cam1 = VCamFactory.Create("ssp://172.29.1.83", buffer0);
        var cam2 = VCamFactory.Create("ssp://172.29.1.84", buffer1);
        cameraManger.InitView();
        cam1.controller.Start();
        cam2.controller.Start();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
