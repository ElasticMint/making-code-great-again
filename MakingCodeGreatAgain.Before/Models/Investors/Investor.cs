using System;
using System.Collections.Generic;
using MakingCodeGreatAgain.Before.Models.Shared;

namespace MakingCodeGreatAgain.Before.Models.Investors
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
