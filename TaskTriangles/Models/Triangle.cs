using TaskTriangles.Models.Contracts;

namespace TaskTriangles.Models
{
    public class Triangle : IFigure
    {
        public Point[] Points { get; set; }
        public int? Level { get; set; }

        public Triangle(Point[] points)
        {
            Points = points;
        }
    }
}
