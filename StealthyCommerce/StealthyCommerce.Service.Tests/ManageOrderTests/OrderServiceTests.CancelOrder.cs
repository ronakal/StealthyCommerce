using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public class CancelOrder
        {
            private Mock<StealthyCommerceDBContext> dbMock;
            private Mock<ILoggerAdapter<IOrderService>> loggerMock;
            private const int ExistingOfferId = 123;
            private Order orderInDB;
            private List<Order> ordersTableInDB;

            [SetUp]
            public void SetUp()
            {
                dbMock = new Mock<StealthyCommerceDBContext>();
                loggerMock = new Mock<ILoggerAdapter<IOrderService>>();
                ordersTableInDB = new List<Order>();
                orderInDB = new Order()
                {
                    CustomerId = 27,
                    OrderId = 493,
                    AmountCharged = 1.99M,
                    Offer = new Offer()
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
                            Term = "Monthly"
                        }
                    },
                    StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
                    EndDate = DateTime.Now
                };

                // Create mock of offers.
                var orderTableInDB = (new Order[] { orderInDB }).ToDbSet();
                dbMock.Setup(m => m.Order).Returns(() => orderTableInDB);
            }

            [Test]
            public void CancelLastDay_NothingRefunded()
            {
                // Arrange...
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                orderService.CancelOrder(orderInDB.CustomerId, orderInDB.OrderId);

                // Assert...
                Assert.AreEqual(0M, orderInDB.AmountRefunded);
            }

            [Test]
            public void CancelSameDay_RefundTotalAmount()
            {
                // Arrange...
                orderInDB.StartDate = DateTime.Now;
                orderInDB.EndDate = DateTime.Now.AddDays(30);
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                orderService.CancelOrder(orderInDB.CustomerId, orderInDB.OrderId);

                // Assert...
                Assert.AreEqual(orderInDB.Offer.Price, orderInDB.AmountRefunded);
            }

            [Test]
            public void CancelSixDaysBeforeMonthEnds_RefundAmountIs40Cents()
            {
                // Arrange...
                orderInDB.StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(24));
                orderInDB.EndDate = DateTime.Now.AddDays(6);
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                orderService.CancelOrder(orderInDB.CustomerId, orderInDB.OrderId);

                // Assert...
                Assert.AreEqual(0.40M, orderInDB.AmountRefunded);
            }

            [Test]
            public void Exception_LogsError()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                orderInDB.StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(24));
                orderInDB.EndDate = DateTime.Now.AddDays(6);
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                orderService.CancelOrder(orderInDB.CustomerId, orderInDB.OrderId);

                // Assert...
                loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            }

            [Test]
            public void Exception_ReturnsFalse()
            {
                // Arrange...
                dbMock.Setup(m => m.SaveChanges()).Throws<Exception>();
                orderInDB.StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(24));
                orderInDB.EndDate = DateTime.Now.AddDays(6);
                var orderService = new OrderService(dbMock.Object, loggerMock.Object);

                // Act...
                var result = orderService.CancelOrder(orderInDB.CustomerId, orderInDB.OrderId);

                // Assert...
                Assert.False(result);
            }
        }
    }
}
