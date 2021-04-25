using Abstractions.Models;
using Domain.Models;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// A service to get real estate agents with
    /// </summary>
    public class RealEstateAgentService : IRealEstateAgentService
    {
        private IRepository _repository;

        public RealEstateAgentService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Get real estate agents with their offers
        /// </summary>
        /// <param name="offerType">The type of offers the retrieve <see cref="OfferType"/></param>
        /// <param name="searchWords">Search wordt that the offers should match with</param>
        /// <param name="top">The amount of real estate agents to return</param>
        /// <param name="orderedByOffersAsc">Indicates if the real estate agent should be ordered by the amount of offers they have</param>
        /// <returns>List of <see cref="RealEstateAgent"/></returns>
        public Task<IEnumerable<RealEstateAgent>> GetRealEstateAgentsWithOffersAsync(OfferType offerType, IEnumerable<string> searchWords, int top, bool orderedByOffersAsc = true)
        {
            if (offerType == OfferType.Unknown)
            {
                throw new ArgumentNullException(nameof(offerType));
            }

            if (searchWords == null)
            {
                throw new ArgumentNullException(nameof(searchWords));
            }

            if (top <= 0)
            {
                top = 10;
            }

            return GetRealEstateAgentsWithOffersInternalAsync(offerType, searchWords, top, orderedByOffersAsc);
        }

        private async Task<IEnumerable<RealEstateAgent>> GetRealEstateAgentsWithOffersInternalAsync(OfferType offerType, IEnumerable<string> searchWords, int top, bool orderedByOffersAsc = true)
        {
            // Get the objects for sale from the repository
            var objectsForSale = await _repository.GetOfferAsync(offerType, searchWords);

            // Group the objects for sale by real estate agent id and name.
            // Then create real estate agents containing all offers from that real estate agent
            // Take the first 10 based on the most amount of offers
            var distinctRealEstateAgents = objectsForSale
              .GroupBy(objectsForSale => (objectsForSale.RealEstateAgentId, objectsForSale.RealEstateAgentName))
              .Select(grouping => new RealEstateAgent(grouping.Key.RealEstateAgentId, grouping.Key.RealEstateAgentName, grouping.Select(s => s.Id)));

            if (orderedByOffersAsc)
            {
                distinctRealEstateAgents = distinctRealEstateAgents.OrderByDescending(realEstateAgent => realEstateAgent.Offers.Count());
            }

            distinctRealEstateAgents = distinctRealEstateAgents.Take(top);

            return distinctRealEstateAgents;
        }
    }
}
