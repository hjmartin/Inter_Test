namespace SunemedicPRO_Inventarios.Server.DTOs.Auth
{
    public class RestablecerPasswordDTO
    {
        public string Correo { get; set; } = null!;
        public string NuevaContrasena { get; set; } = null!;
    }
}
