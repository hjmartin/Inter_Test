namespace RegistroEstudiantil.Domain.Entities
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;

        public ICollection<GrupoClase> Grupos { get; set; } = new List<GrupoClase>();
    }
}


