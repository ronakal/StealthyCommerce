using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using StealthyCommerce.Service.ManageProduct;

namespace StealthyCommerce.WebAPI.Controllers
{
    /// <summary>
    /// Everything product, offers all of the basic CRUD operations. Intended for administrative view use.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Retrieves all of the products in the database, this is recommended for administrator use. Please see the ProductOffer service.
        /// </summary>
        /// <returns>all products</returns>
        // GET: api/Product
        [HttpGet]
        public IEnumerable<ProductDTO> Get(int pageNumber = 0, int pageSize = 10)
        {
            return this.productService.GetProducts().OrderByDescending(o => o.DateModified).Skip((pageNumber + 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Retrieves the specific product for updating and viewing details.
        /// </summary>
        /// <param name="id">the id of the product to retrieve</param>
        /// <returns>the specified product</returns>
        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public ProductDTO Get(int id)
        {
            return this.productService.GetProducts().Where(p => p.ProductId == id).FirstOrDefault();
        }

        /// <summary>
        /// Creates a new product with the information supplied.
        /// </summary>
        /// <param name="product">the product details to save</param>
        /// <returns>the id of the product just created</returns>
        // POST: api/Product
        [HttpPost]
        public int Post([FromBody]ProductDTO product)
        {
            return this.productService.CreateProduct(product);
        }

        /// <summary>
        /// Updates the product with the details supplied, DateCreated and DateModified are not editable.
        /// </summary>
        /// <param name="id">the id of the product to save</param>
        /// <param name="product">the product information to save</param>
        // PUT: api/Product/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ProductDTO product)
        {
            product.ProductId = id;
            this.productService.UpdateProduct(product);
        }

        /// <summary>
        /// The product to delete, this is final, I hope you have one of those "Are you sure?" prompts.
        /// </summary>
        /// <param name="id"></param>
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.productService.DeleteProduct(id);
        }
    }
}
