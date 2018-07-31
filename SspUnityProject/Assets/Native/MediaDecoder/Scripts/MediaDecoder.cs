using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityPlugin.Decoder
{
    public class Surface
    {
        protected MediaDecoder decoder = null;
        protected Rect rect = new Rect(0, 0, 1, 1);

        public Texture GetTexture()
        {
            return decoder.GetResult();
        }

        public Rect GetRect()
        {
            return rect;
        }

        public void SetRect(Rect r)
        {
            rect = r;
        }

        public Surface(MediaDecoder d)
        {
            decoder = d;
        }
    }

    public abstract class MediaDecoder : MonoBehaviour
    {
        protected const string LOG_TAG = "[Decoder]";
        protected Texture2D videoTexYch;
        protected Texture2D videoTexUch;
        protected Texture2D videoTexVch;
        protected int videoWidth = -1;
        protected int videoHeight = -1;
        public string mediaPath;
        public DecoderNative.DecoderState decoderState = DecoderNative.DecoderState.NOT_INITIALIZED;
        public UnityEvent onInitComplete = new UnityEvent();

        public UnityEvent onVideoEnd = new UnityEvent();

        public CustomRenderTexture resultRT;

        protected void setTextures(Texture ytex, Texture utex, Texture vtex)
        {
            if (null == resultRT && ytex != null)
            {
                resultRT = new CustomRenderTexture(ytex.width, ytex.height);
                resultRT.Create();
            }
            if (resultRT)
            {
                if (null == resultRT.material)
                {
                    resultRT.material = new Material(Shader.Find("CustomRenderTexture/RT_YUV2RGBA"));
                }
                resultRT.material.SetTexture("_YTex", ytex);
                resultRT.material.SetTexture("_UTex", utex);
                resultRT.material.SetTexture("_VTex", vtex);
            }
        }

        public virtual Texture GetResult()
        {
            return resultRT;
        }

        public abstract void setResume();

        public abstract void setPause();

        public abstract void startDecoding();

        public abstract void initDecoder(string path);

        public abstract void stopDecoding();
    }

}