using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoRestEventTest
{
    public partial class MainForm : Form
    {
        Reader me = new Reader();
        public MainForm()
        {
            InitializeComponent();
            me.MainProc();
            System.Threading.Thread.Sleep(100);
            txtShow.Text = me.textToShow;
        }
    }
}
