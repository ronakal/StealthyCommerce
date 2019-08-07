using System;
using System.Collections.Generic;

namespace StealthyCommerce.Service.StealthyCommerceDBEntities
{
    public partial class Product
    {
        public Product()
        {
            Offer = new HashSet<Offer>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateModified { get; set; }
        public string Brand { get; set; }
        public string Term { get; set; }

        public virtual ICollection<Offer> Offer { get; set; }
    }
}
