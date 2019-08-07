using Moq;
using NUnit.Framework;
using System;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.ManageOffer;
using StealthyCommerce.Service.ManageProduct;
using StealthyCommerce.Service.Tests.MockUtilities;
using StealthyCommerce.Service.StealthyCommerceDBEntities;
using Microsoft.EntityFrameworkCore;

namespace StealthyCommerce.Service.Tests.ManageOfferTests
{
    public partial class OfferServiceTests
    {
        [TestFixture]
        public class Delete
        {
            private Mock<StealthyCommerceDBContext> dbMock;
            private Mock<ILoggerAdapter<IOfferService>> loggerMock;
            private Mock<IProductService> productServiceMock;
            private DbSet<Offer> offerSet;

            private const int ExistingOfferId = 123;
            private Offer offerInDB = new Offer()
            {
                OfferId = ExistingOfferId,
                ProductId = 567,
                Description = "This should be overridden at some point.",
                Price = 1.99M,
                DateCreated = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                DateModified = DateTime.Now.Subtract(TimeSpan.FromMinutes(20)),
                IsActive = false
            };

            [SetUp]
            public void SetUp()
            {
                dbMock = new Mock<StealthyCommerceDBContext>();

                // Mock the data store for offers.
                offerSet = (new Offer[] { offerInDB }).ToDbSet();
                dbMock.Setup(m => m.Offer).Returns(() => offerSet);

                loggerMock = new Mock<ILoggerAdapter<IOfferService>>();
                productServiceMock = new Mock<IProductService>();
                productServiceMock.Setup(p => p.Exists(It.IsAny<int>())).Returns(true);
            }

            [Test]
            public void InvalidId_ReturnsFalse()
            {
                // Arrange...
                
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Assert...
                var result = offerService.DeleteOffer(ExistingOfferId + 123);

                // Act...
                Assert.False(result);
            }

            [Test]
            public void ValidId_ReturnsTrue()
            {
                // Arrange...

                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Assert...
                var result = offerService.DeleteOffer(ExistingOfferId);

                // Act...
                Assert.True(result);
            }

            [Test]
            public void Exception_LogsError()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.DeleteOffer(123);

                // Assert...
                loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            }

            [Test]
            public void Exception_ReturnsFalse()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.DeleteOffer(123);

                // Assert...
                Assert.False(result);
            }
        }
    }
}
