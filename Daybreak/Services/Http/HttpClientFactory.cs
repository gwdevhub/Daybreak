using Daybreak.Services.Logging;
using Slim.Resolvers;
using System;

namespace Daybreak.Services.Http
{
    public sealed class HttpClientFactory : IDependencyResolver
    {
        private readonly Type clientType = typeof(HttpClient<>);

        public bool CanResolve(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IHttpClient<>))
            {
                return true;
            }

            return false;
        }

        public object Resolve(Slim.IServiceProvider serviceProvider, Type type)
        {
            var typedClientType = clientType.MakeGenericType(type.GetGenericArguments());
            return Activator.CreateInstance(typedClientType, serviceProvider.GetService<ILogger>());
        }
    }
}
