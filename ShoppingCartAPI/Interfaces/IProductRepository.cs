using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsByPriceAsync(decimal priceMin, decimal priceMax);
        Task<List<Product>> GetProductsByCategoryAsync(string category);
        Task<List<Product>> GetProductsByNameAsync(string name);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);
        IQueryable<Product> GetQueryable();
        Task<bool> UpdateProductStock(List<Cart> cartItems);
    }
}
