using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PPECard
{
    /// <summary>
    /// 用于保存接收和发送数据
    /// </summary>
    class ReadWriteObject : IDisposable
    {
        public TcpClient client;
        public NetworkStream netStream;
        public byte[] readBytes;
        public byte[] writeBytes;
        public MemoryStream memoryStream;
        public ReadWriteObject(TcpClient client)
        {
            this.client = client;
            netStream = client.GetStream();
            readBytes = new byte[client.ReceiveBufferSize];
            writeBytes = new byte[client.SendBufferSize];
            memoryStream = new MemoryStream();
        }
        public void InitReadArray() { readBytes = new byte[client.ReceiveBufferSize]; }
        public void InitWriteArray() { writeBytes = new byte[client.SendBufferSize]; }

        #region IDisposable 成员

        public void Dispose()
        {
            netStream.Dispose();
            client.Close();
            client.Client.Close(10);
        }

        #endregion
    }
}
