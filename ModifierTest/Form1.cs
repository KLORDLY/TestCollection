using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModifierTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int number = new ClassB().G(new ClassA());
        }
    }
}
