using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnumDescriptionTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TestEnumDescription test = new TestEnumDescription();
            txt.Text = test.Text;

            string strTmp = "abcdefg某某某";
            int i = System.Text.Encoding.Default.GetBytes(strTmp).Length;

        }
    }
}
