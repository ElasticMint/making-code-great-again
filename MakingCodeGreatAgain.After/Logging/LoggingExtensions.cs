using Elasticsearch.Net;
using Nest;
using Serilog;

namespace MakingCodeGreatAgain.After.Logging
{
    internal static class LoggingExtensions
    {
        private static readonly IElasticClient ElasticClient = new ElasticClient();

        public static void LogQuery(this ILogger logger, string message, string queryUri, ISearchRequest queryContainer)
        {
            var queryContainerString = ElasticClient.SourceSerializer.SerializeToString(queryContainer);
            logger.Verbose(
                $"Requested {message}:" + "\n{@elasticSearchQuery}",
                $"{queryUri}\n{queryContainerString}");
        }
    }
}