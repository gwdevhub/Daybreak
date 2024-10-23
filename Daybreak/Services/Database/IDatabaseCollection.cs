using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Daybreak.Services.Database;

public interface IDatabaseCollection<TObject>
    where TObject : new()
{
    long Count();

    bool Add(TObject value);

    bool AddBulk(IEnumerable<TObject> value);

    bool Update(TObject value);

    void Delete(TObject obj);

    void DeleteAll();

    IEnumerable<TObject> FindAll();

    IEnumerable<TObject> FindAll(Expression<Func<TObject, bool>> filter);
}
