using Microsoft.AspNetCore.Mvc;
using System.Linq;
using StealthyCommerce.Service.ManageProduct;

namespace StealthyCommerce.WebAPI.Controllers
{
    /// <summary>
    /// Shows what product offerings are currently available. Provides capabilities for paging, sorting.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOfferController : ControllerBase
    {
        private readonly ISearchService searchService;

        public ProductOfferController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        /// <summary>
        /// Retrieves all of the product offerings that are currently available.
        /// </summary>
        /// <param name="pageNumber">the page number to pull</param>
        /// <param name="pageSize">the number of records to sort by</param>
        /// <param name="sortOption">which column to order by, can only order by one item right now (Price - 1, Created - 2, Name - 4)</param>
        /// <param name="descending">if it selected column should be ascending or descending</param>
        /// <returns>list of products currently available, sorted and paged</returns>
        // GET: api/ProductOffer
        [HttpGet]
        public IQueryable<ProductOfferDTO> Get(int pageNumber = 0, int pageSize = 10, SortOptions sortOption = SortOptions.Created, bool descending = true)
        {
            IQueryable<ProductOfferDTO> result = null;

            switch (sortOption)
            {
                case SortOptions.Price:
                    result = this.searchService.GetProductsByPrice(descending);
                    break;
                case SortOptions.Created:
                    result = this.searchService.GetProductsByDateCreated(descending);
                    break;
                case SortOptions.Name:
                    result = this.searchService.GetProductsByName(descending);
                    break;
                default:
                    result = this.searchService.GetProducts();
                    break;
            }

            return result.Skip((pageNumber + 1) * pageSize).Take(pageSize);
        }
    }
}