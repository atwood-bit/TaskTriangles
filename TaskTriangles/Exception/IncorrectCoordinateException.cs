namespace TaskTriangles.Exception
{
    public class IncorrectCoordinateException : System.Exception
    {
        public IncorrectCoordinateException(int lineNumber) : base($"Incorrect coordinates at line with index {lineNumber}")
        {

        }
    }
}
