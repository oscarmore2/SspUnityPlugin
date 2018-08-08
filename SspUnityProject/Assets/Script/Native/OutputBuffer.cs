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

    public static OutputConfig BuildConfig(int width, int height, int fps, string path)
    {
        var conf = new OutputConfig();
        conf.Width = width;
        conf.Height = height;
        conf.FPS = fps;
        conf.OutputPath = path;
        return conf;
    }
}

public class OutputBuffer : Singleton<OutputBuffer>, IIniConfigable {

    //RenderTexture buffer;
    Material bufferMaterial;
    public Configuration config { get; private set; }
    public OutputConfig OutputConf { get; private set; }
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
        OutputConf = OutputConfig.BuildConfig(width, height, frame, config["Output"]["outputUrl"].ToString());
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
