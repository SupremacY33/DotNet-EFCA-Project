using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentServices;

        public StudentController(IStudentServices studentServices)
        {
            _studentServices = studentServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentServices.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentServices.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDTOs studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdStudent = await _studentServices.CreateStudentAsync(studentDto);
            CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
            return Ok($"Student Record Was Created Successfully!!!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDTOs studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != studentDto.Id)
                return BadRequest("Id in URL and body must match.");

            var existingStudent = await _studentServices.GetStudentByIdAsync(id);
            if (existingStudent == null)
                return NotFound();

            await _studentServices.UpdateStudentAsync(studentDto);
            return Ok($"Student Record With Id: {id} Was Updated Successfully!!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var existingStudent = await _studentServices.GetStudentByIdAsync(id);
            if (existingStudent == null)
                return NotFound();

            await _studentServices.DeleteStudentAsync(id);
            return Ok($"Student Record With Id: {id} Was Deleted Successfully!!");
        }
    }
}