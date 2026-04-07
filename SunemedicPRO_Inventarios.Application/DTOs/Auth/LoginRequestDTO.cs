using System.ComponentModel.DataAnnotations;

namespace SunemedicPRO_Inventarios.Server.DTOs.Auth
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Pass { get; set; }
    }
}
