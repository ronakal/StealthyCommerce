using System;

namespace StealthyCommerce.Service.ManageOrder
{
    /// <summary>
    /// A customers order.
    /// </summary>
    public class OrderDTO
    {
        /// <summary>
        /// The primary identifier of the order.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// When the subscription started.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// When the subscription will end.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// If this is not null, then the order was canceled.
        /// </summary>
        public DateTime? Cancelled { get; set; }

        /// <summary>
        /// The amount that was refunded.
        /// </summary>
        public decimal? AmountRefunded { get; set; }

        /// <summary>
        /// The amount the customer was origianlly charged.
        /// </summary>
        public decimal? AmountCharged { get; set; }
    }
}
