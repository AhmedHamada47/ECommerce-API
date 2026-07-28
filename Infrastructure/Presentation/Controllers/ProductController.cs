using Microsoft.AspNetCore.Mvc;
using Shared.Dto;
using Shared.Dto.Products;
using ServicesAbstractions;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ProductResponse>>> GetAllProduct([FromQuery] ProductQueryParameter parameter)
        {
            var products = await serviceManager.ProductService.GetAllProductsAsync(parameter);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            var product = await serviceManager.ProductService.GetProductAsync(id);
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<BrandResponse>> GetBrand()
        {
            var brands = await serviceManager.ProductService.GetBrandAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<TypeResponse>> GetTypes()
        {
            var types = await serviceManager.ProductService.GetTypeAsync();
            return Ok(types);
        }
    }
}
