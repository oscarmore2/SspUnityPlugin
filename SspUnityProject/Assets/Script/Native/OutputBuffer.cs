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

public class OutputBuffer : MonoBehaviour, IConfigable {

    //RenderTexture buffer;
    Material bufferMaterial;
    public Configuration config { get; private set; }
    MediaEncoder encoder = new MediaEncoder();
    

	public void LoadConfig ()
	{
        if (config == null)
            config = (Configuration)IniFiles.LoadFile(Path.Combine(Application.streamingAssetsPath, "/config/StreamSetting.ini"));
        if (config == null)
        {
            SetDefaultConfig();
            return;
        }
        config.IniReadValue("Output", "outputUrl");
        config.IniReadValue("Output", "width");
        config.IniReadValue("Output", "height");
        config.IniReadValue("Output", "frame");
    }

    void Awake()
    {
        bufferMaterial = new Material(Shader.Find("RenderProcess/Output"));
        encoder = EncoderFactory.InitLiveEncoder(bufferMaterial, NativeEncoder.VIDEO_CAPTURE_TYPE.LIVE, config[(object)"width"], config[(object)"height"], config[(object)"frame"]);
    }

	public void SetConfig()
	{
		
	}

    public void StartPush(Texture outputBuffer)
    {
        encoder.GetComponent<Camera>().enabled = true;
        bufferMaterial.mainTexture = outputBuffer;
        encoder.StartLiveStreaming(config["outputUrl"].ToString());
    }

    public void StopPush()
    {
        encoder.GetComponent<Camera>().enabled = false;
        encoder.StopCapture();
    }

    public void SetDefaultConfig()
    {
        config = new Configuration();
        config[(object)"width"] = 1920;
        config[(object)"height"] = 1080;
        config[(object)"frame"] = 30;
        config["outputUrl"] = "rtmp://172.29.1.13/hls/pushtest2";
    }
}
