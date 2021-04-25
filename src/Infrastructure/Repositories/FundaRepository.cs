using Abstractions.Models;
using Domain.Repositories;
using Infrastructure.Builders;
using Infrastructure.Extensions.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// A repository that talks to the funda Api to retrieve items.
    /// </summary>
    public class FundaRepository : IRepository
    {
        private readonly FundaApiOptions _options;
        private readonly HttpClient _httpClient;

        public FundaRepository(IOptions<FundaApiOptions> options, HttpClient httpClient)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get offers from the funda api
        /// </summary>
        /// <param name="offerType">The type of offers the retrieve <see cref="OfferType"/></param>
        /// <param name="searchWords">Search wordt that the offers should match with</param>
        /// <returns>A list of <see cref="Offer"/></returns>
        public Task<IEnumerable<Offer>> GetOfferAsync(OfferType offerType, IEnumerable<string> searchWords)
        {
            if (offerType == OfferType.Unknown)
            {
                throw new ArgumentNullException(nameof(offerType));
            }

            if (searchWords == null)
            {
                throw new ArgumentNullException(nameof(searchWords));
            }

            // Map enum to string which can be used in the uri.
            string type = offerType switch
            {
                OfferType.Rent => "huur",
                OfferType.Buy => "koop",
                _ => "koop",
            };

            return GetOffersInternalAsync(type, searchWords.Any() ? $"/{string.Join("/", searchWords)}/" : default);
        }

        private async Task<IEnumerable<Offer>> GetOffersInternalAsync(string type, string search)
        {
            // Get the first page of result and add them to a offers list.
            var offers = new List<Offer>();
            var initialPageResult = await GetOffersForPageIndexAndSizeAsync(type, search, 1, 25).ConfigureAwait(false);
            offers.AddRange(initialPageResult.Offers);

            // Call the endpoint for every page, and add the retrieved offers to our offers list.
            for (var currentPage = initialPageResult.PagingInformation.CurrentPage + 1; currentPage <= initialPageResult.PagingInformation.NumberOfPages; currentPage++)
            {
                var currentPageResult = await GetOffersForPageIndexAndSizeAsync(type, search, currentPage, 25).ConfigureAwait(false);
                offers.AddRange(currentPageResult.Offers);
            }

            return offers;
        }

        private async Task<PageResult<Offer>> GetOffersForPageIndexAndSizeAsync(string type, string search, int pageIndex, int pageSize)
        {
            // Build a requestUri based on the provided information
            var requestUriBuilder = new FundaRequestUriBuilder()
                .WithResponseContentType(_options.ResponseContentType)
                .WithAccessToken(_options.AccessToken)
                .WithType(type);

            if (!string.IsNullOrWhiteSpace(search))
            {
                requestUriBuilder.WithSearch(search);
            }

            requestUriBuilder
                .WithPageIndex(pageIndex)
                .WithPageSize(pageSize);

            var requestUri = requestUriBuilder.Build();

            // Create a request and send it
            // HttpCompletionOption.ResponseHeadersRead is used for a small performance gain.
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<PageResult<Offer>>(responseJson);
        }
    }
}
