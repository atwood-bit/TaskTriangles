using TaskTriangles.Exception;
using TaskTriangles.Validation.Interfaces;

namespace TaskTriangles.Validation
{
    public class CoordinatesValidator : ICoordinatesValidator
    {
        private const int COORDINATES_COUNT = 6;
        private const int MAX_RANGE_COORDINATE = 1000;

        public void ValidateTriangleCoordinates(int[] trianglePointsArray, int lineNumber)
        {
            if (trianglePointsArray.Length != COORDINATES_COUNT || trianglePointsArray.Any(x => x < 0 || x > MAX_RANGE_COORDINATE))
            {
                throw new IncorrectCoordinateException(lineNumber);
            }
        }
    }
}
