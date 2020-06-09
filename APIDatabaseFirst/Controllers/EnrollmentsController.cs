using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDatabaseFirst.DTOs.Request;
using APIDatabaseFirst.DTOs.Responses;
using APIDatabaseFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIDatabaseFirst.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _dbservice;

        public EnrollmentsController(IStudentDbService dbservice)
        {
            _dbservice = dbservice;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            EnrollStudentResponse response;
            try
            {
                response = _dbservice.EnrollStudent(request);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created("Dodano studenta", response);

        }

        [HttpPost]
        [Route("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            PromoteStudentsResponse response;
            try
            {
                response = _dbservice.PromoteStudents(request);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Created("Promocja na kolejny semestr nadana", response);

        }


    }
}
