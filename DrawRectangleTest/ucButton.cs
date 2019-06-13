using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawRectangleTest
{
    public class ucButton:Control
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            int penWidth = 2;
            e.Graphics.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(0, 0, this.Width, this.Height));

            Pen pen = new Pen(Color.Red, penWidth);
            //e.Graphics.DrawRectangle(new Pen(Color.Red, penWidth), new Rectangle(penWidth / 2, penWidth / 2, this.Width - ((int)pen.Width + 1) / 2 - penWidth / 2 - (penWidth + 1) % 2, this.Height - ((int)pen.Width + 1) / 2 - penWidth / 2 - (penWidth + 1) % 2));
            e.Graphics.DrawRectangle(new Pen(Color.Red, penWidth), new Rectangle(penWidth / 2, penWidth / 2, this.Width - penWidth, this.Height - penWidth));
            
            //e.Graphics.DrawLine(pen, 0, this.Height / 2, this.Width, this.Height / 2);
        }
    }
}
