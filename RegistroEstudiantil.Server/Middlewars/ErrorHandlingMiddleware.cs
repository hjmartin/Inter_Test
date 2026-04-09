
using RegistroEstudiantil.Application.Common.Exceptions;

namespace RegistroEstudiantil.Server.Middlewars
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = ex.StatusCode;

                if (ex.Payload is not null)
                {
                    await context.Response.WriteAsJsonAsync(ex.Payload);
                    return;
                }

                await context.Response.WriteAsJsonAsync(new
                {
                    mensaje = ex.Message,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OcurriÃ³ un error inesperado");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    mensaje = "OcurriÃ³ un error inesperado.",
                    message = "OcurriÃ³ un error inesperado."
                });
            }
        }
    }
}

