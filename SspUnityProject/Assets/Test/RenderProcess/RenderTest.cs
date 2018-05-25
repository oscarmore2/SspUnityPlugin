using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class RenderTest : MonoBehaviour {

    VideoPlayer player;

    public RawImage img;

    public RawImage OutputImg;

    public Canvas resourceRender;

    RenderProcessManager process;

    public string path;

	// Use this for initialization
	void Start () {
        player = gameObject.AddComponent<VideoPlayer>();
        player.url = path;
        player.playOnAwake = true;
        player.frame = 0;

        player.prepareCompleted += started;

        process = RenderProcessFactory.CreateProcessManager();
        process.CreateBseicRenderProcess();
        player.Prepare();
        player.Play();

        

    }

    void started(VideoPlayer p)
    {
        img.texture = p.texture;
        process.StartRender(player.texture);
        resourceRender.worldCamera = process.PostProcess.renderCamera;
        OutputImg.texture = process.PostProcess.ProcessResult;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
