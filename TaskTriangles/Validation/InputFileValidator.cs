using Microsoft.Extensions.Options;
using TaskTriangles.Exception;
using TaskTriangles.Models;
using TaskTriangles.Validation.Interfaces;

namespace TaskTriangles.Validation
{
    public class InputFileValidator : IInputFileValidator
    {
        private readonly AppSettings _appSettings;

        public InputFileValidator(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task ValidateInputFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            await ValidateTrianglesCount(filePath);
        }

        private async Task ValidateTrianglesCount(string filePath)
        {
            using var reader = new StreamReader(filePath);
            var trianglesCount = Convert.ToInt32(await reader.ReadLineAsync());

            ValidateTrianglesCount(trianglesCount);
        }

        public void ValidateTrianglesCount(int trianglesCount)
        {
            if (trianglesCount <= _appSettings.MinCountOfTriangles || trianglesCount > _appSettings.MaxCountOfTriangles)
            {
                throw new IncorrectTrianglesCount(_appSettings.MaxCountOfTriangles, _appSettings.MinCountOfTriangles, trianglesCount);
            }
        }
    }
}
