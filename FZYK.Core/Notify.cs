using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using WeifenLuo.WinFormsUI.Docking;
using FZYK.Core;
using System.IO;
using FZYK.Com;
using System.Net.Sockets;
using System.Threading;


namespace FZYK.Core
{
    public static class Notify
    {
        
        private static bool ifConnect = true;
        #region 定义委托
        private delegate string RecMsgdelegate(byte[] Data);
        private delegate void Connentdelegate();
        private delegate void CloseAlertFormdelegate(Form frm);
        #endregion
        public static void OnStart()
        {
            ShowNewMessage("Notify");
            Connentdelegate outdelegate = new Connentdelegate(Connect);
            AsyncCallback acb = new AsyncCallback(CallBackConnent);
            IAsyncResult iar = outdelegate.BeginInvoke(acb, outdelegate); 
        }
        #region 回调函数
        private static void CallBackConnent(IAsyncResult ar)
        {
            //从C#异步调用状态ar.AsyncState中，获取委托对象  
            Connentdelegate dn = (Connentdelegate)ar.AsyncState;
            //一定要EndInvoke，否则你的下场很惨  
            dn.EndInvoke(ar);
        }
        private static void CallBackRecMsg(IAsyncResult ar)
        {
            //从C#异步调用状态ar.AsyncState中，获取委托对象  
            RecMsgdelegate dn = (RecMsgdelegate)ar.AsyncState;
            //一定要EndInvoke，否则你的下场很惨  
            string Type=dn.EndInvoke(ar);
            ShowNewMessage(Type);
            //OnStart();
        }
        private static void CallBackCloseAlertForm(IAsyncResult ar)
        {
            //从C#异步调用状态ar.AsyncState中，获取委托对象  
            CloseAlertFormdelegate dn = (CloseAlertFormdelegate)ar.AsyncState;
            //一定要EndInvoke，否则你的下场很惨  
            dn.EndInvoke(ar);
        }
        #endregion
        /// <summary>
        /// 监听网络变化
        /// </summary>
        private static void IfConnect()
        {
            while (true)
            {
                if (YKCommonSql.IfConnect())//断开连接
                {
                    ifConnect = true; ;
                }
                else if (YKCommonSql.IfConnect())//连接
                {
                    ifConnect = false;
                }
                Thread.Sleep(20 * 5000);
            }
        }

        /// <summary>
        /// 建立连接
        /// </summary>
        private static void Connect()
        {
            Thread PingThread = new Thread(IfConnect);
            PingThread.IsBackground = true;
            PingThread.Start();
            Com.YKCommonSql.SendLoginAndClose("Login");
            Com.UserInfo.threadClient = new Thread(RecMsg);
            Com.UserInfo.threadClient.IsBackground = true;
            Com.UserInfo.threadClient.Start();
        }


        /// <summary>
        /// 线程获取数据
        /// </summary>
        private static void RecMsg()
        {

            while (ifConnect)
            {
                // 定义一个2M的缓存区；   
                byte[] arrMsgRec = new byte[1024 * 1024 * 2];
                // 将接受到的数据存入到输入  arrMsgRec中；   
                int length = 0;
                try
                {
                    if (Com.UserInfo.sockClient != null && Com.UserInfo.sockClient.Connected)
                    {
                        length = Com.UserInfo.sockClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度；  
                        if (length > 0)
                        {
                            RecMsgdelegate outdelegate = new RecMsgdelegate(Revice);
                            AsyncCallback acb = new AsyncCallback(CallBackRecMsg);
                            IAsyncResult iar = outdelegate.BeginInvoke(arrMsgRec, acb, outdelegate); 
                        }
                    }
                }
                catch (SocketException se)
                {
                    Com.UserInfo.sockClient.Dispose();
                }
            }
        }

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="arrMsgRec"></param>
        private static string Revice(byte[] arrMsgRec)
        {
            try
            {
                FZYK.Com.Msg.YKClassMsg msg = new FZYK.Com.Msg.YKClassSerializers().DeSerializeBinary((new System.IO.MemoryStream(arrMsgRec))) as FZYK.Com.Msg.YKClassMsg;
                if (msg.msgID == "Notify")
                {
                    string Message = System.Text.Encoding.Unicode.GetString(msg.Data);
                    if (Com.YKPageValidate.IsDecimal(Message))
                    {
                        int nbID = Convert.ToInt32(Message);
                        string sql = "select * from SM_NotifyBoard where nbID=" + nbID;
                        DataTable dtChange = FZYK.Com.YKDataClass.getDataTable(sql);
                        if (dtChange != null && dtChange.Rows.Count > 0)
                        {
                            DataRow dr = Com.UserInfo.dsMessage.Tables[0].NewRow();
                            for (int i = 0; i < Com.UserInfo.dsMessage.Tables[0].Columns.Count; i++)
                            {
                                dr[Com.UserInfo.dsMessage.Tables[0].Columns[i].ColumnName] = dtChange.Rows[0][Com.UserInfo.dsMessage.Tables[0].Columns[i].ColumnName];
                            }
                            Com.UserInfo.dsMessage.Tables[0].Rows.InsertAt(dr, 0);
                        }
                        Com.UserInfo.dsMessage.Tables[0].AcceptChanges();
                    }
                }
                return msg.msgID;
                
            }
            catch (SocketException se)
            {
                return "";
            }
        }
        #region 弹出提醒框
        /// <summary>
        /// 关闭AlertForm
        /// </summary>
        /// <param name="frm"></param>
        public static void CloseAllAlertForm(Form frm)
        {
            if (frm.InvokeRequired)
            {
                CloseAlertFormdelegate outdelegate = new CloseAlertFormdelegate(CloseAllAlertForm);
                AsyncCallback acb = new AsyncCallback(CallBackCloseAlertForm);
                IAsyncResult iar = outdelegate.BeginInvoke(frm,acb, outdelegate); 
            }
            else
                frm.Close();
        }
        public static void ShowNewMessage(string Type)
        {
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                if (Application.OpenForms[i].Name == "AlertForm")
                {
                    Form frm = Application.OpenForms[i];
                    CloseAllAlertForm(frm);
                }
            }
            if (Type == "Approve")
            {
                string message = "";
                message = YKCommonSql.GetNewestMessage(Com.UserInfo.eID);
                if (message != "")
                {
                    Form frm = Core.Provider.ShowAlertForm(message);
                }
            }
            else if (Type == "Notify")
            {
                //BaseSelectDialog frm = Core.Provider.GetDeptSelect();
                //frm.ShowDialog();
                for (int i = 0; i < Com.UserInfo.dsMessage.Tables[0].Rows.Count; i++)
                {
                    if (Com.UserInfo.dsMessage.Tables[0].Rows[i].RowState != DataRowState.Deleted)
                    {
                        string Content = "Notify#" + Com.UserInfo.dsMessage.Tables[0].Rows.Count.ToString()
                            + "#" + Com.UserInfo.dsMessage.Tables[0].Rows[i]["nbID"].ToString()
                            + "#" + Com.UserInfo.dsMessage.Tables[0].Rows[i]["Writer"].ToString()
                            + " " + Com.UserInfo.dsMessage.Tables[0].Rows[i]["WriteDate"].ToString()
                            + "#" + Com.UserInfo.dsMessage.Tables[0].Rows[i]["nbTitle"].ToString();
                        Form frm = Core.Provider.ShowAlertForm(Content);
                        break;
                    }
                }
            }
        }
        
        #endregion
    }
}
