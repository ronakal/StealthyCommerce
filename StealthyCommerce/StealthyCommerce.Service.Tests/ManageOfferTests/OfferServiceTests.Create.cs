using Moq;
using NUnit.Framework;
using System;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.ManageOffer;
using StealthyCommerce.Service.ManageProduct;
using StealthyCommerce.Service.StealthyCommerceDBEntities;

namespace StealthyCommerce.Service.Tests.ManageOfferTests
{
    public partial class OfferServiceTests
    {
        [TestFixture]
        public class Create
        {
            private Mock<StealthyCommerceDBContext> dbMock;
            private Mock<ILoggerAdapter<IOfferService>> loggerMock;
            private Mock<IProductService> productServiceMock;

            [SetUp]
            public void SetUp()
            {
                dbMock = new Mock<StealthyCommerceDBContext>();
                loggerMock = new Mock<ILoggerAdapter<IOfferService>>();
                productServiceMock = new Mock<IProductService>();
                productServiceMock.Setup(p => p.Exists(It.IsAny<int>())).Returns(true);
            }

            [Test]
            public void NullObject_ReturnsNegativeOne()
            {
                // Arrange...
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.CreateOffer(null);

                // Assert...
                Assert.AreEqual(-1, result);
            }

            [Test]
            public void DescriptionSupplied_DescriptionSaved()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>())).Callback((Offer o) => { offer = o; });
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var testValue = "Test Description.";

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { Description = testValue });

                // Assert...
                Assert.AreEqual(testValue, offer.Description);
            }

            [Test]
            public void NumberOfTermsSupplied_NumberOfTermsSaved()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>())).Callback((Offer o) => { offer = o; });
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = 1;

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { NumberOfTerms = 1 });

                // Assert...
                Assert.AreEqual(testValue, offer.NumberOfTerms);
            }

            [Test]
            public void PriceSupplied_PriceSaved()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>())).Callback((Offer o) => { offer = o; });
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = 1.99M;

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { Price = 1.99M });

                // Assert...
                Assert.AreEqual(testValue, offer.Price);
            }

            [Test]
            public void DateCreated_DateCreatedSaved()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>())).Callback((Offer o) => { offer = o; });
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { Price = 1.99M });

                // Assert...
                Assert.That(offer.DateCreated, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void ProductIdDoesNotExist_ReturnsNegativeOne()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>())).Callback((Offer o) => { offer = o; });
                dbMock.Setup(m => m.SaveChanges()).Returns(0);
                var productId = 123;
                productServiceMock.Setup(p => p.Exists(It.Is<int>(i => i == productId))).Returns(false);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { ProductId = productId });

                // Assert...
                Assert.AreEqual(-1, result);
            }

            [Test]
            public void ProductIdExists_Created()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>())).Callback((Offer o) => { offer = o; });
                dbMock.Setup(m => m.SaveChanges()).Returns(0);
                var productId = 123;
                productServiceMock.Setup(p => p.Exists(It.Is<int>(i => i == productId))).Returns(true);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { ProductId = productId });

                // Assert...
                Assert.That(offer.ProductId, Is.EqualTo(productId));
            }

            [Test]
            public void DateModifiedNull_Created()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>()))
                    .Callback((Offer o) => 
                    {
                        offer = o;
                        offer.OfferId = 123;
                    });
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { Price = 1.99M });

                // Assert...
                Assert.That(result, Is.GreaterThan(0));
            }

            [Test]
            public void DateModifiedSupplied_CreatedWithDateModifiedNull()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>())).Callback((Offer o) => { offer = o; });
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { DateModified = DateTime.Now });

                // Assert...
                Assert.That(offer.DateModified, Is.Null);
            }

            [Test]
            public void IsActiveSupplied_CreateAsActive()
            {
                // Arrange...
                Offer offer = null;
                dbMock.Setup(m => m.Offer.Add(It.IsAny<Offer>())).Callback((Offer o) => { offer = o; });
                dbMock.Setup(m => m.SaveChanges()).Returns(1);
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = true;

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { IsActive = testValue });

                // Assert...
                Assert.That(offer.IsActive, Is.EqualTo(testValue));
            }

            [Test]
            public void Exception_LogsError()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = true;

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { IsActive = testValue });

                // Assert...
                loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            }

            [Test]
            public void Exception_ReturnsNegativeOne()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                var offerService = new OfferService(dbMock.Object, loggerMock.Object, productServiceMock.Object);
                var testValue = true;

                // Act...
                var result = offerService.CreateOffer(new OfferDTO() { IsActive = testValue });

                // Assert...
                Assert.AreEqual(-1, result);
            }
        }
    }
}
