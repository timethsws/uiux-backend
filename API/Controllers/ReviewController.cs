using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Core.Database;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    /// <summary>
    /// Review Controller
    /// </summary>
    [Route("api/review")]
    public class ReviewController : Controller
    {

        /// <summary>
        /// The AppDbContext
        /// </summary>
        private readonly AppDbContext dbContext;

        public ReviewController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get the review details for a specific user
        /// </summary>
        /// <param name="reviewId">Identification of the review</param>
        /// <param name="userId">Identification of the user</param>
        /// <returns>Review Details</returns>
        [HttpGet("{reviewId}/details/{userId}")]
        public async Task<IActionResult> GetReview([FromRoute] Guid reviewId, [FromRoute] Guid userId)
        {
            // Validate Request
            if (reviewId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("Invalid Request");
            }

            try
            {
                // validate Request Data
                if (!dbContext.Reviews.Any(r => r.Id == reviewId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Invalid Data");
                }

                // get the review
                var review = dbContext.Reviews
                    .Include(r => r.Image)
                    .Include(r => r.Comments)
                    .Include(r => r.Likes)
                    .Include(r => r.Reviewer).ThenInclude(u => u.ProfilePicture)
                    .FirstOrDefault(r => r.Id == reviewId);

                // transoform the review to reviewDTO
                var res = new ReviewDTO
                {
                    Id = review.Id,
                    Title = review.Title,
                    Content = review.Content,
                    Image = review.Image?.Url ?? "/images/placeholder-image.png",
                    Owner = new UserThumbDTO
                    {
                        Name = review.Reviewer.UserName,
                        ProfileImage = review.Reviewer.ProfilePicture.Url
                    },
                    ReviewedOn = review.ReviewedOn,
                    Rating = review.Rating,
                    CommentCount = review.Comments.Count(),
                    LikesCount = review.Likes.Count()
                };

                // Find if the user has liked the review
                res.Liked = dbContext.ReviewLikes.Any(r => r.ReviewId == reviewId && r.UserId == userId);

                // Return the reviewDTO
                return Json(res);

            }
            catch (Exception ex)
            {
                // If an Exception is catch return status code 500 and the exception message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{reviewId}/like/{userId}")]
        public async Task<IActionResult> LikeReview([FromRoute] Guid reviewId, [FromRoute] Guid userId)
        {
            if (reviewId == Guid.Empty && userId == Guid.Empty)
            {
                return BadRequest("Invalid data");
            }

            try
            {
                if (!dbContext.Reviews.Any(p => p.Id == reviewId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Incorrect Data");
                }
                var res = new LikeDTO();

                var reviewLike = await dbContext.ReviewLikes.FirstOrDefaultAsync(f => f.ReviewId == reviewId && f.UserId == userId);
                if (reviewLike == null)
                {
                    dbContext.ReviewLikes.Add(new ReviewLike { ReviewId = reviewId, UserId = userId });
                    res.Liked = true;
                }
                else
                {
                    dbContext.ReviewLikes.Remove(reviewLike);
                    res.Liked = false;
                }

                dbContext.SaveChanges();
                res.Count = dbContext.ReviewLikes.Count(r => r.ReviewId == reviewId);

                return Json(res);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{reviewId}/comments/{userId}")]
        public async Task<IActionResult> GetComments([FromRoute] Guid reviewId, [FromRoute]Guid userId)
        {
            if (reviewId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                if (!dbContext.Reviews.Any(r => r.Id == reviewId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Invalid data");
                }

                var comments = await dbContext.Comments
                    .Include(c => c.Owner).ThenInclude(u => u.ProfilePicture)
                    .Include(c => c.Likes)
                    .Where(c => c.ReviewId == reviewId)
                    .OrderByDescending(c => c.CreatedOn)
                    .ToListAsync();

                var res = new List<CommentDTO>();

                foreach (var comment in comments)
                {
                    var item = new CommentDTO
                    {
                        Id = comment.Id,
                        Body = comment.CommentBody,
                        LikesCount = comment.Likes.Count(),
                        Owner = new UserThumbDTO
                        {
                            Name = comment.Owner.UserName,
                            ProfileImage = comment.Owner.ProfilePicture.Url
                        },
                        Liked = comment.Likes.Any(c => c.UserId == userId),
                        CreatedOn = comment.CreatedOn
                    };

                    res.Add(item);
                }

                return Json(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("comment")]
        public async Task<IActionResult> AddComment([FromForm] CommentModel commentData)
        {
            if (commentData == null || commentData.ReviewId == Guid.Empty || commentData.UserId == Guid.Empty)
            {
                return BadRequest("Invalid Request");
            }

            try
            {
                if (!dbContext.Reviews.Any(r => r.Id == commentData.ReviewId) || !dbContext.Users.Any(u => u.Id == commentData.UserId))
                {
                    return BadRequest("Invalid Data");
                }

                var comment = new Comment
                {
                    CommentBody = commentData.Body,
                    OwnerId = commentData.UserId,
                    ReviewId = commentData.ReviewId,
                    CreatedOn = DateTime.UtcNow
                };

                dbContext.Comments.Add(comment);
                dbContext.SaveChanges();

                return Json(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{commentId}/like-comment/{userId}")]
        public async Task<IActionResult> LikeComment([FromRoute]Guid commentId, [FromRoute] Guid userId)
        {
            if (commentId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                if(!dbContext.Comments.Any(c => c.Id == commentId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Invalid data");
                }

                var commentLike = await dbContext.CommentLikes.FirstOrDefaultAsync(c => c.CommentId == commentId && c.UserId == userId);
                var res = new LikeDTO();

                if(commentLike == null)
                {
                    dbContext.CommentLikes.Add(new CommentLike
                    {
                        CommentId = commentId,
                        UserId = userId
                    });
                    res.Liked = true;
                }
                else
                {
                    dbContext.CommentLikes.Remove(commentLike);
                    res.Liked = false;
                }

                dbContext.SaveChanges();
                res.Count = dbContext.CommentLikes.Count(c => c.CommentId == commentId);

                return Json(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
