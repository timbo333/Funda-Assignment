using Newtonsoft.Json;

namespace Abstractions.Models
{
    /// <summary>
    /// Loosely based type on what I would expect in a nuget package from the Funda API.
    /// </summary>
    public class PagingInformation
    {
        [JsonProperty("AantalPaginas")]
        public int NumberOfPages { get; set; }

        [JsonProperty("HuidigePagina")]
        public int CurrentPage { get; set; }

        [JsonProperty("VolgendeUrl")]
        public string NextUrl { get; set; }

        [JsonProperty("VorigeUrl")]
        public string PreviousUrl { get; set; }
    }
}
