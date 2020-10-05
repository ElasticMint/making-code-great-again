using System.Collections.Generic;
using MakingCodeGreatAgain.After.ElasticSearch.Investors.Model;
using Nest;

namespace MakingCodeGreatAgain.After.ElasticSearch.Investors.Query
{
    internal static class QueryContainerExtensions
    {
        public static QueryContainer IsActive(
            this QueryContainerDescriptor<Investor> query)
        {
            return query.Term(t => t.Field(f => f.IsActive).Value(true));
        }

        public static QueryContainer IsInCountry(
            this QueryContainerDescriptor<Investor> query,
            IEnumerable<string> countries)
        {
            return query.Terms(t => t.Field(f => f.Address.Country).Terms(countries));
        }
    }
}