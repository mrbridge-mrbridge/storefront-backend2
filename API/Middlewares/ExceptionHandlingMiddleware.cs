using System.Net;
using System.Text.Json;
using Application.Core;

namespace Api.Middlewares
{
	public class ExceptionHandlingMiddleware
    {
        private RequestDelegate _next { get; }
        private ILogger<ExceptionHandlingMiddleware> _logger { get; }
        private IHostEnvironment _env { get; }
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() ? new ExceptionResult(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ExceptionResult(context.Response.StatusCode, "Internal Server error");

                var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }

}