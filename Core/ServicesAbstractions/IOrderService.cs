using Shared.Dto.Order;

namespace ServicesAbstractions
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IEnumerable<OrderDto>> GetOrdersForUserAsync(string buyerEmail);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Shared.Dto.Order.ShippingAddressDto shippingAddress);
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
    }
}
