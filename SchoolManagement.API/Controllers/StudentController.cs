using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Infrastructure.Persistance;

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentServices;
        private readonly IUserServices _userServices;
        private readonly StudentDbContext _studentDbContext;
        private readonly IAuthServices _authServices;

        public StudentController(IStudentServices studentServices, IUserServices userServices, 
            StudentDbContext studentDbContext, IAuthServices authServices)
        {
            _studentServices = studentServices;
            _userServices = userServices;
            _studentDbContext = studentDbContext;
            _authServices = authServices;
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

            using var transaction = await _studentDbContext.Database.BeginTransactionAsync();

            try
            {
                // Step 1 - Create a student record
                var createStudent = await _studentServices.CreateStudentAsync(studentDto);

                // Step 2 - Create a link user account for the student
                var (createUser, tempPassword) = await _userServices.CreateUserForStudentAsync(createStudent);

                // Step 3 - Commit both transaction as one
                await transaction.CommitAsync();

                // Step 4 - Return the created student details along with temporary password
                return CreatedAtAction
                    (
                        nameof(GetStudentById),
                        new { id = createStudent.Id },
                        new
                        {
                            Message = $"Student Record (Id: {createStudent.Id}) and User Account Created Successfully!",
                            Student = new
                            {
                                createStudent.Id,
                                createStudent.FirstName,
                                createStudent.LastName,
                                createStudent.Email,
                                createStudent.JoinDate,
                                createStudent.DateOfBirth,
                                createStudent.Gender,
                                createStudent.Nationality,
                                createStudent.Religion
                            },
                            UserCredential = new
                            {
                                Username = createUser.Username,
                                TemporaryPassword = tempPassword
                            }
                        }
                    );
            }
            catch (Exception ex)
            {
                // Rollback transaction if any error occurs
                await transaction.RollbackAsync();
                return StatusCode(500, new 
                {
                    Message = "An error occurred while creating the student and user account.",
                    Details = ex.Message
                });
            }
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else 
            {
                var token = await _authServices.AuthenticationAsync(loginDTO);

                if (token == null)
                {
                    return Unauthorized("Invalid username or password.");
                }
                else 
                {
                    return Ok(new 
                    {
                        Token = token,
                        Message = "Login Successful!!",
                        Expiration = DateTime.UtcNow.AddHours(2)
                    });
                }
            }
        }
    }
}