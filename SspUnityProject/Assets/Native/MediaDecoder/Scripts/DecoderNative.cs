
using System;
using System.Runtime.InteropServices;
using UnityEngine;
namespace UnityPlugin.Decoder
{
    public class DecoderNative
    {
        public enum DecoderState
        {
            INIT_FAIL = -2,
            STOP,
            NOT_INITIALIZED,
            INITIALIZING,
            INITIALIZED,
            START,
            PAUSE,
            SEEK_FRAME,
            BUFFERING,
            EOF
        }

        public const string NATIVE_LIBRARY_NAME = "NativeDecoder";


        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern int nativeCreateDecoder(string filePath, ref int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern int nativeCreateDecoderAsync(string filePath, ref int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeCreateTexture(int id, ref IntPtr tex0, ref IntPtr tex1, ref IntPtr tex2);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeDestroyDecoder(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeFreeAudioData(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern float nativeGetAudioData(int id, ref IntPtr output, ref int lengthPerChannel);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeGetAudioFormat(int id, ref int channel, ref int frequency, ref float totalTime);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern int nativeGetDecoderState(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern int nativeGetMetaData(string filePath, out IntPtr key, out IntPtr value);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeGetVideoFormat(int id, ref int width, ref int height, ref float totalTime);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern bool nativeIsAudioEnabled(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern bool nativeIsContentReady(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern bool nativeIsEOF(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern bool nativeIsSeekOver(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern bool nativeIsVideoBufferEmpty(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern bool nativeIsVideoBufferFull(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern bool nativeIsVideoEnabled(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeLoadThumbnail(int id, float time, IntPtr texY, IntPtr texU, IntPtr texV);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeSetAudioAllChDataEnable(int id, bool isEnable);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeSetAudioEnable(int id, bool isEnable);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeSetSeekTime(int id, float sec);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeSetVideoEnable(int id, bool isEnable);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeSetVideoTime(int id, float currentTime);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern bool nativeStartDecoding(int id);

        [DllImport(NATIVE_LIBRARY_NAME)]
        public static extern void nativeRegistLogHandler(IntPtr f);

        public static void nativeLogHandler(string str)
        {
            Debug.LogFormat("[Native] {0}", str);
        }

        [DllImport(DecoderNative.NATIVE_LIBRARY_NAME)]
        public static extern IntPtr GetRenderEventFunc();
    }

}