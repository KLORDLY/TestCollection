using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaticConstructorTest
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            lblX.Text = TestClassA.x.ToString();
            lblY.Text = TestClassB.y.ToString();
        }
    }
}
