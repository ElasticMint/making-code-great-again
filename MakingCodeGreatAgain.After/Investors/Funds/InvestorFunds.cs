using System;
using System.Threading.Tasks;
using MakingCodeGreatAgain.After.ElasticSearch;

namespace MakingCodeGreatAgain.After.Investors.Funds
{
    internal class InvestorFunds : IInvestorFunds
    {
        public Task Add(
            Guid fundId,
            string name,
            decimal? amount,
            string currency,
            Address address)
        {
            throw new NotImplementedException();
        }
    }
}