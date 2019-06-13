using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SelectMyFonts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string sourcePath = @"F:\Font\字体";
        string destPath = @"F:\Font\字体大宝\";
        int count = 0;
        private void btnStart_Click(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);

            foreach (DirectoryInfo di in dirInfo.GetDirectories())
            {
                foreach (DirectoryInfo dii in di.GetDirectories())
                {
                    foreach (FileInfo fi in dii.GetFiles("*.ttf"))//|*.TTF
                    {
                        fi.CopyTo(destPath + fi.Name, true);
                        count++;
                    }
                }
            }
            txtSuccess.Text = count.ToString();
        }
    }
}
