using Microsoft.AspNetCore.Mvc;
using System.Linq;
using StealthyCommerce.Service.ManageOrder;

namespace StealthyCommerce.WebAPI.Controllers
{
    /// <summary>
    /// Gets the order details for the customer to view.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderDetailsController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        /// <summary>
        /// Retrieves the full order details.
        /// </summary>
        /// <param name="customerId">the customer id to pull the order for</param>
        /// <param name="id">the id of the order</param>
        /// <returns>the order details</returns>
        // GET: api/Orders/5
        [HttpGet("{customerId}")]
        public OrderDTO Get(int customerId, int id)
        {
            return orderService.ExistingOrders(customerId).FirstOrDefault(o => o.OrderId == id);
        }
    }
}
