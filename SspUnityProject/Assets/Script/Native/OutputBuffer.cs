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
        OutputConf = OutputConfig.BuildConfig(width, height, frame, config["Output"]["outputUrl"].ToString());
        encoder = gameObject.AddComponent<MediaEncoder>();
        encoder.videoCaptureType = NativeEncoder.VIDEO_CAPTURE_TYPE.LIVE;
        encoder.liveVideoWidth = width;
        encoder.liveVideoHeight = height;
        encoder.liveVideoBitRate = 400000;
        encoder.liveVideoFrameRate = OutputConf.FPS;
        encoder.liveStreamUrl = OutputConf.OutputPath;
    }

	public void SetConfig()
	{
        config["Output"]["width"] = 3840.ToString();
        config["Output"]["height"] = 1920.ToString();
        config.Parser.SaveFile(Path.Combine(Application.streamingAssetsPath, "/config/StreamSetting.ini"), config);
    }


    bool isPushing = false;
    public void StartPush(Texture outputBuffer)
    {
        isPushing = true;
        StartCoroutine(Pushing(outputBuffer));
        encoder.StartLiveStreaming(OutputConf.OutputPath);
    }

    IEnumerator Pushing(Texture outputBuffer)
    {
        yield return new WaitForEndOfFrame();

        while (isPushing)
        {
            yield return new WaitForEndOfFrame();
            encoder.EncodeFrame((RenderTexture)outputBuffer);
        }
    }

    public void StopPush()
    {
        isPushing = false;
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
