using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using StealthyCommerce.Service.ManageOrder;

namespace StealthyCommerce.WebAPI.Controllers
{
    /// <summary>
    /// Use this endpoint for viewing, adding and deleting customers orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        /// <summary>
        /// The customers order history.
        /// </summary>
        /// <param name="customerId">the id of the customer to get the orders from</param>
        /// <returns>the full list of the customers order history</returns>
        // GET: api/Orders
        [HttpGet("{customerId}", Name = "GetAllOrders")]
        public IQueryable<OrderDTO> Get(int customerId)
        {
            return orderService.ExistingOrders(customerId);
        }

        /// <summary>
        /// Use this to add a new order to the customers history.
        /// </summary>
        /// <param name="customerId">the id of the customer being charged</param>
        /// <param name="offers">the offers the customer is buying</param>
        /// <returns>list of ids of orders if successfully added, empty list otherwise</returns>
        // POST: api/Orders
        [HttpPost]
        public List<int> Post(int customerId, [FromBody] int[] offers)
        {
            return orderService.AddOrders(customerId, offers);
        }
    }
}
