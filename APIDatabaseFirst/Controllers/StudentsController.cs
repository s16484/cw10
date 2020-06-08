using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDatabaseFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var result = _context.Student
                .Select(s => new
                {
                    s.IndexNumber,
                    s.FirstName,
                    s.LastName
                }).ToList() ;

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent([FromRoute] string id)
        {
            var result = _context.Student
                .Select(s => new
                {
                    s.IndexNumber,
                    s.FirstName,
                    s.LastName,
                    s.BirthDate,
                    s.IdEnrollment,
                    s.Role
                })
                .Where(s => s.IndexNumber.Equals(id))
                .ToList();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent([FromRoute]string id, [FromBody] Student stud)
        {
            var student = new Student
            {
                IndexNumber = id,
                FirstName = stud.FirstName,
                LastName = stud.LastName,
                BirthDate = stud.BirthDate,
                IdEnrollment = stud.IdEnrollment,
                Role = stud.Role
            };
            _context.Attach(student);

            if (stud.FirstName != null){
                _context.Entry(student).Property("FirstName").IsModified = true;
            }
            if (stud.LastName != null){
                _context.Entry(student).Property("LastName").IsModified = true;
            }
            if (stud.BirthDate != default){
                _context.Entry(student).Property("BirthDate").IsModified = true;
            }
            if (stud.IdEnrollment != default)
            {
                _context.Entry(student).Property("IdEnrollment").IsModified = true;
            }
            if (stud.Role != null){
                _context.Entry(student).Property("Role").IsModified = true;
            }

            _context.SaveChanges();
            return Ok("Aktualizacja zakończona powodzeniem");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(string id)
        {
            var student = new Student
            {
                IndexNumber = id
            };
            _context.Attach(student);
            _context.Remove(student);
            _context.SaveChanges();

            var result = "Usunięto studenta: " + id;
            return Ok(result);
        }



    }
}
