using ApiCrudProjectS.Models;
using ApiCrudProjectS.Services.IService;
using ApiProjectSabaipare.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudProjectS.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly DataContext _dataContext;

        public PromotionService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<Promotion>> GetPromotion()
        {
            return await _dataContext.Promotion.ToListAsync();
        }

        public async Task<Promotion> CreateAndUpdatePromotion(Promotion promotion)
        {
            if (promotion.Id == 0) _dataContext.Entry(promotion).State = EntityState.Added;
            else _dataContext.Entry(promotion).State = EntityState.Modified;

            await _dataContext.SaveChangesAsync();
            return promotion;
        }
        public async Task<bool> DeletePromotion(int id)
        {
            bool check = false;
            var promotion = await _dataContext.Promotion.FindAsync(id);

            if (promotion != null)
            {
                check = true;
                _dataContext.Entry(promotion).State = EntityState.Deleted;
                await _dataContext.SaveChangesAsync();
            }
            else check = false;

            return check;
        }
    }
}
