using System;

namespace MakingCodeGreatAgain.Before.Models.Investors
{
    public class Fund
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Amount<decimal?> InvestedAmount { get; set; }
    }
}