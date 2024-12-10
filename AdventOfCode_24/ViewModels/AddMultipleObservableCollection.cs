using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace AdventOfCode_24.ViewModels
{
    public class AddMultipleObservableCollection<T> : ObservableCollection<T>
    {


        public AddMultipleObservableCollection() { }
        public AddMultipleObservableCollection(IEnumerable<T> items) :base(items)
        {

        }

        public void InsertRange(IEnumerable<T> items)
        {
            this.CheckReentrancy();
            foreach (var item in items)
                this.Items.Add(item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddWithoutNotify(T item)
        {
            this.CheckReentrancy();
            this.Items.Add(item);
        }

    }
}
