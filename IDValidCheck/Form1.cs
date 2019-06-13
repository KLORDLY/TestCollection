using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDValidCheck
{
    public partial class Form1 : Form
    {
        AlgorithmClass algorithmClass = new AlgorithmClass();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInput1.Text))
            {
                if (algorithmClass.IsValid(txtInput1.Text))
                {
                    MessageBox.Show("身份证号码有效");
                }
                else
                {
                    MessageBox.Show("身份证号码无效！");
                }
            }
            else
            {
                MessageBox.Show("请输入身份证号码！");
            }
        }

        private void btnCaculate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInput2.Text))
            {
                MessageBox.Show(string.Format("身份证号码的最后一位是:{0}", algorithmClass.GetLastPosition(txtInput2.Text)));
            }
            else
            {
                MessageBox.Show("请输入身份证号码前17位！");
            }
        }
    }
}
