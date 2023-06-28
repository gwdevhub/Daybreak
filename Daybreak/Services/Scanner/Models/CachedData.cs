using System;

namespace Daybreak.Services.Scanner.Models;

public sealed class CachedData<T>
{
    public T? Data { get; private set; }
    public DateTime SetTime { get; private set; } = DateTime.MinValue;

    public void SetData(T data)
    {
        this.Data = data;
        this.SetTime = DateTime.Now;
    }
}
