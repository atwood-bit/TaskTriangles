using Microsoft.Extensions.Options;
using TaskTriangles.Exception;
using TaskTriangles.Models;
using TaskTriangles.Validation.Interfaces;

namespace TaskTriangles.Validation
{
    public class CoordinatesValidator : ICoordinatesValidator
    {
        private readonly AppSettings _appSettings;

        public CoordinatesValidator(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public void ValidateTriangleCoordinates(int[] trianglePointsArray, int lineNumber)
        {
            if (trianglePointsArray.Length != _appSettings.CoordinatesCount || 
                trianglePointsArray.Any(x => x < _appSettings.MinRangeOfCoordinate || x > _appSettings.MaxRangeOfCoordinate))
            {
                throw new IncorrectCoordinateException(lineNumber);
            }
        }
    }
}
