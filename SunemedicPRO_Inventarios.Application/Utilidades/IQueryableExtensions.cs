using SunemedicPRO_Inventarios.Server.DTOs.Shared;

namespace SunemedicPRO_Inventarios.Server.Utilidades
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
