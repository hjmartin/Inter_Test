namespace SunemedicPRO_Inventarios.Server.DTOs.Auth
{
    public class CambioPasswordDTO
    {
        public string ContrasenaActual { get; set; } = null!;
        public string NuevaContrasena { get; set; } = null!;
    }
}
