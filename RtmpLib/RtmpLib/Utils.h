#pragma once

//����360�Ⱥ�180��ֱ��������
typedef enum VrType
{
    VR180 = 0,
    VR360,
}VrType;


typedef enum SourceType
{
    NONE_SOURCE = -1,
    PANO_SOURCE = 0,
    PROCPOST_SOURCE = 1,
}SourceType;

typedef enum SdiScaleType
{
    ORI_SCALE = 0, //����ԭʼ����������֮����䣬360�ȱ��ֵײ�ɫ��180��Ĭ�ϵײ�������ɫ����ɫ��ѡ ���
    CHANGE_SCALE, //��������1080 or 2160����䣬360����������180������Ĭ�Ϻ�ɫ����ɫ��ѡ ����
}SdiScaleType;

struct StreamOpt {
    int w;//�ֱ���
    int h;
    int vb;//��Ƶ����
    int ab;//��Ƶ����
    int vc;//��Ƶ����
    int ac;//��Ƶ����
    int sdiad = 0; //�����ӳ�
    int devid = 0; //SDI���
    long adms; //���������ӳ� ��Ƶ�ӳ�0
    long fillcol = 0; //vr180�����ɫ
    SourceType source = PROCPOST_SOURCE;
    char uri[1024] = { 0 };
    //char file[1024];
    char sdilogo[1024] = { 0 };
    SdiScaleType scaletype = ORI_SCALE;
    bool isvip = false;
    char ip[64] = { 0 };
    short port;
};


typedef enum OUTPUT_STATUS
{
    STATUS_IDLE = 0,
    STATUS_PREPARING,
    STATUS_PREPARED,
    STATUS_START,
    STATUS_RETRYING,
    STATUS_STOP,
    STATUS_END,
    STATUS_FAIL
}OUTPUT_STATUS;


//
typedef enum OutputType
{
    OT_UDPCLIENT = -4,
    OT_VIPSTREAMER = -3,
    OT_MANAGER = -2,
    OT_ENCODER = -1,
    OT_FILE = 0,
    OT_RTMP = 1,
    OT_SDI = 2
}OutputType;

//
typedef enum ResultType
{
    RT_FAIL = -1,
    RT_OK = 0,
    RT_RETRYING,
    RT_END
}ResultType;

class VROutputCallback
{
public:
    virtual void OnMsg(OutputType nType, ResultType nResult, char *msg) {};
};