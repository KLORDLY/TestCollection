using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ThreadInterruptTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string name = "Main";
            Thread.CurrentThread.Name = name;

            ThreadStart ts = new ThreadStart(ThreadEntry);
            Thread worker = new Thread(ts);
            worker.IsBackground = true;
            worker.Name = "Worker";
            worker.Start();
            worker.Interrupt();
            worker.Join();
            lstBox.Items.Add(string.Format("{0}:End", name));
        }
        void ThreadEntry()
        {
            try
            {
                Thread.Sleep(10000);//如果不发生阻塞，则永远不会进入下面异常
            }
            catch (Exception ex)
            {
                //好奇怪，竟然没有出现跨线程异常
                lstBox.Items.Add(string.Format("{0}:Catch{1}", Thread.CurrentThread.Name, ex.GetType()));
            }
            lstBox.Items.Add(string.Format("{0}:End", Thread.CurrentThread.Name));
        }
    }
}
