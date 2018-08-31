using System;

namespace FZYK.Com.Msg
{
	/// <summary>
	/// ClassMsg ��ժҪ˵����
	/// </summary>
	[Serializable]
	public class YKClassMsg
	{
        public String SID = "";//���ͷ����
        public String SIP = "";//���ͷ�IP
        public String SPort = "";//���ͷ��˿ں�

        public String RID = "";//���շ����
        public String RIP = "";//���շ�IP
        public String RPort = "";//���շ��˿ں�

        public SendKind sendKind = SendKind.SendNone;//������Ϣ���ͣ�Ĭ��Ϊ������

        public MsgCommand msgCommand = MsgCommand.None;//��Ϣ����

        public SendState sendState = SendState.None;//��Ϣ����״̬

        public String msgID = "";//��ϢID��GUID

        public byte[] Data;

 	}
    /// <summary>
    /// �û�ע����Ϣ
    /// </summary>
    [Serializable]
    public class RegisterMsg 
    {
        public string UserName;//�û���
        public string PassWord;//����
    }

    /// <summary>
    /// ��Ϣ����
    /// </summary>
    public enum MsgCommand
    {
        None,
        Registering,//�û�ע��
        Registered,//�û�ע�����
        Logining,//�û���¼
        Logined,//�û���¼����,����
        SendToOne,//���͵��û�
        SendToAll,//���������û�
        UserList,//�û��б�
        UpdateState,//�����û�״̬
        VideoOpen,//����Ƶ
        Videoing,//������Ƶ
        VideoClose,//�ر���Ƶ
        Close//����
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum SendKind
    {
        SendNone,//������
        SendCommand,//��������
        SendMsg,//������Ϣ
        SendFile//�����ļ�
    }

    /// <summary>
    /// ����״̬
    /// </summary>
    public enum SendState
    {
        None,//��״̬
        Single,//����Ϣ���ļ�
        Start,//���Ϳ�ʼ�����ļ�
        Sending,//���ڷ����У�д���ļ�
        End//���ͽ���
    }
}
