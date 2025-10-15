using SchoolManagement.Application.DTOs;
using SchoolManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IStudentServices
    {
        Task<IEnumerable<StudentDTOs>> GetAllStudentsAsync();
        Task<StudentDTOs> GetStudentByIdAsync(int id);
        Task<StudentDTOs> CreateStudentAsync(StudentDTOs studentDto);
        Task UpdateStudentAsync(StudentDTOs studentDto);
        Task DeleteStudentAsync(int id);
    }
}
