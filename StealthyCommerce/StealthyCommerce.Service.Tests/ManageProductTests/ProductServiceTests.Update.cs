using Moq;
using NUnit.Framework;
using System;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.ManageProduct;
using StealthyCommerce.Service.Tests.MockUtilities;
using StealthyCommerce.Service.StealthyCommerceDBEntities;

namespace StealthyCommerce.Service.Tests.ManageProductTests
{
    public partial class ProductServiceTests
    {
        [TestFixture]
        public class Update
        {
            private Mock<StealthyCommerceDBContext> dbMock;
            private Mock<ILoggerAdapter<IProductService>> loggerMock;

            private const int ExistingProductId = 123;
            private Product productInDB = new Product()
            {
                ProductId = ExistingProductId,
                Name = "Albatross Sweatpants",
                Brand = "StealthyCommerce",
                Term = "Annually",
                DateCreated = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                DateModified = DateTime.Now.Subtract(TimeSpan.FromMinutes(20)),
                IsActive = false
            };

            [SetUp]
            public void SetUp()
            {
                dbMock = new Mock<StealthyCommerceDBContext>();
                loggerMock = new Mock<ILoggerAdapter<IProductService>>();

                // Mock the data store for products.
                var productSet = (new Product[] { productInDB }).ToDbSet();
                dbMock.Setup(m => m.Product).Returns(() => productSet);
            }

            [Test]
            public void NullProduct_ReturnsFalse()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var testResult = productSerivce.UpdateProduct(null);

                // Assert...
                Assert.IsFalse(testResult);
            }

            [Test]
            public void ProductIdHasNoMatch_ReturnsFalse()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = 4567 });

                // Assert...
                Assert.IsFalse(testResult);
            }

            [Test]
            public void NameSupplied_NameSet()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Swooshbatross";

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = ExistingProductId, Name = testValue });

                // Assert...
                Assert.That(productInDB.Name, Is.EqualTo(testValue));
            }

            [Test]
            public void BrandSupplied_BrandUpdated()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Test Brand";

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = ExistingProductId, Brand = testValue });

                // Assert...
                Assert.That(productInDB.Brand, Is.EqualTo(testValue));
            }

            [Test]
            public void TermSupplied_TermSet()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Monthly";

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = ExistingProductId, Term = testValue });

                // Assert...
                Assert.That(productInDB.Term, Is.EqualTo(testValue));
            }

            [Test]
            public void IsActiveSupplied_IsActiveSet()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = true;

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = ExistingProductId, IsActive = testValue });

                // Assert...
                Assert.That(productInDB.IsActive, Is.EqualTo(testValue));
            }

            [Test]
            public void DateCreatedSupplied_DateCreatedIgnored()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = true;
                var beforeUpdate = productInDB.DateCreated;

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = ExistingProductId, IsActive = testValue, DateCreated = DateTime.Now });

                // Assert...
                Assert.That(productInDB.DateCreated, Is.EqualTo(beforeUpdate));
            }

            [Test]
            public void DateModifiedSupplied_DateModifiedIgnored()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = true;

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = ExistingProductId, IsActive = testValue, DateModified = DateTime.Now.Subtract(TimeSpan.FromHours(37)) });

                // Assert...
                Assert.That(productInDB.DateModified, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void ThrowException_LogsError()
            {
                // Arrange...
                dbMock.Setup(db => db.SaveChanges()).Callback(() => { throw new Exception(); });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = ExistingProductId });

                // Assert...
                loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            }

            [Test]
            public void ThrowException_ReturnsFalse()
            {
                // Arrange...
                dbMock.Setup(db => db.SaveChanges()).Callback(() => { throw new Exception(); });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var testResult = productSerivce.UpdateProduct(new ProductDTO() { ProductId = ExistingProductId });

                // Assert...
                Assert.IsFalse(testResult);
            }
        }
    }
}
