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
    [Route("api/question")]
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

                    var answer = dbContext.Answers
                        .Include(q => q.Likes)
                        .Include(q => q.User).ThenInclude(u => u.ProfilePicture)
                        .Where(a => a.QuestionId == question.Id)
                        .OrderByDescending(a => a.Likes.Count())
                        .Select(a => new AnswerDTO
                        {
                            Body = a.Content,
                            Id = a.Id,
                            Owner = new UserThumbDTO
                            {
                                Name = a.User.UserName,
                                ProfileImage = a.User.ProfilePicture.Url
                            },
                            CreatedOn = a.CreatedOn,
                            LikesCount = a.Likes.Count(),
                            Liked = a.Likes.Any(l => l.UserId == userId)
                        })
                        .FirstOrDefault();

                    item.Answer = answer;

                    res.Add(item);
                }

                return Json(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
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

        [HttpPost("{questionId}/like/{userId}")]
        public async Task<IActionResult> LikeQuestion([FromRoute] Guid questionId, [FromRoute] Guid userId)
        {
            if (questionId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                if (!dbContext.Questions.Any(c => c.Id == questionId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Invalid data");
                }

                var questionLike = await dbContext.QuestionLikes.FirstOrDefaultAsync(c => c.QuestionId == questionId && c.UserId == userId);
                var res = new LikeDTO();

                if (questionLike == null)
                {
                    dbContext.QuestionLikes.Add(new QuestionLikes
                    {
                        QuestionId = questionId,
                        UserId = userId
                    });
                    res.Liked = true;
                }
                else
                {
                    dbContext.QuestionLikes.Remove(questionLike);
                    res.Liked = false;
                }

                dbContext.SaveChanges();
                res.Count = dbContext.QuestionLikes.Count(c => c.QuestionId == questionId);

                return Json(res);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{questionId}/details/{userId}")]
        public async Task<IActionResult> GetQuestion ([FromRoute] Guid questionId, [FromRoute] Guid userId)
        {
            if (questionId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                if (!dbContext.Questions.Any(c => c.Id == questionId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Invalid data");
                }

                var question = await dbContext.Questions
                    .Include(q => q.Owner).ThenInclude(q => q.ProfilePicture)
                    .Include(q => q.Likes)
                    .Include(q => q.Answers)
                    .FirstOrDefaultAsync(c => c.Id == questionId && c.OwnerId == userId);

                var res = new QuestionDetailDTO
                {
                    Id = question.Id,
                    Body = question.QuestionBody,
                    CreatedOn = question.CreatedOn,
                    Owner = new UserThumbDTO
                    {
                        Name = question.Owner.UserName,
                        ProfileImage = question.Owner.ProfilePicture.Url
                    },
                    AnswerCount = question.Answers.Count(),
                    LikesCount = question.Likes.Count(),
                    Answered = question.Answers.Any(q => q.UserId == userId),
                    Liked = question.Likes.Any(q => q.UserId == userId)
                };

                return Json(res);

                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("answer")]
        public async Task<IActionResult> AnswerQuestion([FromForm] AnswerModel answerModel)
        {
            if(answerModel == null || answerModel.questionId == Guid.Empty || answerModel.userId == Guid.Empty)
            {
                return BadRequest("Invalid Request");
            }

            try
            {
                var answer = new Answer
                {
                    Content = answerModel.body,
                    QuestionId = answerModel.questionId,
                    UserId = answerModel.userId,
                    CreatedOn = DateTime.UtcNow,
                };

                dbContext.Answers.Add(answer);
                dbContext.SaveChanges();

                return Json(true);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{questionId}/answers/{userId}")]
        public async Task<IActionResult> GetAnsweres([FromRoute] Guid questionId, [FromRoute] Guid userId)
        {
            if (questionId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                if (!dbContext.Questions.Any(c => c.Id == questionId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Invalid data");
                }

                var answers = dbContext.Answers
                    .Include(a => a.User).ThenInclude(u => u.ProfilePicture)
                    .Include(a => a.Likes)
                    .Where(a => a.QuestionId == questionId)
                    .OrderByDescending(a => a.CreatedOn)
                    .ToList();
                var res = new List<AnswerDTO>();

                foreach(var answer in answers)
                {
                    var item = new AnswerDTO
                    {
                        Id = answer.Id,
                        Body = answer.Content,
                        Owner = new UserThumbDTO
                        {
                            Name = answer.User.UserName,
                            ProfileImage = answer.User.ProfilePicture.Url
                        },
                        Liked = answer.Likes.Any(a => a.UserId == userId),
                        LikesCount = answer.Likes.Count(),
                        CreatedOn = answer.CreatedOn
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

        [HttpPost("{answerId}/like-answer/{userId}")]
        public async Task<IActionResult> LikeAnswer ([FromRoute] Guid answerId, [FromRoute] Guid userId)
        {
            if (answerId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                if (!dbContext.Answers.Any(c => c.Id == answerId) || !dbContext.Users.Any(u => u.Id == userId))
                {
                    return BadRequest("Invalid data");
                }

                var answerLike = await dbContext.AnswerLikes.FirstOrDefaultAsync(c => c.AnswerId == answerId && c.UserId == userId);
                var res = new LikeDTO();

                if (answerLike == null)
                {
                    dbContext.AnswerLikes.Add(new AnswerLike
                    {
                        AnswerId = answerId,
                        UserId = userId
                    });
                    res.Liked = true;
                }
                else
                {
                    dbContext.AnswerLikes.Remove(answerLike);
                    res.Liked = false;
                }

                dbContext.SaveChanges();
                res.Count = dbContext.AnswerLikes.Count(c => c.AnswerId == answerId);

                return Json(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
