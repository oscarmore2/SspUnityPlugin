//#include "stdafx.h"
#include "MessageDelivery.h"

MessageHandlerInterface* MessageDelivery::mHandler;

MessageDelivery::MessageDelivery()
{
}

MessageDelivery::~MessageDelivery()
{
}

void MessageDelivery::sendMessage(int what, int extra)
{
    if (mHandler)
        mHandler->onMessage(what, extra);
}

void MessageDelivery::setMessageHander(MessageHandlerInterface* handler)
{
    mHandler = handler;
}

std::string MessageDelivery::messageToStr(int what, int extra)
{
    switch (what)
    {
    case 0:
        return "";
    case EventDeviceStartSuc:
        return "�豸�����ɹ�";
    case EventDeviceStartFailed:
        return "�豸����ʧ��";
    case EventDeviceConnectSuc:
        return "�豸���ӳɹ�";
	case EventDeviceDisConnect:
		return "�豸���ӶϿ�";
    case EventStreamPrepare:
        return "���׼��";
    case EventStreamStart:
        return "�����ʼ";
    case EventStreamEnd:
        return "�������";
    case EventStreamFailed:
        return "���ʧ��";
    case EventEncodePrepare:
        return "����׼��";
    case EventEncodeStart:
        return "���뿪ʼ";
    case EventEncodeEnd:
        return "�������";
    case EventEncodeFailed:
        return "����ʧ��";
    case EventOutputEnd:
        return "�����������";
    case EventStandardizationEnd:
        return "��ͷ�궨���......";
    case EventGeometricEnd:
        return "����λ�ü������......";
    case EventMapEnd:
        return "ӳ����������......";
    case EventSeamcutEnd:
        return "ƴ�ӷ�������......";
    case EventCalibrationEnd:
        return "У׼�������,�����ںϹ���......";
    case EventStitchinitEnd:
        return "�ںϳ�ʼ�����......";
    case EventInputDeivceParamError:
        return "�����豸��������";
    case ErrorStreamIOOpen:
        return "�������ʧ��";
    case ErrorStreamIOSend:
        return "�����д��ʧ��";
    case ErrorEncoderVideo:
        return "ErrorEncoderVideo";
    case ErrorEncoderVideoInit:
        return "��Ƶ��������ʼ��ʧ��";
    case ErrorEncoderVideoEncode:
        return "��Ƶ����ʧ��";
	case ErrorStreamVideoInit:
		return "��Ƶ�������ʼ��ʧ��";
	case ErrorStreamAudioInit:
		return "��Ƶ�������ʼ��ʧ��";
    case ErrorOutputVideoQueue:
        return "OutputManager��Ƶ֡���������";
    case ErrorEncoderVideoQueue:
        return "Encoder��Ƶ֡���������";
    case ErrorStreamVideoQueue:
        return "Stream��Ƶ֡���������";
    case ErrorEncoderAudio:
        return "��Ƶ�ز���ʧ��";
    case ErrorEncoderAudioInit:
        return "��Ƶ��������ʼ��ʧ��";
    case ErrorEncoderAudioEncode:
        return "��Ƶ����ʧ��";
    case ErrorSdiInit:
        return "SDI��ʼ��ʧ��";
    case ErrorSdiDevInit:
        return "SDI����ʼ��ʧ��";
    case ErrorSdiInputWidth:
        return "SDI����Դwidth����";
    case ErrorGeometric:
        return "����λ�ü����������룬��������Ϊ���Ƶ㲻�������µģ��볢�����½��м���......";
    case ErrorState:
        return "״̬����";
	case ErrorVipStreamerQueue:
		return "VIP��Ƶ֡���������";
	case EventDeviceConnectFailed:
		return "����ʧ��";
    case EventDeviceConnect:
        return "���ڳ��������豸";
    case ErrorInvalidCube:
        return "�Ƿ���CUBE�ļ�";
    default:
        return "";
    }
}