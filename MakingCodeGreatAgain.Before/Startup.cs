using System;
using Elasticsearch.Net;
using MakingCodeGreatAgain.Before.Services;
using MakingCodeGreatAgain.Before.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;

namespace MakingCodeGreatAgain.Before
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwagger();

            services.AddScoped<IInvestorService, InvestorsService>();

            services.AddSingleton<IElasticClient, ElasticClient>(
                serviceProvider =>
                {
                    var connectionSettings = new ElasticSearchConnection();
                    _configuration.GetSection("ElasticConnectionSettings").Bind(connectionSettings);
                    var uri = new Uri(connectionSettings.Url);
                    var pool = new SingleNodeConnectionPool(uri);
                    var config = new ConnectionSettings(pool);
                    config.ThrowExceptions();

                    return new ElasticClient(config);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); })
                .UseSwagger("MakingCodeGreatAgain");
        }
    }
}
