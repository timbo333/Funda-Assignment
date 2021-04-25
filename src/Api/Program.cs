using Abstractions.Models;
using Domain;
using Domain.Extensions;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Funda.Assignment
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("=== Retrieving the top 10 real estate agents with the most offers in Amsterdam that have a garden ===");
            var realEstateAgentService = host.Services.GetRequiredService<IRealEstateAgentService>();
            var realEstateAgents = await realEstateAgentService.GetRealEstateAgentsWithOffersAsync(OfferType.Buy, new string[] { "amsterdam", "tuin" }, 10, true);
            Console.WriteLine("=== Done retrieving ===");
            Console.WriteLine();

            realEstateAgents.ToList().ForEach(realEstateAgent => 
                Console.WriteLine($"Real estate agent named: \"{realEstateAgent.Name}\" has \"{realEstateAgent.Offers.Count()}\" open offers")
            );
            Console.ReadLine();

            return 0;
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    configuration.Build();
                }).ConfigureServices((context, services) =>
                {
                    services.AddDomainLayer();
                    services.AddInfrastructureLayer(context.Configuration);
                });
    }
}
