using Moq;
using NUnit.Framework;
using System;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.ManageProduct;
using StealthyCommerce.Service.StealthyCommerceDBEntities;

namespace StealthyCommerce.Service.Tests.ManageProductTests
{
    public partial class ProductServiceTests
    {
        [TestFixture]
        public class Create
        {
            private Mock<StealthyCommerceDBContext> dbMock;
            private Mock<ILoggerAdapter<IProductService>> loggerMock;

            [SetUp]
            public void SetUp()
            {
                dbMock = new Mock<StealthyCommerceDBContext>();
                loggerMock = new Mock<ILoggerAdapter<IProductService>>();
            }

            [Test]
            public void NullParameter_ReturnsNegativeOne()
            {
                // Arrange...
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var testResult = productSerivce.CreateProduct(null);

                // Assert...
                Assert.AreEqual(-1, testResult);
            }

            [Test]
			public void SetBrand_SavesBrand()
            {
                // Arrange...
                Product dbProduct = null;
                dbMock.Setup(db => db.Product.Add(It.IsAny<Product>())).Callback((Product p) => { dbProduct = p; });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Test Brand";

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO() { Brand = testValue });

                // Assert...
                Assert.That(dbProduct.Brand, Is.EqualTo(testValue));
            }

            [Test]
            public void CreateProduct_DateCreatedSet()
            {
                // Arrange...
                Product dbProduct = null;
                dbMock.Setup(db => db.Product.Add(It.IsAny<Product>())).Callback((Product p) => { dbProduct = p; });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Test Brand";

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO() { Brand = testValue });

                // Assert...
                Assert.That(dbProduct.DateCreated, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void CreateProductWithDateCreated_DateCreatedOverridden()
            {
                // Arrange...
                Product dbProduct = null;
                dbMock.Setup(db => db.Product.Add(It.IsAny<Product>())).Callback((Product p) => { dbProduct = p; });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Test Brand";

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO() { Brand = testValue, DateCreated = DateTime.Now.Subtract(TimeSpan.FromHours(5))});

                // Assert...
                Assert.That(dbProduct.DateCreated, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void CreateProduct_DateModifiedNull()
            {
                // Arrange...
                Product dbProduct = null;
                dbMock.Setup(db => db.Product.Add(It.IsAny<Product>())).Callback((Product p) => { dbProduct = p; });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Test Brand";

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO() { Brand = testValue });

                // Assert...
                Assert.That(dbProduct.DateModified, Is.Null);
            }

            [Test]
            public void CreateProductWithDateModified_DateModifiedOverriden()
            {
                // Arrange...
                Product dbProduct = null;
                dbMock.Setup(db => db.Product.Add(It.IsAny<Product>())).Callback((Product p) => { dbProduct = p; });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Test Brand";

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO() { Brand = testValue, DateModified = DateTime.Now.Subtract(TimeSpan.FromMinutes(30)) });

                // Assert...
                Assert.That(dbProduct.DateModified, Is.Null);
            }

            [Test]
            public void IsActiveProvided_IsActiveSet()
            {
                // Arrange...
                Product dbProduct = null;
                dbMock.Setup(db => db.Product.Add(It.IsAny<Product>())).Callback((Product p) => { dbProduct = p; });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = true;

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO() { IsActive = testValue });

                // Assert...
                Assert.That(dbProduct.IsActive, Is.EqualTo(testValue));
            }

            [Test]
            public void WithName_NameSet()
            {
                // Arrange...
                Product dbProduct = null;
                dbMock.Setup(db => db.Product.Add(It.IsAny<Product>())).Callback((Product p) => { dbProduct = p; });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Test Name";

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO() { Name = testValue });

                // Assert...
                Assert.That(dbProduct.Name, Is.EqualTo(testValue));
            }

            [Test]
            public void WithTerm_TermSet()
            {
                // Arrange...
                Product dbProduct = null;
                dbMock.Setup(db => db.Product.Add(It.IsAny<Product>())).Callback((Product p) => { dbProduct = p; });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);
                var testValue = "Annual";

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO() { Term = testValue });

                // Assert...
                Assert.That(dbProduct.Term, Is.EqualTo(testValue));
            }

            [Test]
            public void ThrowException_LogsError()
            {
                // Arrange...
                dbMock.Setup(db => db.SaveChanges()).Callback(() => { throw new Exception(); });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO());

                // Assert...
                loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            }

            [Test]
            public void ThrowException_ReturnsNegativeOne()
            {
                // Arrange...
                dbMock.Setup(db => db.SaveChanges()).Callback(() => { throw new Exception(); });
                var productSerivce = new ProductService(dbMock.Object, loggerMock.Object);

                // Act...
                var testResult = productSerivce.CreateProduct(new ProductDTO());

                // Assert...
                Assert.AreEqual(-1, testResult);
            }
        }
    }
}
