using System;
using System.Drawing;
using System.Windows.Forms;
namespace seminar12
    /*Diagrama Voronoi a unei mulțimi de trei puncte în plan: determinarea centrului
cercului circumscris triunghiului determinat de cele trei puncte date și trasarea
mediatoarelor laturilor triunghiului*/
{
    public partial class Form1 : Form
    {
        private PointF[] points = new PointF[3]; // Vector pentru a stoca cele 3 puncte
        private PointF[] voronoiVertices = new PointF[3]; // Vector pentru a stoca vârfurile diagramei Voronoi
        private PointF circumcenter; // Centrul cercului circumscris
        private float circumradius; // Raza cercului circumscris

        public Form1()
        {
            InitializeComponent();
            panel1.MouseClick += panel1_MouseClick;
        }

       
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
           
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == PointF.Empty)
                {
                    points[i] = e.Location;
                    break;
                }
            }

            if (Array.TrueForAll(points, p => p != PointF.Empty))
            {
                CalculateCircumcircle();
                panel1.Invalidate();
            }

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (Array.TrueForAll(points, p => p != PointF.Empty))
            {
                // Se desenează muchiile diagramei Voronoi
                for (int i = 0; i < points.Length; i++)
                {
                    e.Graphics.DrawLine(Pens.Black, circumcenter, voronoiVertices[i]);
                }

                // Se desenează cercul circumscris
                e.Graphics.DrawEllipse(Pens.Blue, circumcenter.X - circumradius, circumcenter.Y - circumradius, circumradius * 2, circumradius * 2);

                // Se desenează punctele
                foreach (var point in points)
                {
                    e.Graphics.FillEllipse(Brushes.Red, point.X - 3, point.Y - 3, 6, 6);
                }
            }
        }
        // Metoda pentru calculul cercului circumscris
        private void CalculateCircumcircle()
        {
            // Se calculează mijloacele laturilor triunghiului
            PointF midAB = new PointF((points[0].X + points[1].X) / 2, (points[0].Y + points[1].Y) / 2);
            PointF midBC = new PointF((points[1].X + points[2].X) / 2, (points[1].Y + points[2].Y) / 2);

            // Se calculează pantele bisectoarelor perpendiculare
            float slopeAB = -(points[1].X - points[0].X) / (points[1].Y - points[0].Y);
            float slopeBC = -(points[2].X - points[1].X) / (points[2].Y - points[1].Y);

            // Se calculează centrele bisectoarelor perpendiculare
            PointF centerAB = new PointF(midAB.X + slopeAB * (points[0].Y - midAB.Y), midAB.Y + slopeAB * (midAB.X - points[0].X));
            PointF centerBC = new PointF(midBC.X + slopeBC * (points[1].Y - midBC.Y), midBC.Y + slopeBC * (midBC.X - points[1].X));

            // Se calculează centrul cercului circumscris
            circumcenter = Intersection(centerAB, slopeAB, centerBC, slopeBC);

            // Se calculează raza cercului circumscris
            circumradius = (float)Math.Sqrt(Math.Pow(circumcenter.X - points[0].X, 2) + Math.Pow(circumcenter.Y - points[0].Y, 2));

            // Se calculează vârfurile diagramei Voronoi
            for (int i = 0; i < points.Length; i++)
            {
                voronoiVertices[i] = new PointF(2 * circumcenter.X - points[i].X, 2 * circumcenter.Y - points[i].Y);
            }
        }

        private PointF Intersection(PointF p1, float m1, PointF p2, float m2)
        {
            float x = (m1 * p1.X - m2 * p2.X + p2.Y - p1.Y) / (m1 - m2);
            float y = m1 * (x - p1.X) + p1.Y;
            return new PointF(x, y);
        }
    }
}
