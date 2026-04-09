using RegistroEstudiantil.Application.DTOs.Shared;

namespace RegistroEstudiantil.Application.Utilidades
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> Queryable, PaginacionDTO paginacion)
        {
            return Queryable.
                Skip((paginacion.Pagina - 1) * paginacion.RecordsPorPagina)
                .Take(paginacion.RecordsPorPagina);
        }
    }
}


