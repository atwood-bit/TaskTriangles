namespace TaskTriangles.Models
{
    public class ResultModel
    {
        public int TrianglesCount { get; set; }
        public int? ShadesCount { get; set; }
        public string WarningMessage { get; set; } = string.Empty;
        public IEnumerable<Triangle> Items { get; set; } = new List<Triangle>();
        public bool IsTrianglesIntersect { get; set; }
    }
}
