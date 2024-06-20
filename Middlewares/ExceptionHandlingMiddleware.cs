using OVS360SolutionsAPI.Constants;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace OVS360SolutionsAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            string errorResponse;
            switch (exception)
            {
                case BadHttpRequestException ex:                    
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ex.Message;
                    break;
                
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = WebConstants.internalServerErrorMessage;
                    break;
            }
            _logger.LogError(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
