using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeginInvokeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private delegate int AsynDelagateTest(int ms);

        private int AsynMethod(int ms)
        {
            Thread.Sleep(ms);
            return 100;
        }

        private void AsynMethodCompleted(IAsyncResult asyncResult)
        {
            if (asyncResult == null) return;
            //UIThreadSafe(textBox1, delegate
            //{
            //    textBox1.Text = ((MyMethod)(((AsyncResult)asyncResult).AsyncDelegate)).EndInvoke(asyncResult).ToString();
            //});

            UIThreadSafe(textBox1, delegate
            {
                textBox1.Text = (asyncResult.AsyncState as AsynDelagateTest).EndInvoke(asyncResult).ToString();
            });
        }

        private void btnExam4_Click(object sender, EventArgs e)
        {
            AsynDelagateTest my = AsynMethod;
            IAsyncResult asyncResult = my.BeginInvoke(15000, AsynMethodCompleted, my);
        }

        private void btnOpenForm_Click(object sender, EventArgs e)
        {
            MessageBox.Show("等待中...");
        }

        private void btnExam2_Click(object sender, EventArgs e)
        {
            AsynDelagateTest my = AsynMethod;
            IAsyncResult asyncResult = my.BeginInvoke(15000, null, null);
            while (!asyncResult.IsCompleted)
            {
                Thread.Sleep(1000);
                UIThreadSafe(textBox1, delegate
                {
                    textBox1.Text += "*";
                });
            }
        }

        private void btnExam3_Click(object sender, EventArgs e)
        {
            AsynDelagateTest my = AsynMethod;
            IAsyncResult asyncResult = my.BeginInvoke(15000, null, null);
            while (!asyncResult.AsyncWaitHandle.WaitOne(100, false))
            {
                Thread.Sleep(1000);
                UIThreadSafe(textBox1, delegate
                {
                    textBox1.Text += "*";
                });
            }
        }

        private void btnExam1_Click(object sender, EventArgs e)
        {

        }

        public delegate void UIThreadDelegate();
        public static void UIThreadSafe(Control control, UIThreadDelegate code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
            }
            else
            {
                code.Invoke();
            }
        }
    }
}
