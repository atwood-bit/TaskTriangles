namespace TaskTriangles.Models
{
    public class AppSettings
    {
        public string DefaultInputFileName { get; set; } = string.Empty;
        public double MaxRangeOfTransparency { get; set; }
        public double MinRangeOfTransparency { get; set; }
        public int MaxCountOfTriangles { get; set; }
        public int MinCountOfTriangles { get; set; }
        public int CoordinatesCount { get; set; }
        public int MaxRangeOfCoordinate { get; set; }
        public int MinRangeOfCoordinate { get; set; }
    }
}
