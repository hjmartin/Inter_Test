namespace RegistroEstudiantil.Application.Common.Exceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string message, object? payload = null)
            : base(message, payload)
        {
        }
    }
}
