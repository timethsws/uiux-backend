using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.DTOs;
using Core.Database;
using Core.Entities;
using Core.Model;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserService userService;

        private readonly AppDbContext dbContext;

        public UserController (UserService userService, AppDbContext dbContext)
        {
            this.userService = userService;
            this.dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser ([FromBody] ApplicationUserModel userData)
        {
            if(userData == null)
            {
                return BadRequest();
            }

            try
            {
                var res = await userService.AddUser(userData);
                if(!res.Status)
                {
                    return Json(OperationActionResult.Failed<ApplicationUser>(res.Message));
                }

                return Json(OperationActionResult.Success(res.Value));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login ([FromBody] LoginModel loginData)
        {
            if(loginData == null)
            {
                return BadRequest();
            }

            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(loginData.Username));
                if(user == null)
                {
                    return Json(OperationActionResult.Failed<ApplicationUserDTO>("InvalidUserNamePassword"));
                }
                var res = userService.CheckPassword(user, loginData.Password);

                if(!res.Status)
                {
                    return Json(OperationActionResult.Failed<ApplicationUserDTO>("InvalidUserNamePassword"));
                }

                return Json(OperationActionResult.Success(new ApplicationUserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                }));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
