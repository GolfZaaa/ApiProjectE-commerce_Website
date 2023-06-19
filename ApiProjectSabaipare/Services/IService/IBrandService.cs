using ApiCrudProjectS.Models;

namespace ApiCrudProjectS.Services.IService
{
    public interface IBrandService
    {
        Task<List<Brand>> GetBrand();
        Task<Brand> CreateAndUpdateBrand(Brand brand);
        Task<bool> DeleteBrand(int id);
    }
}
