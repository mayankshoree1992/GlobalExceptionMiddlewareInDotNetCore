using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GlobalException.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context ,Exception ex)
        {
            var serverError = HttpStatusCode.InternalServerError;
            var output = JsonSerializer.Serialize(new { error = "The Error occured" });
            context.Response.ContentType = "application/json"; ;
            context.Response.StatusCode = (int)serverError;
            return context.Response.WriteAsync(output);
        }
    }
}
