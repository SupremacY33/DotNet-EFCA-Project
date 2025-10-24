using SchoolManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Core.Entities
{
    public class User : BaseEntity
    {
        public required string Username { get; set; } = string.Empty;
        public required string PasswordHash { get; set; } = string.Empty;
        public required string Role { get; set; } = "Student"; // Admin, Teacher, Student
        public bool IsActive { get; set; } = true;

        public int? StudentId { get; set; }
        public Students? Student { get; set; }
    }
}