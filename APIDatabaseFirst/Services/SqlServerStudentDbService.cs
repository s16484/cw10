using APIDatabaseFirst.DTOs.Request;
using APIDatabaseFirst.DTOs.Responses;
using APIDatabaseFirst.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDatabaseFirst.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {

        private readonly s16484Context _context;
        public SqlServerStudentDbService(s16484Context context)
        {
            _context = context;
        }

        public IEnumerable<dynamic> GetStudents()
        {
            var result = _context.Student
                .Select(s => new
                {
                    s.IndexNumber,
                    s.FirstName,
                    s.LastName
                }).ToList();

            return result;

        }

        public IEnumerable<dynamic> GetStudent(string id)
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
            return result;
        }

        public void UpdateStudent(string id, Student stud)
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

            if (stud.FirstName != null)
            {
                _context.Entry(student).Property("FirstName").IsModified = true;
            }
            if (stud.LastName != null)
            {
                _context.Entry(student).Property("LastName").IsModified = true;
            }
            if (stud.BirthDate != default)
            {
                _context.Entry(student).Property("BirthDate").IsModified = true;
            }
            if (stud.IdEnrollment != default)
            {
                _context.Entry(student).Property("IdEnrollment").IsModified = true;
            }
            if (stud.Role != null)
            {
                _context.Entry(student).Property("Role").IsModified = true;
            }

            _context.SaveChanges();
        }

        public void DeleteStudent(string id)
        {
            var student = new Student
            {
                IndexNumber = id
            };
            _context.Attach(student);
            _context.Remove(student);
            _context.SaveChanges();
        }

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {

            EnrollStudentResponse response;
            DateTime now = DateTime.Now;

            var studyName = _context.Studies
                .Where(s => s.Name == request.StudyName)
                .FirstOrDefault();

            if (studyName == null)
            {
                throw new ArgumentException("Podane studia nie istnieja");
            }

            var enr = _context.Enrollment
                .Where(e => e.IdStudy == studyName.IdStudy && e.Semester == 1)
                .Select(e => new
                {
                    semester = e.Semester,
                    idEnrollment = e.IdEnrollment,
                    startDate = e.StartDate
                })
                .FirstOrDefault();

            var idEnr = enr.idEnrollment;

            if (idEnr == default)
            {
                idEnr = _context.Enrollment.Max(e => e.IdEnrollment) + 1;

                var newEnrollment = new Enrollment
                {
                    IdEnrollment = idEnr,
                    Semester = 1,
                    IdStudy = studyName.IdStudy,
                    StartDate = now
                };

                _context.Attach(newEnrollment);
                _context.Enrollment.Add(newEnrollment);
                _context.SaveChanges();
            }


            var studIndexNumber = _context.Student
                .Where(s => s.IndexNumber == request.IndexNumber)
                .Select(s => new
                {
                    index = s.IndexNumber
                })
                .FirstOrDefault();

            if (studIndexNumber != default)
            {
                throw new ArgumentException("Student już istnieje w bazie!");
            }


            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IndexNumber = request.IndexNumber,
                IdEnrollment = idEnr
            };
            _context.Attach(student);
            _context.Student.Add(student);
            _context.SaveChanges();

            response = new EnrollStudentResponse()
            {
                LastName = request.LastName,
                IdEnrollment = idEnr,
                IdStudy = studyName.IdStudy,
                Semester = 1,
                StartDate = DateTime.Now
            };

            return response;

        }

        public PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request)
        {
            PromoteStudentsResponse response;
            DateTime now = DateTime.Now;

            var checkStudies = _context.Studies
                .Where(s => s.Name == request.Studies)
                .FirstOrDefault();

            if (checkStudies == null)
            {
                throw new ArgumentException("Podane studia nie istnieja");
            }

            var studies = _context.Enrollment
                          .Join(_context.Studies,
                                  enr => enr.IdStudy,
                                  stu => stu.IdStudy,
                                  (enr, stu) => new
                                  {
                                      enr,
                                      stu
                                  })
                          .Where(s => s.stu.Name == request.Studies
                                   && s.enr.Semester == request.Semester)
                          .FirstOrDefault();

            if (studies == default)
            {
                throw new ArgumentException("Wpis z podanym semestrem nie istnieje w bazie");
            }

            var result = _context.Enrollment
                .FromSqlRaw("EXEC [dbo].[PromoteStudents] {0}, {1}", request.Studies, request.Semester)
                .AsEnumerable()
                .FirstOrDefault();

            if (result != null)
            {
                response = new PromoteStudentsResponse()
                {
                    IdEnrollment = result.IdEnrollment,
                    IdStudy = result.IdStudy,
                    Semester = result.Semester,
                    StartDate = result.StartDate
                };

                return response;
            }
            return null;
        }
    }
}
