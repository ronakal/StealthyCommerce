using System.Linq;

namespace StealthyCommerce.Service.ManageOffer
{
    /// <summary>
    /// Manages offers, will provide basic CRUD operations.
    /// </summary>
    public interface IOfferService
    {
        /// <summary>
        /// Retrieves all of the offers, will require additional filtering before converting to an enumerable.
        /// </summary>
        /// <returns>all offers in the database</returns>
        IQueryable<OfferDTO> GetOffers();

        /// <summary>
        /// Will create a new offer in the database.
        /// </summary>
        /// <param name="offer">the offer to create</param>
        /// <returns>the id if successfully added, -1 otherwise</returns>
        int CreateOffer(OfferDTO offer);

        /// <summary>
        /// Will update the offer int he database. Sets the ModifiedDate audit column.
        /// </summary>
        /// <param name="offer">the offer to update</param>
        /// <returns>true if successfully updated, false otherwise</returns>
        bool UpdateOffer(OfferDTO offer);

        /// <summary>
        /// Will delete the specified offer in the database.
        /// </summary>
        /// <param name="offerId">the id of the offer to delete</param>
        /// <returns>true if deleted, false otherwise</returns>
        bool DeleteOffer(int offerId);
    }
}