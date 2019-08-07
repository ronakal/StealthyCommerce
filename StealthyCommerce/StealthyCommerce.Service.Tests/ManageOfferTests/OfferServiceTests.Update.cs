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
        public class Update
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
            public void NullObject_ReturnsFalse()
            {
                // Arrange...
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.UpdateOffer(null);

                // Assert...
                Assert.IsFalse(result);
            }

            [Test]
            public void DescriptionSupplied_DescriptionSaved()
            {
                // Arrange...
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var testValue = "Test Description.";

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = ExistingOfferId, Description = testValue });

                // Assert...
                Assert.AreEqual(testValue, offerInDB.Description);
            }

            [Test]
            public void NumberOfTermsSupplied_NumberOfTermsSaved()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = 1;

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = 123, NumberOfTerms = 1 });

                // Assert...
                Assert.AreEqual(testValue, offerInDB.NumberOfTerms);
            }

            [Test]
            public void PriceSupplied_PriceSaved()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = 4.99M;

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = 123, Price = testValue });

                // Assert...
                Assert.AreEqual(testValue, offerInDB.Price);
            }

            [Test]
            public void DateCreatedSupplied_DateCreatedNotSaved()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = 123, DateCreated = DateTime.Now });

                // Assert...
                Assert.That(offerInDB.DateCreated, Is.Not.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void DateModifiedSupplied_DateModifiedNotSaved()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = 123, DateModified = DateTime.Now.Add(TimeSpan.FromMinutes(30)) });

                // Assert...
                Assert.That(offerInDB.DateModified, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void ProductIdDoesNotExist_NotEdited()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Returns(0);
                var productId = 45646;
                productServiceMock.Setup(p => p.Exists(It.Is<int>(i => i == productId))).Returns(false);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = 123, ProductId = productId });

                // Assert...
                Assert.False(result);
            }

            [Test]
            public void ProductIdExists_Edited()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Returns(0);
                var productId = 546456;
                productServiceMock.Setup(p => p.Exists(It.Is<int>(i => i == productId))).Returns(true);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = 123, ProductId = productId });

                // Assert...
                Assert.That(offerInDB.ProductId, Is.EqualTo(productId));
            }

            [Test]
            public void DateModifiedNull_DateUpdated()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = 123, Price = 1.99M });

                // Assert...
                Assert.That(offerInDB.DateModified, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            }
            
            [Test]
            public void IsActiveSupplied_EditAsActive()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = true;

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = 123, IsActive = testValue });

                // Assert...
                Assert.That(offerInDB.IsActive, Is.EqualTo(testValue));
            }

            [Test]
            public void Exception_LogsError()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                productServiceMock.Setup(p => p.Exists(It.IsAny<int>())).Returns(true);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = true;

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { OfferId = offerInDB.OfferId, IsActive = testValue });

                // Assert...
                loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            }

            [Test]
            public void Exception_ReturnsFalse()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = true;

                // Act...
                var result = offerService.UpdateOffer(new OfferDTO() { IsActive = testValue });

                // Assert...
                Assert.False(result);
            }
        }
    }
}
