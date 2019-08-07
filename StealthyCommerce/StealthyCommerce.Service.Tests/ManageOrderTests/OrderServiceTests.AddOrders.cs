using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.ManageOrder;
using StealthyCommerce.Service.Tests.MockUtilities;
using StealthyCommerce.Service.StealthyCommerceDBEntities;

namespace StealthyCommerce.Service.Tests.ManageOrderTests
{
    public partial class OrderServiceTests
    {
        [TestFixture]
        public class AddOrders
        {
            private Mock<StealthyCommerceDBContext> dbMock;
            private Mock<ILoggerAdapter<IOrderService>> loggerMock;
            private const int ExistingOfferId = 123;
            private Offer productInDB;
            private List<Order> ordersTableInDB;

            [SetUp]
            public void SetUp()
            {
                dbMock = new Mock<StealthyCommerceDBContext>();
                loggerMock = new Mock<ILoggerAdapter<IOrderService>>();
                ordersTableInDB = new List<Order>();
                productInDB = new Offer()
                {
                    OfferId = ExistingOfferId,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Description = "Test instance.",
                    IsActive = true,
                    NumberOfTerms = 1,
                    Price = 1.99M,
                    Product = new Product()
                    {
                        Brand = "A&WW",
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        Name = "Test Product",
                        Term = "Annually"
                    }
                };

                // Create mock of offers.
                var offerTableInDB = (new Offer[] { productInDB }).ToDbSet();
                dbMock.Setup(m => m.Offer).Returns(() => offerTableInDB);

                // Create mock of orders.
                var ordersDbSet = (new Order[] { })
                    .ToDbSet<Order, IEnumerable<Order>>(s => s.AddRange(It.IsAny<IEnumerable<Order>>()), // Setup
                                (Order) => ordersTableInDB.AddRange(Order)); // Callback

                dbMock.Setup(m => m.Order).Returns(() => ordersDbSet);
            }

            [Test]
            public void EmptyList_EmptyResult()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                var result = orderService.AddOrders(0, new int[0]);

                // Assert...
                Assert.IsEmpty(result);
            }

            [Test]
            public void NullList_EmptyResult()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                var result = orderService.AddOrders(0, null);

                // Assert...
                Assert.IsEmpty(result);
            }

            [Test]
            public void InvalidOfferId_EmptyResult()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                var result = orderService.AddOrders(0, null);

                // Assert...
                Assert.IsEmpty(result);
            }

            [Test]
            public void ValidOfferId_NewProductId()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                var result = orderService.AddOrders(0, new int[] { ExistingOfferId });

                // Assert...
                Assert.AreEqual(1, result.Count);
            }

            [Test]
            public void ValidOfferId_OrderCreatedWithValidEndDate()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);
                this.productInDB.Product.Term = "Annually";
                var estimatedEndDate = DateTime.Now.AddYears(1);

                // Act...
                var result = orderService.AddOrders(0, new int[] { ExistingOfferId });
                var order = ordersTableInDB.FirstOrDefault();

                // Assert...
                Assert.That(order.EndDate, Is.EqualTo(estimatedEndDate).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void OfferTermsInMonths_OrderCreatedWithValidEndDate()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);
                this.productInDB.Product.Term = "Monthly";
                var estimatedEndDate = DateTime.Now.AddMonths(1);

                // Act...
                var result = orderService.AddOrders(0, new int[] { ExistingOfferId });
                var order = ordersTableInDB.FirstOrDefault();

                // Assert...
                Assert.That(order.EndDate, Is.EqualTo(estimatedEndDate).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void OfferValid_OrderCreatedWithEndDateOf30Days()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);
                this.productInDB.Product.Term = "";
                var estimatedEndDate = DateTime.Now.AddMonths(1);

                // Act...
                var result = orderService.AddOrders(0, new int[] { ExistingOfferId });
                var order = ordersTableInDB.FirstOrDefault();

                // Assert...
                Assert.That(order.EndDate, Is.EqualTo(estimatedEndDate).Within(TimeSpan.FromSeconds(1)));
            }

            [Test]
            public void OfferValid_OrderCreatedWithCustomerIdSpecified()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);
                this.productInDB.Product.Term = "";
                var estimatedEndDate = DateTime.Now.AddMonths(1);

                // Act...
                var result = orderService.AddOrders(555, new int[] { ExistingOfferId });
                var order = ordersTableInDB.FirstOrDefault();

                // Assert...
                Assert.That(order.CustomerId, Is.EqualTo(555));
            }
        }
    }
}
