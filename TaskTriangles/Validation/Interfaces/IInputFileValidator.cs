namespace TaskTriangles.Validation.Interfaces
{
    public interface IInputFileValidator
    {
        Task ValidateInputFile(string filePath);
        void ValidateTrianglesCount(int trianglesCount);
    }
}
