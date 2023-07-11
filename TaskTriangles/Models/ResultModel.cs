namespace TaskTriangles.Models
{
    public class ResultModel
    {
        public int TrianglesCount { get; set; }
        public int? ShadesCount { get; set; }
        public string WarningMessage { get; set; } = string.Empty;
        public IEnumerable<Triangle> Items { get; set; } = new List<Triangle>();
        public bool IsTrianglesIntersect { get; set; }
        public double TransparencyStepByLevel { get; set; }
        public string Color { get; set; }

        public void CalculateTransparencyStep(double minRangeValue, double maxRangeValue)
        {
            TransparencyStepByLevel = (Math.Abs(minRangeValue) + Math.Abs(maxRangeValue)) / ShadesCount.Value;
        }
    }
}
