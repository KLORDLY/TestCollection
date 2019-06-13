using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloatTest
{
    public partial class Form1 : Form
    {
        float a = -0.1f;
        int b = 11;
        public Form1()
        {
            InitializeComponent();
            float aa =  a/b;
            MessageBox.Show(string.Format("float {0}/int {1}={2}" ,a,b,aa));
        }
    }
}
