using System;

namespace FZYK.Com.Msg
{
	/// <summary>
	/// ClassMsg 的摘要说明。
	/// </summary>
	[Serializable]
	public class YKClassMsg
	{
        public String SID = "";//发送方编号
        public String SIP = "";//发送方IP
        public String SPort = "";//发送方端口号

        public String RID = "";//接收方编号
        public String RIP = "";//接收方IP
        public String RPort = "";//接收方端口号

        public SendKind sendKind = SendKind.SendNone;//发送消息类型，默认为无类型

        public MsgCommand msgCommand = MsgCommand.None;//消息命令

        public SendState sendState = SendState.None;//消息发送状态

        public String msgID = "";//消息ID，GUID

        public byte[] Data;

 	}
    /// <summary>
    /// 用户注册信息
    /// </summary>
    [Serializable]
    public class RegisterMsg 
    {
        public string UserName;//用户名
        public string PassWord;//密码
    }

    /// <summary>
    /// 消息命令
    /// </summary>
    public enum MsgCommand
    {
        None,
        Registering,//用户注册
        Registered,//用户注册结束
        Logining,//用户登录
        Logined,//用户登录结束,上线
        SendToOne,//发送单用户
        SendToAll,//发送所有用户
        UserList,//用户列表
        UpdateState,//更新用户状态
        VideoOpen,//打开视频
        Videoing,//正在视频
        VideoClose,//关闭视频
        Close//下线
    }

    /// <summary>
    /// 发送类型
    /// </summary>
    public enum SendKind
    {
        SendNone,//无类型
        SendCommand,//发送命令
        SendMsg,//发送消息
        SendFile//发送文件
    }

    /// <summary>
    /// 发送状态
    /// </summary>
    public enum SendState
    {
        None,//无状态
        Single,//单消息或文件
        Start,//发送开始生成文件
        Sending,//正在发送中，写入文件
        End//发送结束
    }
}
