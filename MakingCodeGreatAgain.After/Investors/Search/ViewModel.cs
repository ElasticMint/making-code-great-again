using System;

namespace MakingCodeGreatAgain.After.Investors.Search
{
    public class ViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string[] Sectors { get; set; }
    }
}
