namespace SunemedicPRO_Inventarios.Server.DTOs
{
    public class RegistroRequestDTO
    {
        public long Documento { get; set; }
        public string Correo { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public int Dependencia { get; set; }
    }
}
