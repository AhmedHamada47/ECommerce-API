using Domain.Contracts;
using Domain.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;

namespace E_Commerce.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController(
        IUnitOfWork unitOfWork,
        IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shared.Dto.Order.OrderDto>>> GetAll(
            [FromQuery] string? email = null)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var userOrders = await serviceManager.OrderService.GetOrdersForUserAsync(email);
                return Ok(userOrders);
            }

            var repo = unitOfWork.GetReposatory<Order, int>();
            var orders = await repo.GetAllAsync();
            return Ok(orders);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] OrderStatus status)
        {
            var repo = unitOfWork.GetReposatory<Order, int>();
            var order = await repo.GetAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            repo.Update(order);
            await unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}