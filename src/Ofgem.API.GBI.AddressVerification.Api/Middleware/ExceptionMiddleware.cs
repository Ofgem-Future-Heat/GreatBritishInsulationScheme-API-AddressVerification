using Ofgem.API.GBI.AddressVerification.Application.Exceptions;
using System.Text.Json;

namespace Ofgem.API.GBI.AddressVerification.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex is AddressNotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else if (ex is AddressValidationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            var response = JsonSerializer.Serialize(new { error = ex.Message });
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(response);
        }
    }
}
