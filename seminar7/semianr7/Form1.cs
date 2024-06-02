using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace semianr7
    /*Partiționarea unui poligon simplu cu n>3 vârfuri în triunghiuri(triangularea)
folosind diagonalele.*/
{
    public partial class Form1 : Form
    {
        private List<Point> polygonPoints;
        private List<Tuple<Point, Point, Point>> triangles;
        public Form1()
        {
            InitializeComponent();
            polygonPoints = new List<Point>();
            triangles = new List<Tuple<Point, Point, Point>>();
            this.panel1.Paint += new PaintEventHandler(Panel1_Paint);
            this.panel1.MouseClick += new MouseEventHandler(Panel1_MouseClick);
            this.button1.Click += new EventHandler(button1_Click);
        }
        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            polygonPoints.Add(new Point(e.X, e.Y));
            panel1.Invalidate();
        }
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (polygonPoints.Count > 1)
            {
                e.Graphics.DrawPolygon(Pens.Black, polygonPoints.ToArray());
            }
            foreach (var triangle in triangles)
            {
                Point[] points = new Point[] { triangle.Item1, triangle.Item2, triangle.Item3 };
                e.Graphics.DrawPolygon(Pens.Red, points);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (polygonPoints.Count > 2)
            {
                TriangulatePolygon();
                listBox1.Items.Clear();
                foreach (var triangle in triangles)
                {
                    listBox1.Items.Add($"({triangle.Item1}, {triangle.Item2}, {triangle.Item3})");
                }
                panel1.Invalidate();
            }
        }
        private void TriangulatePolygon()
        {
            triangles.Clear();
            List<Point> points = new List<Point>(polygonPoints);
            while (points.Count > 3)
            {
                bool earFound = false;
                for (int i = 0; i < points.Count; i++)
                {
                    int prevIndex = (i == 0) ? points.Count - 1 : i - 1;
                    int nextIndex = (i == points.Count - 1) ? 0 : i + 1;
                    Point prevPoint = points[prevIndex];
                    Point currPoint = points[i];
                    Point nextPoint = points[nextIndex];

                    if (IsEar(prevPoint, currPoint, nextPoint, points))
                    {
                        triangles.Add(Tuple.Create(prevPoint, currPoint, nextPoint));
                        points.RemoveAt(i);
                        earFound = true;
                        break;
                    }
                }
                if (!earFound)
                {
                    break; 
                }
            }
            if (points.Count == 3)
            {
                triangles.Add(Tuple.Create(points[0], points[1], points[2]));
            }
        }

        private bool IsEar(Point p1, Point p2, Point p3, List<Point> points)
        {
            if (GetOrientation(p1, p2, p3) != -1)
            {
                return false;
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] != p1 && points[i] != p2 && points[i] != p3 &&
                    IsPointInTriangle(points[i], p1, p2, p3))
                {
                    return false;
                }
            }
            return true;
        }

        private int GetOrientation(Point p1, Point p2, Point p3)
        {
            int val = (p2.Y - p1.Y) * (p3.X - p2.X) - (p2.X - p1.X) * (p3.Y - p2.Y);
            if (val == 0)
            {
                return 0; // collinear
            }
            return (val > 0) ? 1 : -1; // clock or counterclock wise
        }

        private bool IsPointInTriangle(Point pt, Point v1, Point v2, Point v3)
        {
            bool b1, b2, b3;

            b1 = Sign(pt, v1, v2) < 0.0f;
            b2 = Sign(pt, v2, v3) < 0.0f;
            b3 = Sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        private float Sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }
    }

}
