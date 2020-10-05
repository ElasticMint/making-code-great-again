using Nest;

namespace MakingCodeGreatAgain.Before.Requests.Investors
{
    public class GetInvestorsRequest
    {
        public string[] Countries { get; set; }
        public string[] Sectors { get; set; }
        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
        public int Page { get; set; }
    }
}
