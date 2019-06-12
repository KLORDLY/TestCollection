using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncBindingTest
{
    public partial class Form1 : Form
    {
        DataSource dataSource = new DataSource();
        public Form1()
        {
            InitializeComponent();
            AsyncBindingHelper.AddBinding(lblValue, "Text", (INotifyPropertyChanged)dataSource,"Value");
           
        }

        int a = 0;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            a++;
            dataSource.Value = a.ToString();
        }

    }
}
