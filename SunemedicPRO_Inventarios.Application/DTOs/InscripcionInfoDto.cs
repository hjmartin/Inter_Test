namespace SunemedicPRO_Inventarios.Server.DTOs
{
    public class InscripcionInfoDto
    {
        public int Id { get; set; }
        public string NombreGrupo { get; set; } = string.Empty;
        public string Materia { get; set; } = string.Empty;
        public string Profesor { get; set; } = string.Empty;
        public int Creditos { get; set; }
    }
}
