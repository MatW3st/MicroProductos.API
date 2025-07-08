namespace MicroService.Product.API.Models.Dto
{
    public class ResponseDto
    {
        public object Result { get; set; } //Corrigio todos los errores
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
