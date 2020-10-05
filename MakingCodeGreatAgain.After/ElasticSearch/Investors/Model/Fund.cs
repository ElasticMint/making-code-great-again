using System;

namespace MakingCodeGreatAgain.After.ElasticSearch.Investors.Model
{
    public class Fund
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Amount<decimal?> InvestedAmount { get; set; }
    }
}