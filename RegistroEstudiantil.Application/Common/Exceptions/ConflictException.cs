namespace RegistroEstudiantil.Application.Common.Exceptions
{
    public class ConflictException : AppException
    {
        public ConflictException(string message, object? payload = null)
            : base(message, payload)
        {
        }
    }
}
