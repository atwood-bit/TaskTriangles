using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Windows.Input;
using TaskTriangles.Commands;
using TaskTriangles.Models;
using TaskTriangles.Services.Interfaces;
using TaskTriangles.Validation.Interfaces;

namespace TaskTriangles.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddCommand { get; set; }
        public string FilePath { get; set; }
        public string ResultMessage { get; set; }
        public string WarningMessage { get; set; } = string.Empty;

        public MainViewModel(
            IFigureService figureService,
            IInputFileValidator inputFileValidator,
            IOptions<AppSettings> settings)
        {
            FilePath = $"{Environment.CurrentDirectory}\\{settings.Value.DefaultInputFileName}";
            ResultMessage = BuildResultMessage(null);
            AddCommand = new DrawTrianglesCommand(async () =>
            {
                try
                {
                    await inputFileValidator.ValidateInputFile(FilePath);
                    var result = await figureService.BuildTree(FilePath);
                    WarningMessage = result.WarningMessage;
                    ResultMessage = BuildResultMessage(result);
                }
                catch (Exception ex)
                {
                    var t = ex.ToString();
                    var z = 1;
                }
            });
        }

        private string BuildResultMessage(ResultModel? result)
        {
            return $"Triangles count = {result?.TrianglesCount ?? 0}\nShades count = {result?.ShadesCount ?? 0}";
        }
    }
}
