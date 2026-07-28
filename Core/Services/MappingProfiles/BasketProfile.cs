using AutoMapper;
using Domain.Basket;
using Shared.Dto.Basket;

namespace Services.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDto>()
                .ForMember(d => d.BasketItems, o => o.MapFrom(s => s.Items))
                .ReverseMap()
                .ForMember(d => d.Items, o => o.MapFrom(s => s.BasketItems));
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        }
    }
}
