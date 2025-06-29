global using AutoMapper;
global using Domain.Models;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Shared.Dto.Products;

namespace Services.MappingProfiles
{
    internal class ProductProfile:Profile  // must use static file in program .cs 
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductsRespone>()
                .ForMember(d => d.BrandName,
                options => options.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.TyepeName,
                options => options.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl
                , options => options.MapFrom<PictureUrlResolver>());


                CreateMap<ProductBrand, BrandResponse>();
                  CreateMap<ProductType, TypeResponse>();

        }
    }
}
    internal class PictureUrlResolver(IConfiguration configuration)
        : IValueResolver<Product, ProductsRespone, string>
    {
        public string Resolve(Product source, ProductsRespone destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
            return $"{configuration["BaseUrl"]}{source.PictureUrl}";
        }
            return "";
        }
    }
