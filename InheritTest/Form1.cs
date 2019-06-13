using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InheritTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SubTestClass subTestClass = new SubTestClass() ;
            subTestClass.DoSomething();

            bool isNormal = IsNormal();

        }

        bool _isNormal = false;
        private bool IsNormal()
        {
            return _isNormal = true;
        }
    }
}
