using System;
using System.Collections.Generic;

namespace MakingCodeGreatAgain.After.ElasticSearch.Investors.Model
{
    public class Investor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Address Address { get; set; }
        public NaturalResources NaturalResources { get; set; }
        public RealEstate RealEstate { get; set; }
        public List<Fund> Funds { get; set; }
    }
}
