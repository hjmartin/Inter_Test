#nullable enable

namespace SunemedicPRO_Inventarios.Server.Application.Common.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public object? Payload { get; }

        public ApiException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiException(int statusCode, string message, object? payload)
            : base(message)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }
}
