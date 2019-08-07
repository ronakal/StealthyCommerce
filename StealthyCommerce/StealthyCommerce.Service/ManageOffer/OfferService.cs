using System;
using System.Linq;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.ManageProduct;
using StealthyCommerce.Service.StealthyCommerceDBEntities;

namespace StealthyCommerce.Service.ManageOffer
{
    /// <summary>
    /// Manages offers, will provide basic CRUD operations.
    /// </summary>
    public class OfferService : IOfferService
    {
        private ILoggerAdapter<IOfferService> logger;
        private IProductService productService;
        private StealthyCommerceDBContext context;

        public OfferService(StealthyCommerceDBContext context, ILoggerAdapter<IOfferService> logger, IProductService productService)
        {
            this.context = context;
            this.logger = logger;
            this.productService = productService;
        }
        
        /// <summary>
        /// Retrieves all of the offers, will require additional filtering before converting to an enumerable.
        /// </summary>
        /// <returns>all offers in the database</returns>
        public IQueryable<OfferDTO> GetOffers()
        {
            return context.Offer.Select(o => new OfferDTO()
            {
                OfferId = o.OfferId,
                ProductId = o.ProductId,
                Description = o.Description,
                Price = o.Price,
                NumberOfTerms = o.NumberOfTerms.Value,
                DateCreated = o.DateCreated,
                DateModified = o.DateModified,
                IsActive = o.IsActive
            });
        }

        /// <summary>
        /// Will create a new offer in the database.
        /// </summary>
        /// <param name="offer">the offer to create</param>
        /// <returns>the id if successfully added, -1 otherwise</returns>
        public int CreateOffer(OfferDTO offer)
        {
            var offerId = -1;

            try
            {
                // Make sure the offer is passed in and has a valid foreign key before adding.
                if (offer != null && this.productService.Exists(offer.ProductId))
                {
                    var newOffer = new Offer()
                    {
                        ProductId = offer.ProductId,
                        Description = offer.Description,
                        Price = offer.Price,
                        NumberOfTerms = offer.NumberOfTerms,
                        DateCreated = DateTime.Now,
                        DateModified = null,
                        IsActive = offer.IsActive
                    };

                    context.Offer.Add(newOffer);

                    context.SaveChanges();
                    offerId = newOffer.OfferId;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to create offer.", ex);
                offerId = -1;
            }

            return offerId;
        }

        /// <summary>
        /// Will update the offer in the database. Sets the ModifiedDate audit column.
        /// </summary>
        /// <param name="offer">the offer to update</param>
        /// <returns>true if successfully updated, false otherwise</returns>
        public bool UpdateOffer(OfferDTO offer)
        {
            var success = false;

            try
            {
                if (offer != null)
                {
                    var existing = context.Offer.Where(o => o.OfferId == offer.OfferId).SingleOrDefault();

                    if (existing != null && this.productService.Exists(offer.ProductId))
                    {
                        existing.ProductId = offer.ProductId;
                        existing.Description = offer.Description;
                        existing.Price = offer.Price;
                        existing.NumberOfTerms = offer.NumberOfTerms;
                        existing.DateModified = DateTime.Now;
                        existing.IsActive = offer.IsActive;
                        context.SaveChanges();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to add offer.", ex);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Will delete the specified offer in the database.
        /// </summary>
        /// <param name="offerId">the id of the offer to delete</param>
        /// <returns>true if deleted, false otherwise</returns>
        public bool DeleteOffer(int offerId)
        {
            var success = false;

            try
            {
                var toDelete = context.Offer.Where(o => o.OfferId == offerId).SingleOrDefault();

                if (toDelete != null)
                {
                    context.Offer.Remove(toDelete);
                    context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to delete offer.", ex);
                success = false;
            }

            return success;
        }
    }
}
