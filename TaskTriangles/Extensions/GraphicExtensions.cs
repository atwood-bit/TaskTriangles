using System;
namespace TaskTriangles.Extensions
{
    public static class GraphicExtensions
    {
        public static void FillTriangle(this Graphics graphic, Point[] points, Color color, double transparency)
        {
            var brush = new SolidBrush(color.ChangeColorBrightness(transparency));
            graphic.FillPolygon(brush, points, System.Drawing.Drawing2D.FillMode.Alternate);
        }
    }
}
