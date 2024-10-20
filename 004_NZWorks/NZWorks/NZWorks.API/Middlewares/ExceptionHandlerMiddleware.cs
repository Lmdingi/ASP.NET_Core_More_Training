using System.Net;

namespace NZWorks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _loger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> loger, RequestDelegate next)
        {
            _loger = loger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                _loger.LogError(ex, $"{errorId} : {ex.Message}");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! We are looking into resolving this."
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
