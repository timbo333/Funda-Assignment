using Abstractions.Models;
using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Test.Unit
{
    [TestClass]
    public class RealEstateAgentServiceTests
    {
        private IRealEstateAgentService _sut;
        private IRepository _repositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = Substitute.For<IRepository>();
            _sut = new RealEstateAgentService(_repositoryMock);
        }

        [TestMethod]
        public void GetRealEstateAgentsWithOffersAsync_OfferTypeUnknown_ThrowsArgumentNullException()
        {
            // Arrange
            var offerType = OfferType.Unknown;
            Func<Task> func = async () => await _sut.GetRealEstateAgentsWithOffersAsync(offerType, Enumerable.Empty<string>());
            
            // Act && Assert
            func.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void GetRealEstateAgentsWithOffersAsync_NullForSearchWords_ThrowsArgumentNullException()
        {
            // Arrange
            var offerType = OfferType.Buy;
            Func<Task> func = async () => await _sut.GetRealEstateAgentsWithOffersAsync(offerType, null);

            // Act && Assert
            func.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetRealEstateAgentsWithOffersAsync_OfferTypeBuy_EmptySearchWords_Success()
        {
            // Arrange
            var offerType = OfferType.Buy;
            var searchWords = Enumerable.Empty<string>();

            // Act
            await _sut.GetRealEstateAgentsWithOffersAsync(offerType, searchWords);

            // Assert
            _ = await _repositoryMock.Received(1).GetOfferAsync(Arg.Is(offerType), Arg.Is(searchWords));
        }


        [TestMethod]
        public async Task GetRealEstateAgentsWithOffersAsync_OfferTypeRent_EmptySearchWords_Success()
        {
            // Arrange
            var offerType = OfferType.Rent;
            var searchWords = Enumerable.Empty<string>();

            // Act
            await _sut.GetRealEstateAgentsWithOffersAsync(offerType, searchWords);

            // Assert
            _ = await _repositoryMock.Received(1).GetOfferAsync(Arg.Is<OfferType>(offerType), Arg.Is(searchWords));
        }

        [TestMethod]
        public async Task GetRealEstateAgentsWithOffersAsync_SearchWordsProvided_Success()
        {
            // Arrange
            var offerType = OfferType.Buy;
            var searchWords = new string[] { "searchTermOne", "searchTermtwo" };

            // Act
            await _sut.GetRealEstateAgentsWithOffersAsync(offerType, searchWords);

            // Assert
            _ = await _repositoryMock.Received(1).GetOfferAsync(
                Arg.Is<OfferType>(offerType),
                Arg.Is<IEnumerable<string>>(s => s.SequenceEqual(searchWords)));
        }
    }
}
