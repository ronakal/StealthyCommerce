using System;
using System.Collections.Generic;
using System.Linq;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.StealthyCommerceDBEntities;

namespace StealthyCommerce.Service.ManageOrder
{
    /// <summary>
    /// Manages the adding and canceling orders and viewing order history.
    /// </summary>
    public class OrderService : IOrderService
    {
        private StealthyCommerceDBContext context;
        private ILoggerAdapter<IOrderService> logger;

        public OrderService(StealthyCommerceDBContext context, ILoggerAdapter<IOrderService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Finds the offers the user would like to buy and adds them to their orders.
        /// </summary>
        /// <param name="customerId">the customer making the purchase</param>
        /// <param name="offerIds">the offers being purchased</param>
        /// <returns>list of ids of the orders if successful, empty list otherwise</returns>
        public List<int> AddOrders(int customerId, int[] offerIds)
        {
            var successIds = new List<int>();

            try
            {
                var orders = new List<Order>();

                foreach (var offerId in offerIds)
                {
                    var offer = this.context.Offer.FirstOrDefault(o => o.OfferId == offerId);

                    if (offer == null)
                    {
                        throw new ArgumentOutOfRangeException("offerId", offerId, "The offer id specified is invalid.");
                    }

                    var endDate = DateTime.MinValue;

                    switch (offer.Product.Term?.ToLower())
                    {
                        case "monthly":
                            endDate = DateTime.Now.AddMonths(offer.NumberOfTerms ?? 1);
                            break;
                        case "annually":
                            endDate = DateTime.Now.AddYears(offer.NumberOfTerms ?? 1);
                            break;
                        default:
                            endDate = DateTime.Now.AddMonths(offer.NumberOfTerms ?? 1);
                            break;
                    }

                    var order = new Order()
                    {
                        CustomerId = customerId,
                        StartDate = DateTime.Now,
                        EndDate = endDate,
                        OfferId = offerId,
                        AmountCharged = offer.Price
                    };

                    orders.Add(order);
                }

                this.context.Order.AddRange(orders);
                this.context.SaveChanges();

                // Send back all of the successfully created order ids.
                successIds = orders.Select(o => o.OrderId).ToList();
            }
            catch(Exception ex)
            {
                logger.LogError("Failed to add orders for user.", ex);
            }

            return successIds;
        }

        /// <summary>
        /// List of orders that the customer already has, all of them.
        /// </summary>
        /// <param name="customerId">the customer who has the orders</param>
        /// <returns>list of orders in the database</returns>
        public IQueryable<OrderDTO> ExistingOrders(int customerId)
        {
            return this.context.Customer
                .Where(c => c.CustomerId == customerId)
                .SelectMany(c => c.Order)
                .Select(o => new OrderDTO()
                {
                    OrderId = o.OrderId,
                    AmountCharged = o.AmountCharged,
                    Cancelled = o.CancelDate,
                    Start = o.StartDate,
                    End = o.EndDate,
                    AmountRefunded = o.AmountRefunded
                });
        }

        /// <summary>
        /// Will calculate the total amount to be refunded and update the order with a cancel status and amount to refund.
        /// </summary>
        /// <param name="customerId">the customer to refund to</param>
        /// <param name="orderId">the order to refund</param>
        /// <returns>true if successfully canceled, false otherwise</returns>
        public bool CancelOrder(int customerId, int orderId)
        {
            var success = false;

            try
            {
                var order = this.context.Order.Where(o => o.CustomerId == customerId && o.OrderId == orderId).FirstOrDefault();

                if (order != null)
                {
                    // Determine number of days left, then get the difference.
                    var daysRemaining = (decimal)(order.EndDate - DateTime.Now).Days;

                    // Determine how many days there were originally.
                    var orignalDays = (order.EndDate - order.StartDate).Days;

                    // Avoiding the dreaded divide by zero errors.
                    if (order.AmountCharged.HasValue && order.AmountCharged.Value > 0 && orignalDays > 0)
                    {
                        var amountPerDay = order.AmountCharged.Value / orignalDays;
                        decimal refundAmount = 0.00M;

                        if (daysRemaining > 0)
                        {
                            // When calculating the total, do not consider today as already passed.
                            refundAmount = Math.Round(amountPerDay * (daysRemaining + 1), 2);
                        }

                        // Now denote the cancellation and refund amount.
                        order.CancelDate = DateTime.Now;
                        order.AmountRefunded = refundAmount;

                        this.context.SaveChanges();
                        success = true;
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError("Failed to cancel the customers order.", ex);
                success = false;
            }

            return success;
        }
    }
}
