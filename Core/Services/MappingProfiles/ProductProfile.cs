using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Shared.Dto.Products;

namespace Services.MappingProfiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(d => d.BrandName,
                options => options.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.TypeName,
                options => options.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl,
                options => options.MapFrom<PictureUrlResolver>());

            CreateMap<ProductBrand, BrandResponse>();
            CreateMap<ProductType, TypeResponse>();
        }
    }

    internal class PictureUrlResolver(IConfiguration configuration)
        : IValueResolver<Product, ProductResponse, string>
    {
        public string Resolve(Product source, ProductResponse destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{configuration["BaseUrl"]}{source.PictureUrl}";
            }
            return "";
        }
    }
}
