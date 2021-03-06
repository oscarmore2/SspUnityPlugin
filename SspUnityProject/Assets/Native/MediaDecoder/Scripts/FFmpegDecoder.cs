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
    public class FFmpegDecoder : MediaDecoder
    {

        private const int AUDIO_FRAME_SIZE = 2048;
        //  Audio clip data size. Packed from audioDataBuff.
        private const int SWAP_BUFFER_NUM = 4;
        //	How many audio source to swap.
        private const double OVERLAP_TIME = 0.02;
        //  Our audio clip is defined as: [overlay][audio data][overlap].
        private readonly object _lock = new object();
        private readonly AudioSource[] audioSource = new AudioSource[SWAP_BUFFER_NUM];
        private List<float> audioDataBuff;
        //  Buffer to keep audio data decoded from native.
        private int audioDataLength;
        //  (AUDIO_FRAME_SIZE + 2 * audioOverlapLength) * audioChannel.
        private int audioOverlapLength;
        //  OVERLAP_TIME * audioFrequency.
        private double audioProgressTime = -1.0;
        private BackgroundWorker backgroundWorker;
        private int decoderID = -1;
        private double firstAudioFrameTime = -1.0;

        //	Time control
        private double globalStartTime;
        //  Video and audio progress are based on this start time.
        private double hangTime = -1.0f;
        //  Used to set progress time after seek/resume.
        private bool isAllAudioChEnabled;
        private bool isAudioReadyToReplay;
        private bool isVideoReadyToReplay;
        private DecoderNative.DecoderState lastState = DecoderNative.DecoderState.NOT_INITIALIZED;
        public bool playOnAwake = false;
        private bool seekPreview;
        //  To preview first frame of seeking when seek under paused state.
        //public Material texMaterial;

        private bool useDefault = true;
        //  To set default texture before video initialized.

        private float volume = 1.0f;

        public bool isVideoEnabled { get; private set; }

        public bool isAudioEnabled { get; private set; }

        public int audioFrequency { get; private set; }

        public int audioChannels { get; private set; }

        public float videoTotalTime { get; private set; }
        //  Video duration.
        public float audioTotalTime { get; private set; }
        //  Audio duration.

        private void Awake()
        {
            if (playOnAwake)
            {
                onInitComplete.AddListener(startDecoding);
                initDecoder(mediaPath);
            }

        }

        //  Video progress is triggered using Update. Progress time would be set by nativeSetVideoTime.
        private void Update()
        {
            switch (decoderState)
            {
                case DecoderNative.DecoderState.START:
                    if (isVideoEnabled)
                    {
                        //  Prevent empty texture generate green screen.(default 0,0,0 in YUV which is green in RGB)
                        if (useDefault && DecoderNative.nativeIsContentReady(decoderID))
                        {
                            getTextureFromNative();
                            setTextures(videoTexYch, videoTexUch, videoTexVch);
                            useDefault = false;
                        }

                        //	Update video frame by dspTime.
                        var setTime = AudioSettings.dspTime - globalStartTime;

                        //	Normal update frame.
                        if (setTime < videoTotalTime || videoTotalTime == -1.0f)
                        {
                            if (seekPreview && DecoderNative.nativeIsContentReady(decoderID))
                            {
                                setPause();
                                seekPreview = false;
                                unmute();
                            }
                            else
                            {
                                DecoderNative.nativeSetVideoTime(decoderID, (float)setTime);
                                GL.IssuePluginEvent(DecoderNative.GetRenderEventFunc(), decoderID);
                            }
                        }
                        else
                        {
                            isVideoReadyToReplay = true;
                        }
                    }

                    if (DecoderNative.nativeIsVideoBufferEmpty(decoderID) && !DecoderNative.nativeIsEOF(decoderID))
                    {
                        decoderState = DecoderNative.DecoderState.BUFFERING;
                        hangTime = AudioSettings.dspTime - globalStartTime;
                    }

                    break;

                case DecoderNative.DecoderState.SEEK_FRAME:
                    if (DecoderNative.nativeIsSeekOver(decoderID))
                    {
                        globalStartTime = AudioSettings.dspTime - hangTime;
                        decoderState = DecoderNative.DecoderState.START;
                        if (lastState == DecoderNative.DecoderState.PAUSE)
                        {
                            seekPreview = true;
                            mute();
                        }
                    }

                    break;

                case DecoderNative.DecoderState.BUFFERING:
                    if (DecoderNative.nativeIsVideoBufferFull(decoderID) || DecoderNative.nativeIsEOF(decoderID))
                    {
                        decoderState = DecoderNative.DecoderState.START;
                        globalStartTime = AudioSettings.dspTime - hangTime;
                    }

                    break;

                case DecoderNative.DecoderState.PAUSE:
                case DecoderNative.DecoderState.EOF:
                default:
                    break;
            }

            if (isVideoEnabled || isAudioEnabled)
            if ((!isVideoEnabled || isVideoReadyToReplay) &&
                (!isAudioEnabled || isAllAudioChEnabled || isAudioReadyToReplay))
            {
                decoderState = DecoderNative.DecoderState.EOF;
                isVideoReadyToReplay = isAudioReadyToReplay = false;

                if (onVideoEnd != null)
                    onVideoEnd.Invoke();
            }
        }

        public override void initDecoder(string path)
        {
            isAllAudioChEnabled = false;
            StartCoroutine(initDecoderAsync(path));
        }

        private IEnumerator initDecoderAsync(string path)
        {
            print(LOG_TAG + " init Decoder.");
            decoderState = DecoderNative.DecoderState.INITIALIZING;

            mediaPath = path;
            decoderID = -1;
            DecoderNative.nativeCreateDecoderAsync(mediaPath, ref decoderID);

            var result = 0;
            do
            {
                yield return null;
                result = DecoderNative.nativeGetDecoderState(decoderID);
            } while (!(result == 1 || result == -1));

            //  Init success.
            if (result == 1)
            {
                print(LOG_TAG + " Init success.");
                isVideoEnabled = DecoderNative.nativeIsVideoEnabled(decoderID);
                if (isVideoEnabled)
                {
                    var duration = 0.0f;
                    DecoderNative.nativeGetVideoFormat(decoderID, ref videoWidth, ref videoHeight, ref duration);
                    videoTotalTime = duration > 0 ? duration : -1.0f;
                    print(LOG_TAG + " Video format: (" + videoWidth + ", " + videoHeight + ")");
                    print(LOG_TAG + " Total time: " + videoTotalTime);

                    setTextures(null, null, null);
                    useDefault = true;
                }

                //	Initialize audio.
                isAudioEnabled = DecoderNative.nativeIsAudioEnabled(decoderID);
                print(LOG_TAG + " isAudioEnabled = " + isAudioEnabled);
                if (isAudioEnabled)
                {
                    if (isAllAudioChEnabled)
                    {
                        DecoderNative.nativeSetAudioAllChDataEnable(decoderID, isAllAudioChEnabled);
                        getAudioFormat();
                    }
                    else
                    {
                        getAudioFormat();
                        initAudioSource();
                    }
                }

                decoderState = DecoderNative.DecoderState.INITIALIZED;

                if (onInitComplete != null)
                    onInitComplete.Invoke();
            }
            else
            {
                print(LOG_TAG + " Init fail.");
                decoderState = DecoderNative.DecoderState.INIT_FAIL;
            }
        }

        private void getAudioFormat()
        {
            var channels = 0;
            var freqency = 0;
            var duration = 0.0f;
            DecoderNative.nativeGetAudioFormat(decoderID, ref channels, ref freqency, ref duration);
            audioChannels = channels;
            audioFrequency = freqency;
            audioTotalTime = duration > 0 ? duration : -1.0f;
            print(LOG_TAG + " audioChannel " + audioChannels);
            print(LOG_TAG + " audioFrequency " + audioFrequency);
            print(LOG_TAG + " audioTotalTime " + audioTotalTime);
        }

        private void initAudioSource()
        {
            getAudioFormat();
            audioOverlapLength = (int)(OVERLAP_TIME * audioFrequency + 0.5f);

            audioDataLength = (AUDIO_FRAME_SIZE + 2 * audioOverlapLength) * audioChannels;
            for (var i = 0; i < SWAP_BUFFER_NUM; i++)
            {
                if (audioSource[i] == null)
                    audioSource[i] = gameObject.AddComponent<AudioSource>();
                audioSource[i].clip =
                    AudioClip.Create("testSound" + i, audioDataLength, audioChannels, audioFrequency, false);
                audioSource[i].playOnAwake = false;
                audioSource[i].volume = volume;
                audioSource[i].minDistance = audioSource[i].maxDistance;
            }
        }

        public override void startDecoding()
        {
            if (decoderState == DecoderNative.DecoderState.INITIALIZED)
            {
                if (!DecoderNative.nativeStartDecoding(decoderID))
                {
                    print(LOG_TAG + " Decoding not start.");
                    return;
                }

                decoderState = DecoderNative.DecoderState.BUFFERING;
                globalStartTime = AudioSettings.dspTime;
                hangTime = AudioSettings.dspTime - globalStartTime;

                isVideoReadyToReplay = isAudioReadyToReplay = false;
                if (isAudioEnabled && !isAllAudioChEnabled)
                {
                    StartCoroutine("audioPlay");
                    backgroundWorker = new BackgroundWorker();
                    backgroundWorker.WorkerSupportsCancellation = true;
                    backgroundWorker.DoWork += pullAudioData;
                    backgroundWorker.RunWorkerAsync();
                }
            }
        }

        private void pullAudioData(object sender, DoWorkEventArgs e)
        {
            var dataPtr = IntPtr.Zero; //	Pointer to get audio data from native.
            var tempBuff = new float[0]; //	Buffer to copy audio data from dataPtr to audioDataBuff.
            var audioFrameLength = 0;
            double lastTime = -1.0f; //	Avoid to schedule the same audio data set.

            audioDataBuff = new List<float>();
            while (decoderState >= DecoderNative.DecoderState.START)
                if (decoderState != DecoderNative.DecoderState.SEEK_FRAME)
                {
                    double audioNativeTime =
                        DecoderNative.nativeGetAudioData(decoderID, ref dataPtr, ref audioFrameLength);
                    if (0 < audioNativeTime && lastTime != audioNativeTime &&
                        decoderState != DecoderNative.DecoderState.SEEK_FRAME &&
                        audioFrameLength != 0)
                    {
                        if (firstAudioFrameTime == -1.0)
                            firstAudioFrameTime = audioNativeTime;

                        lastTime = audioNativeTime;
                        audioFrameLength *= audioChannels;
                        if (tempBuff.Length != audioFrameLength)
                            tempBuff = new float[audioFrameLength];
                        Marshal.Copy(dataPtr, tempBuff, 0, audioFrameLength);
                        lock (_lock)
                        {
                            audioDataBuff.AddRange(tempBuff);
                        }
                    }

                    if (audioNativeTime != -1.0)
                        DecoderNative.nativeFreeAudioData(decoderID);

                    Thread.Sleep(2);
                }

            lock (_lock)
            {
                audioDataBuff.Clear();
                audioDataBuff = null;
            }
        }

        private void getTextureFromNative()
        {
            ReleaseTexture();

            var nativeTexturePtrY = new IntPtr();
            var nativeTexturePtrU = new IntPtr();
            var nativeTexturePtrV = new IntPtr();
            DecoderNative.nativeCreateTexture(decoderID, ref nativeTexturePtrY, ref nativeTexturePtrU,
                ref nativeTexturePtrV);
            videoTexYch = Texture2D.CreateExternalTexture(
                videoWidth, videoHeight, TextureFormat.Alpha8, false, false, nativeTexturePtrY);
            videoTexUch = Texture2D.CreateExternalTexture(
                videoWidth / 2, videoHeight / 2, TextureFormat.Alpha8, false, false, nativeTexturePtrU);
            videoTexVch = Texture2D.CreateExternalTexture(
                videoWidth / 2, videoHeight / 2, TextureFormat.Alpha8, false, false, nativeTexturePtrV);
        }

        private void ReleaseTexture()
        {
            setTextures(null, null, null);

            videoTexYch = null;
            videoTexUch = null;
            videoTexVch = null;

            useDefault = true;
        }

        public void replay()
        {
            if (setSeekTime(0.0f))
            {
                globalStartTime = AudioSettings.dspTime;
                isVideoReadyToReplay = isAudioReadyToReplay = false;
            }
        }

        public void getAllAudioChannelData(out float[] data, out double time, out int samplesPerChannel)
        {
            if (!isAllAudioChEnabled)
            {
                print(LOG_TAG + " this function only works for isAllAudioEnabled == true.");
                data = null;
                time = 0;
                samplesPerChannel = 0;
                return;
            }

            var dataPtr = new IntPtr();
            var lengthPerChannel = 0;
            double audioNativeTime = DecoderNative.nativeGetAudioData(decoderID, ref dataPtr, ref lengthPerChannel);
            float[] buff = null;
            if (lengthPerChannel > 0)
            {
                buff = new float[lengthPerChannel * audioChannels];
                Marshal.Copy(dataPtr, buff, 0, buff.Length);
                DecoderNative.nativeFreeAudioData(decoderID);
            }

            data = buff;
            time = audioNativeTime;
            samplesPerChannel = lengthPerChannel;
        }

        private IEnumerator audioPlay()
        {
            print(LOG_TAG + " start audio play coroutine.");
            var swapIndex = 0; //	Swap between audio sources.
            var audioDataTime = (double)AUDIO_FRAME_SIZE / audioFrequency;
            var playedAudioDataLength = AUDIO_FRAME_SIZE * audioChannels; //  Data length exclude the overlap length.

            print(LOG_TAG + " audioDataTime " + audioDataTime);

            audioProgressTime = -1.0; //  Used to schedule each audio clip to be played.
            while (decoderState >= DecoderNative.DecoderState.START)
            {
                if (decoderState == DecoderNative.DecoderState.START)
                {
                    var currentTime = AudioSettings.dspTime - globalStartTime;
                    if (currentTime < audioTotalTime || audioTotalTime == -1.0f)
                    {
                        if (audioDataBuff != null && audioDataBuff.Count >= audioDataLength)
                        {
                            if (audioProgressTime == -1.0)
                            {
                                //  To simplify, the first overlap data would not be played.
                                //  Correct the audio progress time by adding OVERLAP_TIME.
                                audioProgressTime = firstAudioFrameTime + OVERLAP_TIME;
                                globalStartTime = AudioSettings.dspTime - audioProgressTime;
                            }

                            while (audioSource[swapIndex].isPlaying ||
                                   decoderState == DecoderNative.DecoderState.SEEK_FRAME)
                                yield return null;

                            //  Re-check data length if audioDataBuff is cleared by seek.
                            if (audioDataBuff.Count >= audioDataLength)
                            {
                                var playTime = audioProgressTime + globalStartTime;
                                var endTime = playTime + audioDataTime;

                                //  If audio is late, adjust start time and re-calculate audio clip time.
                                if (playTime <= AudioSettings.dspTime)
                                {
                                    globalStartTime = AudioSettings.dspTime - audioProgressTime;
                                    playTime = audioProgressTime + globalStartTime;
                                    endTime = playTime + audioDataTime;
                                }

                                audioSource[swapIndex].clip
                                    .SetData(audioDataBuff.GetRange(0, audioDataLength).ToArray(), 0);
                                audioSource[swapIndex].PlayScheduled(playTime);
                                audioSource[swapIndex].SetScheduledEndTime(endTime);
                                audioSource[swapIndex].time = (float)OVERLAP_TIME;
                                audioProgressTime += audioDataTime;
                                swapIndex = (swapIndex + 1) % SWAP_BUFFER_NUM;

                                lock (_lock)
                                {
                                    audioDataBuff.RemoveRange(0, playedAudioDataLength);
                                }
                            }
                        }
                    }
                    else
                    {
                        //print(LOG_TAG + " Audio reach EOF. Prepare replay.");
                        isAudioReadyToReplay = true;
                        audioProgressTime = firstAudioFrameTime = -1.0;
                        if (audioDataBuff != null)
                            lock (_lock)
                            {
                                audioDataBuff.Clear();
                            }
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }

        public override void stopDecoding()
        {
            if (decoderState >= DecoderNative.DecoderState.INITIALIZING)
            {
                print(LOG_TAG + " stop decoding.");
                decoderState = DecoderNative.DecoderState.STOP;
                ReleaseTexture();
                if (isAudioEnabled && !isAllAudioChEnabled)
                {
                    StopCoroutine("audioPlay");
                    backgroundWorker.CancelAsync();

                    if (audioSource != null)
                        for (var i = 0; i < SWAP_BUFFER_NUM; i++)
                            if (audioSource[i] != null)
                            {
                                Destroy(audioSource[i].clip);
                                Destroy(audioSource[i]);
                                audioSource[i] = null;
                            }
                }

                DecoderNative.nativeDestroyDecoder(decoderID);
                decoderID = -1;
                decoderState = DecoderNative.DecoderState.NOT_INITIALIZED;

                isVideoEnabled = isAudioEnabled = isAllAudioChEnabled = false;
                isVideoReadyToReplay = isAudioReadyToReplay = false;
                isAllAudioChEnabled = false;
            }
        }

        public bool setSeekTime(float seekTime)
        {
            if (decoderState != DecoderNative.DecoderState.SEEK_FRAME &&
                decoderState >= DecoderNative.DecoderState.START)
            {
                lastState = decoderState;
                decoderState = DecoderNative.DecoderState.SEEK_FRAME;

                var setTime = 0.0f;
                if (isVideoEnabled && seekTime > videoTotalTime ||
                    isAudioEnabled && !isAllAudioChEnabled && seekTime > audioTotalTime ||
                    isVideoReadyToReplay || isAudioReadyToReplay ||
                    seekTime < 0.0f)
                {
                    print(LOG_TAG + " Seek over end. ");
                    setTime = 0.0f;
                }
                else
                {
                    setTime = seekTime;
                }

                print(LOG_TAG + " set seek time: " + setTime);
                hangTime = setTime;
                DecoderNative.nativeSetSeekTime(decoderID, setTime);
                DecoderNative.nativeSetVideoTime(decoderID, setTime);

                if (isAudioEnabled && !isAllAudioChEnabled)
                {
                    lock (_lock)
                    {
                        audioDataBuff.Clear();
                    }

                    audioProgressTime = firstAudioFrameTime = -1.0;
                    foreach (var src in audioSource)
                        src.Stop();
                }

                return true;
            }

            return false;
        }

        public bool isSeeking()
        {
            return decoderState >= DecoderNative.DecoderState.INITIALIZED &&
            (decoderState == DecoderNative.DecoderState.SEEK_FRAME ||
            !DecoderNative.nativeIsContentReady(decoderID));
        }

        public bool isVideoEOF()
        {
            return decoderState == DecoderNative.DecoderState.EOF;
        }

        public void setStepForward(float sec)
        {
            var targetTime = AudioSettings.dspTime - globalStartTime + sec;
            if (setSeekTime((float)targetTime))
                print(LOG_TAG + " set forward : " + sec);
        }

        public void setStepBackward(float sec)
        {
            var targetTime = AudioSettings.dspTime - globalStartTime - sec;
            if (setSeekTime((float)targetTime))
                print(LOG_TAG + " set backward : " + sec);
        }

        public void getVideoResolution(ref int width, ref int height)
        {
            width = videoWidth;
            height = videoHeight;
        }

        public float getVideoCurrentTime()
        {
            if (decoderState == DecoderNative.DecoderState.PAUSE ||
                decoderState == DecoderNative.DecoderState.SEEK_FRAME)
                return (float)hangTime;
            return (float)(AudioSettings.dspTime - globalStartTime);
        }

        public DecoderNative.DecoderState getDecoderState()
        {
            return decoderState;
        }

        public override void setPause()
        {
            if (decoderState == DecoderNative.DecoderState.START)
            {
                hangTime = AudioSettings.dspTime - globalStartTime;
                decoderState = DecoderNative.DecoderState.PAUSE;
                if (isAudioEnabled && !isAllAudioChEnabled)
                    foreach (var src in audioSource)
                        src.Pause();
            }
        }

        public override void setResume()
        {
            if (decoderState == DecoderNative.DecoderState.PAUSE)
            {
                globalStartTime = AudioSettings.dspTime - hangTime;
                decoderState = DecoderNative.DecoderState.START;
                if (isAudioEnabled && !isAllAudioChEnabled)
                    foreach (var src in audioSource)
                        src.UnPause();
            }
        }

        public void setVolume(float vol)
        {
            volume = Mathf.Clamp(vol, 0.0f, 1.0f);
            foreach (var src in audioSource)
                if (src != null)
                    src.volume = volume;
        }

        public float getVolume()
        {
            return volume;
        }

        public void mute()
        {
            var temp = volume;
            setVolume(0.0f);
            volume = temp;
        }

        public void unmute()
        {
            setVolume(volume);
        }

        public static void getMetaData(string filePath, out string[] key, out string[] value)
        {
            var keyptr = IntPtr.Zero;
            var valptr = IntPtr.Zero;

            var metaCount = DecoderNative.nativeGetMetaData(filePath, out keyptr, out valptr);

            var keys = new IntPtr[metaCount];
            var vals = new IntPtr[metaCount];
            Marshal.Copy(keyptr, keys, 0, metaCount);
            Marshal.Copy(valptr, vals, 0, metaCount);

            var keyArray = new string[metaCount];
            var valArray = new string[metaCount];
            for (var i = 0; i < metaCount; i++)
            {
                keyArray[i] = Marshal.PtrToStringAnsi(keys[i]);
                valArray[i] = Marshal.PtrToStringAnsi(vals[i]);
                Marshal.FreeCoTaskMem(keys[i]);
                Marshal.FreeCoTaskMem(vals[i]);
            }

            Marshal.FreeCoTaskMem(keyptr);
            Marshal.FreeCoTaskMem(valptr);

            key = keyArray;
            value = valArray;
        }

        public static void loadVideoThumb(GameObject obj, string filePath, float time)
        {
            if (!File.Exists(filePath))
            {
                print(LOG_TAG + " File not found!");
                return;
            }

            var decID = -1;
            var width = 0;
            var height = 0;
            var totalTime = 0.0f;
            DecoderNative.nativeCreateDecoder(filePath, ref decID);
            DecoderNative.nativeGetVideoFormat(decID, ref width, ref height, ref totalTime);
            if (!DecoderNative.nativeStartDecoding(decID))
            {
                print(LOG_TAG + " Decoding not start.");
                return;
            }

            var thumbY = new Texture2D(width, height, TextureFormat.Alpha8, false);
            var thumbU = new Texture2D(width / 2, height / 2, TextureFormat.Alpha8, false);
            var thumbV = new Texture2D(width / 2, height / 2, TextureFormat.Alpha8, false);
            var thumbMat = obj.GetComponent<MeshRenderer>().material;
            if (thumbMat == null)
            {
                print(LOG_TAG + " Target has no MeshRenderer.");
                DecoderNative.nativeDestroyDecoder(decID);
                return;
            }

            thumbMat.SetTexture("_YTex", thumbY);
            thumbMat.SetTexture("_UTex", thumbU);
            thumbMat.SetTexture("_VTex", thumbV);

            DecoderNative.nativeLoadThumbnail(decID, time, thumbY.GetNativeTexturePtr(), thumbU.GetNativeTexturePtr(),
                thumbV.GetNativeTexturePtr());
            DecoderNative.nativeDestroyDecoder(decID);
        }

        public void setAudioEnable(bool isEnable)
        {
            DecoderNative.nativeSetAudioEnable(decoderID, isEnable);
            if (isEnable)
                setSeekTime(getVideoCurrentTime());
        }

        public void setVideoEnable(bool isEnable)
        {
            DecoderNative.nativeSetVideoEnable(decoderID, isEnable);
            if (isEnable)
                setSeekTime(getVideoCurrentTime());
        }

        private void OnApplicationQuit()
        {
            print(LOG_TAG + " OnApplicationQuit");
            stopDecoding();
        }

        private void OnDestroy()
        {
            print(LOG_TAG + " OnDestroy");
            stopDecoding();
        }
    }
}