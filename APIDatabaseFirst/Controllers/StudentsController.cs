using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDatabaseFirst.Models;
using APIDatabaseFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIDatabaseFirst.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDbService _dbservice;
        public StudentsController(IStudentDbService dbservice)
        {
            _dbservice = dbservice;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var result = _dbservice.GetStudents();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent([FromRoute] string id)
        {
            var result = _dbservice.GetStudent(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent([FromRoute] string id, [FromBody] Student stud)
        {
            _dbservice.UpdateStudent(id,stud);
            return Ok("Aktualizacja zakończona powodzeniem");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(string id)
        {
            _dbservice.DeleteStudent(id);
            return Ok("Usunięto studenta: " + id);
        }



    }
}
