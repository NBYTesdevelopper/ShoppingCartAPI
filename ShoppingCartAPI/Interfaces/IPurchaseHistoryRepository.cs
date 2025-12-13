using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Interfaces
{
    public interface IPurchaseHistoryRepository
    {
        Task AddCartToHistoryAsync(int customerId);
        Task<List<PurchaseHistory>> GetHistoryAsync(int customerId);
    }
}
