using System.Net;

namespace Domain.Exceptions
{
    public static class BusinessErrorHttpStatusMapper
    {
        public static HttpStatusCode Map(BusinessErrorCode errorCode) => errorCode switch
        {
            BusinessErrorCode.EmailAlreadyRegistered => HttpStatusCode.Conflict,
            BusinessErrorCode.InvalidCredentials => HttpStatusCode.Unauthorized,
            BusinessErrorCode.Unauthorized => HttpStatusCode.Unauthorized,
            BusinessErrorCode.Forbidden => HttpStatusCode.Forbidden,
            BusinessErrorCode.BadRequest => HttpStatusCode.BadRequest,
            BusinessErrorCode.BasketUpdateFailed => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.NotFound
        };
    }
}