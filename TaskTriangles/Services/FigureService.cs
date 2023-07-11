using TaskTriangles.Models;
using TaskTriangles.Services.Interfaces;
using TaskTriangles.Validation.Interfaces;

namespace TaskTriangles.Services
{
    public class FigureService : IFigureService
    {
        private readonly ICoordinatesValidator _triangleValidator;
        private readonly IInputFileValidator _inputFileValidator;
        private bool _isTrianglesIntersect = false;

        public FigureService(ICoordinatesValidator triangleValidator, IInputFileValidator inputFileValidator)
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
                var pointA = first.Points[i];
                var vectorProductsPointA = CalculateVectorProducts(pointA, second.Points);

                isTriangleNestedInAnother = IsAllVectorProductsPositiveOrNegative(vectorProductsPointA);

                if (!_isTrianglesIntersect)
                {
                    var nextI = (i + 1) % first.Points.Length;
                    var pointB = first.Points[nextI];
                    CheckIsTriangleSegmentIntersectWithAnother(vectorProductsPointA, pointB, second);
                }
            }

            return isTriangleNestedInAnother;
        }

        private int[] CalculateVectorProducts(Point point, Point[] trianglePoints)
        {
            var vectorProductA = GetVectorProduct(trianglePoints[0], trianglePoints[1], point);
            var vectorProductB = GetVectorProduct(trianglePoints[1], trianglePoints[2], point);
            var vectorProductC = GetVectorProduct(trianglePoints[2], trianglePoints[0], point);
            //var vectorProductA = GetVectorProduct
            //    (trianglePoints[0].X - point.X,
            //    trianglePoints[1].Y - trianglePoints[0].Y,
            //    trianglePoints[1].X - trianglePoints[0].X,
            //    trianglePoints[0].Y - point.Y);
            //var vectorProductB = GetVectorProduct(trianglePoints[1].X - point.X, trianglePoints[2].Y - trianglePoints[1].Y, trianglePoints[2].X - trianglePoints[1].X, trianglePoints[1].Y - point.Y);
            //var vectorProductC = GetVectorProduct(trianglePoints[2].X - point.X, trianglePoints[0].Y - trianglePoints[2].Y, trianglePoints[0].X - trianglePoints[2].X, trianglePoints[2].Y - point.Y);

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

        private void CheckIsTriangleSegmentIntersectWithAnother(int[] vectorProductsPointA, Point pointB, Triangle triangle)
        {
            var vectorProductsPointB = CalculateVectorProducts(pointB, triangle.Points);
            _isTrianglesIntersect = vectorProductsPointA.All(x => x <= 0) && vectorProductsPointB.All(x => x <= 0);
        }
    }
}
