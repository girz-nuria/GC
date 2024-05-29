using System;
using System.Drawing;
using System.Windows.Forms;

namespace seminar6ex3
//Triangularea unui poligon convex
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Size = this.ClientSize;
            pictureBox1.Paint += new PaintEventHandler(PictureBox1_Paint);
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Definim vârfurile unui poligon convex
            PointF[] vertices = new PointF[]
            {
                new PointF(100, 200),
                new PointF(200, 100),
                new PointF(300, 150),
                new PointF(400, 300),
                new PointF(250, 400),
                new PointF(150, 350)
            };

            // Desenăm poligonul
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawPolygon(Pens.Black, vertices);

            // Triangulăm poligonul
            TriangulateAndDraw(e.Graphics, vertices);

        }
        private void TriangulateAndDraw(Graphics g, PointF[] vertices)
        {
            int n = vertices.Length;
            for (int i = 1; i < n - 1; i++)
            {
                g.DrawLine(Pens.Red, vertices[0], vertices[i]);
                g.DrawLine(Pens.Red, vertices[i], vertices[i + 1]);
            }
        }
    }
}
