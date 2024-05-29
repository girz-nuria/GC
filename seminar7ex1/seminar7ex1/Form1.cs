using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace seminar7ex1
    //Partiționarea unui poligon simplu cu n>3 vârfuri în triunghiuri (triangularea)
//folosind diagonalele
{
    public partial class Form1 : Form
    {
        private List<Point> polygon = new List<Point>();
        private List<List<Point>> triangles = new List<List<Point>>();
        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseClick += PictureBox1_MouseClick;
        }
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            // Adăugăm vârful poligonului la fiecare clic al mouse-ului
            polygon.Add(e.Location);
            pictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Verificăm dacă avem cel puțin 3 vârfuri pentru a triangula poligonul
            if (polygon.Count < 3)
            {
                MessageBox.Show("Poligonul trebuie să aibă cel puțin 3 vârfuri.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Triangulăm poligonul
            triangles = TriangulatePolygon(polygon);
            pictureBox1.Refresh();

        }
        private List<List<Point>> TriangulatePolygon(List<Point> polygon)
        {
            List<List<Point>> triangles = new List<List<Point>>();

            // Verificăm dacă poligonul are mai mult de 3 vârfuri
            if (polygon.Count >= 3)
            {
                for (int i = 1; i < polygon.Count - 1; i++)
                {
                    // Construim un triunghi folosind vârful inițial și cele două vârfuri consecutive
                    List<Point> triangle = new List<Point> { polygon[0], polygon[i], polygon[i + 1] };
                    triangles.Add(triangle);
                }
            }

            return triangles;
        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Desenăm poligonul
            if (polygon.Count >= 3)
            {
                e.Graphics.DrawPolygon(Pens.Black, polygon.ToArray());
            }

            // Desenăm triunghiurile rezultate din triangularea poligonului
            foreach (var triangle in triangles)
            {
                e.Graphics.DrawPolygon(Pens.Red, triangle.ToArray());
            } 

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
