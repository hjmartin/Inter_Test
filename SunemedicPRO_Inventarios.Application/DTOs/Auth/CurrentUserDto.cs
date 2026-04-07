#nullable enable

namespace SunemedicPRO_Inventarios.Server.Application.DTOs.Auth
{
    public class CurrentUserDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Rol { get; set; }
    }
}
