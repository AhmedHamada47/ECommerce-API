namespace ServicesAbstractions
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IBasketService BasketService { get; }
        IOrderService OrderService { get; }
        IAuthService AuthService { get; }
        IUserService UserService { get; }
    }
}