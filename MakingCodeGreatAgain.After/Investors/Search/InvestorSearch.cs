using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using MakingCodeGreatAgain.After.ElasticSearch;
using MakingCodeGreatAgain.After.ElasticSearch.Investors.Model;
using MakingCodeGreatAgain.After.ElasticSearch.Investors.Query;
using MakingCodeGreatAgain.After.Logging;
using Nest;
using Serilog;

namespace MakingCodeGreatAgain.After.Investors.Search
{
    public class InvestorSearch : IInvestorSearch
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger _logger;

        public InvestorSearch(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
            _logger = Log.ForContext<InvestorSearch>();
        }

        public async Task<List<ViewModel>> Get(Request request)
        {
            var query = new SearchDescriptor<Investor>()
                .Index(Indexes.Investors)
                .SearchType(SearchType.DfsQueryThenFetch)
                .Type("")
                .From(request.From)
                .Size(request.PageSize)
                .Source(s => s.Includes(i => i.Fields(
                    "id",
                    "name",
                    "address.country"
                )))
                .ScriptFields(script => script.InvestmentAmountUsd().InvestmentAmountUsd())
                .Query(q => q.IsActive() &&
                            q.IsInCountry(request.Countries))
                .Sort(s => s.Field(f => f.Field(request.SortBy).Order(request.SortOrder)));

            _logger.LogQuery("Search for investors", "GET investors/search", query);

            var result = await _elasticClient.SearchAsync<Investor>(query);
            return Map(result);
        }

        private static List<ViewModel> Map(
            ISearchResponse<Investor> result)
        {
            var list = new List<ViewModel>();
            foreach (var investor in result.Documents)
            {
                list.Add(new ViewModel
                {
                    Id = investor.Id,
                    Name = investor.Name,
                    Country = investor.Address.Country
                });
            }

            return list;
        }
    }
}
