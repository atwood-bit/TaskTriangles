using System.Windows.Input;

namespace TaskTriangles.Commands
{
    public class DrawTrianglesCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action _action;

        public DrawTrianglesCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            _action?.Invoke();
        }
    }
}
