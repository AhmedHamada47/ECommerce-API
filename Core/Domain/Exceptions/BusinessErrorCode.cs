namespace Domain.Exceptions
{
    public enum BusinessErrorCode
    {
        // General
        BadRequest = 1,
        Forbidden = 2,
        Unauthorized = 3,
        NotFound = 4,

        // Auth
        EmailAlreadyRegistered = 101,
        InvalidCredentials = 102,

        // Users
        UserNotFound = 201,

        // Products
        ProductNotFound = 301,

        // Basket
        BasketNotFound = 401,
        BasketUpdateFailed = 402,

        // Orders
        OrderNotFound = 501,
        DeliveryMethodNotFound = 502,
    }
}