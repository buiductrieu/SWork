﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWork.ServiceContract.Interfaces;
using SWork.Data.DTO.StudentDTO;
using System.Security.Claims;

namespace SWork.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound($"Student with ID {id} not found");

            return Ok(student);
        }

        /// <summary>
        /// Get student by user ID
        /// </summary>
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetStudentByUserId(string userId)
        {
            var student = await _studentService.GetStudentByUserIdAsync(userId);
            if (student == null)
                return NotFound($"Student with User ID {userId} not found");

            return Ok(student);
        }

        /// <summary>
        /// Get all students
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        /// <summary>
        /// Create new student
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDTO studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("sub")?.Value;
                // Debug: log userId và token
                Console.WriteLine($"userId: {userId}");
                Console.WriteLine($"Token: {Request.Headers["Authorization"]}");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized($"Cannot determine userId from token. Token: {Request.Headers["Authorization"]}");
                var student = await _studentService.CreateStudentAsync(studentDto, userId);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentID }, student);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Update student information
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentCreateDTO studentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Cannot determine userId from token");
                var student = await _studentService.UpdateStudentAsync(id, studentDto, userId);
                return Ok(student);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete student
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudentAsync(id);
            if (!result)
                return NotFound($"Student with ID {id} not found");

            return NoContent();
        }

        /// <summary>
        /// Get students by skill
        /// </summary>
        [HttpGet("skill/{skillId}")]
        [Authorize]
        public async Task<IActionResult> GetStudentsBySkill(int skillId)
        {
            var students = await _studentService.GetStudentsBySkillAsync(skillId);
            return Ok(students);
        }

        /// <summary>
        /// Get students by university
        /// </summary>
        [HttpGet("university/{university}")]
        [Authorize]
        public async Task<IActionResult> GetStudentsByUniversity(string university)
        {
            var students = await _studentService.GetStudentsByUniversityAsync(university);
            return Ok(students);
        }

        /// <summary>
        /// Get students by major
        /// </summary>
        [HttpGet("major/{major}")]
        [Authorize]
        public async Task<IActionResult> GetStudentsByMajor(string major)
        {
            var students = await _studentService.GetStudentsByMajorAsync(major);
            return Ok(students);
        }
    }
}
