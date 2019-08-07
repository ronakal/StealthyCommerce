using System;
using System.Collections.Generic;

namespace StealthyCommerce.Service.StealthyCommerceDBEntities
{
    public partial class Offer
    {
        public Offer()
        {
            Order = new HashSet<Order>();
        }

        public int OfferId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public int? NumberOfTerms { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
