using System;
using System.Linq;
using StealthyCommerce.Service.Logging;
using StealthyCommerce.Service.StealthyCommerceDBEntities;

namespace StealthyCommerce.Service.ManageProduct
{
    /// <summary>
    /// Manages products, offers basic CRUD operations and checks for existence. Use this class for managing products in
    /// an administrative view.
    /// </summary>
    public class ProductService : IProductService
    {
        private ILoggerAdapter<IProductService> logger;
        private StealthyCommerceDBContext context;

        public ProductService(StealthyCommerceDBContext context, ILoggerAdapter<IProductService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all products in the database, will require additional filtering before converting to an enumerable.
        /// </summary>
        /// <returns>all products in the database</returns>
        public IQueryable<ProductDTO> GetProducts()
        {
            return context.Product.Select(p => new ProductDTO()
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Brand = p.Brand,
                Term = p.Term,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                IsActive = p.IsActive
            });
        }

        /// <summary>
        /// Creates a product and returns the product id.
        /// </summary>
        /// <param name="product">the product to add</param>
        /// <returns>id of the product created, -1 if it failed</returns>
        public int CreateProduct(ProductDTO product)
        {
            var productId = -1;

            try
            {
                // Make sure the offer is passed in and has a valid foreign key before adding.
                if (product != null)
                {
                    var newProduct = new Product()
                    {
                        Brand = product.Brand,
                        IsActive = product.IsActive,
                        Name = product.Name,
                        Term = product.Term,
                        DateCreated = DateTime.Now
                    };

                    context.Product.Add(newProduct);
                    context.SaveChanges();
                    productId = newProduct.ProductId;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to create product.", ex);
                productId = -1;
            }

            return productId;
        }

        /// <summary>
        /// Will update the product in the database. Sets the modified date column.
        /// </summary>
        /// <param name="product">product information to update</param>
        /// <returns>true if successful, false otherwise</returns>
        public bool UpdateProduct(ProductDTO product)
        {
            var success = false;

            try
            {
                if (product != null)
                {
                    var existing = context.Product.Where(p => p.ProductId == product.ProductId).SingleOrDefault();

                    if (existing != null)
                    {
                        existing.Name = product.Name;
                        existing.Brand = product.Brand;
                        existing.Term = product.Term;
                        existing.DateModified = DateTime.Now;
                        existing.IsActive = product.IsActive;
                        context.SaveChanges();
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to update product.", ex);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Deletes the specified product in the database.
        /// </summary>
        /// <param name="productId">id of the product to delete</param>
        /// <returns>true if successful, false otherwise</returns>
        public bool DeleteProduct(int productId)
        {
            var success = false;

            try
            {
                var toDelete = context.Product.Where(p => p.ProductId == productId).SingleOrDefault();

                if (toDelete != null)
                {
                    context.Product.Remove(toDelete);
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

        /// <summary>
        /// Checks if the product exists in the database.
        /// </summary>
        /// <param name="productId">the product id to check for existence</param>
        /// <returns>true if exists, false otherwise</returns>
        public bool Exists(int productId)
        {
            var exists = false;

            try
            {
                exists = context.Product.Any(p => p.ProductId == productId);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to validate if product exists.", ex);
            }

            return exists;
        }
    }
}
