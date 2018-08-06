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
    public abstract class MediaDecoder : MonoBehaviour
    {
        protected const string LOG_TAG = "[Decoder]";
        [SerializeField]
        protected Texture2D videoTexYch;
        [SerializeField]
        protected Texture2D videoTexUch;
        [SerializeField]
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
            if(null == ytex || null == utex || null == vtex)
            {
                return;
            }
            if (null == resultRT)
            {
                var res  = Resources.Load<CustomRenderTexture>("YUV2RGBA");
                resultRT = CustomRenderTexture.Instantiate(res);
                resultRT.material = new Material(Shader.Find("CustomRenderTexture/RT_YUV2RGBA"));
                resultRT.width = ytex.width;
                resultRT.height = ytex.height;
                resultRT.Initialize();
            }
            if (resultRT != null)
            {
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