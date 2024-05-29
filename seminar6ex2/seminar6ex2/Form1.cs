
using System;
using System.Drawing;
using System.Windows.Forms;

namespace seminar6ex2
//Reprezentarea grafică a unui poligon simplu cu n vârfuri (prin desenare)
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
            // Setăm dimensiunea PictureBox-ului la dimensiunea formei
            pictureBox1.Size = this.ClientSize;
            // Adăugăm evenimentul Paint
            pictureBox1.Paint += new PaintEventHandler(PictureBox1_Paint);
        }

        

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Numărul de vârfuri ale poligonului
            int n = 5; 

            // Dimensiunea și centrul desenului
            int radius = 100;
            Point center = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);

            // Calculăm vârfurile poligonului
            PointF[] points = new PointF[n];
            for (int i = 0; i < n; i++)
            {
                double angle = i * 2 * Math.PI / n;
                points[i] = new PointF(
                    center.X + (float)(radius * Math.Cos(angle)),
                    center.Y + (float)(radius * Math.Sin(angle))
                );
            }

            // Desenăm poligonul
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawPolygon(Pens.Black, points);
        }
    }
}
