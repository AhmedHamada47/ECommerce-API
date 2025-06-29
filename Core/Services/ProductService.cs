global using ServicesAbstractions;
global using Shared.Dto;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exeptions;
using Domain.Models;
using Services.Specefication;
using Shared.Dto.Products;

namespace Services
{
    public  class ProductService (IUnitOfWork unitOfWork ,IMapper mapper )
        : IProductService
    {
        public async Task<PagenatedRespones<ProductsRespone>> GetAllProductsAsync(ProductQueryParameter parameter)
        {
            var specification=new ProductWithBrandAndTypeSpecifications(parameter);
            var result =await unitOfWork.GetReposatory<Product, int>().GetAllAsync(specification);
            // using Auto Mapper
            var data= mapper.Map<IEnumerable<Product>, IEnumerable<ProductsRespone>>(result);
            var pagecnt=data.Count();

            var totalcount = await unitOfWork.GetReposatory<Product, int>()
                .CountAsync(new ProductCountSpecefications(parameter));

            return new(parameter.PageIndex, parameter.PageSize, totalcount, data);
        }
        public async Task<ProductsRespone> GetProductAsync(int id)
        {
            var specification = new ProductWithBrandAndTypeSpecifications(id);
            var product = await unitOfWork.GetReposatory<Product, int>().GetAsync(specification)??
                throw new ProductNotFoundEx(id);
            return mapper.Map<Product,ProductsRespone>(product);
        }
        public async Task<IEnumerable<BrandResponse>> GetBrandAsync()
        {
            // calling UnitOfWork
            var repo = unitOfWork.GetReposatory<ProductBrand, int>();
            var barnd = await repo.GetAllAsync();
            // using Auto Mapper
            // retuen result
            var result = mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandResponse>>(barnd);
            return result;
        }
        public async Task<IEnumerable<TypeResponse>> GetTypeAsync()
        {
            // calling UnitOfWork
            var repo = unitOfWork.GetReposatory<ProductType, int>();
            var types = await repo.GetAllAsync();
            // using Auto Mapper
            // retuen result
            var result = mapper.Map<IEnumerable<ProductType>,IEnumerable<TypeResponse>>(types);
            return result;
        }

        
    }
}
