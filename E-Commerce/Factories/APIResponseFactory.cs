using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.Factories
{
    public static class APIResponseFactory
    {
        public static IActionResult GenerateAPIValidationResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => new ApiError
                {
                    Code = "ValidationError",
                    Description = e.Value!.Errors.First().ErrorMessage,
                    Field = e.Key
                });

            var response = ApiResponse<object?>.Failure("Validation failed", errors);
            return new BadRequestObjectResult(response);
        }
    }
}