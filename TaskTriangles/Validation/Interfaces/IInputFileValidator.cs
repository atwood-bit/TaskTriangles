namespace TaskTriangles.Validation.Interfaces
{
    public interface IInputFileValidator
    {
        Task<bool> ValidateInputFile(string filePath);
    }
}
