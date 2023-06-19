using ApiCrudProjectS.Models;
using ApiCrudProjectS.Services.IService;
using ApiProjectSabaipare.Data;
using ApiProjectSabaipare.DTOs;
using ApiProjectSabaipare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudProjectS.Services
{
    public class ReviewService : ControllerBase, IReviewService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewService(DataContext dataContext, UserManager<ApplicationUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<List<Review>> GetReview()
        {
            return await _dataContext.Review.ToListAsync();
        }

        public async Task<Object> CreateAndUpdateReview(Review review)
        {
            if (_userManager != null && User?.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "User not found" });
            }

            if (review.Id == 0) _dataContext.Entry(review).State = EntityState.Added;
            else _dataContext.Entry(review).State = EntityState.Modified;

            await _dataContext.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReview(int id)
        {
            bool check = false;
            var review = await _dataContext.Review.FindAsync(id);

            if (review != null)
            {
                check = true;
                _dataContext.Entry(review).State = EntityState.Deleted;
                await _dataContext.SaveChangesAsync();
            }
            else check = false;

            return check;
        }
    }
}
