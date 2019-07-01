using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModifyImages
{
    public partial class Form1 : Form
    {
        private string directory = "C:\\Users\\wangzhe\\Desktop\\动画跳绳";
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                Image image1 = Bitmap.FromFile(string.Format("{0}\\{1}.png", directory, i + 1));
                Image image2 = Bitmap.FromFile(string.Format("{0}\\{1}.png", directory, i + 2));
                Bitmap image3 = new Bitmap(image1.Width, image1.Height);
                Graphics g = Graphics.FromImage(image3);
                System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();
                attributes.SetColorKey(image3.GetPixel(100, 100), image3.GetPixel(10, 10));
                g.DrawImage(image2, new Rectangle(0, 0, image3.Width, image3.Height / 2), 0, 0, image2.Width, image2.Height / 2, GraphicsUnit.Pixel, attributes);
                g.DrawImage(image1, new Rectangle(0, image3.Height / 2, image3.Width, image3.Height / 2), 0, image1.Height / 2, image1.Width, image1.Height / 2, GraphicsUnit.Pixel);
                g.DrawString((i + 1).ToString(), DefaultFont, new SolidBrush(Color.Black), 0, 0);
                g.Dispose();
                image3.Save(string.Format("{0}\\{1}.png", directory, i + 201));
            }
        }

    }
}
