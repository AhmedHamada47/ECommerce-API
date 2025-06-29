
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Dto;
using Shared.Dto.Products;
// using Shared.Dto;

namespace Presentation.Controllers
{

    [Route("api/[controller]")]

    [ApiController]
    public class ProductController(IServiceManger serviceManger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagenatedRespones<ProductsRespone>>> GetAllProduct([FromQuery]ProductQueryParameter parameter) // Get BaseUrl/product=>GetAll
        {
            var products = await serviceManger.ProductService.GetAllProductsAsync(parameter);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsRespone>> GetProduct(int id) // Get BaseUrl/api/product=>GetAll
        {
            var product = await serviceManger.ProductService.GetProductAsync(id);
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<BrandResponse>> GetBrand() // Get BaseUrl/api/product/BrandType
        {
            var Brand = await serviceManger.ProductService.GetBrandAsync();
            return Ok(Brand);
        }

        [HttpGet("types")]
        public async Task<ActionResult<BrandResponse>> GetTypes() // Get BaseUrl/api/product/types
        {
            var types = await serviceManger.ProductService.GetTypeAsync();
            return Ok(types);
        }


    }
}
 