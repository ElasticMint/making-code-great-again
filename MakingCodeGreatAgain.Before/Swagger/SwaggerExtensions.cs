using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MakingCodeGreatAgain.Before.Swagger
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "MakingCodeGreatAgain",
                            Version = "v1"
                        });

                    c.CustomSchemaIds(x => x.FullName);
                });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string apiName)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", apiName);
                    c.RoutePrefix = apiName;
                });

            return app;
        }
    }
}
