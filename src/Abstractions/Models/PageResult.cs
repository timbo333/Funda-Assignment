using Newtonsoft.Json;
using System.Collections.Generic;

namespace Abstractions.Models
{
    /// <summary>
    /// Loosely based type on what I would expect in a nuget package from the Funda API.
    /// </summary>
    public class PageResult<T>
    {
        [JsonProperty("Objects")]
        public IEnumerable<T> Offers { get; set; }

        [JsonProperty("Paging")]
        public PagingInformation PagingInformation { get; set; }

        [JsonProperty("TotaalAantalObjecten")]
        public int TotalObjectsForSale { get; set; }
    }
}
