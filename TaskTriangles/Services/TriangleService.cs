using TaskTriangles.Models;
using TaskTriangles.Services.Interfaces;
using TaskTriangles.Validation.Interfaces;

namespace TaskTriangles.Services
{
    public class TriangleService : IFigureService
    {
        private readonly ICoordinatesValidator _triangleValidator;
        private readonly IInputFileValidator _inputFileValidator;
        private bool _isTrianglesIntersect = false;

        public TriangleService(ICoordinatesValidator triangleValidator, IInputFileValidator inputFileValidator)
        {
            _triangleValidator = triangleValidator;
            _inputFileValidator = inputFileValidator;
        }

        public async Task<ResultModel> BuildTree(string filePath)
        {
            await _inputFileValidator.ValidateInputFile(filePath);
            var resultModel = new ResultModel();
            var lines = await File.ReadAllLinesAsync(filePath);
            var declaredNumberTriangles = Convert.ToInt32(lines.First());

            var triangles = lines.Skip(1)
                .AsParallel()
                .Select(MapLineToTrianglePoints)
                .ToList();

            _inputFileValidator.ValidateTrianglesCount(triangles.Count);

            if (declaredNumberTriangles != triangles.Count)
            {
                resultModel.WarningMessage = $"Declared number of triangles ({declaredNumberTriangles}) in file not equal with real triangles count ({triangles.Count})";
            }

            foreach (var triangle in triangles)
            {
                triangle.DepthLevel = FindTriangleLevel(triangle, triangles);
            }

            triangles = triangles.OrderBy(x => x.DepthLevel).ToList();
            resultModel.TrianglesCount = triangles.Count;
            resultModel.ShadesCount = triangles.Last()?.DepthLevel;
            resultModel.Items = triangles;
            resultModel.IsTrianglesIntersect = _isTrianglesIntersect;
            _isTrianglesIntersect = false;

            return resultModel;
        }

        private Triangle MapLineToTrianglePoints(string line, int index)
        {
            var triangleCoordinates = line.Trim().Split(" ").Select(x => Convert.ToInt32(x)).ToArray();

            var lineNumer = index + 1;
            _triangleValidator.ValidateTriangleCoordinates(triangleCoordinates, lineNumer);

            var trianglePoints = triangleCoordinates.Where((x, index) => index % 2 == 0)
                .Select((x, index) =>
                    new Point
                    {
                        X = x,
                        Y = triangleCoordinates[index * 2 + 1]
                    })
                .ToArray();

            return new Triangle(trianglePoints, lineNumer);
        }

        private int FindTriangleLevel(Triangle triangle, List<Triangle> allTriangles)
        {
            if (triangle.DepthLevel.HasValue)
            {
                return triangle.DepthLevel.Value;
            }

            var parents = allTriangles.Where(t => IsTriangleNestedInAnother(triangle, t));
            var triangleLevel = 0;

            foreach (var parent in parents)
            {
                if (!parent.DepthLevel.HasValue)
                {
                    triangleLevel = FindTriangleLevel(parent, allTriangles);
                }
                else if (triangleLevel < parent.DepthLevel.Value)
                {
                    triangleLevel = parent.DepthLevel.Value;
                }
            }

            return triangleLevel + 1;
        }

        private bool IsTriangleNestedInAnother(Triangle first, Triangle second)
        {
            if (first.Equals(second)) return false;

            if (first.Points.Length != second.Points.Length)
            {
                throw new System.Exception($"Incorrect points at Triangle with Id {first.Id} or {second.Id}");
            }

            var isTriangleNestedInAnother = false;
            for (int i = 0; i < first.Points.Length; i++)
            {
                var vectorProducts = CalculateVectorProducts(first.Points[i], second.Points);

                isTriangleNestedInAnother = IsAllVectorProductsPositiveOrNegative(vectorProducts);

                if (!_isTrianglesIntersect)
                {
                    var nextPointIndex = (i + 1) % first.Points.Length;
                    IsSegmentsIntersect(first.Points[i], first.Points[nextPointIndex], second.Points[i], second.Points[nextPointIndex]);
                }
            }

            return isTriangleNestedInAnother;
        }

        private int[] CalculateVectorProducts(Point point, Point[] trianglePoints)
        {
            var vectorProductA = GetVectorProduct(trianglePoints[0], trianglePoints[1], point);
            var vectorProductB = GetVectorProduct(trianglePoints[1], trianglePoints[2], point);
            var vectorProductC = GetVectorProduct(trianglePoints[2], trianglePoints[0], point);

            return new int[] { vectorProductA, vectorProductB, vectorProductC };
        }

        private int GetVectorProduct(Point trianglePointA, Point trianglePointB, Point c)
        {
            int x1 = trianglePointA.X - c.X,
                y1 = trianglePointB.Y - trianglePointA.Y,
                x2 = trianglePointB.X - trianglePointA.X,
                y2 = trianglePointA.Y - c.Y;

            return x1 * y1 - x2 * y2;
        }

        private bool IsAllVectorProductsPositiveOrNegative(int[] vectorProducts)
        {
            return vectorProducts.All(x => x > 0) || vectorProducts.All(x => x < 0);
        }

        private void IsSegmentsIntersect(Point firstPointA, Point firstPointB, Point secondPointA, Point secondPointB)
        {
            _isTrianglesIntersect = 
                GetVectorProduct(firstPointA, firstPointB, secondPointA) * GetVectorProduct(firstPointA, firstPointB, secondPointB) <= 0 &&
                GetVectorProduct(secondPointA, secondPointB, firstPointA) * GetVectorProduct(secondPointA, secondPointB, firstPointB) <= 0;
        }
    }
}
