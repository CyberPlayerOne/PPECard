using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PPECard
{
    class ClientNString
    {
      // public  string str = "";
        /// <summary>
        /// 存储从网络流中读取的字节们(暂存)
        /// </summary>
       public  byte[] bytes;
        /// <summary>
        /// 构造函数需要的TcpClient对象
        /// </summary>
       public  TcpClient client;
        /// <summary>
        /// 用于存储读取的字节们，每读取一次就存在这里，一遍又一遍，直到!DataAvailble，已收到全部可用数据
        /// </summary>
        public MemoryStream memoryStream;
        /// <summary>
        /// 初始化三个成员
        /// </summary>
        /// <param name="clt"></param>
        public ClientNString(TcpClient clt)
        {
            bytes = new byte[clt.ReceiveBufferSize];
            client = clt;
            memoryStream = new MemoryStream();
        }
    }
}
