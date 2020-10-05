using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using MakingCodeGreatAgain.Before.Models;
using MakingCodeGreatAgain.Before.Models.Investors;
using MakingCodeGreatAgain.Before.Models.Shared;
using MakingCodeGreatAgain.Before.Requests.Investors;
using MakingCodeGreatAgain.Before.Utils;
using MakingCodeGreatAgain.Before.ViewModels;
using Nest;

namespace MakingCodeGreatAgain.Before.Services
{
    public class InvestorsService : IInvestorService
    {
        private IElasticClient _elasticClient;

        public InvestorsService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<List<InvestorViewModel>> GetInvestors(GetInvestorsRequest request)
        {
            var result = await _elasticClient.SearchAsync<Investor>(
                q => q.Index(Constants.InvestorsIndex)
                    .SearchType(SearchType.DfsQueryThenFetch)
                    .Type("")
                    .From((request.Page -1) * 20)
                    .Size(20)
                    .Source(s => s.Includes(i => i.Fields(
                        "id",
                        "name",
                        "address.country"
                        )))
                    .ScriptFields(sc => sc.ScriptField("investmentAmount", descriptor => descriptor.Source(Constants.InvestmentAmountScript)))
                    .Query(q => q.Bool(b => b.Must(
                        m => m.Term(t => t.Field(f => f.IsActive).Value(true))
                            ).Must(m => m.Terms(t => t.Field(f => f.Address.Country).Terms(request.Countries)))))
                    .Sort(s => s.Field(f => f.Field(request.SortBy).Order(request.SortOrder)))
                );

            return InvestorMappingHelper.MapInvestors(result.Documents);
        }

        public Task AddFund(Guid fundId, string name,
            decimal? amount, string currency, Address address)
        {
            throw new NotImplementedException();
        }
    }
}
