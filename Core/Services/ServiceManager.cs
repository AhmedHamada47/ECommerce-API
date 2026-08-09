using AutoMapper;
using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServicesAbstractions;

namespace Services
{
    public class ServiceManager(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IBasketRepository basketReposatory,
        UserManager<Domain.Identity.AppUser> userManager,
        SignInManager<Domain.Identity.AppUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration) : IServiceManager
    {
        private readonly Lazy<IProductService> _lazyProductService =
            new(() => new ProductService(unitOfWork, mapper));
        private readonly Lazy<IBasketService> _lazyBasketService =
            new(() => new BasketService(basketReposatory, mapper));
        private readonly Lazy<IOrderService> _lazyOrderService =
            new(() => new OrderService(unitOfWork, basketReposatory, mapper));
        private readonly Lazy<IAuthService> _lazyAuthService =
            new(() => new AuthService(userManager, signInManager, mapper, configuration));
        private readonly Lazy<IUserService> _lazyUserService =
            new(() => new UserService(userManager, roleManager));

        public IProductService ProductService => _lazyProductService.Value;
        public IBasketService BasketService => _lazyBasketService.Value;
        public IOrderService OrderService => _lazyOrderService.Value;
        public IAuthService AuthService => _lazyAuthService.Value;
        public IUserService UserService => _lazyUserService.Value;
    }
}