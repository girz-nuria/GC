using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace s4e2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, PaintEventArgs e)
        {
            
        }
        static float D(PointF p1, PointF p2, PointF p3)
        {
            return (float)((p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y) - (p3.X * p2.Y + p2.X * p1.Y + p1.X * p3.Y));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p1 = new Pen(Color.DarkOliveGreen, 3), p2 = new Pen(Color.MidnightBlue, 2);
            Random rnd = new Random();
            int num = rnd.Next(20, 50);
            List<PointF> points = new List<PointF>(num);
            float raza = 1;

            for (int i = 0; i < num; i += 1)
            {
                PointF x = new PointF();
                x.X = rnd.Next(50, this.Width - 50);
                x.Y = rnd.Next(50, this.Height - 50);
                points.Add(x);
                g.DrawEllipse(p1, points[i].X - raza, points[i].Y - raza, raza * 2, raza * 2);
            }

            PointF slay = new PointF();

            for (int i = 0; i < num - 1; i += 1)
                for (int j = i + 1; j < num; j += 1)
                {
                    if (points[i].X > points[j].X)
                    {
                        slay = points[i];
                        points[i] = points[j];
                        points[j] = slay;
                    }
                    else if (points[i].X == points[j].X)
                        if (points[i].Y < points[j].Y)
                        {
                            slay = points[i];
                            points[i] = points[j];
                            points[j] = slay;
                        }
                }
            List<PointF> upperBorder = new List<PointF>();

            upperBorder.Add(points[0]);
            upperBorder.Add(points[1]);

            for (int i = 2; i < num; i += 1)
            {
                upperBorder.Add(points[i]);
                while (upperBorder.Count > 2 && D(upperBorder[upperBorder.Count - 3], upperBorder[upperBorder.Count - 2], upperBorder[upperBorder.Count - 1]) < 0)
                    upperBorder.Remove(upperBorder[upperBorder.Count - 2]);
            }

            List<PointF> lowerBorder = new List<PointF>();

            lowerBorder.Add(points[num - 1]);
            lowerBorder.Add(points[num - 2]);

            for (int i = num - 3; i >= 0; i -= 1)
            {
                lowerBorder.Add(points[i]);
                while (lowerBorder.Count > 2 && D(lowerBorder[lowerBorder.Count - 1], lowerBorder[lowerBorder.Count - 2], lowerBorder[lowerBorder.Count - 3]) > 0)
                    lowerBorder.Remove(lowerBorder[lowerBorder.Count - 2]);
            }

            for (int i = 0; i < upperBorder.Count - 1; i += 1)
                g.DrawLine(p2, upperBorder[i], upperBorder[i + 1]);

            for (int i = 0; i < lowerBorder.Count - 1; i += 1)
                g.DrawLine(p2, lowerBorder[i], lowerBorder[i + 1]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

