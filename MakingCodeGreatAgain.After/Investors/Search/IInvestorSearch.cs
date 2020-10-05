using System.Collections.Generic;
using System.Threading.Tasks;

namespace MakingCodeGreatAgain.After.Investors.Search
{
    public interface IInvestorSearch
    {
        public Task<List<ViewModel>> Get(Request request);

    }
}