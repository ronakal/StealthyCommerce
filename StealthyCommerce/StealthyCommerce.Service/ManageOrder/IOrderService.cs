using System.Collections.Generic;
using System.Linq;

namespace StealthyCommerce.Service.ManageOrder
{
    /// <summary>
    /// Manages the adding and canceling orders and viewing order history.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Finds the offers the user would like to buy and adds them to their orders.
        /// </summary>
        /// <param name="customerId">the customer making the purchase</param>
        /// <param name="offerIds">the offers being purchased</param>
        /// <returns>list of ids of the orders if successful, empty list otherwise</returns>
        List<int> AddOrders(int customerId, int[] offerIds);

        /// <summary>
        /// List of orders that the customer already has, all of them.
        /// </summary>
        /// <param name="customerId">the customer who has the orders</param>
        /// <returns>list of orders in the database</returns>
        IQueryable<OrderDTO> ExistingOrders(int customerId);

        /// <summary>
        /// Will calculate the total amount to be refunded and update the order with a cancel status and amount to refund.
        /// </summary>
        /// <param name="customerId">the customer to refund to</param>
        /// <param name="orderId">the order to refund</param>
        /// <returns>true if successfully canceled, false otherwise</returns>
        bool CancelOrder(int customerId, int orderId);
    }
}