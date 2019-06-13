using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OnPaintRuntimeTest
{
    public class MyButton : Button
    {
        public MyButton()
        {

        }

        string text = string.Empty;
        public override string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Bitmap image = new Bitmap(this.Width, this.Height);
            Graphics gImage = Graphics.FromImage(image);
            gImage.Clear(Color.Red);

            e.Graphics.DrawImage(image, 0, 0);
            gImage.Dispose();
            image.Dispose();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //TO DO NOTHING HERE.
            //base.OnPaintBackground(e);
        }
    }
}
