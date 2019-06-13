using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServerApp
{
    public class Server
    {
        private const int Buffersize = 8192;
        private byte[] buffer;
        private TcpClient client;
        private NetworkStream streamToClient;
        private ProtocolHandler handler;
        public OnMessageEventHandler OnMessageEvent = null;

        public Server(TcpClient client)
        {
            this.client = client;
            //打印连接到的客户端信息
            if (OnMessageEvent != null)
                OnMessageEvent(string.Format("\nClient Connected! Local:{0} <-- Client:{1}",
                client.Client.LocalEndPoint, client.Client.RemoteEndPoint));
            //获得流
            streamToClient = client.GetStream();
            buffer = new byte[Buffersize];
            handler = new ProtocolHandler();
        }

        //开始进行读取
        public void BeginRead()
        {
            AsyncCallback callBack = new AsyncCallback(OnReadComplete);
            streamToClient.BeginRead(buffer, 0, Buffersize, callBack, null);
        }

        //在读取完成时进行回调
        private void OnReadComplete(IAsyncResult ar)
        {
            int bytesRead = 0;
            try
            {
                bytesRead = streamToClient.EndRead(ar);
                OnMessageEvent(string.Format("Reading data, {0} bytes ...", bytesRead));
                if (bytesRead == 0)
                {
                    Console.WriteLine("Client offline.");
                    OnMessageEvent("Client offline.");
                    return;
                }

                string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                Array.Clear(buffer, 0, buffer.Length); //清空缓存，避免脏读

                //获取protocol数组
                string[] protocolArray = handler.GetProtocol(msg);
                foreach (string pro in protocolArray)
                {
                    //这里异步调用，不然这里可能会比较耗时
                    ParameterizedThreadStart start =
                        new ParameterizedThreadStart(handleProtocol);
                    //start.BeginInvoke(pro, new AsyncCallback(handleProtocol), null);
                    start.BeginInvoke(pro, null, null);
                }

                //再次调用BeginRead(),完成时调用自身，形成无限循环
                AsyncCallback callBack = new AsyncCallback(OnReadComplete);
                streamToClient.BeginRead(buffer, 0, Buffersize, callBack, null);
            }
            catch (Exception ex)
            {
                if (streamToClient != null)
                    streamToClient.Dispose();
                client.Close();
                Console.WriteLine(ex.Message);  //捕获异常时退出程序
            }
        }

        //处理protocol
        private void handleProtocol(object obj)
        {
            string pro = obj as string;
            ProtocolHelper helper = new ProtocolHelper(pro);
            FileProtocol protocol = helper.GetProtocol();

            if (protocol.Mode == FileRequestMode.Receive)
            {
                //客户端发送文件，对服务端来说则是接收文件
                receiveFile(protocol);
            }
            else if (protocol.Mode == FileRequestMode.Send)
            {
                //客户端接收文件，对服务端来说则是发送文件
                //sendFile(protocol);
            }
        }

        //接收文件
        private void receiveFile(FileProtocol protocol)
        {
            //获取远程客户端的位置
            IPEndPoint endPoint = client.Client.RemoteEndPoint as IPEndPoint;
            IPAddress ip = endPoint.Address;

            //使用新端口号，获得远程用于接收文件的端口
            endPoint = new IPEndPoint(ip, protocol.Port);

            //连接到远程客户端
            TcpClient localClient;
            try
            {
                localClient = new TcpClient();
                localClient.Connect(endPoint);
            }
            catch
            {
                OnMessageEvent(string.Format("无法连接到客户端 --> {0}", endPoint));
                return;
            }

            //获取发送文件的流
            NetworkStream streamToClient = localClient.GetStream();

            //随机生成一个在当前目录下的文件名称
            string path =
                Environment.CurrentDirectory + "/" + generateFileName(protocol.FileName);

            byte[] fileBuffer = new byte[1024];  //每次收1KB
            FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);

            //从缓存Buffer中读入到文件流中
            int bytesRead;
            int totalBytes = 0;
            do
            {
                bytesRead = streamToClient.Read(buffer, 0, Buffersize);

                fs.Write(buffer, 0, bytesRead);
                totalBytes += bytesRead;
                OnMessageEvent(string.Format("Receiving {0} bytes ...", totalBytes));

            } while (bytesRead > 0);
            OnMessageEvent(string.Format("Total {0} bytes received, Done!", totalBytes));
            streamToClient.Dispose();
            fs.Dispose();
            localClient.Close();
        }

        //随机获取一个图片名称
        private string generateFileName(string fileName)
        {
            DateTime now = DateTime.Now;
            return String.Format("{0}_{1}_{2}_{3}",
                now.Minute, now.Second, now.Millisecond, fileName);
        }
    }
    public delegate void OnMessageEventHandler(string msg);
}
