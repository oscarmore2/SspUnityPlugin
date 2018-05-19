using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VCamTest : MonoBehaviour
{
    public string path = Path.Combine(Application.streamingAssetsPath, "tech.mp4");
    // Use this for initialization
    void Start ()
	{
	    var cam = VCamFactory.Create(path, gameObject);
        cam.controller.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
