using System;

namespace MakingCodeGreatAgain.Before.ViewModels
{
    public class InvestorViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string[] Sectors { get; set; }
    }
}
