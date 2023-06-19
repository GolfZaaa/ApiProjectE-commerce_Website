using ApiCrudProjectS.Models;
using ApiCrudProjectS.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudProjectS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [Route("GetBrand")]
        public async Task<IEnumerable<Brand>> GetBrands()
        {
            return await _brandService.GetBrand();
        }

        [HttpPost]
        [Route("CreateAndUpdateBrand")]
        public async Task<Brand> CreateAndUpdateBrand(Brand brand)
        {
            return await _brandService.CreateAndUpdateBrand(brand);
        }

        [HttpDelete]
        [Route("DeleteBrand/{id}")]
        public async Task<bool> DeleteBrand(int id)
        {
            return await _brandService.DeleteBrand(id);
        }


    }
}
