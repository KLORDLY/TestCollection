using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TcpClientApp
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Client c = new Client();
            c.OnMessageEvent += new OnMessageEventHandler(Entity_OnMessageEvent);
            string filePath = Environment.CurrentDirectory + "/" + "PressureSensor.png";
            if (File.Exists(filePath))
                c.BeginSendFile(filePath);
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
    }
}
