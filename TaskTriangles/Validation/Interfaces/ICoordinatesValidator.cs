namespace TaskTriangles.Validation.Interfaces
{
    public interface ICoordinatesValidator
    {
        void ValidateTriangleCoordinates(int[] trianglePointsArray, int lineNumber);
    }
}
