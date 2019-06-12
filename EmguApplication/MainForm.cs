using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmguApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnGray_Click(object sender, EventArgs e)
        {
            using (GrayForm grayForm = new GrayForm())
            {
                grayForm.ShowDialog();
            }
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {

        }

        private void btnEyeDetect_Click(object sender, EventArgs e)
        {

        }
    }
}
