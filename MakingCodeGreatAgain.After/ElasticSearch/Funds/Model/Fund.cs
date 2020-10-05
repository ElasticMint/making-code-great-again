using System;

namespace MakingCodeGreatAgain.After.ElasticSearch.Funds.Model
{
    public class Fund
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
