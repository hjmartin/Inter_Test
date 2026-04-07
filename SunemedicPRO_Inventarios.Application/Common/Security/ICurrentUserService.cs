#nullable enable

namespace SunemedicPRO_Inventarios.Server.Application.Common.Security
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Email { get; }
        string? Role { get; }
    }
}
