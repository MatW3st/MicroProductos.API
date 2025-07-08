using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MicroService.Product.API.Models.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
        public string ImageLocalPath { get; set; }
        public IFormFile Image {  get; set; }
    }
}
