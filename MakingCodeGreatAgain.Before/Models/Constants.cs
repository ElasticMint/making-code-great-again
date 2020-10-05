namespace MakingCodeGreatAgain.Before.Models
{
    public class Constants
    {
        public const string InvestorsIndex = "Investors";

        public const string InvestmentAmountScript =
            @"
def commitments = params['_source'].funds != null ?
params['_source'].funds
                        .stream()
                        .map(m->m.investedAmount?.usdMn)
                        .collect(Collectors.toList())
                    : [];

def amount = 0;
for (def i = 0; i < commitments.length; ++i) {
amount += commitments[i];
}

return [
   'investmentAmount' : amount;
];
";
    }
}
