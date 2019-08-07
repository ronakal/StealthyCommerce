
namespace StealthyCommerce.Service.ManageProduct
{
    /// <summary>
    /// What the product lists should be sortable by, in the future will be combinable.
    /// </summary>
    public enum SortOptions
    {
        /// <summary>
        /// The price, highest to lowest.
        /// </summary>
        Price = 0x1,
        /// <summary>
        /// When the offer was last modified or created.
        /// </summary>
        Created = 0x2,
        /// <summary>
        /// The name of the product alphabetically.
        /// </summary>
        Name = 0x4
    }
}
