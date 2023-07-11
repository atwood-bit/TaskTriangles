using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TaskTriangles.Models
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool _preventNotification = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_preventNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            _preventNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }
            _preventNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
