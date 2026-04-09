namespace RegistroEstudiantil.Application.Common.Exceptions
{
    public abstract class AppException : Exception
    {
        public object? Payload { get; }

        protected AppException(string message, object? payload = null)
            : base(message)
        {
            Payload = payload;
        }
    }
}
