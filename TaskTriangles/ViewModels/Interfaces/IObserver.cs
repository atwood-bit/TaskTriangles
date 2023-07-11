using TaskTriangles.Enums;

namespace TaskTriangles.ViewModels.Interfaces
{
    public interface IObserver
    {
        void Update(object ob, NotifyAction action);
    }
}
