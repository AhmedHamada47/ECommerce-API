using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto;
using Shared.Dto.Products;
using ServicesAbstractions;

namespace E_Commerce.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsController(
        IUnitOfWork unitOfWork,
        IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ProductResponse>>> GetAll()
        {
            var products = await serviceManager.ProductService.GetAllProductsAsync(new ProductQueryParameter());
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] ProductUpsertDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                PictureUrl = dto.PictureUrl,
                Price = dto.Price,
                BrandId = dto.BrandId,
                TypeId = dto.TypeId
            };

            var repo = unitOfWork.GetRepository<Product, int>();
            repo.Add(product);
            await unitOfWork.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductUpsertDto dto)
        {
            var repo = unitOfWork.GetRepository<Product, int>();
            var product = await repo.GetAsync(id);
            if (product == null) return NotFound();

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.PictureUrl = dto.PictureUrl;
            product.Price = dto.Price;
            product.BrandId = dto.BrandId;
            product.TypeId = dto.TypeId;

            repo.Update(product);
            await unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var repo = unitOfWork.GetRepository<Product, int>();
            var product = await repo.GetAsync(id);
            if (product == null) return NotFound();
            repo.Delete(product);
            await unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}
