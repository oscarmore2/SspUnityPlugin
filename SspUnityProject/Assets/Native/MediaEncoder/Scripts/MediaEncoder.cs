using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.VR;

namespace UnityPlugin.Encoder
{
    /// <summary>
    /// Media encoder.
    /// usage: Start -> EncodeFrame -> EncodeFrame -> EncodeFrame -> ... -> Stop
    /// </summary>
    public class MediaEncoder : MonoBehaviour
    {
        public void EncodeFrame(RenderTexture cameraRenderTexture)
        {
            StartCoroutine(ScreenBufferBlit(cameraRenderTexture));
        }


        [Header("Capture Options")]
        protected NativeEncoder.CAPTURE_MODE captureMode = NativeEncoder.CAPTURE_MODE.NON_360_CAPTURE;
        protected NativeEncoder.CAPTURE_TEXTURE_FORMAT captureTextureFormat = NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGB_CAPTURE;
        protected NativeEncoder.PROJECTION_TYPE projectionType = NativeEncoder.PROJECTION_TYPE.EQUIRECT;
        public NativeEncoder.VIDEO_CAPTURE_TYPE videoCaptureType = NativeEncoder.VIDEO_CAPTURE_TYPE.VOD;

        [Header("Capture Hotkeys")]
        public KeyCode startScreenShot = KeyCode.F1;
        public KeyCode startEncoding = KeyCode.F2;
        public KeyCode stopEncoding = KeyCode.F3;

        /// <summary>
        /// Encoding Setting Variables
        /// </summary>
        //    Live Video
        [Header("Live Video Settings")]
        public NativeEncoder.RESOLUTION_PRESET liveVideoPreset = NativeEncoder.RESOLUTION_PRESET.CUSTOM;
        public Int32 liveVideoWidth = 1920;
        public Int32 liveVideoHeight = 1080;
        public Int32 liveVideoFrameRate = 30;
        public Int32 liveVideoBitRate = 5000000;
        public string liveStreamUrl = "";
        // You can get test live stream key on "https://www.facebook.com/live/create".
        // ex. rtmp://rtmp-api-dev.facebook.com:80/rtmp/xxStreamKeyxx
        private const float encodingInitialFlushCycle = 5f;
        // Video initial flush cycle
        private const float encodingSecondaryFlushCycle = 5f;
        // Video flush cycle
        //Vod
        [Header("VOD Video Settings")]
        public NativeEncoder.RESOLUTION_PRESET vodVideoPreset = NativeEncoder.RESOLUTION_PRESET.CUSTOM;
        public Int32 vodVideoWidth = 4096;
        public Int32 vodVideoHeight = 2048;
        public Int32 vodVideoFrameRate = 30;
        public Int32 vodVideoBitRate = 5000000;
        [Tooltip("Save path for recorded video including file name. File format should be mp4 or h264")]
        public string fullVodSavePath = "";
        // Save path for recorded video including file name (c://xxx.mp4)

        //    Screenshot
        [Header("Screenshot Settings")]
        public NativeEncoder.RESOLUTION_PRESET screenshotPreset = NativeEncoder.RESOLUTION_PRESET.CUSTOM;
        public Int32 screenShotWidth = 4096;
        public Int32 screenShotHeight = 2048;
        [Tooltip("Save path for screenshot including file name. File format should be jpg")]
        public string fullScreenshotSavePath = "";
        // Save path for screenshot including file name (c://xxx.jpg)

        //    Preview Video
        [Header("Preview Video Settings")]
        public NativeEncoder.RESOLUTION_PRESET previewVideoPreset = NativeEncoder.RESOLUTION_PRESET.CUSTOM;
        public Int32 previewVideoWidth = 4096;
        public Int32 previewVideoHeight = 2048;
        public Int32 previewVideoFrameRate = 30;
        public Int32 previewVideoBitRate = 5000000;

        private Int32 outputWidth = 0;
        private Int32 outputHeight = 0;

        //    Callback for error handling
        public delegate void OnStatusCallback(NativeEncoder.CAPTURE_ERROR error,NativeEncoder.FBCAPTURE_STATUS? captureStatus);

        public event OnStatusCallback OnError = delegate {};

        // Private Members
        private bool captureStarted = false;
        private NativeEncoder.CAPTURE_TYPE captureStartedType = NativeEncoder.CAPTURE_TYPE.NONE;

