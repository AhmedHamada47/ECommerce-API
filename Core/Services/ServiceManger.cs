using AutoMapper;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services

{
    public class ServiceManger(IMapper mapper,IUnitOfWork unitOfWork,IBasketReposatory basketReposatory)
        : IServiceManger
    {
        private readonly Lazy<IProductService> _lazyproductService=
                        new Lazy<IProductService>(() => new ProductService(unitOfWork,mapper));
        private readonly Lazy<IBasketService> _lazyBasketService =
     new Lazy<IBasketService>(() => new BasketService(basketReposatory, mapper));

        public IProductService ProductService => _lazyproductService.Value;
        public IBasketService BasketService => _lazyBasketService.Value;

      
    }
}