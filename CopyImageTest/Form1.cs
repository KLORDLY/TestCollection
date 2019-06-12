using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CopyImageTest
{
    public partial class Form1 : Form
    {
        //string path = System.Environment.CurrentDirectory;
        Bitmap _Additel = new Bitmap(200, 333);
        Bitmap resAdditel;
        public Form1()
        {
            InitializeComponent();

            //if (File.Exists(path + "\\Additel.bmp"))
            //{
            //    Graphics gAdditel = Graphics.FromImage(_Additel);
            //    FileStream fs = new FileStream(path + "\\Additel.bmp", FileMode.Open);
            //    try
            //    {
            //        resAdditel = new Bitmap(fs);
            //    }
            //    catch (Exception ee)
            //    {
            //    }
            //    gAdditel.DrawImage(resAdditel, new Rectangle(0, 0, _Additel.Width, _Additel.Height),
            //        new Rectangle(0, 0, resAdditel.Width, resAdditel.Height), GraphicsUnit.Pixel);
            //    resAdditel.Dispose();
            //    gAdditel.Dispose();
            //}

            try
            {
                resAdditel = Properties.Resources.Additel;
            }
            catch (Exception ee)
            {
            }
            Graphics gAdditel = Graphics.FromImage(_Additel);
            gAdditel.DrawImage(resAdditel, new Rectangle(0, 0, _Additel.Width, _Additel.Height),
                new Rectangle(0, 0, resAdditel.Width, resAdditel.Height), GraphicsUnit.Pixel);
            resAdditel.Dispose();
            gAdditel.Dispose();
        }
    }
}
