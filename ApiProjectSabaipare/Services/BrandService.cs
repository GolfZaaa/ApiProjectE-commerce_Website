using ApiCrudProjectS.Models;
using ApiCrudProjectS.Services.IService;
using ApiProjectSabaipare.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudProjectS.Services
{
    public class BrandService : IBrandService
    {
        private readonly DataContext _dataContext;

        public BrandService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<Brand>> GetBrand()
        {
            return await _dataContext.Brand.ToListAsync();
        }

        public async Task<Brand> CreateAndUpdateBrand(Brand brand)
        {
            if (brand.Id == 0) _dataContext.Entry(brand).State = EntityState.Added;
            else _dataContext.Entry(brand).State = EntityState.Modified;

            await _dataContext.SaveChangesAsync();
            return brand;
        }
        public async Task<bool> DeleteBrand(int id)
        {
            bool check = false;
            var brand = await _dataContext.Brand.FindAsync(id);

            if (brand != null)
            {
                check = true;
                _dataContext.Entry(brand).State = EntityState.Deleted;
                await _dataContext.SaveChangesAsync();
            }
            else check = false;

            return check;
        }
    }
}
