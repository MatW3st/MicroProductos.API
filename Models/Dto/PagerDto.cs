namespace MicroService.Product.API.Models.Dto
{
    public record PagerDto (int Page =1, int RecordPerPage = 10) //clase que no cambia, lo que se pone es y sera siempre fijo
    {
        private const int MaxRecordPerPage = 50;

        public int Page { get; set; } = Math.Max(1, Page);
        public int RecordPerPage { get; set; } = Math.Clamp(RecordPerPage, 1, MaxRecordPerPage);
    }
}
