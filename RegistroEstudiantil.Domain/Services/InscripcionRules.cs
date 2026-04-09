using RegistroEstudiantil.Domain.Exceptions;

namespace RegistroEstudiantil.Domain.Services
{
    public static class InscripcionRules
    {
        public static void ValidarNuevaInscripcion(
            int materiasEnPeriodo,
            bool tieneMismoProfesorEnPeriodo,
            bool tieneMismaMateriaEnPeriodo)
        {
            if (materiasEnPeriodo >= 3)
            {
                throw new DomainRuleException("El estudiante ya tiene 3 materias.");
            }

            if (tieneMismoProfesorEnPeriodo)
            {
                throw new DomainRuleException("Ya tiene clases con el mismo profesor.");
            }

            if (tieneMismaMateriaEnPeriodo)
            {
                throw new DomainRuleException("Ya esta inscrito en esa materia en este periodo.");
            }
        }
    }
}
