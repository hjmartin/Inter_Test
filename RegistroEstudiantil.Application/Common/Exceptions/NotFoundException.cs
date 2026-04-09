namespace RegistroEstudiantil.Application.Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message, object? payload = null)
            : base(message, payload)
        {
        }
    }
}
