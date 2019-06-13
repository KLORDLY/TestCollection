using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuncAndAction
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        public Form1()
        {
            InitializeComponent();
            //对象为N置Null测试
            if (timer != null)
            {
                timer.Dispose();
            }
            if (timer != null)
            {
                timer = null;
            }
            Recursion(count);
        }

        string s = string.Empty;
        private bool Execute(int count, string ss)
        {
            s += count.ToString() + ss;
            return true;
        }
        private void Repeat(RepeatAction<int, string> func, int counts, string sss)
        {
            int count = 0;
            while (count++ < 3)
            {
                func(counts, sss);
            }
        }
        int count = 5;
        /// <summary>
        /// 递归测试
        /// </summary>
        /// <param name="num"></param>
        private void Recursion(int num)
        {
            num--;
            if (num < 0)
                return;
            Recursion(num);
        }

        private void btnRepeat_Click(object sender, EventArgs e)
        {
            Repeat(Execute, 6, "hh");

            MessageBox.Show(s);
        }
        /// <summary>
        /// 设置半透明颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Color c1 = Color.FromArgb(50, Color.Blue);
            Color c2 = Color.FromArgb(200, Color.Blue);
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(c1), new Rectangle(0, 0, this.Width / 2, this.Height / 2));
            g.FillRectangle(new SolidBrush(c2), new Rectangle(this.Width / 2, this.Height / 2, this.Width / 2, this.Height / 2));
            g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(0, this.Height / 2, this.Width / 2, this.Height / 2));
            g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(this.Width / 2, 0, this.Width / 2, this.Height / 2));
        }
    }
    public delegate bool RepeatAction<T1, T2>(T1 obj1, T2 obj2);
}
