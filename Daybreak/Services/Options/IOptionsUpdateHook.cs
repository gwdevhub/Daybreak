using System;

namespace Daybreak.Services.Options;

public interface IOptionsUpdateHook
{
    void RegisterHook<TOptionsType>(Action action)
        where TOptionsType : class;

    void UnregisterHook<TOptionsType>(Action action)
        where TOptionsType : class;
}
