using System.Linq;

namespace StealthyCommerce.Service.ManageProduct
{
    /// <summary>
    /// Manages products, offers basic CRUD operations and checks for existence. Use this class for managing products in
    /// an administrative view.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products in the database, will require additional filtering before converting to an enumerable.
        /// </summary>
        /// <returns>all products in the database</returns>
        IQueryable<ProductDTO> GetProducts();

        /// <summary>
        /// Creates a product and returns the product id.
        /// </summary>
        /// <param name="product">the product to add</param>
        /// <returns>id of the product created, -1 if it failed</returns>
        int CreateProduct(ProductDTO product);

        /// <summary>
        /// Will update the product in the database. Sets the modified date column.
        /// </summary>
        /// <param name="product">product information to update</param>
        /// <returns>true if successful, false otherwise</returns>
        bool UpdateProduct(ProductDTO product);

        /// <summary>
        /// Deletes the specified product in the database.
        /// </summary>
        /// <param name="productId">id of the product to delete</param>
        /// <returns>true if successful, false otherwise</returns>
        bool DeleteProduct(int productId);

        /// <summary>
        /// Checks if the product exists in the database.
        /// </summary>
        /// <param name="productId">the product id to check for existence</param>
        /// <returns>true if exists, false otherwise</returns>
        bool Exists(int productId);
    }
}