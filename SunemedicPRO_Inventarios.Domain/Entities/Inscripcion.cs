namespace SunemedicPRO_Inventarios.Server.Entities
{
    public class Inscripcion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; } = null!;

        public int GrupoClaseId { get; set; }
        public GrupoClase GrupoClase { get; set; } = null!;

        // Campo redundante solo para consultas rápidas (opcional)
        public string Periodo { get; set; } = null!;
    }
}
