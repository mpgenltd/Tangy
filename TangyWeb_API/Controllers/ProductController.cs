using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Tangy_DataAccess.Repositories;
using Tangy_Models;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAll();
            foreach (var product in products)
            {
                AddPrices(product);
            }
            
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int? productId)
        {
            if (productId == null || productId == 0)
            {
                return BadRequest(new ErrorModelDto
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var product = await _productRepository.Get(productId.Value);
            if (product == null)
            {
                return BadRequest(new ErrorModelDto
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            AddPrices(product);
            
            return Ok(product);
        }
        
        private void AddPrices(ProductDto product)
        {
            var prices = new List<ProductPriceDto>();
            prices.Add(new ProductPriceDto
            {
                Id = (product.Id * 1000) + 1,
                Price = 1.50,
                ProductId = product.Id,
                Size = "Medium"
            });
            prices.Add(new ProductPriceDto
            {
                Id = (product.Id * 1000) + 2,
                Price = 2.50,
                ProductId = product.Id,
                Size = "Large"
            });

            product.ProductPrices = prices;
        }
    }
}