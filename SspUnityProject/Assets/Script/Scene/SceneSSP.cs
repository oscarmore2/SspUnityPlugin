using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        var cam1 = VCamFactory.Create("ssp://192.168.100.1", buffer0);
        cam1.controller.Start();
        var cam2 = VCamFactory.Create("ssp://192.168.100.2", buffer1);
        cam2.controller.Start();
        cameraManger.InitView();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
