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
        public class Delete
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
            public void InvalidId_ReturnsFalse()
            {
                // Arrange...
                var productService = new ProductService(dbMock.Object, loggerMock.Object);

                // Assert...
                var result = productService.DeleteProduct(ExistingProductId + 123);

                // Act...
                Assert.False(result);
            }

            [Test]
            public void ValidId_ReturnsTrue()
            {
                // Arrange...
                var productService = new ProductService(dbMock.Object, loggerMock.Object);

                // Assert...
                var result = productService.DeleteProduct(ExistingProductId);

                // Act...
                Assert.True(result);
            }

            [Test]
            public void Exception_LogsError()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                var productService = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var result = productService.DeleteProduct(123);

                // Assert...
                loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            }

            [Test]
            public void Exception_ReturnsFalse()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                var productService = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var result = productService.DeleteProduct(123);

                // Assert...
                Assert.False(result);
            }
        }
    }
}