        //Set ture if you want to mute Audio
        public bool enabledAudioCapture = true;
        public bool enabledMicCapture = true;

        private RenderTexture outputTexture;


        private NativeEncoder.VRDEVICE_TYPE attachedHMD;

        private bool screenshotReady = false;
        private bool screenshotStarted = false;

        void Awake()
        {
            captureStarted = false;
            screenshotStarted = false;
            captureStartedType = NativeEncoder.CAPTURE_TYPE.NONE;

            OnError += CaptureStatusLog;

            // Live video preset
            if (liveVideoPreset == NativeEncoder.RESOLUTION_PRESET._720P)
            {
                liveVideoWidth = 1280;
                liveVideoHeight = 720;
                liveVideoBitRate = 2000000;
            }
            else if (liveVideoPreset == NativeEncoder.RESOLUTION_PRESET._1080P)
            {
                liveVideoWidth = 1920;
                liveVideoHeight = 1080;
                liveVideoBitRate = 4000000;
            }
            else if (liveVideoPreset == NativeEncoder.RESOLUTION_PRESET._4K)
            {
                liveVideoWidth = 4096;
                liveVideoHeight = 2048;
                liveVideoBitRate = 10000000;
            }

            // VOD video preset
            if (vodVideoPreset == NativeEncoder.RESOLUTION_PRESET._720P)
            {
                vodVideoWidth = 1280;
                vodVideoHeight = 720;
                vodVideoBitRate = 2000000;
            }
            else if (vodVideoPreset == NativeEncoder.RESOLUTION_PRESET._1080P)
            {
                vodVideoWidth = 1920;
                vodVideoHeight = 1080;
                vodVideoBitRate = 4000000;
            }
            else if (vodVideoPreset == NativeEncoder.RESOLUTION_PRESET._4K)
            {
                vodVideoWidth = 4096;
                vodVideoHeight = 2048;
                vodVideoBitRate = 10000000;
            }

            // Screenshot video preset
            if (screenshotPreset == NativeEncoder.RESOLUTION_PRESET._720P)
            {
                screenShotWidth = 1280;
                screenShotHeight = 720;
            }
            else if (screenshotPreset == NativeEncoder.RESOLUTION_PRESET._1080P)
            {
                screenShotWidth = 1920;
                screenShotHeight = 1080;
            }
            else if (screenshotPreset == NativeEncoder.RESOLUTION_PRESET._4K)
            {
                screenShotWidth = 4096;
                screenShotHeight = 2048;
            }

            // Preview video preset
            if (previewVideoPreset == NativeEncoder.RESOLUTION_PRESET._720P)
            {
                previewVideoWidth = 1280;
                previewVideoHeight = 720;
                previewVideoBitRate = 2000000;
            }
            else if (previewVideoPreset == NativeEncoder.RESOLUTION_PRESET._1080P)
            {
                previewVideoWidth = 1920;
                previewVideoHeight = 1080;
                previewVideoBitRate = 4000000;
            }
            else if (previewVideoPreset == NativeEncoder.RESOLUTION_PRESET._4K)
            {
                previewVideoWidth = 4096;
                previewVideoHeight = 2048;
                previewVideoBitRate = 10000000;
            }

            // Retrieve attached VR devie for sound and microphone capture in VR
            // If expected VR device is not attached, it will capture default audio device
            string vrDeviceName = UnityEngine.XR.XRDevice.model.ToLower();
            if (vrDeviceName.Contains("rift"))
            {
                attachedHMD = NativeEncoder.VRDEVICE_TYPE.OCULUS_RIFT;
            }
            else if (vrDeviceName.Contains("vive"))
            {
                attachedHMD = NativeEncoder.VRDEVICE_TYPE.HTC_VIVE;
            }
            else
            {
                attachedHMD = NativeEncoder.VRDEVICE_TYPE.UNKNOWN;
            }
        }

