using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VCamTest : MonoBehaviour
{
    // Use this for initialization
    void Start ()
	{
        VcamList.Create();
	    var cam = VCamFactory.Create(Path.Combine(Application.streamingAssetsPath, "tech.mp4"), gameObject);
        cam.controller.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
