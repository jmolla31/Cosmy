using Cosmy.Config;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cosmy.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmyClient(this IServiceCollection services, CosmyConfiguration configuration)
        {
            var client = new DocumentClient(new Uri(configuration.Endpoint), configuration.Key);

            services.AddSingleton(configuration);
            services.AddSingleton(client);
            services.AddSingleton<ICosmyClient, CosmyClient>();
            return services;
        }

        public static IServiceCollection AddCosmyClient(this IServiceCollection services, string endpoint, string key)
        {
            var configuration = new CosmyConfiguration
            {
                Endpoint = endpoint,
                Key = key
            };

            var client = new DocumentClient(new Uri(configuration.Endpoint), configuration.Key);

            services.AddSingleton(configuration);
            services.AddSingleton(client);
            services.AddSingleton<ICosmyClient, CosmyClient>();
            return services;
        }
    }
}
