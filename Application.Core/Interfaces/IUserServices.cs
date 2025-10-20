using SchoolManagement.Application.DTOs;
using SchoolManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IUserServices
    {
        // returns the created user and the temporary password (only once)
        Task<(User user, string tempPassword)> CreateUserForStudentAsync(StudentDTOs studentDTOs);

        Task<User?> GetByUsernameAsync(string username);
    }
}
