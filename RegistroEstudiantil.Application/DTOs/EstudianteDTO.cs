namespace RegistroEstudiantil.Application.DTOs
{
    public class EstudianteDTO
    {
        public int Id { get; set; }
        public string Documento { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relación 1-1 con Usuario para el “registro en línea”
        public int UsuarioId { get; set; }
    }
}


