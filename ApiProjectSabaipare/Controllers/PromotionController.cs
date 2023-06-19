using ApiCrudProjectS.Models;
using ApiCrudProjectS.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCrudProjectS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        [Route("GetPromotion")]
        public async Task<IEnumerable<Promotion>> GetPromotions()
        {
            return await _promotionService.GetPromotion();
        }

        [HttpPost]
        [Route("CreateAndUpdatePromotion")]
        public async Task<Promotion> CreateAndUpdatePromotion(Promotion promotion)
        {
            return await _promotionService.CreateAndUpdatePromotion(promotion);
        }

        [HttpDelete]
        [Route("DeletePromotion/{id}")]
        public async Task<bool> DeletePromotion(int id)
        {
            return await _promotionService.DeletePromotion(id);
        }

    }
}
