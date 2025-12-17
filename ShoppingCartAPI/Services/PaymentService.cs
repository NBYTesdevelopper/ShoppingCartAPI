using ShoppingCartAPI.Exceptions;
using ShoppingCartAPI.Interfaces;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repositories;

namespace ShoppingCartAPI.Services
{
    public class PaymentService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPurchaseHistoryRepository _historyRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public PaymentService(
            ICartRepository cartRepo,
            IPurchaseHistoryRepository historyRepo,
            ICustomerRepository customerRepo,
            IProductRepository productRepo)
        {
            _cartRepository = cartRepo;
            _historyRepository = historyRepo;
            _customerRepository = customerRepo;
            _productRepository = productRepo;
        }

        // 9. Purchase
        public async Task<bool> PurchaseAsync(int customerId)
        {
            List<Cart> cartItems = await _cartRepository.GetAllCartItemsAsync(customerId);

            if (!cartItems.Any())
                throw new CartEmptyException(customerId);

            decimal total = cartItems.Sum(i => i.Product.Price * i.Quantity);

            Customer? customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
                throw new CustomerNotFoundException(customerId);

            // 🔴 COMPOUND DECISION (MCDC burada!)
            if (customer.Budget < total && total > 0)
                throw new InsufficientBudgetException(total, customer.Budget);

            var insufficientProducts = cartItems
                .Where(ci => ci.Product.Stock < ci.Quantity)
                .ToList();

            // 🔴 COMPOUND DECISION (istersen bonus)
            if (insufficientProducts.Any() && cartItems.Count > 0)
                throw new InsufficientStockException(insufficientProducts);

            // Payment
            decimal newBudget = customer.Budget - total;
            await _customerRepository.UpdateBudgetAsync(newBudget, customer);

            foreach (var cartItem in cartItems)
            {
                cartItem.Product.Stock -= cartItem.Quantity;
            }
            // Update product stock
            await _productRepository.UpdateProductStock(cartItems);

            // Move cart to history
            await _historyRepository.AddCartToHistoryAsync(customerId);

            // Empty the cart
            await _cartRepository.DeleteCartAsync(customerId);

            return true;
        }


        // 10. PurchaseHistory
        public async Task<List<PurchaseHistory>> PurchaseHistoryAsync(int customerId)
        {
            return await _historyRepository.GetHistoryAsync(customerId);
        }
        
    }

}
