using System.Net;

namespace Domain.Exceptions
{
    public static class BusinessErrorHttpStatusMapper
    {
        public static HttpStatusCode Map(BusinessErrorCode errorCode) => errorCode switch
        {
            BusinessErrorCode.BadRequest or BusinessErrorCode.BasketUpdateFailed => HttpStatusCode.BadRequest,
            BusinessErrorCode.Unauthorized or BusinessErrorCode.InvalidCredentials => HttpStatusCode.Unauthorized,
            BusinessErrorCode.Forbidden => HttpStatusCode.Forbidden,
            BusinessErrorCode.EmailAlreadyRegistered => HttpStatusCode.Conflict,
            BusinessErrorCode.NotFound or BusinessErrorCode.UserNotFound or BusinessErrorCode.ProductNotFound
                or BusinessErrorCode.BasketNotFound or BusinessErrorCode.OrderNotFound
                or BusinessErrorCode.DeliveryMethodNotFound => HttpStatusCode.NotFound,
            _ => throw new ArgumentOutOfRangeException(nameof(errorCode), errorCode, null)
        };
    }
}