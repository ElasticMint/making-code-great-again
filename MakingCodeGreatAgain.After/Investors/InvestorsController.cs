using System.Collections.Generic;
using System.Threading.Tasks;
using MakingCodeGreatAgain.After.Investors.Search;
using Microsoft.AspNetCore.Mvc;

namespace MakingCodeGreatAgain.After.Investors
{
    [Route("makingcodegreatagain/[controller]")]
    [ApiController]
    public class InvestorsController : ControllerBase
    {
        private readonly IInvestorSearch _investorSearch;

        public InvestorsController(IInvestorSearch investorSearch)
        {
            _investorSearch = investorSearch;
        }

        [HttpGet]
        public Task<List<ViewModel>> Get(Request request)
        {
            return _investorSearch.Get(request);
        }
    }
}
