using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestStringEmpty
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {
                string s = null;
                string s1 = string.Empty;
                string s2 = " ";
                string s3 = "a";
                s3 = s3 + s2;
                string[] ss = new string[0];
                ss[0] = s3;

                //NaN测试
                float min = float.NaN;
                if (float.IsNaN(min))
                {
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
