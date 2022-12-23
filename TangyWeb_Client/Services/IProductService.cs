using Tangy_Models;

namespace TangyWeb_Client.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAll();
        public Task<ProductDto> Get(int productId);
    }
}