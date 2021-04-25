using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all services in the domain layer to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the services to</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddTransient<IRealEstateAgentService, RealEstateAgentService>();

            return services;
        }
    }
}
