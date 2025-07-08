using AutoMapper;
using MicroService.Product.API.Models.Dto;
//using MicroService.Product.API.Models.Dto;

namespace MicroService.Product.API
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, MicroService.Product.API.Models.Product>()
                .ReverseMap();
            });
            return mappingConfig;
        }
    }
}