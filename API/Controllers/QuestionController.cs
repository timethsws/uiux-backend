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
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly AppDbContext dbContext;
        public QuestionController (AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{placeId}/questions/{userId}")]
        public async Task<IActionResult> GetQuestions([FromRoute]Guid placeId, [FromRoute] Guid userId)
        {
            if(placeId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("Invalid Request");
            }

            try
            {
                if(!dbContext.Places.Any(p => p.Id == placeId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Invalid data");
                }

                var questions = dbContext.Questions
                    .Include(q => q.Owner).ThenInclude(u => u.ProfilePicture)
                    .Include(q => q.Likes)
                    .Include(q => q.Answers)
                    .Where(q => q.PlaceId == placeId && q.OwnerId == userId)
                    .OrderByDescending(q => q.CreatedOn)
                    .ToList();

                var res = new List<QuestionDTO>();

                foreach(var question in questions)
                {
                    var item = new QuestionDTO
                    {
                        Id = question.Id,
                        Body = question.QuestionBody,
                        Owner = new UserThumbDTO
                        {
                            Name = question.Owner.UserName,
                            ProfileImage = question.Owner.ProfilePicture.Url
                        },
                        CreatedOn = question.CreatedOn,
                        LikesCount = question.Likes.Count(),
                        AnswerCount = question.Answers.Count(),
                        Answered = question.Answers.Any(),
                        Liked = question.Likes.Any(l => l.UserId == userId),
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

        [HttpPost("question")]
        public async Task<IActionResult> AddQuestion([FromForm] QuestionModel questionModel)
        {
            if(questionModel == null || questionModel.placeId == Guid.Empty || questionModel.userId == Guid.Empty)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                if (!dbContext.Places.Any(p => p.Id == questionModel.placeId) || !dbContext.Users.Any(u => u.Id == questionModel.userId))
                {
                    return BadRequest("Invalid data");
                }

                var question = new Question
                {
                    PlaceId = questionModel.placeId,
                    OwnerId = questionModel.userId,
                    QuestionBody = questionModel.Question,
                    CreatedOn = DateTime.UtcNow
                };

                dbContext.Questions.Add(question);
                dbContext.SaveChanges();

                return Json(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
