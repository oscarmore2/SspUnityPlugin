using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NativeEncoder
{
    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    private static extern FBCAPTURE_STATUS fbc_reset();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setLiveCaptureSettings(
        int width,
        int height,
        int frameRate,
        int bitRate,
        float flushCycleStart,
        float flushCycleAfter,
        string streamUrl,
        bool is360,
        bool verticalFlip,
        bool horizontalFlip,
        PROJECTION_TYPE projectionType,
        STEREO_MODE stereoMode);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setVodCaptureSettings(
        int width,
        int height,
        int frameRate,
        int bitRate,
        string fullSavePath,
        bool is360,
        bool verticalFlip,
        bool horizontalFlip,
        PROJECTION_TYPE projectionType,
        STEREO_MODE stereoMode);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    private static extern FBCAPTURE_STATUS fbc_setPreviewCaptureSettings(
        int width,
        int height,
        int frameRate,
        bool is360,
        bool verticalFlip,
        bool horizontalFlip);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setScreenshotSettings(
        int width,
        int height,
        string fullSavePath,
        bool is360,
        bool verticalFlip,
        bool horizontalFlip);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setCameraOverlaySettings(
        float widthPercentage,
        UInt32 viewPortTopLeftX,
        UInt32 viewPortTopLeftY);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_enumerateMicDevices();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern UInt32 fbc_getMicDevicesCount();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.LPStr)]
    public static extern string fbc_getMicDeviceName(UInt32 index);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setMicDevice(UInt32 index);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_unsetMicDevice();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setMicEnabledDuringCapture(bool enabled);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setAudioEnabledDuringCapture(bool enabled);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_enumerateCameraDevices();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern UInt32 fbc_getCameraDevicesCount();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.LPStr)]
    public static extern string fbc_getCameraDeviceName(UInt32 index);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setCameraDevice(UInt32 index);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_unsetCameraDevice();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setCameraEnabledDuringCapture(bool enabled);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_setMicAndAudioRenderDeviceByVRDeviceType(VRDEVICE_TYPE vrDevice);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_startLiveCapture();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_startVodCapture();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    private static extern FBCAPTURE_STATUS fbc_startPreviewCapture();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_startScreenshot();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_captureTexture(IntPtr texturePtr);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    private static extern FBCAPTURE_STATUS fbc_previewCapture(IntPtr texturePtr);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    private static extern FBCAPTURE_STATUS fbc_previewCamera(IntPtr texturePtr);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_getCaptureStatus();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_getScreenshotStatus();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern void fbc_stopCapture();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_getCaptureCapability();

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern FBCAPTURE_STATUS fbc_saveScreenShot(IntPtr texturePtr);

    [DllImport("FBCapture", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern GRAPHICS_CARD fbc_checkGPUManufacturer();

    private const int ERROR_VIDEO_ENCODING_CAUSE_ERRORS = 100;
    private const int ERROR_AUDIO_ENCODING_CAUSE_ERRORS = 200;
    private const int ERROR_TRANSCODING_MUXING_CAUSE_ERRORS = 300;
    private const int ERROR_RTMP_CAUSE_ERRORS = 400;
    private const int ERROR_GRAPHICS_CAPTURE_ERRORS = 500;
    private const int ERROR_CONFIGURATION_ERRORS = 600;
    private const int ERROR_SYSTEM_ERRORS = 700;
    private const int ERROR_ENCODING_CAPABILITY = 800;

    public enum FBCAPTURE_STATUS
    {
        // Common error codes
        OK = 0,
        FBCAPTURE_STATUS_ENCODE_IS_NOT_READY,
        FBCAPTURE_STATUS_NO_INPUT_FILE,
        FBCAPTURE_STATUS_FILE_READING_ERROR,
        FBCAPTURE_STATUS_OUTPUT_FILE_OPEN_FAILED,
        FBCAPTURE_STATUS_OUTPUT_FILE_CREATION_FAILED,
        FBCAPTURE_STATUS_DXGI_CREATING_FAILED,
        FBCAPTURE_STATUS_DEVICE_CREATING_FAILED,

        // Video/Image encoding specific error codes      
        FBCAPTURE_STATUS_ENCODE_INIT_FAILED = ERROR_VIDEO_ENCODING_CAUSE_ERRORS,
        FBCAPTURE_STATUS_ENCODE_SET_CONFIG_FAILED,
        FBCAPTURE_STATUS_ENCODER_CREATION_FAILED,
        FBCAPTURE_STATUS_INVALID_TEXTURE_POINTER,
        FBCAPTURE_STATUS_CONTEXT_CREATION_FAILED,
        FBCAPTURE_STATUS_TEXTURE_CREATION_FAILED,
        FBCAPTURE_STATUS_TEXTURE_RESOURCES_COPY_FAILED,
        FBCAPTURE_STATUS_IO_BUFFER_ALLOCATION_FAILED,
        FBCAPTURE_STATUS_ENCODE_PICTURE_FAILED,
        FBCAPTURE_STATUS_ENCODE_FLUSH_FAILED,
        FBCAPTURE_STATUS_MULTIPLE_ENCODING_SESSION,
        FBCAPTURE_STATUS_INVALID_TEXTURE_RESOLUTION,

        // WIC specific error codes
        FBCAPTURE_STATUS_WIC_SAVE_IMAGE_FAILED,

        // Audio encoding specific error codes
        FBCAPTURE_STATUS_AUDIO_DEVICE_ENUMERATION_FAILED = ERROR_AUDIO_ENCODING_CAUSE_ERRORS,
        FBCAPTURE_STATUS_AUDIO_CLIENT_INIT_FAILED,
        FBCAPTURE_STATUS_WRITING_WAV_HEADER_FAILED,
        FBCAPTURE_STATUS_RELEASING_WAV_FAILED,

        // Transcoding and muxing specific error codes
        FBCAPTURE_STATUS_MF_CREATION_FAILED = ERROR_TRANSCODING_MUXING_CAUSE_ERRORS,
        FBCAPTURE_STATUS_MF_INIT_FAILED,
        FBCAPTURE_STATUS_MF_CREATING_WAV_FORMAT_FAILED,
        FBCAPTURE_STATUS_MF_TOPOLOGY_CREATION_FAILED,
        FBCAPTURE_STATUS_MF_TOPOLOGY_SET_FAILED,
        FBCAPTURE_STATUS_MF_TRANSFORM_NODE_SET_FAILED,
        FBCAPTURE_STATUS_MF_MEDIA_CREATION_FAILED,
        FBCAPTURE_STATUS_MF_HANDLING_MEDIA_SESSION_FAILED,

        // WAMEDIA muxing specific error codes
        FBCAPTURE_STATUS_WAMDEIA_MUXING_FAILED,

        // More MF error codes
        FBCAPTURE_STATUS_MF_STARTUP_FAILED,
        FBCAPTURE_STATUS_MF_TRANSFORM_CREATION_FAILED,
        FBCAPTURE_STATUS_MF_SOURCE_READER_CREATION_FAILED,
        FBCAPTURE_STATUS_MF_STREAM_SELECTION_FAILED,
        FBCAPTURE_STATUS_MF_MEDIA_TYPE_CREATION_FAILED,
        FBCAPTURE_STATUS_MF_MEDIA_TYPE_CONFIGURATION_FAILED,
        FBCAPTURE_STATUS_MF_MEDIA_TYPE_SET_FAILED,
        FBCAPTURE_STATUS_MF_MEDIA_TYPE_GET_FAILED,
        FBCAPTURE_STATUS_MF_CREATE_WAV_FORMAT_FROM_MEDIA_TYPE_FAILED,
        FBCAPTURE_STATUS_MF_TRANSFORM_OUTPUT_STREAM_INFO_FAILED,
        FBCAPTURE_STATUS_MF_CREATE_MEMORY_BUFFER_FAILED,
        FBCAPTURE_STATUS_MF_CREATE_SAMPLE_FAILED,
        FBCAPTURE_STATUS_MF_SAMPLE_ADD_BUFFER_FAILED,
        FBCAPTURE_STATUS_MF_READ_SAMPLE_FAILED,
        FBCAPTURE_STATUS_MF_TRANSFORM_FAILED,
        FBCAPTURE_STATUS_MF_BUFFER_LOCK_FAILED,

        // RTMP specific error codes
        FBCAPTURE_STATUS_INVALID_FLV_HEADER = ERROR_RTMP_CAUSE_ERRORS,
        FBCAPTURE_STATUS_INVALID_STREAM_URL,
        FBCAPTURE_STATUS_RTMP_CONNECTION_FAILED,
        FBCAPTURE_STATUS_RTMP_DISCONNECTED,
        FBCAPTURE_STATUS_SENDING_RTMP_PACKET_FAILED,

        // Graphics capture error codes
        FBCAPTURE_STATUS_GRAPHICS_DEVICE_CAPTURE_INIT_FAILED = ERROR_GRAPHICS_CAPTURE_ERRORS,
        FBCAPTURE_STATUS_GRAPHICS_DEVICE_CAPTURE_INVALID_TEXTURE,
        FBCAPTURE_STATUS_GRAPHICS_DEVICE_CAPTURE_OPEN_SHARED_RESOURCE_FAILED,
        FBCAPTURE_STATUS_GRAPHICS_DEVICE_CAPTURE_KEYED_MUTEX_ACQUIRE_FAILED,
        FBCAPTURE_STATUS_GRAPHICS_DEVICE_CAPTURE_KEYED_ACQUIRE_ACQUIRE_SYNC_FAILED,
        FBCAPTURE_STATUS_GRAPHICS_DEVICE_CAPTURE_KEYED_ACQUIRE_RELASE_SYNC_FAILED,

        // Configuration error codes
        FBCAPTURE_STATUS_MIC_NOT_CONFIGURED = ERROR_CONFIGURATION_ERRORS,
        FBCAPTURE_STATUS_MIC_REQUIRES_ENUMERATION,
        FBCAPTURE_STATUS_MIC_DEVICE_NOT_SET,
        FBCAPTURE_STATUS_MIC_ENUMERATION_FAILED,
        FBCAPTURE_STATUS_MIC_SET_FAILED,
        FBCAPTURE_STATUS_MIC_UNSET_FAILED,
        FBCAPTURE_STATUS_MIC_INDEX_INVALID,
        FBCAPTURE_STATUS_CAMERA_NOT_CONFIGURED,
        FBCAPTURE_STATUS_CAMERA_REQUIRES_ENUMERATION,
        FBCAPTURE_STATUS_CAMERA_DEVICE_NOT_SET,
        FBCAPTURE_STATUS_CAMERA_ENUMERATION_FAILED,
        FBCAPTURE_STATUS_CAMERA_SET_FAILED,
        FBCAPTURE_STATUS_CAMERA_UNSET_FAILED,
        FBCAPTURE_STATUS_CAMERA_INDEX_INVALID,
        FBCAPTURE_STATUS_LIVE_CAPTURE_SETTINGS_NOT_CONFIGURED,
        FBCAPTURE_STATUS_VOD_CAPTURE_SETTINGS_NOT_CONFIGURED,
        FBCAPTURE_STATUS_PREVIEW_CAPTURE_SETTINGS_NOT_CONFIGURED,

        // System error codes
        FBCAPTURE_STATUS_SYSTEM_INITIALIZE_FAILED = ERROR_SYSTEM_ERRORS,
        FBCAPTURE_STATUS_SYSTEM_ENCODING_TEXTURE_CREATION_FAILED,
        FBCAPTURE_STATUS_SYSTEM_PREVIEW_TEXTURE_CREATION_FAILED,
        FBCAPTURE_STATUS_SYSTEM_ENCODING_TEXTURE_FORMAT_CREATION_FAILED,
        FBCAPTURE_STATUS_SYSTEM_SCREENSHOT_TEXTURE_FORMAT_CREATION_FAILED,
        FBCAPTURE_STATUS_SYSTEM_CAPTURE_IN_PROGRESS,
        FBCAPTURE_STATUS_SYSTEM_CAPTURE_NOT_IN_PROGRESS,
        FBCAPTURE_STATUS_SYSTEM_CAPTURE_TEXTURE_NOT_RECEIVED,
        FBCAPTURE_STATUS_SYSTEM_CAMERA_OVERLAY_FAILED,
        FBCAPTURE_STATUS_SYSTEM_CAPTURE_PREVIEW_FAILED,
        FBCAPTURE_STATUS_SYSTEM_CAPTURE_PREVIEW_NOT_IN_PROGRESS,

        // Encoding capability error codes
        FBCAPTURE_STATUS_UNSUPPORTED_ENCODING_ENVIRONMENT = ERROR_ENCODING_CAPABILITY,
        FBCAPTURE_STATUS_UNSUPPORTED_GRAPHICS_CARD_DRIVER_VERSION,
        FBCAPTURE_STATUS_UNSUPPORTED_GRAPHICS_CARD,
        FBCAPTURE_STATUS_UNSUPPORTED_OS_VERSION,
        FBCAPTURE_STATUS_UNSUPPORTED_OS_PROCESSOR,
    }

    public enum VRDEVICE_TYPE
    {
        UNKNOWN,
        OCULUS_RIFT,
        HTC_VIVE,
    }

    public enum STEREO_MODE
    {
        SM_NONE,
        SM_TOP_BOTTOM,
        SM_LEFT_RIGHT
    }

    public enum PROJECTION_TYPE
    {
        NONE,
        EQUIRECT,
        CUBEMAP
    }

    public enum GRAPHICS_CARD
    {
        NVIDIA,
        AMD,
        UNSUPPORTED_DEVICE
    }

    public enum CAPTURE_MODE
    {
        _360_CAPTURE,
        NON_360_CAPTURE,
    }

    public enum CAPTURE_TEXTURE_FORMAT
    {
        RGB_CAPTURE,
        RGBD_CAPTURE,
    }

    public enum CAPTURE_TYPE
    {
        NONE,
        LIVE,
        VOD,
        PREVIEW,
        SCREENSHOT
    }

    public enum VIDEO_CAPTURE_TYPE
    {
        VOD,
        LIVE,
    }

    public enum RESOLUTION_PRESET
    {
        CUSTOM,
        _720P,
        _1080P,
        _4K,
    }

    public enum CAPTURE_STATUS
    {
        STARTING,
        RUNNING,
        STOPPED
    }

    public enum CAPTURE_ERROR
    {
        UNSUPPORTED_SPEC,
        VOD_FAILED_TO_START,
        LIVE_FAILED_TO_START,
        PREVIEW_FAILED_TO_START,
        SCREENSHOT_FAILED_TO_START,
        INVALID_STREAM_URI,
        TEXTURE_ENCODE_FAILED,
        SCREENSHOT_FAILED,
        CAPTURE_ALREADY_IN_PROGRESS,
    }
}
