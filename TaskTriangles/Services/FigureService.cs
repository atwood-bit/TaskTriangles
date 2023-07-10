using TaskTriangles.Models;
using TaskTriangles.Services.Interfaces;

namespace TaskTriangles.Services
{
    public class FigureService : IFigureService
    {
        private const int COORDINATES_COUNT = 6;
        private const int MAX_RANGE_COORDINATE = 1000;

        public async Task<ResultModel> BuildTree(string filePath)
        {
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

            // todo
            //lock (triangles)
            //{
            //    Parallel.ForEach(triangles, t => t.Level = FindTriangleLevel(t, triangles));
            //}

            foreach (var triangle in triangles)
            {
                triangle.Level = FindTriangleLevel(triangle, triangles);
            }

            triangles = triangles.OrderBy(x => x.Level).ToList();
            //DrawTriangles(triangles);
            resultModel.TrianglesCount = triangles.Count;
            resultModel.ShadesCount = triangles.Last()?.Level;

            return resultModel;
        }

        private Triangle MapLineToTrianglePoints(string line, int index)
        {
            var points = line.Trim().Split(" ");

            if (points.Length != COORDINATES_COUNT)
            {
                throw new Exception($"Incorrect coordinates at line with number {index + 1}");
            }

            var convertablePoints = points.Select(x => Convert.ToInt32(x)).ToArray();

            if (convertablePoints.Any(x => x < 0 || x > MAX_RANGE_COORDINATE))
            {
                throw new Exception($"Incorrect coordinates at line with number {index + 1}");
            }

            var trianglePoints = convertablePoints.Where((x, index) => index % 2 == 0)
                .Select((x, index) =>
                    new Point
                    {
                        X = Convert.ToInt32(x),
                        Y = Convert.ToInt32(convertablePoints[index * 2 + 1])
                    })
                .ToArray();

            return new Triangle(trianglePoints);
        }

        private int FindTriangleLevel(Triangle triangle, List<Triangle> allTriangles)
        {
            if (triangle.Level.HasValue)
            {
                return triangle.Level.Value;
            }

            var parents = allTriangles.Where(t => IsTriangleNestedInAnother(triangle, t));
            var triangleLevel = 0;

            foreach (var parent in parents)
            {
                if (!parent.Level.HasValue)
                {
                    triangleLevel = FindTriangleLevel(parent, allTriangles);
                }
                else if (triangleLevel < parent.Level.Value)
                {
                    triangleLevel = parent.Level.Value;
                }
            }

            return triangleLevel + 1;
        }

        private bool IsTriangleNestedInAnother(Triangle first, Triangle second)
        {
            return first.Points.All(p => IsPointInTriangle(p, second));
        }

        private bool IsPointInTriangle(Point point, Triangle triangle)
        {
            var vectorProducts = CalculateVectorProducts(point, triangle.Points);

            return IsAllVectorProductsPositiveOrNegative(vectorProducts);
        }

        private int[] CalculateVectorProducts(Point point, Point[] trianglePoints)
        {
            // refactor
            var vectorProductA = (trianglePoints[0].X - point.X) * (trianglePoints[1].Y - trianglePoints[0].Y) - (trianglePoints[1].X - trianglePoints[0].X) * (trianglePoints[0].Y - point.Y);
            var vectorProductB = (trianglePoints[1].X - point.X) * (trianglePoints[2].Y - trianglePoints[1].Y) - (trianglePoints[2].X - trianglePoints[1].X) * (trianglePoints[1].Y - point.Y);
            var vectorProductC = (trianglePoints[2].X - point.X) * (trianglePoints[0].Y - trianglePoints[2].Y) - (trianglePoints[0].X - trianglePoints[2].X) * (trianglePoints[2].Y - point.Y);

            return new int[] { vectorProductA, vectorProductB, vectorProductC };
        }

        private bool IsAllVectorProductsPositiveOrNegative(int[] vectorProducts)
        {
            return vectorProducts.All(x => x > 0) || vectorProducts.All(x => x < 0);
        }
    }
}
