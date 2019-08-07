using System.Linq;

namespace StealthyCommerce.Service.ManageProduct
{
    /// <summary>
    /// Retrieves all of the product offerings in a customer oriented fashion.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Retrieves all of the products, in no particular order.
        /// </summary>
        /// <returns>all products</returns>
        IQueryable<ProductOfferDTO> GetProducts();

        /// <summary>
        /// Retrieves all of the products by price.
        /// </summary>
        /// <returns>all products by price</returns>
        IQueryable<ProductOfferDTO> GetProductsByPrice(bool descending);

        /// <summary>
        /// Retrieves all of the products by date created.
        /// </summary>
        /// <returns>all products by date created</returns>
        IQueryable<ProductOfferDTO> GetProductsByDateCreated(bool descending);

        /// <summary>
        /// Retrieves all of the products by name.
        /// </summary>
        /// <returns>all products by name</returns>
        IQueryable<ProductOfferDTO> GetProductsByName(bool descending);
    }
}