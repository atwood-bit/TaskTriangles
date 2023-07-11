using TaskTriangles.Enums;

namespace TaskTriangles.ViewModels.Interfaces
{
    public interface IObservable
    {
        void RegisterObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers(object obj, NotifyAction action);
    }
}
