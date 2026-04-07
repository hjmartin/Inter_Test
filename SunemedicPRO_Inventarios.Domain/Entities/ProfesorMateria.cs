using System.ComponentModel.DataAnnotations;

namespace SunemedicPRO_Inventarios.Server.Entities
{
    public class ProfesorMateria
    {
        [Required]
        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }

        [Required]
        public int MateriaId { get; set; }
        public Materia Materia { get; set; }
    }
}
