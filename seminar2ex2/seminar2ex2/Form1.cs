using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace seminar2ex2
{
    public partial class Form1 : Form
    {
        private List<PointF> points;
        private List<PointF> minimalTriangle;

        public Form1()      
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(Form1_Paint);
            points = new List<PointF>
            {
                new PointF(10, 10),
                new PointF(20, 30),
                new PointF(50, 50),
                new PointF(70, 80),
                new PointF(90, 20),
                new PointF(40, 60)
            };

            minimalTriangle = FindMinimalTriangle(points);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Desenarea punctelor
            foreach (var point in points)
            {
                e.Graphics.FillEllipse(Brushes.Blue, point.X - 2, point.Y - 2, 4, 4);
            }

            // Desenarea triunghiului de arie minimă
            if (minimalTriangle.Count == 3)
            {
                e.Graphics.DrawPolygon(Pens.Red, minimalTriangle.ToArray());
            }
        }

        private List<PointF> FindMinimalTriangle(List<PointF> points)
        {
            var sortedPoints = points.OrderBy(p => p.X * p.Y).ToList();
            List<PointF> result = new List<PointF>();
            double minArea = double.MaxValue;

            for (int i = 0; i < sortedPoints.Count - 2; i++)
            {
                for (int j = i + 1; j < sortedPoints.Count - 1; j++)
                {
                    for (int k = j + 1; k < sortedPoints.Count; k++)
                    {
                        double area = CalculateTriangleArea(sortedPoints[i], sortedPoints[j], sortedPoints[k]);
                        if (area < minArea)
                        {
                            minArea = area;
                            result = new List<PointF> { sortedPoints[i], sortedPoints[j], sortedPoints[k] };
                        }
                    }
                }
            }

            return result;
        }

        private double CalculateTriangleArea(PointF p1, PointF p2, PointF p3)
        {
            return Math.Abs((p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y)) / 2.0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}


    
