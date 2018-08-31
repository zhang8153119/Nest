using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace FZYK.Com
{
    [Serializable()]
    public static class UserInfo
    {
        /// <summary>
        /// 员工表ID
        /// </summary>
        public static int eID;
        /// <summary>
        /// 部门ID
        /// </summary>
        public static int dID;
        /// <summary>
        /// 员工编号
        /// </summary>
        public static string eNum;
        /// <summary>
        /// 员工姓名
        /// </summary>
        public static string eName;
        /// <summary>
        /// 所在部门
        /// </summary>
        public static string dName;
        /// <summary>
        /// 部门编号
        /// </summary>
        public static string dNum;
        /// <summary>
        /// 职位
        /// </summary>
        public static string ePostitle;
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public static string eIBase;
        /// <summary>
        /// 所在分公司
        /// </summary>
        public static string dSubName;
        /// <summary>
        /// 是否离职
        /// </summary>
        public static string eValid;
        /// <summary>
        /// 当前登陆的数据库名
        /// </summary>
        public static string DBString;
        /// <summary>
        /// 权限列表
        /// </summary>
        public static Dictionary<string, Rights> htRights;
        /// <summary>
        /// 系统配置信息
        /// </summary>
        public static Dictionary<string, string> configSys;
        /// <summary>
        /// 接收到的公告提醒数据
        /// </summary>
        public static DataSet dsMessage = new DataSet();

        /// <summary>
        /// 部门对应XML菜单文件
        /// </summary>
        public static string xmlName = "";

        /// <summary>
        /// 员工提醒数据
        /// </summary>
        public static DataSet dsOALimit = new DataSet();


        /// <summary>
        /// //创建网络服务,也就是TCP的Socket
        /// </summary>
        public static Socket sockClient = null;
        /// <summary>
        /// 创建用于接收服务端消息的 线程；
        /// </summary>
        public static Thread threadClient = null; 
        /// <summary>
        /// 用户ID
        /// </summary>
        public static string UserIP = Com .YKGetClient .GetIP();
        public static string ServerIP ;
        /// <summary>
        /// 用户端口
        /// </summary>
        public static int UserPort = Com.YKGetClient.GetPoint();
    }
    /// <summary>
    /// 权限类
    /// </summary>
    public class Rights
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Rights() { }
        public Rights(int isee, int iedit, int iaudit, int iapprove)
        {
            ISee = isee == 1 ? true : false;
            IEdit = iedit == 1 ? true : false;
            IAudit = iaudit == 1 ? true : false;
            IApprove = iapprove == 1 ? true : false;
                     
        }
        public Rights(bool isee, bool iedit, bool iaudit, bool iapprove)
        {
            ISee = isee;
            IEdit = iedit;
            IAudit = iaudit;
            IApprove = iapprove;

        }
        /// <summary>
        /// 查看权限
        /// </summary>
        private bool _iSee = false;
        /// <summary>
        /// 查看权限
        /// </summary>
        public bool ISee
        {
            get { return _iSee; }
            set { _iSee = value; }
        }
        /// <summary>
        /// 编辑权限
        /// </summary>
        private bool _iEdit = false;
        /// <summary>
        /// 编辑权限
        /// </summary>
        public bool IEdit
        {
            get { return _iEdit; }
            set { _iEdit = value; }
        }
        /// <summary>
        /// 审核权限
        /// </summary>
        private bool _iAudit = false;
        /// <summary>
        /// 审核权限
        /// </summary>
        public bool IAudit
        {
            get { return _iAudit; }
            set { _iAudit = value; }
        }
        /// <summary>
        /// 批准权限
        /// </summary>
        private bool _iApprove = false;
        /// <summary>
        /// 批准权限
        /// </summary>
        public bool IApprove
        {
            get { return _iApprove; }
            set { _iApprove = value; }
        }

    }
}
