using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Core.Database;
using Core.Entities;
using Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/place/")]
    public class PlaceController : Controller
    {
        private readonly AppDbContext dbContext;

        public PlaceController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{placeId}/details/{userId}")]
        public async Task<IActionResult> getPlace([FromRoute] Guid placeId, [FromRoute] Guid userId)
        {
            var place = await dbContext.Places
                .Include(p => p.Image)
                .FirstOrDefaultAsync(p => p.Id == placeId);
            if (place == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Invalid Place Id");
            }

            var PlaceDto = new PlaceDTO
            {
                Id = place.Id,
                MainImage = place.Image.Url,
                Name = place.Name,
                Description = place.LongDescription,
                Liked = dbContext.Favourits?.Any(f => f.PlaceId == placeId && f.UserId == userId) ?? false
            };

            var images = await dbContext.Reviews.Include(r => r.Image).Where(r => r.PlaceId == placeId).Select(e => e.Image.Url).ToListAsync();

            PlaceDto.Images = images;

            return Json(PlaceDto);
        }

        [HttpGet("{placeId}/reviews/{userId}")]
        public async Task<IActionResult> getReviews([FromRoute] Guid placeId, [FromRoute] Guid userId)
        {
            if (!dbContext.Places.Any(p => p.Id == placeId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Invalid Place Id");
            }

            var reviews = await dbContext.Reviews
                .Include(r => r.Image)
                .Include(r => r.Likes)
                .Include(r => r.Comments)
                .Include(r => r.Reviewer).ThenInclude(p => p.ProfilePicture)
                .Where(p => p.PlaceId == placeId)
                .OrderByDescending(r => r.ReviewedOn)
                .ToListAsync();

            List<ReviewDTO> reviewsList = new List<ReviewDTO>();

            foreach (var r in reviews)
            {
                var reviewDtoItem = new ReviewDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Content = r.Content,
                    Image = r.Image?.Url ?? "/images/placeholder-image.png",
                    Rating = r.Rating,
                    Owner = new UserThumbDTO
                    {
                        Name = r.Reviewer.UserName,
                        ProfileImage = r.Reviewer.ProfilePicture.Url
                    },
                    CommentCount = r.Comments?.Count() ?? 0,
                    LikesCount = r.Likes?.Count() ?? 0,
                    Liked = r.Likes?.Any(l => l.UserId == userId) ?? false,
                    ReviewedOn = r.ReviewedOn,
                };

                reviewsList.Add(reviewDtoItem);
            }

            return Json(reviewsList);
        }

        [HttpPost("review")]
        public async Task<IActionResult> AddReview([FromForm]AddReviewModel addReview)
        {
            if (addReview == null ||
                addReview.UserId == Guid.Empty ||
                addReview.PlaceId == Guid.Empty)
            {
                return BadRequest("Invalid Request Data");
            }

            if (string.IsNullOrWhiteSpace(addReview.Title) || string.IsNullOrWhiteSpace(addReview.Content))
            {
                return BadRequest("Title and Content should be there");
            }

            try
            {
                if (!dbContext.Places.Any(p => p.Id == addReview.PlaceId)) return BadRequest("Invalid Place");
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == addReview.UserId);
                if (user == null) return BadRequest("Invalid User");

                user.Score += 100;

                var review = new Review
                {
                    Title = addReview.Title,
                    Content = addReview.Content,
                    PlaceId = addReview.PlaceId,
                    ReviewerId = addReview.UserId,
                    ReviewedOn = DateTime.UtcNow,
                    Rating = addReview.Rating
                };

                dbContext.Reviews.Add(review);
                dbContext.SaveChanges();

                return Json(review);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{placeId}/favourite/{userId}")]
        public async Task<IActionResult> updateFavourite([FromRoute] Guid placeId, [FromRoute] Guid userId)
        {
            if(placeId == Guid.Empty && userId == Guid.Empty)
            {
                return BadRequest("Invalid data");
            }

            try
            {
                if(!dbContext.Places.Any(p => p.Id == placeId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Incorrect Data");
                }

                var favourite = await dbContext.Favourits.FirstOrDefaultAsync(f => f.PlaceId == placeId && f.UserId == userId);
                if(favourite == null)
                {
                    dbContext.Favourits.Add(new FavouritePlace { PlaceId = placeId, UserId = userId });
                    dbContext.SaveChanges();
                    return Json(true);
                }
                else
                {
                    dbContext.Favourits.Remove(favourite);
                    dbContext.SaveChanges();
                    return Json(false);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

    }
}
