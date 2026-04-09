namespace RegistroEstudiantil.Application.Common.Exceptions
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message, object? payload = null)
            : base(message, payload)
        {
        }
    }
}
