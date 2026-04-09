namespace RegistroEstudiantil.Application.Common.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message, object? payload = null)
            : base(message, payload)
        {
        }
    }
}
