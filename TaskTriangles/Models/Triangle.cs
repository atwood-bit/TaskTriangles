using TaskTriangles.Models.Contracts;

namespace TaskTriangles.Models
{
    public class Triangle : IFigure
    {
        public int Id { get; set; }
        public Point[] Points { get; set; }
        public int? DepthLevel { get; set; }

        public Triangle(Point[] points, int id)
        {
            Points = points;
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Triangle another && Id == another.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
