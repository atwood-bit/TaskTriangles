using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Windows.Input;
using TaskTriangles.Commands;
using TaskTriangles.Models;
using TaskTriangles.Services.Interfaces;

namespace TaskTriangles.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddCommand { get; set; }
        public string FilePath { get; set; }
        public string ResultMessage { get; set; }
        public string WarningMessage { get; set; } = string.Empty;

        public delegate void AddItems(IEnumerable<Triangle> triangles);
        public delegate void ShowError(string message);
        //public delegate void AddTransparency(IEnumerable<KeyValuePair<int, double>> transparency);

        public AddItems? AddItemsToViewCollection { get; set; }
        //public AddTransparency? AddTransparencies { get; set; }
        public ShowError? ShowErrorMessage { get; set; }

        public MainViewModel(
            IFigureService figureService,
            IOptions<AppSettings> settings)
        {
            FilePath = $"{Environment.CurrentDirectory}\\{settings.Value.DefaultInputFileName}";
            ResultMessage = BuildResultMessage(null);
            AddCommand = new DrawTrianglesCommand(async () =>
            {
                try
                {
                    var result = await figureService.BuildTree(FilePath);
                    WarningMessage = result.WarningMessage;
                    ResultMessage = BuildResultMessage(result);
                    //AddTransparencies?.Invoke(null);
                    AddItemsToViewCollection?.Invoke(result.Items);
                }
                catch (System.Exception ex)
                {
                    ShowErrorMessage?.Invoke(ex.Message);
                }
            });
        }

        private string BuildResultMessage(ResultModel? result)
        {
            var shadesCount = result is not null && result.IsTrianglesIntersect ? "Error" : (result?.ShadesCount ?? 0).ToString();
            return $"Triangles count = {result?.TrianglesCount ?? 0}\nShades count = {shadesCount}";
        }

        private void T(int shadesCount)
        {

            var q = new KeyValuePair<int, double>(1, 1.0);
        }
    }
}
