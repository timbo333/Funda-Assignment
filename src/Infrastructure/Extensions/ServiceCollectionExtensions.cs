using Domain.Repositories;
using Infrastructure.Extensions.Options;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all services in the domain layer to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the services to</param>
        /// <param name="config">The config section to use</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            // Make the options available, so the service can get them from DI
            var options = new FundaApiOptions();
            services.Configure<FundaApiOptions>(options => config.GetSection(FundaApiOptions.FundaApi).Bind(options));

            var fundaApiOptions = config.GetSection(FundaApiOptions.FundaApi).Get<FundaApiOptions>();
            services.AddHttpClient<IRepository, FundaRepository>(client =>
                {
                    client.BaseAddress = new Uri(fundaApiOptions.BaseUrl);
                }).ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler()
                    {
                        // Small performance gain when not using a proxy
                        UseProxy = false,
                        // Needed for using the baseurl in the httpClient instead of having the full url in the request send by the repository
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                    };
                }).SetHandlerLifetime(TimeSpan.FromMinutes(5));


            return services;
        }
    }
}
