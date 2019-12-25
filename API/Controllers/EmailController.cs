using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Database;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/email")]
    public class EmailController : Controller
    {

        private readonly AppDbContext dbContext;
        public EmailController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> emailFavourits ([FromRoute] Guid userId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("Invalid Data");
            }
            var favourits = dbContext.Favourits
                .Include(f => f.Place).ThenInclude(p => p.Image)
                .Where(u => u.UserId == userId)
                .Select(u => new Place {
                    Name = u.Place.Name,
                    Image = new Image
                    {
                        Url = u.Place.Image.Url
                    },
                    ShortDescription = u.Place.ShortDescription
                })
                .ToList();

            var cardsContent = @"<style>.card {
    margin: 10px 0;
    box-shadow: 0 0px 8px 0 rgba(0, 0, 0, 0.2);
    transition: 0.3s;
    border-radius: 4px;
    width: 100%;
}

.card-header{
    display: flex;
    align-items: center;
    padding: 10px;
}

.card-header .user-thumb {
    width: 40px;
    height: 40px;
    border-radius: 50%;
}

.user-thumb-small {
    width: 40px;
    height: 40px;
    border-radius: 50%;
}

.card-header .middle {
    padding-left: 10px;
}

.card-header .right {
    margin-left: auto;
}

.rating-bar {
    color: orange;
}

.card .card-body {
    display: flex;
}

.card-body.review {
    flex-direction: row;
    align-items: center;
}

.card .review-image {
    width: 50%;
    height: 50%;
}

.card-body .content {
    padding:10px;
    width:100%;
}

@media only screen and (max-width: 600px) {
    .card-body.review .review-image {
        width: 100%;
    }

    .card-body.review {
        flex-direction: column;
    }
}
.card * .card-actions {
        display: flex;
        width: 100%;
        align-items: center;
        justify-content: flex-end;
        font-size: 12px;
    }

    .card-actions .card-action {
        margin-left: 10px;
        display: flex;
        align-items: center;
        border: none;
        padding: 0;
    }

    .card-action .action-count {
        margin-left: 5px;
    }

    .place-card{
        overflow: hidden;
        display: flex;
        width: 48%;
        margin: 5px;
    }

    .place-image-container{
        overflow: hidden;
        width: 50%;
        height: 120px;
        background-position: center center;
        background-repeat: no-repeat;
        overflow: hidden;
        background-size: cover;
    }

    .place-image{
        height: 100%;
    }

    .place-details{
        width: 50%;
        font-size: 10px;
        padding: 10px;
        padding-bottom: 0;
    }

    .place-header-container{
        display: flex;
        justify-content: space-between;
    }

    .place-header-text{
        font-size: 14px;
    }

    .places-container{
        display: flex;
        flex-wrap: wrap;
        justify-content: space-between;
    }

    .place-action{
        float: right;
        color: red;
        padding: 0 15px;
    }
    @media only screen and (max-width: 600px) {
        .places-container{
            flex-direction: column;
        }
        .place-card{
            width: 97%;
        }
    }
    .header-action{
        min-width: 60px;
    }</style>";

            foreach(var favourite in favourits)
            {
                cardsContent += $"<div class=\"places-container\"><div class=\"card place-card\">\r\n                <div class=\"place-image-container\" style=\"background-image: url({favourite.Image.Url})\"></div>\r\n                <div class=\"place-details\">\r\n                    <div>\r\n                        <div class=\"place-header-container\">\r\n                            <strong class=\"place-header-text\">{favourite.Name}</strong>\r\n                            <div class=\"header-action\" style=\"color: orange;\">\r\n                                <i class=\"zmdi zmdi-star\"></i>\r\n                                <i class=\"zmdi zmdi-star\"></i> \r\n                                <i class=\"zmdi zmdi-star\"></i>\r\n                                <i class=\"zmdi zmdi-star-outline\"></i>\r\n                                <i class=\"zmdi zmdi-star-outline\"></i>\r\n                            </div>\r\n                        </div>\r\n                        <div>\r\n                            <p>{favourite.ShortDescription} </p>\r\n                        </div>\r\n                        <div style=\"float: right;color: red;padding: 0 15px;\">\r\n                            <div class=\"footer-action\"> <i class=\"fas fa-trash\"></i></div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div></div>";
            }

            string subject = "Favourits";
            string plainTextContent = "something";
            string htmlContent = cardsContent;

            var apiKey = "SG.WAkTyKbSTD-vvP-pFK7vLg.Ovc9BpBQa7u3a1j-IgDeOvK36Wg3T9rNPVSK3IPy768";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "No Reply");
            var to = new EmailAddress(user.Email, user.UserName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return Json(true);
        }
    }
}
