using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        public Guid Id { get; set; }

        public TrainStation Start { get; set; }

        public TrainStation End { get; set; }

        public Train Train { get; set; }

        public int TicketClass { get;set;}

        public int Count { get; set; }
    }
}
