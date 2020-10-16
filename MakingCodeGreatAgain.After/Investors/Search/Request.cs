using Nest;

namespace MakingCodeGreatAgain.After.Investors.Search
{
    public class Request
    {
        public string[] Countries { get; set; }
        public string[] Sectors { get; set; }
        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public int From => (Page - 1) * PageSize;
    }
}
