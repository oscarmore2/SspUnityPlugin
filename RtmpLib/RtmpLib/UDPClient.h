/**
�г���Ŀ��Ϣ��������

��������
1.	��Ļ��������ȷ��IP������ӿ�
2.	VIP����վ������������Ļ��IP��Ϣ
3.	VIP����վ��������Ļ������������Ϣ
4.	��Ļ������Ŀ��Ϣ���͸�VIP��λ
5.	VIP���ս�Ŀ��Ϣ����Ⱦ��������

��Ļ��ֹͣ��ĳVIP��λ������Ϣ������
1.	�յ�VIP��λ�������͵ĶϿ��ź�
2.	������ʱ

������ʽ����UDPЭ�飬ÿ����Ϣ����3���Ա�֤��Ϣ�ɴ

��Ϣ��ʽ��
0                   1                   2                   3
0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
+ -+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| check flag |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| message type |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| message index |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+


������Ϣ���������ֽ���

checkflag 0xffffffff
message type�� 0 Ϊ������ 1Ϊ�Ͽ��� 2Ϊ�滭��Ϣ, 3Ϊ����滭��Ϣ
message index��ָ��index����0 ��ʼ������VIP��λ�յ��ظ���indexʱ��ֻ�����һ����

0                   1                   2                   3
0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
+ -+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| check flag |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| message type |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| message index |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| x | y |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| width | height |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| pitch |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| block 1 |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| block 2 |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| block 3 |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
| block n |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

�滭��Ϣ�����ֶ�
x��y �滭��Ϣ���ӵ���Ƶ�����λ��
width��height ÿ����Ŀ��
pitch ÿһ�п������
block ����RGBA��ʾ����ɫ//�������ݰ�
**/
#include "InteractivePIsFilter.h"
#include <WINSOCK2.H>  
//#include "Constants.h"
#define HEARTBEAT_INTERVAL_MICROSEC 3000//����������3000 - 3030ms
#define HEARTBEAT_INTERVAL_MICROSEC_MAX 3030
#define CLIENT_SEND_TIMEOUT 5000   //5s
#define CLIENT_RECV_TIMEOUT 1000   //1s

#define UDPPACKET_MAX_SIZE (sizeof(UDPpacket))
#define UDPPACKET_MIN_SIZE (sizeof(UDPpacket) - (PIS_BLOCKS_MAX_SIZE - 1)*sizeof(int))
#define UDPPACKET_CHECK_FLAG 0xffffffff

typedef struct UDPpacket
{
 
    int check_flag;      //0xffffffff
    int message_type;    //0 Ϊ������ 1Ϊ�Ͽ��� 2Ϊ�滭��Ϣ, 3Ϊ����滭��Ϣ
    int message_index;   //ָ��index����0 ��ʼ������VIP��λ�յ��ظ���indexʱ��ֻ�����һ����
    short x;               //�滭��Ϣ���ӵ���Ƶ�����λ��x
    short y;               //�滭��Ϣ���ӵ���Ƶ�����λ��y
    short width;           //ÿ����Ŀ��
    short height;
    int pitch;           //ÿһ�п������  
	int blocks[PIS_BLOCKS_MAX_SIZE]; //����RGBA��ʾ����ɫ
} UDPpacket;

class UDPClient:public InteractivePIsSource, public VROutputCallback
{
private:

    int loop_flag = 0;
	char server_ip[32];
	bool m_isRunning = false;
	unsigned short serverport;
    int m_pre_index = -0x7FFFFFFF;
	SOCKET sockfd;
	pthread_t thread;
    pthread_t thread_heart;
	UDPpacket m_UDPPackt;
	InteractivePIs m_infos;

    char bufsend[8] = { 0 };
    int m_heartbeat_type = MSG_TYPE_HEART;

    sockaddr_in serverAddr;
    int socklen;
public:
	UDPClient();
	~UDPClient();
	int start_client(char *myip, unsigned short port);
	int stop_client();
private:
	int DecodeUDPpacket(char *buf, int len);
    static void* heartbeat(void *data);
	static void* client(void *data);
	void* doClient();
    void* doheartbeat();
	int charToInt(char* src, int offset);
	short charToshort(char* src, int offset);

    void IntToChar(int value, char* src);
    void ShortToChar(short value, char* src);
    void HbMsgGenerator(int heartbeat_type); //0 for heartbeat ,1 for stop beat
    void OnMsg(OutputType nType, ResultType nResult, char *msg);
};