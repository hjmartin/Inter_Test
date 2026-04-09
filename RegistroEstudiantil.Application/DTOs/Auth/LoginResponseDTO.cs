namespace RegistroEstudiantil.Application.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public int Usuario_Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public DateTime Expiracion { get; set; }
    }
}


