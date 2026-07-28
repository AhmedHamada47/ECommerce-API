using AutoMapper;
using Domain.Models.Order;
using Shared.Dto.Order;

namespace Services.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<Order, OrderDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.GetTotal()))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
