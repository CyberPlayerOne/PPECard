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
        /// �洢���������ж�ȡ���ֽ���(�ݴ�)
        /// </summary>
       public  byte[] bytes;
        /// <summary>
        /// ���캯����Ҫ��TcpClient����
        /// </summary>
       public  TcpClient client;
        /// <summary>
        /// ���ڴ洢��ȡ���ֽ��ǣ�ÿ��ȡһ�ξʹ������һ����һ�飬ֱ��!DataAvailble�����յ�ȫ����������
        /// </summary>
        public MemoryStream memoryStream;
        /// <summary>
        /// ��ʼ��������Ա
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
