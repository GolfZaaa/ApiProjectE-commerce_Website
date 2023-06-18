using ApiProjectSabaipare.DTOs.ProductDto;
using ApiProjectSabaipare.Services;
using ApiProjectSabaipare.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectSabaipare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _productService.GetProductListAsync();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddProduct([FromForm] ProductRequest request)
        {
            await _productService.CreateAsync(request);
            return Ok();
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetTypes()
        {
            var result = await _productService.GetTypeAsync();
            return Ok(result);
        }

    }
}
