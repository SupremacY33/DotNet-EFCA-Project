using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Core.Entities;
using SchoolManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IRepository<User> _Repository;

        public UserServices(IRepository<User> repository)
        {
            _Repository = repository;
        }

        public async Task<(User user, string tempPassword)> CreateUserForStudentAsync(StudentDTOs studentDTOs)
        {
            if(studentDTOs == null)
            {
                throw new ArgumentNullException(nameof(studentDTOs));
            }

            // Determine username (use email or a generated Id)
            string userName = !string.IsNullOrWhiteSpace(studentDTOs.Email) ? studentDTOs.Email : $"student{studentDTOs.Id}";

            // Generating a secure temporary password
            string tempPassword = GeneratedTemporaryPassword();

            var user = new User
            {
                Username = userName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword),
                Role = "Student",
                IsActive = true,
                StudentId = studentDTOs.Id
            };

            await _Repository.AddAsync(user);
            return (user, tempPassword);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));

            var normalized = username.ToLower();

            return await _Repository.FirstOrDefault(u => u.Username.ToLower() == normalized);
        }

        private static string GeneratedTemporaryPassword(int length = 15)
        {
            // Generating a temporary password with a mix of characters
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var data = new byte[length];
            rng.GetBytes(data);
            var result = new StringBuilder(length);
            foreach (var b in data)
            {
                result.Append(chars[b % chars.Length]); 
            }
            return result.ToString();
        }
    }
}
