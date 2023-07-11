namespace TaskTriangles.Extensions
{
    public static class GraphicExtensions
    {
        public static void FillTriangle(this Graphics graphic, Point[] points, Color color, double transparency, bool drawBorder, int borderWidth = 1)
        {
            var brush = new SolidBrush(color.ChangeColorBrightness(transparency));
            graphic.FillPolygon(brush, points, System.Drawing.Drawing2D.FillMode.Alternate);

            if (drawBorder)
            {
                var myPen = new Pen(Color.Black, borderWidth);
                graphic.DrawPolygon(myPen, points);
            }
        }
    }
}
