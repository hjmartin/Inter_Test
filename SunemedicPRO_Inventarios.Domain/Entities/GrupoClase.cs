namespace SunemedicPRO_Inventarios.Server.Entities
{
    public class GrupoClase
    {
        public int Id { get; set; }
        public string Periodo { get; set; } = null!; // p.ej. "2025-2"
        public string? NombreGrupo { get; set; } // p.ej. "A", "B"
        public int Cupo { get; set; } = 40;

        public int MateriaId { get; set; }
        public Materia Materia { get; set; } = null!;

        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; } = null!;

        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}
