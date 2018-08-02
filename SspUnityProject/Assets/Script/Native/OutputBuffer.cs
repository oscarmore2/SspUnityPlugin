using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityPlugin.Encoder;

public class OutputConfig
{
	public int Width;
	public int Height;
	public int FPS;
	public string OutputPath;
}

public class OutputBuffer : Singleton<OutputBuffer>, IConfigable {

    //RenderTexture buffer;
    Material bufferMaterial;
    public Configuration config { get; private set; }
    MediaEncoder encoder = new MediaEncoder();
    

	public void LoadConfig ()
	{ 
        if (config == null)
			config = new Configuration(Path.Combine(Application.streamingAssetsPath, "config/StreamSetting.ini"));
        if (config == null)
        {
            SetDefaultConfig();
            return;
        }
    }

    public void InitFromConfig()
    {
        LoadConfig();
        bufferMaterial = new Material(Shader.Find("RenderProcess/Output"));
        int width = int.Parse(config["Output"]["width"]);
        int height = int.Parse(config["Output"]["height"]);
        int frame = int.Parse(config["Output"]["frame"]);
        encoder = EncoderFactory.InitLiveEncoder(bufferMaterial, NativeEncoder.VIDEO_CAPTURE_TYPE.LIVE, width, height, frame);
    }

	public void SetConfig()
	{
        config["Output"]["width"] = 3840.ToString();
        config["Output"]["height"] = 1920.ToString();
        config.Parser.SaveFile(Path.Combine(Application.streamingAssetsPath, "/config/StreamSetting.ini"), config);
    }

    public void StartPush(Texture outputBuffer)
    {
        encoder.EncodeFrame((RenderTexture)outputBuffer);
        encoder.StartLiveStreaming(config["Output"]["outputUrl"].ToString());
    }

    public void StopPush()
    {
        encoder.GetComponent<Camera>().enabled = false;
        encoder.StopCapture();
    }

    public void SetDefaultConfig()
    {
        
    }

    public override void OnInitialize()
    {
        
    }

    public override void OnUninitialize()
    {
        
    }
}
