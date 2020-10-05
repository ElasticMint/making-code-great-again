using System;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace MakingCodeGreatAgain.After.ElasticSearch
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IElasticClient, ElasticClient>(
                serviceProvider =>
                {
                    var connectionSettings = new Settings();
                    configuration.GetSection("ElasticConnectionSettings").Bind(connectionSettings);
                    var uri = new Uri(connectionSettings.Url);
                    var pool = new SingleNodeConnectionPool(uri);
                    var config = new ConnectionSettings(pool);
                    config.ThrowExceptions();

                    return new ElasticClient(config);
                });
            return services;
        }
    }
}