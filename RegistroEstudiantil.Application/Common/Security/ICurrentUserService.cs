#nullable enable

namespace RegistroEstudiantil.Application.Common.Security
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Email { get; }
        string? Role { get; }
    }
}


