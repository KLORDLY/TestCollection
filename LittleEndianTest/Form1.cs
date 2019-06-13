using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LittleEndianTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 将short类型转为bytes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConvert_Click(object sender, EventArgs e)
        {
            byte[] bytes = BitConverter.GetBytes(short.Parse(textBox1.Text));
            short s = BitConverter.ToInt16(bytes, 0);
            this.textBox2.Text = string.Format("{0}:{1}", bytes[0], bytes[1]);
        }
    }
}
