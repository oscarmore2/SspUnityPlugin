using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPlugin.Decoder;

public abstract class InputSource
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

    public Action<Texture, Texture, Texture> OnSetTextures = null;

    protected void HandleTextures(Texture y, Texture u, Texture v)
    {
        if (OnSetTextures != null)
        {
            OnSetTextures(y, u, v);
        }
    }
    public abstract Texture GetResult();
}

public class InputSourceSsp : InputSource
{
    #region implemented abstract members of InputSource

    public override Texture GetResult()
    {
        return mediaDecoder.GetResult();
    }

    #endregion

    protected SspDecoder mediaDecoder;

    public InputSourceSsp(string url)
    {
        GameObject o = new GameObject("Stream:" + url);
        mediaDecoder = o.AddComponent<SspDecoder>();
        mediaDecoder.onSetTexture = HandleTextures;
        //o.hideFlags = HideFlags.HideAndDontSave;
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

}

public class InputSourceStream :InputSource
{
    #region implemented abstract members of InputSource

    public override Texture GetResult()
    {
        return mediaDecoder.GetResult();
    }

    #endregion

    protected MediaDecoder mediaDecoder;

    public InputSourceStream(string url)
    {
        GameObject  o = new GameObject("Stream:"+url);
        mediaDecoder = o.AddComponent<MediaDecoder>();
        mediaDecoder.onSetTexture = HandleTextures;
        //o.hideFlags = HideFlags.HideAndDontSave;
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
}
