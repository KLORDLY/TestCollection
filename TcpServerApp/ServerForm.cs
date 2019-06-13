using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TcpServerApp
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }
        int connections;
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Visible = false;
            txtMsg.Text += "Server is running ...\r\n";
            IPAddress ip = IPAddress.Parse(GetIpString());
            TcpListener listener = new TcpListener(ip, 8500);

            listener.Start();  //开启对控制端口8500的侦听
            txtMsg.Text += "Start Listening ...\r\n";
            while (connections == 0)
            {
                //获取一个连接，同步方法，在此处中断
                TcpClient client = listener.AcceptTcpClient();
                Server wapper = new Server(client);
                wapper.OnMessageEvent += new OnMessageEventHandler(Entity_OnMessageEvent);
                wapper.BeginRead();
                connections++;
            }
        }

        private void Entity_OnMessageEvent(string msg)
        {
            if (txtMsg.InvokeRequired)
            {
                txtMsg.Invoke(new OnMessageEventHandler(Entity_OnMessageEvent), msg);
            }
            else
            {
                txtMsg.Text += msg + "\r\n";
            }
        }


        private string GetIpString()
        {
            string ipString = string.Empty;
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (ips != null && ips.Length > 0)
            {
                //获取本机IP
                ipString = ips[6].ToString();
            }
            return ipString;
        }

    }
}
