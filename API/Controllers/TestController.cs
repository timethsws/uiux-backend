namespace API.Controllers
{
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
    public class TestController : Controller
    {
        private readonly AppDbContext dbContext;
        public TestController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Json(dbContext.Users.ToList());
        }

    }
}