using Daybreak.Utils;
using Realms;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Daybreak.Services.Database;

internal sealed class RealmDatabaseCollection<TObject> : IDatabaseCollection<TObject>
    where TObject : IRealmObject, new()
{
    private const string RealmDbFileName = "daybreak.realm.db";

    private static readonly ThreadLocal<Realm> RealmInstance = new(() =>
        Realm.GetInstance(PathUtils.GetAbsolutePathFromRoot(RealmDbFileName))
    );

    public RealmDatabaseCollection()
    {
    }

    public long Count()
    {
        var db = RealmInstance.Value.ThrowIfNull();
        return db.All<TObject>().Count();
    }

    public bool Update(TObject value)
    {
        var db = RealmInstance.Value.ThrowIfNull();
        using var transaction = db.BeginWrite();
        var result = db.Add(value, true) is not null;
        transaction.Commit();
        return result;
    }

    public bool AddBulk(IEnumerable<TObject> values)
    {
        var db = RealmInstance.Value.ThrowIfNull();
        using var transaction = db.BeginWrite();
        db.Add(values, false);
        transaction.Commit();
        return true;
    }

    public bool Add(TObject value)
    {
        var db = RealmInstance.Value.ThrowIfNull();
        using var transaction = db.BeginWrite();
        var result = db.Add(value, false) is not null;
        transaction.Commit();
        return result;
    }

    public IEnumerable<TObject> FindAll()
    {
        var db = RealmInstance.Value.ThrowIfNull();
        return db.All<TObject>();
    }

    public IEnumerable<TObject> FindAll(Expression<Func<TObject, bool>> filter)
    {
        var db = RealmInstance.Value.ThrowIfNull();
        return db.All<TObject>().Where(filter);
    }

    public void Delete(TObject obj)
    {
        var db = RealmInstance.Value.ThrowIfNull();
        using var transaction = db.BeginWrite();
        db.Remove(obj);
        transaction.Commit();
    }

    public void DeleteAll()
    {
        var db = RealmInstance.Value.ThrowIfNull();
        using var transaction = db.BeginWrite();
        db.RemoveAll<TObject>();
        transaction.Commit();
    }
}
