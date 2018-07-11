using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPlugin.Decoder;

public abstract class IInputSource
{
	public abstract void Begin();

	public abstract void Pause();

	public abstract void Resume();

	public abstract void End();

	public bool ApplyConfig(InputConfig config)
	{
		foreach (var p in config.All)
		{
			setProperty(p.Key, p.Value);
		}

		return true;
	}

	protected abstract bool setProperty(string name, object value);

	public abstract InputConfig ExtracConfig();
}

public class InputSource : IInputSource
{
	public static readonly string URL_HTTP = "http://";
	public static readonly string URL_SSP = "ssp://";
	public static readonly string URL_FILE = "file://";

	protected MediaDecoder mediaDecoder;

	public InputSource(string url)
	{
		GameObject o = new GameObject("Stream:" + url);
		Debug.Log("init with path " + url);
		if (url.StartsWith(URL_SSP))
		{
			mediaDecoder = o.AddComponent<SspDecoder>();
		}
		else
		{
			mediaDecoder = o.AddComponent<FFmpegDecoder>();
		}
		mediaDecoder.mediaPath = url;
	}

	public override void Begin()
	{
		mediaDecoder.onInitComplete.AddListener(mediaDecoder.startDecoding);
		mediaDecoder.initDecoder(mediaDecoder.mediaPath);
	}

	public override void Pause()
	{
		mediaDecoder.setPause();
	}

	public override void Resume()
	{
		mediaDecoder.setResume();
	}

	public override void End()
	{
		mediaDecoder.stopDecoding();
	}

	protected override bool setProperty(string name, object value)
	{
		throw new System.NotImplementedException();
	}

	public override InputConfig ExtracConfig()
	{
		return new InputConfig();
	}

	public Surface CreateSurface()
	{
		return new Surface(mediaDecoder);
	}
}
