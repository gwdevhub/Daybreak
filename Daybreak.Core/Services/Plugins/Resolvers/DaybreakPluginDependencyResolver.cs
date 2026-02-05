using Plumsy.Resolvers;
using System.Reflection;

namespace Daybreak.Services.Plugins.Resolvers;
internal sealed class DaybreakPluginDependencyResolver : IDependencyResolver
{
    public bool TryResolveDependency(Assembly? requestingAssembly, string dependencyName, out string? path)
    {
        path = default;
        return false;
    }
}
