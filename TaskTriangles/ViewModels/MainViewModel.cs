using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Windows.Input;
using TaskTriangles.Commands;
using TaskTriangles.Enums;
using TaskTriangles.Models;
using TaskTriangles.Services.Interfaces;
using TaskTriangles.ViewModels.Interfaces;

namespace TaskTriangles.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IObservable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddCommand { get; set; }
        public string FilePath { get; set; }
        public string ResultMessage { get; set; }
        public string WarningMessage { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        private List<IObserver> _observers { get; set; } = new();

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
                    result.CalculateTransparencyStep(settings.Value.MinRangeOfTransparency, settings.Value.MaxRangeOfTransparency);
                    result.Color = Color;
                    WarningMessage = result.WarningMessage;
                    ResultMessage = BuildResultMessage(result);
                    NotifyObservers(result, NotifyAction.Success);
                }
                catch (System.Exception ex)
                {
                    NotifyObservers(ex.Message, NotifyAction.Error);
                }
            });
        }

        public void RegisterObserver(IObserver o)
        {
            _observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            _observers.Remove(o);
        }

        public void NotifyObservers(object obj, NotifyAction action)
        {
            _observers.ForEach(x => x.Update(obj, action));
        }

        private string BuildResultMessage(ResultModel? result)
        {
            var shadesCount = result is not null && result.IsTrianglesIntersect ? "Error" : (result?.ShadesCount ?? 0).ToString();
            return $"Triangles count = {result?.TrianglesCount ?? 0}\nShades count = {shadesCount}";
        }
    }
}
