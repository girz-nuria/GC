using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TriangulationColoring
   /* 3-colorarea grafului asociat triangulării obținute(culorile se pot asocia în mod
unic pornind de la vîrfurile triunghiului rămas și apoi urechile determinate,
considerate în ordinea inversă eliminării)*/
{
    public partial class Form1 : Form
    {
        private List<Point> vertices = new List<Point>();
        private List<Tuple<int, int, int>> triangles = new List<Tuple<int, int, int>>();
        private List<int>[] adjacencyList;
        private Color[] colors;

        public Form1()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new MouseEventHandler(this.Form1_MouseClick);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            vertices.Add(new Point(e.X, e.Y));
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (vertices.Count > 2)
            {
                PerformTriangulation();
                ColorGraph();
                DrawTriangulation(g);
            }
            DrawPolygon(g);
        }

        private void DrawPolygon(Graphics g)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Point p1 = vertices[i];
                Point p2 = vertices[(i + 1) % vertices.Count];
                g.DrawLine(Pens.Black, p1, p2);
            }
        }

        private void PerformTriangulation()
        {
            triangles.Clear();
            List<int> indices = Enumerable.Range(0, vertices.Count).ToList();

            while (indices.Count > 3)
            {
                bool earFound = false;
                for (int i = 0; i < indices.Count; i++)
                {
                    int prev = indices[(i + indices.Count - 1) % indices.Count];
                    int curr = indices[i];
                    int next = indices[(i + 1) % indices.Count];

                    if (IsEar(prev, curr, next))
                    {
                        triangles.Add(Tuple.Create(prev, curr, next));
                        indices.RemoveAt(i);
                        earFound = true;
                        break;
                    }
                }
                if (!earFound)
                {
                    throw new InvalidOperationException("Nu am găsit nicio ureche. Poligonul nu este simplu.");
                }
            }

            triangles.Add(Tuple.Create(indices[0], indices[1], indices[2]));
        }

        private bool IsEar(int i, int j, int k)
        {
            Point a = vertices[i];
            Point b = vertices[j];
            Point c = vertices[k];

            if (Orientation(a, b, c) != -1)
                return false;

            for (int m = 0; m < vertices.Count; m++)
            {
                if (m == i || m == j || m == k)
                    continue;

                if (IsPointInTriangle(vertices[m], a, b, c))
                    return false;
            }

            return true;
        }

        private int Orientation(Point p, Point q, Point r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0) return 0;
            return (val > 0) ? 1 : -1;
        }

        private bool IsPointInTriangle(Point p, Point a, Point b, Point c)
        {
            bool b1 = Sign(p, a, b) < 0.0f;
            bool b2 = Sign(p, b, c) < 0.0f;
            bool b3 = Sign(p, c, a) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        private float Sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        private void DrawTriangulation(Graphics g)
        {
            foreach (var tri in triangles)
            {
                Point p1 = vertices[tri.Item1];
                Point p2 = vertices[tri.Item2];
                Point p3 = vertices[tri.Item3];
                g.DrawLine(Pens.Red, p1, p2);
                g.DrawLine(Pens.Red, p2, p3);
                g.DrawLine(Pens.Red, p3, p1);
            }
        }

        private void ColorGraph()
        {
            int n = vertices.Count;
            adjacencyList = new List<int>[n];
            for (int i = 0; i < n; i++)
                adjacencyList[i] = new List<int>();

            foreach (var tri in triangles)
            {
                adjacencyList[tri.Item1].Add(tri.Item2);
                adjacencyList[tri.Item1].Add(tri.Item3);
                adjacencyList[tri.Item2].Add(tri.Item1);
                adjacencyList[tri.Item2].Add(tri.Item3);
                adjacencyList[tri.Item3].Add(tri.Item1);
                adjacencyList[tri.Item3].Add(tri.Item2);
            }

            colors = new Color[n];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.White;

            bool ColorVertex(int v, Color color)
            {
                if (colors[v] != Color.White)
                    return colors[v] == color;
                colors[v] = color;
                foreach (var neighbor in adjacencyList[v])
                {
                    if (!ColorVertex(neighbor, GetDifferentColor(colors[v])))
                        return false;
                }
                return true;
            }

            Color GetDifferentColor(Color c)
            {
                if (c == Color.Red) return Color.Green;
                if (c == Color.Green) return Color.Blue;
                return Color.Red;
            }

            for (int i = 0; i < n; i++)
            {
                if (colors[i] == Color.White)
                {
                    ColorVertex(i, Color.Red);
                }
            }
        }
    }
}
