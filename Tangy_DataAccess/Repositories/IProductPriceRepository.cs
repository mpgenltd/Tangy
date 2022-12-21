using Tangy_Models;

namespace Tangy_DataAccess.Repositories
{
    public interface IProductPriceRepository
    {
        public Task<ProductPriceDto> Create(ProductPriceDto objDto);
        public Task<ProductPriceDto> Update(ProductPriceDto objDto);
        public Task<int> Delete(int id);
        public Task<ProductPriceDto> Get(int id);
        public Task<IEnumerable<ProductPriceDto>> GetAll(int? id = null);
    }
}