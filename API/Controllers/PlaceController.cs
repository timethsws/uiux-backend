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
                Liked = dbContext.Favourits.Any(f => f.PlaceId == placeId && f.UserId == userId)
            };

            var images = await dbContext.Reviews.Include(r => r.Image).Where(r => r.PlaceId == placeId).Select(e => e.Image.Url).ToListAsync();

            PlaceDto.Images = images;

            return Json(PlaceDto);
        }

        [HttpGet("{placeId}/reviews/{userId}")]
        public async Task<IActionResult> getReviews([FromRoute] Guid PlaceId, [FromRoute] Guid userId)
        {
            if (!dbContext.Places.Any(p => p.Id == PlaceId))
            {
                return StatusCode(StatusCodes.Status404NotFound, "Invalid Place Id");
            }

            var reviews = await dbContext.Reviews
                .Include(r => r.Image)
                .Include(r => r.Reviewer).ThenInclude(p => p.ProfilePicture)
                .Where(p => p.PlaceId == PlaceId)
                .ToListAsync();

            List<ReviewDTO> reviewsList = new List<ReviewDTO>();

            foreach (var r in reviews)
            {
                var reviewDtoItem = new ReviewDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Content = r.Body,
                    Image = r.Image.Url,
                    Rating = r.Rating,
                    Owner = new UserThumbDTO
                    {
                        Name = r.Reviewer.FirstName + " " + r.Reviewer.LastName,
                        ProfileImage = r.Reviewer.ProfilePicture.Url
                    },
                    CommentCount = r.Comments.Count(),
                    LikesCount = r.Likes.Count(),
                    Liked = r.Likes.Any(l => l.UserId == userId)
                    // TODO Others
                };

                reviewsList.Add(reviewDtoItem);
            }

            return Json(reviewsList);
        }

        [HttpPost("{placeId}/review")]
        public async Task<IActionResult> AddReview([FromBody]AddReviewModel addReview,[FromForm] List<IFormFile> files)
        {
            if(addReview == null ||
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
                if (!dbContext.Users.Any(u => u.Id == addReview.UserId)) return BadRequest("Invalid User");

                var review = new Review
                {
                    Title = addReview.Title,
                    Body = addReview.Content,
                    PlaceId = addReview.PlaceId,
                    ReviewerId = addReview.UserId,
                    ReviewedOn = DateTime.UtcNow
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
    }
}
