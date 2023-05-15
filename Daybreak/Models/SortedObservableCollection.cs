using System.Collections.Specialized;
using System;
using System.Linq;

namespace Daybreak.Models;

public class SortedObservableCollection<T, TKey> : BatchedObservableCollection<T>
{
    public Func<T, TKey> KeySelector { get; set; }
    public bool Descending { get; set; } = false;

    public SortedObservableCollection(Func<T, TKey> keySelector)
    {
        this.KeySelector = keySelector;
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);
        var query = this
          .Select((item, index) => (Item: item, Index: index));
        query = this.Descending ?
            query.OrderByDescending(tuple => this.KeySelector(tuple.Item)) :
            query.OrderBy(tuple => this.KeySelector(tuple.Item));

        var map = query.Select((tuple, index) => (OldIndex: tuple.Index, NewIndex: index))
         .Where(o => o.OldIndex != o.NewIndex);

        using var enumerator = map.GetEnumerator();
        if (enumerator.MoveNext())
        {
            this.Move(enumerator.Current.OldIndex, enumerator.Current.NewIndex);
        }
    }
}
