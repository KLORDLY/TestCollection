using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpClientApp
{
    public class Client
    {
        private const int Buffersize = 8192;
        private byte[] buffer;
        private TcpClient client;
        private NetworkStream streamToServer;
        public OnMessageEventHandler OnMessageEvent = null;
        public Client()
        {
            try
            {
                client = new TcpClient();
                client.Connect("192.168.2.212", 8500);  //服务器连接
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (OnMessageEvent != null)
                    OnMessageEvent(ex.Message);
                return;
            }
            buffer = new byte[Buffersize];
            //打印连接到的服务端信息
            if (OnMessageEvent != null)
                OnMessageEvent(string.Format("Server Connected! Local:{0}-->Server:{1}",
                    client.Client.LocalEndPoint, client.Client.RemoteEndPoint));
            streamToServer = client.GetStream();
        }

        //发送消息到服务端
        public void SendMessage(string msg)
        {
            byte[] temp = Encoding.Unicode.GetBytes(msg);//获得缓存
            try
            {
                streamToServer.Write(temp, 0, temp.Length);//发往服务器
                OnMessageEvent(string.Format("Sent:{0}", msg));
            }
            catch (Exception ex)
            {
                OnMessageEvent(ex.Message);
            }
        }

        //发送文件 - 异步方法
        public void BeginSendFile(string filePath)
        {
            ParameterizedThreadStart start = new ParameterizedThreadStart(BeginSendFile);
            start.BeginInvoke(filePath, null, null);
        }

        private void BeginSendFile(object obj)
        {
            string filePath = obj as string;
            SendFile(filePath);
        }

        //发送文件
        private void SendFile(string filePath)
        {
            IPAddress ip = IPAddress.Parse("192.168.2.212");
            TcpListener listener = new TcpListener(ip, 0);
            listener.Start();

            //获取本地侦听端口号
            IPEndPoint endPoint = listener.LocalEndpoint as IPEndPoint;
            int listeningPort = endPoint.Port;

            //获取发送的协议字符串
            string fileName = Path.GetFileName(filePath);
            FileProtocol protocol =
                new FileProtocol(FileRequestMode.Send, listeningPort, fileName);
            string pro = protocol.ToString();

            SendMessage(pro); //发送协议到服务端

            //中断，等待远程连接
            TcpClient localClient = listener.AcceptTcpClient();

            OnMessageEvent("Start sending file...");
            NetworkStream stream = localClient.GetStream();

            //创建文件流
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] fileBuffer = new byte[1024];
            int bytesRead;
            int totalBytes = 0;

            //创建获取文件发送状态的类
            SendStatus status = new SendStatus(filePath);

            //将文件转写入网络流
            try
            {
                do
                {
                    Thread.Sleep(10);  //模拟远程传输的视觉效果，暂停10毫秒
                    bytesRead = fs.Read(fileBuffer, 0, fileBuffer.Length);
                    stream.Write(fileBuffer, 0, bytesRead);
                    totalBytes += bytesRead; //发送了的字节数
                    status.PrintStatus(totalBytes);  //打印发送状态
                } while (bytesRead > 0);
                OnMessageEvent(string.Format("Total {0} bytes sent, Done!", totalBytes));
            }
            catch
            {
                OnMessageEvent("Server has lost...");
            }
            finally
            {
                stream.Dispose();
                fs.Dispose();
                localClient.Close();
                listener.Stop();
            }
        }
    }
    public delegate void OnMessageEventHandler(string msg);
}
