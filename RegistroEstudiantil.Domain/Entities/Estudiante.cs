namespace RegistroEstudiantil.Domain.Entities
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Documento { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Relación 1-1 con Usuario para el “registro en línea”
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

        public static Estudiante Crear(string documento, string nombres, string apellidos, Usuario usuario)
        {
            return new Estudiante
            {
                Documento = documento,
                Nombres = nombres,
                Apellidos = apellidos,
                Usuario = usuario
            };
        }

        public bool PerteneceAUsuario(int usuarioId) => UsuarioId == usuarioId;
    }
}


