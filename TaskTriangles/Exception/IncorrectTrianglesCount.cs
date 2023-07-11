namespace TaskTriangles.Exception
{
    public class IncorrectTrianglesCount : System.Exception
    {
        public IncorrectTrianglesCount(int maxRange, int minRange, int count) : base($"Triangles count ({count}) is less than {minRange} or better than {maxRange}")
        {

        }
    }
}
