using System;
using System.Collections.Generic;
using System.Extensions;

namespace Daybreak.Services.Runtime
{
    public sealed class RuntimeStore : IRuntimeStore
    {
        private Dictionary<string, object> InnerStore { get; } = new Dictionary<string, object>();

        public T GetValue<T>(string name)
        {
            if(this.InnerStore.TryGetValue(name, out var value))
            {
                return value.Cast<T>();
            }

            throw new InvalidOperationException($"Could not find any value stored with name {name}");
        }
        public void StoreValue<T>(string name, T value)
        {
            this.InnerStore[name] = value;
        }
        public bool TryGetValue<T>(string name, out T value)
        {
            if (this.InnerStore.TryGetValue(name, out var valueObj))
            {
                value = valueObj.Cast<T>();
                return true;
            }

            value = default;
            return false;
        }
    }
}
