using System.IO;
using TaskTriangles.Validation.Interfaces;

namespace TaskTriangles.Validation
{
    public class InputFileValidator : IInputFileValidator
    {
        private const int MAX_COUNT_OF_TRIANGLES = 1000;

        public async Task<bool> ValidateInputFile(string filePath)
        {
            var isExists = File.Exists(filePath);

            return isExists && await ValidateTrianglesCount(filePath);
        }

        private async Task<bool> ValidateTrianglesCount(string filePath)
        {
            using var reader = new StreamReader(filePath);
            var trianglesCount = Convert.ToInt32(await reader.ReadLineAsync());

            return trianglesCount > 0 && trianglesCount < MAX_COUNT_OF_TRIANGLES;
        }
    }
}
