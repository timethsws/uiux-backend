using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    [Route("api/values")]
    public class ValuesController : Controller
    {
        private readonly AppDbContext dbContext;
        public ValuesController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("seedDb")]
        public bool SeedDb()
        {
            try
            {
                SeedData.SeedDatabase(dbContext);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        [HttpGet("trainStation")]
        public async Task<IActionResult> getStations()
        {
            return Json(dbContext.Stations.ToList());
        }

        [HttpGet("places")]
        public async Task<IActionResult> getPlaces()
        {
            return Json(dbContext.Places.ToList());
        }

    }





}
