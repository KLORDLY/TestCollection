using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawRingTest
{
    public class ucRing : UserControl
    {
        private Bitmap background = new Bitmap(400, 400);
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Graphics g = Graphics.FromImage(background);
            Point center = new Point(background.Width / 2, background.Height / 2);
            int radius = 160;
            int width = 20;
            int correctDegree = 0;
            List<Point> pointsList = new List<Point>();
            List<Point> pointListOutside = new List<Point>();
            for (int i = 25; i < 155; i++)
            {
                pointsList.Add(new Point(center.X + (int)(radius * Math.Cos(Math.PI * (i + correctDegree) / 180)), (int)(center.Y + radius * Math.Sin(Math.PI * (i + correctDegree) / 180))));
            }
            for (int i = 30; i < 150; i++)
            {
                pointListOutside.Add(new Point(center.X + (int)((radius + width) * Math.Cos(Math.PI * (i + correctDegree) / 180)), (int)(center.Y + (radius + width) * Math.Sin(Math.PI * (i + correctDegree) / 180))));
            }
            pointsList.Add(center);
            pointListOutside.Add(center);
            g.FillPolygon(new SolidBrush(Color.LimeGreen), pointListOutside.ToArray());
            g.FillPolygon(new SolidBrush(Color.Black), pointsList.ToArray());
            ImageAttributes attribute = new ImageAttributes();
            attribute.SetColorKey(background.GetPixel(0, 0), background.GetPixel(0, 0));
            e.Graphics.DrawImage(background, new Rectangle(0, 0, background.Width, background.Height), 0, 0, background.Width, background.Height, GraphicsUnit.Pixel, attribute);
            g.Dispose();
        }
    }
}
