using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using MakingCodeGreatAgain.After.ElasticSearch;
using MakingCodeGreatAgain.After.ElasticSearch.Investors.Model;
using MakingCodeGreatAgain.After.ElasticSearch.Investors.Query;
using Nest;

namespace MakingCodeGreatAgain.After.Investors.Search
{
    public class InvestorSearch : IInvestorSearch
    {
        private readonly IElasticClient _elasticClient;

        public InvestorSearch(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<List<ViewModel>> Get(Request request)
        {
            var result = await _elasticClient.SearchAsync<Investor>(
                q => q.Index(Indexes.Investors)
                    .SearchType(SearchType.DfsQueryThenFetch)
                    .Type("")
                    .From((request.Page -1) * 20)
                    .Size(20)
                    .Source(s => s.Includes(i => i.Fields(
                        "id",
                        "name",
                        "address.country"
                        )))
                    .ScriptFields(script => script.InvestmentAmountUsd().InvestmentAmountUsd())
                    .Query(query => query.IsActive() && 
                                    query.IsInCountry(request.Countries))
                    .Sort(s => s.Field(f => f.Field(request.SortBy).Order(request.SortOrder)))
                );

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
