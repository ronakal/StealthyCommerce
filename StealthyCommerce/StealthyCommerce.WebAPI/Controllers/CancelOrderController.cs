using Microsoft.AspNetCore.Mvc;
using StealthyCommerce.Service.ManageOrder;

namespace StealthyCommerce.WebAPI.Controllers
{
    /// <summary>
    /// Responsible for handling order cancellation.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CancelOrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public CancelOrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        /// <summary>
        /// Will cancel the order specified, determine the refund amount and save to the order details in addition to returning the amount.
        /// </summary>
        /// <param name="id">the id of the order</param>
        /// <param name="customerId">the customers id being canceled</param>
        /// <returns>true if successfully canceled, false if otherwise</returns>
        // PUT: api/CancelOrder/5
        [HttpPut("{id}")]
        public bool Put(int id, int customerId)
        {
            return this.orderService.CancelOrder(customerId, id);
        }
    }
}
