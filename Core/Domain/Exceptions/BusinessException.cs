namespace Domain.Exceptions
{
    public class BusinessException(BusinessErrorCode errorCode, params object?[] args)
        : Exception(FormatMessage(errorCode, args))
    {
        public BusinessErrorCode ErrorCode { get; } = errorCode;
        public object?[] Args { get; } = args;

        private static string FormatMessage(BusinessErrorCode errorCode, object?[] args)
        {
            var template = errorCode switch
            {
                BusinessErrorCode.EmailAlreadyRegistered => "Email already registered",
                BusinessErrorCode.InvalidCredentials => "Invalid email or password",
                BusinessErrorCode.UserNotFound => "User with id {0} not found",
                BusinessErrorCode.ProductNotFound => "Product with id {0} not found",
                BusinessErrorCode.BasketNotFound => "Basket with key {0} not found",
                BusinessErrorCode.BasketUpdateFailed => "Could not update basket. Please try again.",
                BusinessErrorCode.OrderNotFound => "Order with id {0} not found",
                BusinessErrorCode.DeliveryMethodNotFound => "Delivery method {0} not found",
                BusinessErrorCode.BadRequest => args.Length > 0 ? args[0]?.ToString() ?? "Bad request" : "Bad request",
                BusinessErrorCode.Forbidden => "You do not have permission to perform this action",
                BusinessErrorCode.Unauthorized => "Authentication required",
                BusinessErrorCode.NotFound => args.Length > 0 ? args[0]?.ToString() ?? "Resource not found" : "Resource not found",
                _ => "An error occurred"
            };

            try { return string.Format(template, args); }
            catch { return template; }
        }
    }
}