namespace Daybreak.Services.Runtime
{
    public interface IRuntimeStore
    {
        void StoreValue<T>(string name, T value);
        bool TryGetValue<T>(string name, out T value);
        T GetValue<T>(string name);
    }
}
