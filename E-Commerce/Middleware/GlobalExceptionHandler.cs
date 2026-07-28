using System.Net;
using System.Text.Json;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Shared.ErrorModels;

namespace E_Commerce.Middleware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = CreateErrorResponse(exception);

            httpContext.Response.StatusCode = response.StatusCode;
            httpContext.Response.ContentType = "application/json";

            var body = ApiResponse<object?>.Failure(
                response.Message,
                response.Errors.Count > 0 ? response.Errors : null);

            await httpContext.Response.WriteAsJsonAsync(body, JsonOptions, cancellationToken);
            return true;
        }

        private (int StatusCode, string Message, List<ApiError> Errors) CreateErrorResponse(Exception exception)
        {
            if (exception is BusinessException businessEx)
            {
                logger.LogWarning(businessEx, "Business error: {Code}", businessEx.ErrorCode);

                var statusCode = (int)BusinessErrorHttpStatusMapper.Map(businessEx.ErrorCode);

                return (statusCode, businessEx.Message,
                [
                    new ApiError
                    {
                        Code = businessEx.ErrorCode.ToString(),
                        Description = businessEx.Message
                    }
                ]);
            }

            if (exception is BadHttpRequestException badRequest)
            {
                logger.LogWarning(badRequest, "Bad request");
                return ((int)HttpStatusCode.BadRequest, "Invalid request",
                [
                    new ApiError { Code = "BadRequest", Description = badRequest.Message }
                ]);
            }

            logger.LogError(exception, "Unhandled exception");
            return ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.",
            [
                new ApiError { Code = "InternalServerError", Description = "An unexpected error occurred." }
            ]);
        }
    }
}