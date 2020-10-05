namespace MakingCodeGreatAgain.Before.Models.Investors
{
    public class Amount<T>
    {
        public T LocalMn { get; set; }
        public T UsdMn { get; set; }
        public T GbpMn { get; set; }
        public string Currency { get; set; }
    }
}