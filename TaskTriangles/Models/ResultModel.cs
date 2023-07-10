namespace TaskTriangles.Models
{
    public class ResultModel
    {
        public int TrianglesCount { get; set; }
        public int? ShadesCount { get; set; }
        public string WarningMessage { get; set; } = string.Empty;
    }
}
