using Abstractions.Models;
using Domain.Repositories;
using Infrastructure.Extensions.Options;
using Infrastructure.Repositories;
using Infrastructure.Test.Unit.Builders;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Test.Unit
{
    [TestClass]
    public class FundaRepositoryTests
    {
        private HttpClient _httpClientMock;
        private IOptions<FundaApiOptions> _optionsMock;
        private IRepository _sut;

        [TestInitialize]
        public void TestInitialize()
        {
            _httpClientMock = Substitute.For<HttpClient>();
            _optionsMock = Options.Create<FundaApiOptions>(new FundaApiOptions
            {
                BaseUrl = "http://test.com",
                AccessToken = "accessToken",
                ResponseContentType = "json"
            });
            _sut = new FundaRepository(_optionsMock, _httpClientMock);
        }

        [TestMethod]
        public void GetOfferAsync_OfferTypeUnknown_ThrowsArgumentNullException()
        {
            // Arrange
            var offerType = OfferType.Unknown;
            Func<Task> func = async () => await _sut.GetOfferAsync(offerType, Enumerable.Empty<string>());

            // Act && Assert
            func.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void GetOfferAsync_NullForSearchWords_ThrowsArgumentNullException()
        {
            // Arrange
            var offerType = OfferType.Buy;
            Func<Task> func = async () => await _sut.GetOfferAsync(offerType, null);

            // Act && Assert
            func.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetOfferAsync_OfferTypeBuy_EmptySearchWords_Success()
        {
            // Arrange
            var offerType = OfferType.Buy;
            var searchWords = Enumerable.Empty<string>();

            var offersToReturn = new List<Offer> { new Offer { Id = Guid.NewGuid() } };
            var pageResult = new PageResult<Offer>
            {
                PagingInformation = new PagingInformation { CurrentPage = 1, NumberOfPages = 1 },
                Offers = offersToReturn
            };
            var requestUri = new StringBuilder()
                .Append("/feeds/Aanbod.svc")
                .Append($"/{_optionsMock.Value.ResponseContentType}/{_optionsMock.Value.AccessToken}/")
                .Append($"?type=koop&page=1&pagesize=25")
                .ToString();

            var httpClient = new HttpClientBuilder()
                .AddResponse(requestUri, HttpStatusCode.OK, pageResult)
                .Build();

            _sut = new FundaRepository(_optionsMock, httpClient);

            // Act
            var result = await _sut.GetOfferAsync(offerType, searchWords);

            // Assert
            result.Count().ShouldBe(offersToReturn.Count);
            foreach (var offerToReturn in offersToReturn)
            {
                result.ShouldContain(offer => offer.Id == offerToReturn.Id);
            }
        }


        [TestMethod]
        public async Task GetOfferAsync_OfferTypeRent_EmptySearchWords_Success()
        {
            // Arrange
            var offerType = OfferType.Rent;
            var searchWords = Enumerable.Empty<string>();

            var offersToReturn = new List<Offer> { new Offer { Id = Guid.NewGuid() } };
            var pageResult = new PageResult<Offer>
            {
                PagingInformation = new PagingInformation { CurrentPage = 1, NumberOfPages = 1 },
                Offers = offersToReturn
            };
            var requestUri = new StringBuilder()
                .Append("/feeds/Aanbod.svc")
                .Append($"/{_optionsMock.Value.ResponseContentType}/{_optionsMock.Value.AccessToken}/")
                .Append($"?type=huur&page=1&pagesize=25")
                .ToString();

            var httpClient = new HttpClientBuilder()
                .AddResponse(requestUri, HttpStatusCode.OK, pageResult)
                .Build();

            _sut = new FundaRepository(_optionsMock, httpClient);

            // Act
            var result = await _sut.GetOfferAsync(offerType, searchWords);

            // Assert
            result.Count().ShouldBe(offersToReturn.Count);
            foreach (var offerToReturn in offersToReturn)
            {
                result.ShouldContain(offer => offer.Id == offerToReturn.Id);
            }
        }

        [TestMethod]
        public async Task GetOfferAsync_SearchWordsProvided_Success()
        {
            // Arrange
            var offerType = OfferType.Buy;
            var searchWords = new string[] { "searchTermOne", "searchTermtwo" };

            var offersToReturn = new List<Offer> { new Offer { Id = Guid.NewGuid() } };
            var pageResult = new PageResult<Offer>
            {
                PagingInformation = new PagingInformation { CurrentPage = 1, NumberOfPages = 1 },
                Offers = offersToReturn
            };
            var requestUri = new StringBuilder()
                .Append("/feeds/Aanbod.svc")
                .Append($"/{_optionsMock.Value.ResponseContentType}/{_optionsMock.Value.AccessToken}/")
                .Append($"?type=koop&zo=/{string.Join("/", searchWords)}/&page=1&pagesize=25")
                .ToString();

            var httpClient = new HttpClientBuilder()
                .AddResponse(requestUri, HttpStatusCode.OK, pageResult)
                .Build();

            _sut = new FundaRepository(_optionsMock, httpClient);

            // Act
            var result = await _sut.GetOfferAsync(offerType, searchWords);

            // Assert
            result.Count().ShouldBe(offersToReturn.Count);
            foreach (var offerToReturn in offersToReturn)
            {
                result.ShouldContain(offer => offer.Id == offerToReturn.Id);
            }
        }

        [TestMethod]
        public async Task GetOfferAsync_SearchWordsProvided_MultiplePages_Success()
        {
            // Arrange
            var offerType = OfferType.Buy;
            var searchWords = new string[] { "searchTermOne", "searchTermtwo" };

            // Create first response
            var firstOffersToReturn = new List<Offer> { new Offer { Id = Guid.NewGuid() } };
            var firstPageResult = new PageResult<Offer>
            {
                PagingInformation = new PagingInformation { CurrentPage = 1, NumberOfPages = 2 },
                Offers = firstOffersToReturn
            };
            var firstRequestUri = new StringBuilder()
                .Append("/feeds/Aanbod.svc")
                .Append($"/{_optionsMock.Value.ResponseContentType}/{_optionsMock.Value.AccessToken}/")
                .Append($"?type=koop&zo=/{string.Join("/", searchWords)}/&page=1&pagesize=25")
                .ToString();

            // Create seconds response (for second page)
            var secondOffersToReturn = new List<Offer> { new Offer { Id = Guid.NewGuid() } };
            var secondPageResult = new PageResult<Offer>
            {
                PagingInformation = new PagingInformation { CurrentPage = 2, NumberOfPages = 2 },
                Offers = secondOffersToReturn
            };
            var secondRequestUri = new StringBuilder()
                .Append("/feeds/Aanbod.svc")
                .Append($"/{_optionsMock.Value.ResponseContentType}/{_optionsMock.Value.AccessToken}/")
                .Append($"?type=koop&zo=/{string.Join("/", searchWords)}/&page=2&pagesize=25")
                .ToString();

            var httpClient = new HttpClientBuilder()
                .AddResponse(firstRequestUri, HttpStatusCode.OK, firstPageResult)
                .AddResponse(secondRequestUri, HttpStatusCode.OK, secondPageResult)
                .Build();

            _sut = new FundaRepository(_optionsMock, httpClient);

            // Act
            var result = await _sut.GetOfferAsync(offerType, searchWords);

            // Assert
            var allOffers = firstOffersToReturn.Concat(secondOffersToReturn);
            result.Count().ShouldBe(allOffers.Count());
            foreach (var offerToReturn in allOffers)
            {
                result.ShouldContain(offer => offer.Id == offerToReturn.Id);
            }
        }
    }
}
