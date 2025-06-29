using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.Factories
{
    public static class APIResponseFactory
    {
        public static IActionResult GenerateAPIValidationResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Any())
                .Select(e => new ValidationError
                {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(error => error.ErrorMessage)
                });

            var response = new ValidationErrorResponse
            {
                ValidationError = errors
            };

            return new BadRequestObjectResult(response);
        }
    }
}
