using MakingCodeGreatAgain.After.Investors.Funds;
using MakingCodeGreatAgain.After.Investors.Search;
using Microsoft.Extensions.DependencyInjection;

namespace MakingCodeGreatAgain.After.Investors
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInvestors(this IServiceCollection services)
        {
            services
                .AddScoped<IInvestorSearch, InvestorSearch>()
                .AddScoped<IInvestorFunds, InvestorFunds>();
            return services;
        }
    }
}