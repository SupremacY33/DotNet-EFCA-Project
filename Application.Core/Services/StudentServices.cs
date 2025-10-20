using SchoolManagement.Core.Entities;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Application.DTOs;

namespace SchoolManagement.Application.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly IRepository<Students> _studentRepository;

        public StudentServices(IRepository<Students> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentDTOs>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            if(students == null)
            {
                throw new Exception("No Student Record Was Found!!");
            }
            else
            {
                return students.Select(s => new StudentDTOs
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    JoinDate = s.JoinDate,
                    DateOfBirth = s.DateOfBirth,
                    Gender = s.Gender,
                    Nationality = s.Nationality,
                    Religion = s.Religion,
                    Address = s.Address,
                    Email = s.Email
                });
            }
        }

        public async Task<StudentDTOs> GetStudentByIdAsync(int Id)
        {
            var s = await _studentRepository.GetbyIdAsync(Id);
            if (s == null)
            {
                throw new Exception($"Student With Id: {Id} Record Was Not Found!!");
            }
            else
            {
                return new StudentDTOs
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    JoinDate = s.JoinDate,
                    DateOfBirth = s.DateOfBirth,
                    Gender = s.Gender,
                    Nationality = s.Nationality,
                    Religion = s.Religion,
                    Address = s.Address,
                    Email = s.Email
                };
            }
        }

        public async Task<StudentDTOs> CreateStudentAsync(StudentDTOs studentDto)
        {
            var newStudent = new Students
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                JoinDate = studentDto.JoinDate,
                DateOfBirth = studentDto.DateOfBirth,
                Gender = studentDto.Gender,
                Nationality = studentDto.Nationality,
                Religion = studentDto.Religion,
                Address = studentDto.Address,
                Email = studentDto.Email
            };
            await _studentRepository.AddAsync(newStudent);
            studentDto.Id = newStudent.Id;
            return studentDto;
        }

        public async Task UpdateStudentAsync(StudentDTOs studentDto)
        {
            var student = await _studentRepository.GetbyIdAsync(studentDto.Id);
            if (student == null)
            {
                throw new Exception($"Student with the id: {studentDto.Id} was not found");
            }
            else
            {
                student.FirstName = studentDto.FirstName;
                student.LastName = studentDto.LastName;
                student.JoinDate = studentDto.JoinDate;
                student.DateOfBirth = studentDto.DateOfBirth;
                student.Gender = studentDto.Gender;
                student.Nationality = studentDto.Nationality;
                student.Religion = studentDto.Religion;
                student.Address = studentDto.Address;
                student.Email = studentDto.Email;
            }

            await _studentRepository.UpdateAsync(student);
        }

        public async Task DeleteStudentAsync(int Id)
        {
            var student = await _studentRepository.GetbyIdAsync(Id);
            if (student == null)
            {
                throw new Exception($"Student with the id: {Id} was not found");
            }
            else
            {
                student.IsDeleted = true;
                await _studentRepository.DeleteAsync(Id);
            }
        }
    }
}