        void Update()
        {
            // Input interface for start/stop encoding and screenshot
            if (Input.GetKeyDown(startEncoding))
            {
                if (videoCaptureType == NativeEncoder.VIDEO_CAPTURE_TYPE.LIVE)
                {
                    StartLiveStreaming(liveStreamUrl);
                }
                else if (videoCaptureType == NativeEncoder.VIDEO_CAPTURE_TYPE.VOD)
                {
                    StartVodCapture();
                }
            }
            else if (Input.GetKeyDown(stopEncoding))
            {
                StopCapture();
            }
            else if (Input.GetKeyDown(startScreenShot))
            {
                SaveScreenShot(screenShotWidth, screenShotHeight);
            }

            if (!captureStarted && !screenshotStarted)
                return;

            if (screenshotStarted)
            {
                screenshotReady = true;
            }

            NativeEncoder.FBCAPTURE_STATUS status;
            if (outputTexture && captureStarted)
            {
                status = NativeEncoder.fbc_captureTexture(outputTexture.GetNativeTexturePtr());  // Passing render texture to MediaEncoder
                if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
                {
                    OnError(NativeEncoder.CAPTURE_ERROR.TEXTURE_ENCODE_FAILED, status);
                    StopCapture();
                    return;
                }
            }
        }

        /// <summary>
        /// Configuration for Live Streaming 
        /// </summary>
        /// <param name="streamUrl">live stream key</param>
        public bool StartLiveStreaming(string streamUrl)
        {
            NativeEncoder.FBCAPTURE_STATUS status = NativeEncoder.FBCAPTURE_STATUS.OK;

            if (captureStarted || NativeEncoder.fbc_getCaptureStatus() != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.CAPTURE_ALREADY_IN_PROGRESS, null);
                return false;
            }

            if (string.IsNullOrEmpty(streamUrl))
            {
                OnError(NativeEncoder.CAPTURE_ERROR.INVALID_STREAM_URI, null);
                return false;
            }
            // Live video preset
            if (liveVideoPreset == NativeEncoder.RESOLUTION_PRESET._720P)
            {
                liveVideoWidth = 1280;
                liveVideoHeight = 720;
                liveVideoBitRate = 2000000;
            }
            else if (liveVideoPreset == NativeEncoder.RESOLUTION_PRESET._1080P)
            {
                liveVideoWidth = 1920;
                liveVideoHeight = 1080;
                liveVideoBitRate = 4000000;
            }
            else if (liveVideoPreset == NativeEncoder.RESOLUTION_PRESET._4K)
            {
                liveVideoWidth = 4096;
                liveVideoHeight = 2048;
                liveVideoBitRate = 10000000;
            }

