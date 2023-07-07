using System;
using TaskTriangles.Extensions;
using TaskTriangles.Models;

namespace TaskTriangles.Views
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();
            var points = new Point[3]
            {
                new Point { X = 150, Y = 30 },
                new Point { X = 120, Y = 500 },
                new Point { X = 10, Y = 500 },
            };
            var points2 = new Point[3]
            {
                new Point { X = 140, Y = 35 },
                new Point { X = 110, Y = 450 },
                new Point { X = 30, Y = 450 },
            };
            var q = 10 / 100.0;
            var color = Color.Green;
            g.FillTriangle(points, color, 10/100.0);
            g.FillTriangle(points2, color, 70 / 100.0);
        }
    }
}