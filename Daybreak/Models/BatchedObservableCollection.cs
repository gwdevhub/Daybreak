using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Daybreak.Models;

public class BatchedObservableCollection<T> : ObservableCollection<T>
{
    public void RemoveBatch(IEnumerable<T> values)
    {
        this.CheckReentrancy();
        var changed = false;
        foreach (var value in values)
        {
            this.Items.Remove(value);
            changed = true;
        }

        if (!changed)
        {
            return;
        }

        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void AddBatch(IEnumerable<T> values)
    {
        this.CheckReentrancy();
        var changed = false;
        foreach (var value in values)
        {
            this.Items.Add(value);
        }

        if (!changed)
        { 
            return;
        }

        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
