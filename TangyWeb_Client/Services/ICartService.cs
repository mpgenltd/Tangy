using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Services
{
    public interface ICartService
    {
        public event Action OnChange;
        Task DecrementCart(ShoppingCart shoppingCart);
        Task IncrementCart(ShoppingCart shoppingCart);
    }
}
