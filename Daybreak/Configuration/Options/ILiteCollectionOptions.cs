namespace Daybreak.Configuration.Options;

public interface ILiteCollectionOptions<T> : ILiteCollectionOptions
{
}

public interface ILiteCollectionOptions
{
    string CollectionName { get; }
}
