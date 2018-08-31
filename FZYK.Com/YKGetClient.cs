using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace FZYK.Com
{
    public  class YKGetClient
    {
        #region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
        public YKGetClient()
		{
		}
        #endregion
        /// <summary>
        /// 获取登陆IP
        /// </summary>
        /// <returns>IP</returns>
        public static string GetIP()
        {
            IPHostEntry IpHost = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in IpHost.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    continue;
                return ip.ToString();
            }
            return "";

        }
        /// <summary>
        /// 获得本机电脑的主机名
        /// </summary>
        /// <returns>电脑的主机名</returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }
        /// <summary>
        /// 得到未使用端口
        /// </summary>
        /// <returns></returns>
        public static int GetPoint()
        {
            TcpClient UDP_Server = new TcpClient();
            int i = 9000;
            while (true)
            {
                try
                {
                    IPEndPoint IPEndPoint = new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], i);
                    UDP_Server = new TcpClient(IPEndPoint);
                    UDP_Server.Close();
                    return i;
                }
                catch
                {
                }
                i++;
            }
        }
        ///// <summary>
        ///// 发送
        ///// 鄢国平 2012-11-20 创建
        ///// </summary>
        ///// <param name="Host">发送主机ID</param>
        ///// <param name="Port">发送主机端口</param>
        ///// <param name="Data">发送数据</param>
        //public static bool Send(int eID, string message, string Type)
        //{
        //    //try
        //    //{
        //    //    TcpClient tcpc = new TcpClient(Host.ToString(), Port);
        //    //    NetworkStream ns = tcpc.GetStream();
        //    //    ns.Write(Data, 0, Data.Length);
                
        //    //    ns.Flush();
        //    //    ns.Close();
        //    //    tcpc.Close();
        //        return true;
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    throw e;
        //    //    return false;
        //    //}
        //    //UdpClient UDP_Server = new UdpClient();
        //    //try
        //    //{
        //    //    IPEndPoint server = new IPEndPoint(Host, Port);    //将IP地址和端口号实例化一个IPEndPoint对象
        //    //    UDP_Server.Send(Data, Data.Length, server);         //将消息发给远程计算机
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //   // MessageBox.Show(e.ToString());
        //    //}
        //} 

    }
}
