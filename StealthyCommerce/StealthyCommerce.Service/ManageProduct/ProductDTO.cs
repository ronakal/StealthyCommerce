using System;

namespace StealthyCommerce.Service.ManageProduct
{
    /// <summary>
    /// A product is a subscription that is available depending on the IsActive flag and if there are currently
    /// any active valid offers.
    /// </summary>
    public class ProductDTO
    {
        /// <summary>
        /// The primary identifier of a product.
        /// </summary>
        public int ProductId { get; set; }
        
        /// <summary>
        /// The brand of the product.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the product is active, then customers can see it, else only administrators can see the product.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// How long the interval is for the product availability e.g. Monthly or Annually.
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// When the product was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// When the product was last modified, if null then the product was never updated.
        /// </summary>
        public DateTime? DateModified { get; set; }
    }
}
