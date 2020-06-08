using APIDatabaseFirst.DTOs.Responses;
using APIDatabaseFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDatabaseFirst.Services
{
    public interface IStudentDbService 
    {
        public IEnumerable<dynamic> GetStudents();
        public IEnumerable<dynamic> GetStudent(string id);
        public void UpdateStudent(string id, Student stud);
        public void DeleteStudent(string id);

    }
}
