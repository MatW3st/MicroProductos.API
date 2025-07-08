using AutoMapper;
using MicroService.Product.API.Extensions;
using MicroService.Product.API.Models.Dto;
using MicroService.Product.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroService.Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public ProductController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<MicroService.Product.API.Models.Product>
                    objList = _db.Productos.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,")]
        public ResponseDto Post(ProductDto productDto)
        {
            try
            {
                Product.API.Models.Product product =
                    _mapper.Map<Product.API.Models.Product>(productDto);
                _db.Productos.Add(product);
                _db.SaveChanges();
                if (productDto.Image != null)
                {
                    string fileName =
                        product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    var filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory =
                        Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageURL = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageURL = "https://placehold.co/600x400";
                }

                _db.Productos.Update(product);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(product);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                MicroService.Product.API.Models.Product obj = _db.Productos.First(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("All")]
        [AllowAnonymous]
        public async Task<ResponseDto> All([FromQuery] PagerDto paginacionDto)
        {
            try
            {
                var queryable = _db.Productos.AsQueryable();
                await HttpContext.InsertParamPageHeader(queryable);
                var productosQueryable = await queryable.OrderBy(o => o.Name)
                    .Paginar(paginacionDto).ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(productosQueryable);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMINISTRADOR")]
        public ResponseDto Put(ProductDto ProductDto)
        {
            try
            {
                MicroService.Product.API.Models.Product producto = _mapper.Map<Product.API.Models.Product>(ProductDto);
                if (ProductDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(producto.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), producto.ImageLocalPath);
                        FileInfo archivo = new FileInfo(oldFilePathDirectory);
                        if (archivo.Exists)
                        {
                            archivo.Delete();
                        }
                    }
                    string fileName = producto.ProductId + Path.GetExtension(ProductDto.Image.FileName);
                    string archivoPath = @"wwwroot\Images\" + fileName;
                    var archivoPathDirectory = Path.Combine(Directory.GetCurrentDirectory(), archivoPath);
                    using (var fileStream = new FileStream(archivoPathDirectory, FileMode.Create))
                    {
                        ProductDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    producto.ImageURL = baseUrl + "/Images" + fileName;
                    producto.ImageLocalPath = archivoPath;
                }
                _db.Productos.Update(producto);
                _db.SaveChanges();
            }
            catch (Exception es)
            {
                _response.IsSuccess = false;
                _response.Message = es.Message;
            }
            return _response;
        }

    }
}