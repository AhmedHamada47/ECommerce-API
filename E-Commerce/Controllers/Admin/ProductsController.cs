using Domain.Contracts;
using Domain.Models;
using Domain.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Shared.Dto.Products.ProductResponse>>> GetAll()
        {
            var products = await serviceManager.ProductService.GetAllProductsAsync(new Shared.Dto.Products.ProductQueryParameter());
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            var repo = unitOfWork.GetReposatory<Product, int>();
            repo.Add(product);
            await unitOfWork.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest("Id mismatch");
            var repo = unitOfWork.GetReposatory<Product, int>();
            repo.Update(product);
            await unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var repo = unitOfWork.GetReposatory<Product, int>();
            var product = await repo.GetAsync(id);
            if (product == null) return NotFound();
            repo.Delete(product);
            await unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}