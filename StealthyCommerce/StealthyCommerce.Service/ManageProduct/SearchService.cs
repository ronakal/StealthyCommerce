using System.Linq;
using StealthyCommerce.Service.StealthyCommerceDBEntities;

namespace StealthyCommerce.Service.ManageProduct
{
    /// <summary>
    /// Retrieves all of the product offerings in a customer oriented fashion.
    /// </summary>
    public class SearchService : ISearchService
    {
        private StealthyCommerceDBContext context;

        public SearchService(StealthyCommerceDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves all of the products, in no particular order.
        /// </summary>
        /// <returns>all products</returns>
        public IQueryable<ProductOfferDTO> GetProducts()
        {
            return from o in this.context.Offer
                   join p in this.context.Product on o.ProductId equals p.ProductId
                   where o.IsActive && p.IsActive
                   select new ProductOfferDTO()
                   {
                       ProductId = p.ProductId,
                       OfferId = o.OfferId,
                       Brand = p.Brand,
                       Description = o.Description,
                       Price = o.Price,
                       ProductName = p.Name,
                       CreatedDate = o.DateModified ?? o.DateCreated
                   };
        }

        /// <summary>
        /// Retrieves all of the products by price.
        /// </summary>
        /// <returns>all products by price</returns>
        public IQueryable<ProductOfferDTO> GetProductsByPrice(bool descending)
        {
            if (descending)
            {
                return GetProducts().OrderByDescending(p => p.Price);
            }
            else
            {
                return GetProducts().OrderBy(p => p.Price);
            }
        }

        /// <summary>
        /// Retrieves all of the products by date created.
        /// </summary>
        /// <returns>all products by date created</returns>
        public IQueryable<ProductOfferDTO> GetProductsByDateCreated(bool descending)
        {
            if (descending)
            {
                return GetProducts().OrderBy(p => p.CreatedDate);
            }
            else
            {
                return GetProducts().OrderByDescending(p => p.CreatedDate);
            }
        }

        /// <summary>
        /// Retrieves all of the products by name.
        /// </summary>
        /// <returns>all products by name</returns>
        public IQueryable<ProductOfferDTO> GetProductsByName(bool descending)
        {
            if (descending)
            {
                return GetProducts().OrderByDescending(p => p.ProductName);
            }
            else
            {
                return GetProducts().OrderBy(p => p.ProductName);
            }
        }
    }
}
