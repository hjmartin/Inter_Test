using Microsoft.EntityFrameworkCore;

namespace SunemedicPRO_Inventarios.Server.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext context, IQueryable<T> queryable)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            double cantidad = await queryable.CountAsync(); // necesitas Microsoft.EntityFrameworkCore
            context.Response.Headers.Append("cantidad-total-registros", cantidad.ToString());
        }
    }
}
