using ApiCrudProjectS.Models;

namespace ApiCrudProjectS.Services.IService
{
    public interface IPromotionService
    {
        Task<List<Promotion>> GetPromotion();
        Task<Promotion> CreateAndUpdatePromotion(Promotion promotion);
        Task<bool> DeletePromotion(int id);
    }
}
