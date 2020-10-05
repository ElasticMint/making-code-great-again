using System.Collections.Generic;
using MakingCodeGreatAgain.Before.Models.Investors;
using MakingCodeGreatAgain.Before.ViewModels;

namespace MakingCodeGreatAgain.Before.Utils
{
    public class InvestorMappingHelper
    {
        public static List<InvestorViewModel> MapInvestors(IReadOnlyCollection<Investor> investors)
        {
            var viewModelList = new List<InvestorViewModel>();
            foreach (var investor in investors)
            {
                viewModelList.Add(new InvestorViewModel
                {
                    Id = investor.Id,
                    Name = investor.Name,
                    Country = investor.Address.Country
                });
            }

            return viewModelList;
        }
    }
}