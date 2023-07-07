using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTriangles.Extensions
{
    public static class FigureExtensions
    {
        public static void FillTriangle(this Graphics graphic, Point[] points, Color color, double transparency)
        {
            var brush = new SolidBrush(color.ChangeColorBrightness(transparency));
            graphic.FillPolygon(brush, points);
        }
    }
}
