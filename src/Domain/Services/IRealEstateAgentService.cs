using Abstractions.Models;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// A service to get real estate agents with
    /// </summary>
    public interface IRealEstateAgentService
    {
        /// <summary>
        /// Get real estate agents with their offers
        /// </summary>
        /// <param name="offerType">The type of offers the retrieve <see cref="OfferType"/></param>
        /// <param name="searchWords">Search wordt that the offers should match with</param>
        /// <param name="top">The amount of real estate agents to return</param>
        /// <param name="orderedByOffersAsc">Indicates if the real estate agent should be ordered by the amount of offers they have</param>
        /// <returns>List of <see cref="RealEstateAgent"/></returns>
        Task<IEnumerable<RealEstateAgent>> GetRealEstateAgentsWithOffersAsync(OfferType offerType, IEnumerable<string> searchWords, int top = 10, bool orderedByOffersAsc = true);
    }
}