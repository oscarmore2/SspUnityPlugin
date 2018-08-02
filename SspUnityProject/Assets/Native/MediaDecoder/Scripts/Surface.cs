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

        public Surface(MediaDecoder d, Rect rect)
        {
            decoder = d;
        }
    }
}