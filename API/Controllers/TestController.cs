namespace API.Controllers{
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

     [Route("api/test")]
    class TestController : Controller
    {

        private readonly AppDbContext dbContext;
        public TestController(AppDbContext dbContext){
            this.dbContext = dbContext;
        }

        // [HttpGet("train-stations")]
        // public async Task<IActionResult> GetStations()
        // {
        //     if(false){
        //         // Return HTTP Result Core
        //         return StatusCode(StatusCodes.Status400BadRequest);
        //     }

        //     var comment = new Comment {
        //         Id = Guid.NewGuid(),
        //         CommentBody = "asasdasdasd"
        //     };

        //     return Json();

        // }


[HttpGet("users")]
public async Task<IActionResult> GetUsers()
{
    return Json(dbContext.Users.ToList());
}


    }
}