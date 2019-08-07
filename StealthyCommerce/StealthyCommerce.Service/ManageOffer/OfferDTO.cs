using System;

namespace StealthyCommerce.Service.ManageOffer
{
    /// <summary>
    /// An offer is a current available run of a product. Special deals and exclusive access is offered and purchased
    /// as an offer. 
    /// </summary>
    public class OfferDTO
    {
        /// <summary>
        /// The database identifier of the offer.
        /// </summary>
        public int OfferId { get; set; }

        /// <summary>
        /// A foreign key to the Product id.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// A description of the offer that is presented to customers.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The current rate of the offer.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// How long the subscription is available for.
        /// </summary>
        public int NumberOfTerms { get; set; }

        /// <summary>
        /// When the offer was created, this is for audit purposes.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// When the offer was last modified, this is for audit purposes.
        /// </summary>
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// If the offer is currently active. If the product is still available, then this will be checked so that
        /// customers only see offers that are currently active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
