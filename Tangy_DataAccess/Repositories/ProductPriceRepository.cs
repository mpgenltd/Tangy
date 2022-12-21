using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_DataAccess.Repositories
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductPriceRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductPriceDto> Create(ProductPriceDto objDto)
        {
            var obj = _mapper.Map<ProductPriceDto, ProductPrice>(objDto);

            var addedObj = _db.ProductPrices.Add(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<ProductPrice, ProductPriceDto>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.ProductPrices.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                _db.ProductPrices.Remove(obj);
                return await _db.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<ProductPriceDto> Get(int id)
        {
            var obj = await _db.ProductPrices.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                return _mapper.Map<ProductPrice, ProductPriceDto>(obj);
            }

            return new ProductPriceDto();
        }

        public async Task<IEnumerable<ProductPriceDto>> GetAll(int? id = null)
        {
            if (id != null && id > 0)
            {
                return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDto>>
                    (_db.ProductPrices.Where(u => u.ProductId == id));
            }
            else
            {
                return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDto>>(_db.ProductPrices);
            }
        }

        public async Task<ProductPriceDto> Update(ProductPriceDto objDto)
        {
            var objFromDb = await _db.ProductPrices.FirstOrDefaultAsync(u => u.Id == objDto.Id);
            if (objFromDb != null)
            {
                objFromDb.Price = objDto.Price;
                objFromDb.Size = objDto.Size;
                objFromDb.ProductId = objDto.ProductId;
                _db.ProductPrices.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<ProductPrice, ProductPriceDto>(objFromDb);
            }

            return objDto;
        }
    }
}