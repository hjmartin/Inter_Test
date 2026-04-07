namespace SunemedicPRO_Inventarios.Server.Entities
{
    public class Materia
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public int Creditos { get; set; } = 3; // Regla: cada materia = 3 créditos

        public ICollection<ProfesorMateria> ProfesorMaterias { get; set; } = new List<ProfesorMateria>();
        public ICollection<GrupoClase> Grupos { get; set; } = new List<GrupoClase>();
    }
}
