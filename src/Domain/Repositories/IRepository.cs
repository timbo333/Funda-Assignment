using Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    /// <summary>
    /// A repository that talks to the funda Api to retrieve items.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Get offers from the funda api
        /// </summary>
        /// <param name="offerType">The type of offers the retrieve <see cref="OfferType"/></param>
        /// <param name="searchWords">Search wordt that the offers should match with</param>
        /// <returns>A list of <see cref="Offer"/></returns>
        Task<IEnumerable<Offer>> GetOfferAsync(OfferType type, IEnumerable<string> search);
    }
}
