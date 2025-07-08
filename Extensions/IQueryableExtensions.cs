using MicroService.Product.API.Models.Dto;

namespace MicroService.Product.API.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar <T> (this IQueryable<T> queryable, PagerDto PagerDto)
        {
            return queryable
                .Skip((PagerDto.Page - 1) * PagerDto.RecordPerPage) //establece el numero de pagina que se ca a saltar
                .Take(PagerDto.RecordPerPage); //toma la cantidad de registros devueltos en la paginacion
        }
    }
}
