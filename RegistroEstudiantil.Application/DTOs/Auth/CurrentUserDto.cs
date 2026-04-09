#nullable enable

namespace RegistroEstudiantil.Application.DTOs.Auth
{
    public class CurrentUserDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Rol { get; set; }
    }
}


