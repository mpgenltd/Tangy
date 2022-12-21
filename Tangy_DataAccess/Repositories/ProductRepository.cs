using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDto> Create(ProductDto objDto)
        {
            var obj = _mapper.Map<ProductDto, Product>(objDto);
            var addedObj = _db.Products.Add(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<Product, ProductDto>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                _db.Products.Remove(obj);
                return await _db.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<ProductDto> Get(int id)
        {
            var obj = await _db.Products
                .Include(u => u.Category)
                .Include(u => u.ProductPrices)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                return _mapper.Map<Product, ProductDto>(obj);
            }

            return new ProductDto();
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(_db.Products
                .Include(u => u.Category)
                .Include(u => u.ProductPrices));
        }

        public async Task<ProductDto> Update(ProductDto objDto)
        {
            var objFromDb = await _db.Products.FirstOrDefaultAsync(u => u.Id == objDto.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = objDto.Name;
                objFromDb.Description = objDto.Description;
                objFromDb.ImageUrl = objDto.ImageUrl;
                objFromDb.CategoryId = objDto.CategoryId;
                objFromDb.Color = objDto.Color;
                objFromDb.ShopFavorites = objDto.ShopFavorites;
                objFromDb.CustomerFavorites = objDto.CustomerFavorites;
                _db.Products.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Product, ProductDto>(objFromDb);
            }

            return objDto;
        }
    }
}