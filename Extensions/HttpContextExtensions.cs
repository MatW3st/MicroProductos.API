using Microsoft.EntityFrameworkCore;

namespace MicroService.Product.API.Extensions
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParamPageHeader <T>(this HttpContext httpContext, IQueryable<T> queryable) //IQueryable, es un metodo de extencion
        {
            if (httpContext is null)
            {
                throw new ArgumentException(nameof(httpContext));
            }
            // contamos el total de la consulta que recganzo el iQueryable
            // una vez calculado el total de registros, consultamos se asigna a la variable total
            double total = await queryable.CountAsync();
            httpContext.Response.Headers.Append("Cantidad-total-registros" , total.ToString());
        }
    }
}

// ¿Creaste una aplicacion Multitenant?
// 
