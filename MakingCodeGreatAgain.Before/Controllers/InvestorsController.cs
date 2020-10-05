using System.Collections.Generic;
using System.Threading.Tasks;
using MakingCodeGreatAgain.Before.Requests.Investors;
using MakingCodeGreatAgain.Before.Services;
using MakingCodeGreatAgain.Before.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MakingCodeGreatAgain.Before.Controllers
{
    [Route("makingcodegreatagain/[controller]")]
    [ApiController]
    public class InvestorsController : ControllerBase
    {
        private IInvestorService _investorService;

        public InvestorsController(IInvestorService investorService)
        {
            _investorService = investorService;
        }

        [HttpGet]
        public async Task<List<InvestorViewModel>> Get(GetInvestorsRequest request)
        {
            return await _investorService.GetInvestors(request);
        }
    }
}
