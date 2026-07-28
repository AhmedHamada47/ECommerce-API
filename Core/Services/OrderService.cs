using AutoMapper;
using Domain.Basket;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Order;
using Services.Specification;
using Shared.Dto.Order;
using ServicesAbstractions;

namespace Services
{
    internal class OrderService(
        IUnitOfWork unitOfWork,
        IBasketReposatory basketReposatory,
        IMapper mapper) : IOrderService
    {
        public async Task<OrderDto> CreateOrderAsync(
            string buyerEmail,
            int deliveryMethodId,
            string basketId,
            ShippingAddressDto shippingAddress)
        {
            var basket = await basketReposatory.GetAsync(basketId)
                ?? throw new BusinessException(BusinessErrorCode.BasketNotFound, basketId);

            var deliveryMethod = await unitOfWork
                .GetReposatory<DeliveryMethod, int>().GetAsync(deliveryMethodId)
                ?? throw new BusinessException(BusinessErrorCode.DeliveryMethodNotFound, deliveryMethodId);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork
                    .GetReposatory<Domain.Models.Product, int>().GetAsync(item.Id);
                if (product != null)
                {
                    orderItems.Add(new OrderItem
                    {
                        ProductId = item.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl,
                        Price = product.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            var shippingAddressEntity = mapper.Map<ShippingAddress>(shippingAddress);

            var order = new Order
            {
                BuyerEmail = buyerEmail,
                ShippingAddress = shippingAddressEntity,
                DeliveryMethodId = deliveryMethodId,
                DeliveryMethod = deliveryMethod,
                OrderItems = orderItems,
                Subtotal = subtotal,
                Status = OrderStatus.Pending
            };

            var orderRepo = unitOfWork.GetReposatory<Order, int>();
            orderRepo.Add(order);
            await unitOfWork.SaveChangesAsync();

            await basketReposatory.DeleteAsync(basketId);

            return mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsSpecification(id, buyerEmail);
            var order = await unitOfWork
                .GetReposatory<Order, int>().GetAsync(spec)
                ?? throw new BusinessException(BusinessErrorCode.OrderNotFound, id);
            return mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsSpecification(buyerEmail);
            var orders = await unitOfWork
                .GetReposatory<Order, int>().GetAllAsync(spec);
            return mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork
                .GetReposatory<DeliveryMethod, int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }
    }
}