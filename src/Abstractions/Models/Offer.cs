using Newtonsoft.Json;
using System;

namespace Abstractions.Models
{
    /// <summary>
    /// Loosely based type on what I would expect in a nuget package from the Funda API.
    /// </summary>
    public class Offer
    {
        public Guid Id { get; set; }

        [JsonProperty("Adres")]
        public string Address { get; set; }

        [JsonProperty("Woonplaats")]
        public string City { get; set; }

        [JsonProperty("MakelaarId")]
        public int RealEstateAgentId { get; set; }

        [JsonProperty("MakelaarNaam")]
        public string RealEstateAgentName { get; set; }

        [JsonProperty("Soort-aanbod")]
        public string Type { get; set; }
    }
}