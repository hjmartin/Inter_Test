using Microsoft.AspNetCore.Http;
using RegistroEstudiantil.Application.Common.Exceptions;
using RegistroEstudiantil.Domain.Exceptions;

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
            catch (AppException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = MapearCodigoEstado(ex);

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
            catch (DomainRuleException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    mensaje = ex.Message,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error inesperado");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    mensaje = "Ocurrio un error inesperado.",
                    message = "Ocurrio un error inesperado."
                });
            }
        }

        private static int MapearCodigoEstado(AppException ex) => ex switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
