using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantil.Application.DTOs.Auth
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Pass { get; set; }
    }
}


