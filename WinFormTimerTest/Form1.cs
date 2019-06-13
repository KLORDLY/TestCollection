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

namespace WinFormTimerTest
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        public Form1()
        {
            InitializeComponent();

            NumberFormatInfo myCultureInfo = new NumberFormatInfo();

            myCultureInfo.NaNSymbol = "aa";

            timer.Tick += timer_Tick;
            timer.Enabled = true;
            timer.Interval = 5000;

            string a = double.NaN.ToString(myCultureInfo);
            string aa = double.MinValue.ToString();

            //double d = double.Parse(a, CultureInfo.InvariantCulture);
            //CultureInfo.CurrentCulture.NumberFormat.NaNSymbol

            //double d=double.Parse("NAN",
            //System.Threading.Thread.Sleep(5000);
        }

        int count = 0;
        void timer_Tick(object sender, EventArgs e)
        {
            count++;
            timer.Enabled = false;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            timer.Interval = 10000;
        }
    }
}
