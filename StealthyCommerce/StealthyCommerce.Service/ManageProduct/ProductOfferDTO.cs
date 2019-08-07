
using System;

namespace StealthyCommerce.Service.ManageProduct
{
    public class ProductOfferDTO
    {
        public int ProductId { get; set; }

        public int OfferId { get; set; }

        public string Brand { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
