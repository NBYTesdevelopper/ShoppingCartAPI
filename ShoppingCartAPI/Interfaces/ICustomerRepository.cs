using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(string name, string password);
        Task UpdateBudgetAsync(decimal budget, Customer customer);
        Task<Customer?> GetCustomerByNameAsync(string name);
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<Customer?> GetCustomerByCredentials(string name, string password);
    }
}
