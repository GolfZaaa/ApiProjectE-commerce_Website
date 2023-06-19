using ApiCrudProjectS.Models;

namespace ApiCrudProjectS.Services.IService
{
    public interface IReviewService
    {
        Task<List<Review>> GetReview();
        Task<object> CreateAndUpdateReview(Review review);
        Task<bool> DeleteReview(int id);
    }
}
