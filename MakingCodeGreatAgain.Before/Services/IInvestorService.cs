using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MakingCodeGreatAgain.Before.Models.Shared;
using MakingCodeGreatAgain.Before.Requests.Investors;
using MakingCodeGreatAgain.Before.ViewModels;

namespace MakingCodeGreatAgain.Before.Services
{
    public interface IInvestorService
    {
        public Task<List<InvestorViewModel>> GetInvestors(GetInvestorsRequest request);

        public Task AddFund(Guid fundId, string name, decimal? amount, string currency, Address address);
    }
}