using Domain.Exeptions;
using Shared.ErrorModels;
using System.Net;
using System.Text.Json;

namespace E_Commerce.MiddleWarers
{
    public class CustomExHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExHandlerMiddleware> _logger;
        public CustomExHandlerMiddleware(RequestDelegate next, ILogger<CustomExHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpcontext)
        {

            try
            {
                await _next.Invoke(httpcontext);
                // Logic 
                await HandleNotFoundEndPointAync(httpcontext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                await HandleExceptionAsync(httpcontext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpcontext, Exception ex)
        {
            // set status code
            //httpcontext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;//500
            // set custom type  
            httpcontext.Response.ContentType = "application/json";
            // reponse obj
            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message
            };
            response.StatusCode = ex switch
            {
                NotFoundEx => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            httpcontext.Response.StatusCode = response.StatusCode;
            // return response as json 
            await httpcontext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleNotFoundEndPointAync(HttpContext httpcontext)
        {
            if (httpcontext.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                httpcontext.Response.ContentType = "application/json";
                var response = new ErrorDetails
                {
                    ErrorMessage = $"End Point{httpcontext.Request.Path} Not Found",
                    StatusCode = (int)HttpStatusCode.NotFound
                };
                await httpcontext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
