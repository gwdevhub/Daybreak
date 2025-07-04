﻿using System.Collections.Specialized;

namespace Daybreak.Shared.Models;

public class SortedObservableCollection<T, TKey>(Func<T, TKey> keySelector) : BatchedObservableCollection<T>
{
    public Func<T, TKey> KeySelector { get; set; } = keySelector;
    public bool Descending { get; set; } = false;

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
