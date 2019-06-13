using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObjRefTest
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        public Form1()
        {
            InitializeComponent();
            timer.Enabled = true;
            timer.Interval = 500;
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (OnTimeChangedEvent != null)
                OnTimeChangedEvent();
        }
        public event OnTimeChangedEventHandler OnTimeChangedEvent = null;

        private void button1_Click(object sender, EventArgs e)
        {
            using (Form2 f2 = new Form2(ref OnTimeChangedEvent))
            {
                f2.ShowDialog();
            }
        }
    }
    public delegate void OnTimeChangedEventHandler();
}
