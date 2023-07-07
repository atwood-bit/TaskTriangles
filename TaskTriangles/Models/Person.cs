using System.ComponentModel;

namespace TaskTriangles.Models
{
    public class Person : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int value)
        {
            Name = name;
            Age = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
