using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto.Order;
using ServicesAbstractions;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var orders = await serviceManager.OrderService.GetOrdersForUserAsync(email);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var order = await serviceManager.OrderService.GetOrderByIdAsync(id, email);
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var order = await serviceManager.OrderService.CreateOrderAsync(
                email,
                createOrderDto.DeliveryMethodId,
                createOrderDto.BasketId,
                createOrderDto.ShippingAddress);
            return Ok(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var methods = await serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(methods);
        }
    }
}