            // Check hardware capability for live video encoding
            status = NativeEncoder.fbc_getCaptureCapability();
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.UNSUPPORTED_SPEC, status);
                return false;
            }

            // MAX video encoding resolution
            // AMD:     4096 x 2048
            // NVIDIA:  4096 x 4096
            if (NativeEncoder.GRAPHICS_CARD.AMD == NativeEncoder.fbc_checkGPUManufacturer() &&
                (liveVideoWidth > 4096 ||
                (captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? liveVideoHeight * 2 : liveVideoHeight) > 2048))
            {
                Debug.Log("Max video encoding resolution on AMD is 4096 x 2048");
                OnError(NativeEncoder.CAPTURE_ERROR.UNSUPPORTED_SPEC, null);
                return false;
            }
            else if (NativeEncoder.GRAPHICS_CARD.NVIDIA == NativeEncoder.fbc_checkGPUManufacturer() &&
                     (liveVideoWidth > 4096 ||
                     (captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? liveVideoHeight * 2 : liveVideoHeight) > 4096))
            {
                Debug.Log("Max video encoding resolution on NVIDIA is 4096 x 4096");
                OnError(NativeEncoder.CAPTURE_ERROR.UNSUPPORTED_SPEC, null);
                return false;
            }
            else if (NativeEncoder.GRAPHICS_CARD.UNSUPPORTED_DEVICE == NativeEncoder.fbc_checkGPUManufacturer())
            {
                Debug.Log("Unsupported gpu device or you missed to call fbc_getCaptureCapability supporting gpu device check");
                OnError(NativeEncoder.CAPTURE_ERROR.UNSUPPORTED_SPEC, null);
                return false;
            }

            // Create RenderTextures which will be used for live video encoding
            CreateRenderTextures(liveVideoWidth, liveVideoHeight);
           
            // Video Encoding and Live Configuration Settings
            status = NativeEncoder.fbc_setLiveCaptureSettings(
                width: outputWidth,
                height: captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? outputHeight * 2 : outputHeight,
                frameRate: liveVideoFrameRate,
                bitRate: liveVideoBitRate,
                flushCycleStart: encodingInitialFlushCycle,
                flushCycleAfter: encodingSecondaryFlushCycle,
                streamUrl: streamUrl,
                is360: captureMode == NativeEncoder.CAPTURE_MODE._360_CAPTURE ? true : false,
                verticalFlip: false,
                horizontalFlip: false,
                projectionType: projectionType,
                stereoMode: NativeEncoder.STEREO_MODE.SM_NONE);
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.LIVE_FAILED_TO_START, status);
                return false;
            }

            // Pick attached audio device resources for audio capture
            status = NativeEncoder.fbc_setMicAndAudioRenderDeviceByVRDeviceType(attachedHMD);
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.LIVE_FAILED_TO_START, status);
                return false;
            }

            // Make enable audio output capture(ex. speaker)
            status = NativeEncoder.fbc_setAudioEnabledDuringCapture(enabledAudioCapture);
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.LIVE_FAILED_TO_START, status);
                return false;
            }

            // Make enable audio input capture(ex. microphone)
            status = NativeEncoder.fbc_setMicEnabledDuringCapture(enabledMicCapture);
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.LIVE_FAILED_TO_START, status);
                return false;
            }

            status = NativeEncoder.fbc_startLiveCapture();
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.LIVE_FAILED_TO_START, status);
                return false;
            }

            OnCaptureStarted(NativeEncoder.CAPTURE_TYPE.LIVE);
            return true;
        }

        /// <summary>
        /// Configuration for Video Recording
        /// </summary>
        public bool StartVodCapture()
        {
            NativeEncoder.FBCAPTURE_STATUS status = NativeEncoder.FBCAPTURE_STATUS.OK;

            if (captureStarted || NativeEncoder.fbc_getCaptureStatus() != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.CAPTURE_ALREADY_IN_PROGRESS, null);
                return false;
            }

            if ((projectionType != NativeEncoder.PROJECTION_TYPE.EQUIRECT && captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE) ||
                (captureMode == NativeEncoder.CAPTURE_MODE.NON_360_CAPTURE && captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE))
            {
                Debug.Log("We only support RGB-D capture with equirect projection type and 360 capture mode");
                captureTextureFormat = NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGB_CAPTURE;
            }

            if (captureMode == NativeEncoder.CAPTURE_MODE._360_CAPTURE && projectionType == NativeEncoder.PROJECTION_TYPE.NONE)
            {
                Debug.Log("ProjectionType should be set for 360 capture. " +
                    "We want to set type to equirect for generating texture properly");
                projectionType = NativeEncoder.PROJECTION_TYPE.EQUIRECT;
            }
            else if (captureMode == NativeEncoder.CAPTURE_MODE.NON_360_CAPTURE)
            {  // Non 360 capture doesn't have projection type
                projectionType = NativeEncoder.PROJECTION_TYPE.NONE;
            }

            // VOD video preset
            if (vodVideoPreset == NativeEncoder.RESOLUTION_PRESET._720P)
            {
                vodVideoWidth = 1280;
                vodVideoHeight = 720;
                vodVideoBitRate = 2000000;
            }
            else if (vodVideoPreset == NativeEncoder.RESOLUTION_PRESET._1080P)
            {
                vodVideoWidth = 1920;
                vodVideoHeight = 1080;
                vodVideoBitRate = 4000000;
            }
            else if (vodVideoPreset == NativeEncoder.RESOLUTION_PRESET._4K)
            {
                vodVideoWidth = 4096;
                vodVideoHeight = 2048;
                vodVideoBitRate = 10000000;
            }

            // Check hardware capability for video encoding
            status = NativeEncoder.fbc_getCaptureCapability();
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.VOD_FAILED_TO_START, status);
                return false;
            }

            // MAX video encoding resolution
            // AMD:     4096 x 2048
            // NVIDIA:  4096 x 4096
            if (NativeEncoder.GRAPHICS_CARD.AMD == NativeEncoder.fbc_checkGPUManufacturer() &&
                (vodVideoWidth > 4096 ||
                (captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? vodVideoHeight * 2 : vodVideoHeight) > 2048))
            {
                Debug.Log("Max video encoding resolution on AMD is 4096 x 2048");
                OnError(NativeEncoder.CAPTURE_ERROR.UNSUPPORTED_SPEC, null);
                return false;
            }
            else if (NativeEncoder.GRAPHICS_CARD.NVIDIA == NativeEncoder.fbc_checkGPUManufacturer() &&
                     (vodVideoWidth > 4096 ||
                     (captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? vodVideoHeight * 2 : vodVideoHeight) > 4096))
            {
                Debug.Log("Max video encoding resolution on NVIDIA is 4096 x 4096");
                OnError(NativeEncoder.CAPTURE_ERROR.UNSUPPORTED_SPEC, null);
                return false;
            }
            else if (NativeEncoder.GRAPHICS_CARD.UNSUPPORTED_DEVICE == NativeEncoder.fbc_checkGPUManufacturer())
            {
                Debug.Log("Unsupported gpu device or you missed to call fbc_getCaptureCapability supporting gpu device check");
                OnError(NativeEncoder.CAPTURE_ERROR.UNSUPPORTED_SPEC, null);
                return false;
            }

            // Create RenderTextures which will be used for video encoding
            CreateRenderTextures(vodVideoWidth, vodVideoHeight);

            // If we haven't set the save path, we want to use project folder and timestamped file name by default
            string savePath;
            if (string.IsNullOrEmpty(fullVodSavePath))
            {
                savePath = string.Format("{0}/movie_{1}x{2}_{3}.mp4",
                    Directory.GetCurrentDirectory(),
                    vodVideoWidth, captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? outputHeight * 2 : outputHeight,
                    DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss"));
            }
            else
            {
                savePath = fullVodSavePath;
            }

            // Video Encoding Configuration Settings
            status = NativeEncoder.fbc_setVodCaptureSettings(
                width: outputWidth,
                height: captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? outputHeight * 2 : outputHeight,
                frameRate: vodVideoFrameRate,
                bitRate: vodVideoBitRate,
                fullSavePath: savePath,
                is360: captureMode == NativeEncoder.CAPTURE_MODE._360_CAPTURE ? true : false,
                verticalFlip: false,
                horizontalFlip: captureMode == NativeEncoder.CAPTURE_MODE._360_CAPTURE ? true : false,
                projectionType: projectionType,
                stereoMode: NativeEncoder.STEREO_MODE.SM_NONE);
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.VOD_FAILED_TO_START, status);
                return false;
            }

            // Pick attached audio device resources for audio capture
            status = NativeEncoder.fbc_setMicAndAudioRenderDeviceByVRDeviceType(attachedHMD);
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.VOD_FAILED_TO_START, status);
                return false;
            }

            // Make enable audio output capture(ex. speaker)
            status = NativeEncoder.fbc_setAudioEnabledDuringCapture(enabledAudioCapture);
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.VOD_FAILED_TO_START, status);
                return false;
            }

            // Make enable audio input capture(ex. microphone)
            status = NativeEncoder.fbc_setMicEnabledDuringCapture(enabledMicCapture);
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.VOD_FAILED_TO_START, status);
                return false;
            }

            // Start VOD capture
            status = NativeEncoder.fbc_startVodCapture();
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.VOD_FAILED_TO_START, status);
                return false;
            }

            OnCaptureStarted(NativeEncoder.CAPTURE_TYPE.VOD);
            return true;
        }

        /// <summary>
        /// Configuration for Screenshot
        /// </summary>
        /// <param name="width">  Screenshot resolution - width </param>
        /// <param name="height"> Screenshot resolution - height </param>
        public bool SaveScreenShot(int width, int height)
        {
            NativeEncoder.FBCAPTURE_STATUS status;

            //non360Camera.enabled = true;
            //depthCubemapCamera.enabled = true;
            //cubemapCamera.enabled = true;

            // Check current screenshot status.
            // It should return FBCAPTURE_STATUS.OK when it's not in progress
            status = NativeEncoder.fbc_getScreenshotStatus();
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.CAPTURE_ALREADY_IN_PROGRESS, null);
                return false;
            }

            if ((projectionType != NativeEncoder.PROJECTION_TYPE.EQUIRECT && captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE) ||
                (captureMode == NativeEncoder.CAPTURE_MODE.NON_360_CAPTURE && captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE))
            {
                Debug.Log("We only support RGB-D capture with equirect projection type and suruound capture mode");
                captureTextureFormat = NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGB_CAPTURE;
            }

            if (captureMode == NativeEncoder.CAPTURE_MODE._360_CAPTURE && projectionType == NativeEncoder.PROJECTION_TYPE.NONE)
            {
                Debug.Log("ProjectionType should be set for 360 capture. " +
                    "We want to set type to equirect for generating texture properly");
                projectionType = NativeEncoder.PROJECTION_TYPE.EQUIRECT;
            }

            // Screenshot video preset
            if (screenshotPreset == NativeEncoder.RESOLUTION_PRESET._720P)
            {
                screenShotWidth = 1280;
                screenShotHeight = 720;
            }
            else if (screenshotPreset == NativeEncoder.RESOLUTION_PRESET._1080P)
            {
                screenShotWidth = 1920;
                screenShotHeight = 1080;
            }
            else if (screenshotPreset == NativeEncoder.RESOLUTION_PRESET._4K)
            {
                screenShotWidth = 3840;
                screenShotHeight = 2160;
            }

            // In the sample, we only allow to use same resolution with video encoding WHEN video encoding is started
            if (captureStarted && screenShotWidth != outputWidth && screenShotHeight != outputHeight)
            {
                screenShotWidth = outputWidth;
                screenShotHeight = outputHeight;
            }

            // Create RenderTextures which will be used for screenshot
            CreateRenderTextures(screenShotWidth, screenShotHeight);

            // If we haven't set the save path, we want to use project folder and timestamped file name by default
            string savePath;
            if (string.IsNullOrEmpty(fullScreenshotSavePath))
            {
                savePath = string.Format("{0}/movie_{1}x{2}_{3}.jpg",
                    Directory.GetCurrentDirectory(),
                    outputWidth, captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? outputHeight * 2 : outputHeight,
                    DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss"));
            }
            else
            {
                savePath = fullScreenshotSavePath;
            }

            // Check hardware capability for screenshot
            status = NativeEncoder.fbc_getCaptureCapability();
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.SCREENSHOT_FAILED_TO_START, status);
                return false;
            }

            // Screenshot Configuration Settings in FBCapture SDK 
            status = NativeEncoder.fbc_setScreenshotSettings(
                width: outputWidth,
                height: captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? outputHeight * 2 : outputHeight,
                fullSavePath: savePath,
                is360: captureMode == NativeEncoder.CAPTURE_MODE._360_CAPTURE ? true : false,
                verticalFlip: false,
                horizontalFlip: captureMode == NativeEncoder.CAPTURE_MODE._360_CAPTURE ? true : false);

            // Start ScreenShot
            status = NativeEncoder.fbc_startScreenshot();
            if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
            {
                OnError(NativeEncoder.CAPTURE_ERROR.SCREENSHOT_FAILED_TO_START, status);
                return false;
            }

            screenshotStarted = true;
            Debug.Log("Screenshot started");

            return true;
        }

        /// <summary>
        /// Capture Stop Routine with Unity resource release
        /// </summary>
        public void StopCapture()
        {
            if (captureStarted != screenshotStarted)
            {
                NativeEncoder.fbc_stopCapture();

                if (outputTexture)
                {
                    Destroy(outputTexture);
                    outputTexture = null;
                }

                captureStarted = false;
                screenshotReady = false;

                captureStartedType = NativeEncoder.CAPTURE_TYPE.NONE;

                Debug.Log("Capture stopped");
            }
        }

        private IEnumerator ScreenBufferBlit(RenderTexture src)
        {
            yield return new WaitForEndOfFrame();
            if (null == src)
            {
                yield break;
            }
            if (captureStarted || screenshotStarted)
            {
                RenderTexture active = RenderTexture.active;
                RenderTexture.active = src;
                Graphics.Blit(src, outputTexture);
                RenderTexture.active = active;
                if (screenshotStarted)
                {
                    yield return new WaitUntil(() => screenshotReady);
                    screenshotReady = false;

                    NativeEncoder.FBCAPTURE_STATUS status = NativeEncoder.fbc_saveScreenShot(outputTexture.GetNativeTexturePtr());
                    if (status != NativeEncoder.FBCAPTURE_STATUS.OK)
                    {
                        OnError(NativeEncoder.CAPTURE_ERROR.SCREENSHOT_FAILED, status);
                    }
                    StopCapture();
                    screenshotStarted = false;
                }
            }
        }

        private void OnCaptureStarted(NativeEncoder.CAPTURE_TYPE captureType)
        {
            captureStarted = true;
            captureStartedType = captureType;
            Debug.Log("Capture started");
        }

        private void CaptureStatusLog(NativeEncoder.CAPTURE_ERROR error, NativeEncoder.FBCAPTURE_STATUS? captureStatus)
        {
            Debug.Log("Capture SDK Error Occured of type: " + error + " [Error code: " + captureStatus + " ] \n" +
                "Please check MediaEncoder.txt log file for more information");
        }

        /// <summary>
        /// Create the RenderTexture for encoding texture
        /// </summary>
        /// <param name="width">  Encoding Resolution - width </param>
        /// <param name="height"> Encoding Resolution - height </param>
        private void CreateRenderTextures(int width, int height)
        {
            if (captureStarted)
            {
                Debug.Log("Capture is already started. You can't resize texture and generate new texture");
                return;
            }

            outputWidth = width;
            outputHeight = height;

            outputTexture = new RenderTexture(outputWidth, captureTextureFormat == NativeEncoder.CAPTURE_TEXTURE_FORMAT.RGBD_CAPTURE ? outputHeight * 2 : outputHeight, 0, RenderTextureFormat.ARGB32);
            outputTexture.hideFlags = HideFlags.HideAndDontSave;
            outputTexture.Create();
        }

        /// <summary>
        /// Create materials which will be used for equirect and cubemap generation
        /// </summary>
        /// <param name="s"> shader code </param>
        /// <param name="m2Create"> material </param>
        /// <returns></returns>
        protected Material CreateMaterial(Shader s, Material m2Create)
        {
            if (!s)
            {
                Debug.Log("Missing shader in " + ToString());
                return null;
            }

            if (m2Create && (m2Create.shader == s) && (s.isSupported))
                return m2Create;

            if (!s.isSupported)
            {
                return null;
            }

            m2Create = new Material(s);
            m2Create.hideFlags = HideFlags.DontSave;

            return m2Create;
        }



        void OnDestroy()
        {
            StopCapture();
        }

        void OnApplicationQuit()
        {
            StopCapture();
        }

        public bool IsCaptureInProgress()
        {
            return captureStarted;
        }

        public UInt32 GetMicDevicesCount()
        {
            NativeEncoder.fbc_enumerateMicDevices();
            return NativeEncoder.fbc_getMicDevicesCount();
        }

        public string GetMicDeviceName(UInt32 index)
        {
            return NativeEncoder.fbc_getMicDeviceName(index);
        }

        public void SetMicDevice(UInt32 index)
        {
            NativeEncoder.fbc_setMicDevice(index);
        }

        public void UnsetMicDevice()
        {
            NativeEncoder.fbc_unsetMicDevice();
        }

        public UInt32 GetCameraDevicesCount()
        {
            NativeEncoder.fbc_enumerateCameraDevices();
            return NativeEncoder.fbc_getCameraDevicesCount();
        }

        public string GetCameraDeviceName(UInt32 index)
        {
            return NativeEncoder.fbc_getCameraDeviceName(index);
        }

        public void SetCameraDevice(UInt32 index)
        {
            NativeEncoder.fbc_setCameraDevice(index);
        }

        public void UnsetCameraDevice()
        {
            NativeEncoder.fbc_unsetCameraDevice();
        }

        public void SetCameraEnabledDuringCapture(bool enabled)
        {
            NativeEncoder.fbc_setCameraEnabledDuringCapture(enabled);
        }

        public void SetCameraOverlaySettings(float widthPercentage, UInt32 viewPortTopLeftX, UInt32 viewPortTopLeftY)
        {
            NativeEncoder.fbc_setCameraOverlaySettings(widthPercentage, viewPortTopLeftX, viewPortTopLeftY);
        }
    }

    public class EncoderFactory
    {
        public static MediaEncoder InitLiveEncoder(Material imgMat, NativeEncoder.VIDEO_CAPTURE_TYPE type = NativeEncoder.VIDEO_CAPTURE_TYPE.LIVE, int width = 1920, int height = 1080, int frameRate = 30, int bitRate = 400000)
        {
//            if (null == MediaEncoder.Instance)
//            {
//                var renderer = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Renderer"));
//                MeshRenderer mesh = renderer.GetComponentInChildren<MeshRenderer>();
//                mesh.material = imgMat;
//                renderer.transform.position = (Vector3.down + Vector3.left) * 3000;
//                MediaEncoder.Create(renderer);
//            }
//            
//            MediaEncoder.Instance.videoCaptureType = type;
//            MediaEncoder.Instance.liveVideoWidth = width;
//            MediaEncoder.Instance.liveVideoFrameRate = frameRate;
//            MediaEncoder.Instance.liveVideoHeight = height;
//            MediaEncoder.Instance.liveVideoBitRate = bitRate;
//            return MediaEncoder.Instance;
            return null;
        }
    }
}
