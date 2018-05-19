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


}

public class InputSourceSsp : InputSource
{
    protected SspDecoder mediaDecoder;

    public InputSourceSsp(string url,GameObject o)
    {
        if(null == o)
         o = new GameObject("Stream:" + url);
        mediaDecoder = o.AddComponent<SspDecoder>();
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
    protected MediaDecoder mediaDecoder;

    public InputSourceStream(string url,GameObject o)
    {
        if (o == null)
          o = new GameObject("Stream:"+url);
        mediaDecoder = o.AddComponent<MediaDecoder>();
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
