﻿using ApiCrudProjectS.Models;
using ApiCrudProjectS.Services.IService;
using ApiProjectSabaipare.Data;
using ApiProjectSabaipare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudProjectS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext;

        public ReviewController(IReviewService reviewService, UserManager<ApplicationUser> userManager, DataContext dataContext)
        {
            _reviewService = reviewService;
            _userManager = userManager;
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("GetReview")]
        public async Task<IEnumerable<Review>> GetReviews()
        {
            return await _reviewService.GetReview();
        }



        [HttpPost]
        [Route("CreateAndUpdateReview")]
        public async Task<Object> CreateAndUpdateReview(Review review)
        {
            if (review.Id == 0) _dataContext.Set<Review>().Add(review);

            else _dataContext.Set<Review>().Update(review);

            await _dataContext.SaveChangesAsync();
            return review;
        }






        [HttpDelete]
        [Route("DeleteReview/{id}")]
        public async Task<bool> DeleteReview(int id)
        {
            return await _reviewService.DeleteReview(id);
        }

    }
}
