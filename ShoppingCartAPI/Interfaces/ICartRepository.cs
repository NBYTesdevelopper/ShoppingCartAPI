using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> InsertCartItemAsync(int customerId, int productId, int quantity);
        Task<List<Cart>> GetAllCartItemsAsync(int customerId);
        Task DeleteCartItemAsync(Cart cart);
        Task DeleteCartAsync(int customerId);
        Task<Cart?> GetCartItemByIdAsync(int cartItemId);
        Task UpdateProductQuantityAsync(Cart existingProduct, int quantity);
        Task<bool> UpdateCartItemQuantityAsync(Cart cartItem, int quantity);
        Task<Cart?> GetCartItemByProductId(int productId, int customerId);
    }
}
