using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPlugin.Encoder;

public class OutputConfig
{
	public int Width;
	public int Height;
	public int FPS;
	public string OutputPath;
}

public class OutputBuffer : MonoBehaviour, IConfigable {

    //RenderTexture buffer;
    Material bufferMaterial;
    public Configuration config { get; private set; }
    MediaEncoder encoder = new MediaEncoder();
    

	public void LoadConfig (object config)
	{
        if (config == null)
        {
            SetDefaultConfig();
            return;
        }
    }

    void Awake()
    {
        LoadConfig(null);
        bufferMaterial = new Material(Shader.Find("RenderProcess/Output"));
        encoder = EncoderFactory.InitLiveEncoder(bufferMaterial, NativeEncoder.VIDEO_CAPTURE_TYPE.LIVE, config["width"].ToInt(), config["height"].ToInt(), config["frame"].ToInt());
    }

	public void SetConfig()
	{
		
	}

    public void StartPush(Texture outputBuffer)
    {
        bufferMaterial.mainTexture = outputBuffer;
        encoder.StartLiveStreaming(config["outputUrl"].ToString());
    }

    public void StopPush()
    {
        encoder.StopCapture();
    }

    public void SetDefaultConfig()
    {
        config["width"] = 1920;
        config["height"] = 1080;
        config["frame"] = 30;
        config["outputUrl"] = "rtmp://172.29.1.13/hls/pushtest2";
    }
}
