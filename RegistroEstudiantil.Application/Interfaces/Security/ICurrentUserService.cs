#nullable enable

namespace RegistroEstudiantil.Application.Interfaces.Security
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Email { get; }
        string? Role { get; }
    }
}
