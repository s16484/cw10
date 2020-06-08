using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDatabaseFirst.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIDatabaseFirst.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly s16484Context _context;

        public StudentsController(s16484Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok();
        }
    }
}
