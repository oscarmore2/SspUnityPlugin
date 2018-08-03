using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPlugin.Decoder;

public abstract class IInputSource
{
	public static readonly ConfigProperty<float> Exposure = new ConfigProperty<float>("Exposure", 1f);
    public static readonly ConfigProperty<float> Temperature = new ConfigProperty<float>("Temperature", 1f);

	public abstract void Begin();

	public abstract void Pause();

	public abstract void Resume();

	public abstract void End();

	public abstract Surface CreateSurface(Rect rect);

}

public class InputSource : IInputSource
{
	public static readonly string URL_HTTP = "http://";
	public static readonly string URL_SSP = "ssp://";
	public static readonly string URL_FILE = "file://";
	public static readonly string URL_DECKLINK = "decklink://";

	protected MediaDecoder mediaDecoder;

	public static Type FindDecoderType(string url) 
	{
		if (url.StartsWith(URL_SSP))
		{
			return typeof(SspDecoder);
		}
		else if (url.StartsWith(URL_DECKLINK)) 
		{
			return typeof(DeckLinkDecoder);
		}
		else
		{
			return typeof(FFmpegDecoder);
		}
	}
	public InputSource(string url)
	{
		GameObject o = new GameObject("Stream:" + url);
		Type decoderType = FindDecoderType(url);
        mediaDecoder = (MediaDecoder)o.AddComponent(decoderType);
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

	public override Surface CreateSurface(Rect rect)
	{
		return new Surface(mediaDecoder, rect);
	}
}
