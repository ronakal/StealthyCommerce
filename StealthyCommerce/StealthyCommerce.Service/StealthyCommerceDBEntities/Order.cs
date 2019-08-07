using System;
using System.Collections.Generic;

namespace StealthyCommerce.Service.StealthyCommerceDBEntities
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int OfferId { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int CustomerId { get; set; }
        public DateTime? CancelDate { get; set; }
        public decimal? AmountRefunded { get; set; }
        public decimal? AmountCharged { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
