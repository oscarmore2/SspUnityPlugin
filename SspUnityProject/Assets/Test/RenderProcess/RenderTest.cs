using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class RenderTest : MonoBehaviour {

    VideoPlayer player;

    public RawImage img;

    public Canvas resourceRender;

    public string path;

	// Use this for initialization
	void Start () {
        player = gameObject.AddComponent<VideoPlayer>();
        player.url = path;
        player.playOnAwake = true;
        player.frame = 0;

        player.prepareCompleted += started;

        RenderProcessManager.Create();
        player.Prepare();
        player.Play();

        

    }

    void started(VideoPlayer p)
    {
        img.texture = p.texture;
        RenderProcessManager.Instance.StartRender(player.texture);
        resourceRender.worldCamera = RenderProcessManager.Instance.PostProcess.renderCamera;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
