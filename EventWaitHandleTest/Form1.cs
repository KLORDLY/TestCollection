using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventWaitHandleTest
{
    public partial class Form1 : Form
    {
        public static ManualResetEvent StartintUpClosedEvent = new ManualResetEvent(false);
        public Form1()
        {
            InitializeComponent();
            Thread daemonThread = new Thread(Go);
            daemonThread.IsBackground = true;
            daemonThread.Start();
        }

        private void Go()
        {
            StartintUpClosedEvent.WaitOne(10000, false);
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            StartintUpClosedEvent.Set();
        }
    }
}
