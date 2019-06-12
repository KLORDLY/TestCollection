using Emgu.CV;
using Emgu.CV.Structure;
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
    public partial class GrayForm : Form
    {
        string filePath = System.Windows.Forms.Application.StartupPath + "\\Resources\\Cat.png";
        public GrayForm()
        {
            InitializeComponent();
            Image<Bgr, Byte> src = new Image<Bgr, Byte>(filePath);
            Image<Gray, Single> dst = src.Convert<Gray, Single>();
            picBoxSrc.Image = src.ToBitmap();
            picBoxDst.Image = dst.ToBitmap();
        }
    }
}
