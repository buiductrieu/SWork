using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWork.Data.Entities;
using SWork.RepositoryContract.Basic;
using SWork.RepositoryContract.Interfaces;
using SWork.RepositoryContract.IUnitOfWork;
using SWork.ServiceContract.Interfaces;
using SWork.Data.DTO.StudentDTO;

namespace SWork.Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Resume> _resumeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        private const string DefaultIncludes = "User,Resumes,Applications,JobBookmarks";

        public StudentService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _studentRepository = _unitOfWork.GenericRepository<Student>();
            _resumeRepository = _unitOfWork.GenericRepository<Resume>();
        }

        public async Task<StudentResponseDTO> GetStudentByIdAsync(int id)
        {
            var student = await _unitOfWork.GenericRepository<StudentResponseDTO>().GetByIdAsync(id);
            if (student == null)
                throw new KeyNotFoundException($"Student with ID {id} not found");

            return _mapper.Map<StudentResponseDTO>(student);
        }

        public async Task<StudentResponseDTO> GetStudentByUserIdAsync(string userId)
        {
            // Verify user exists and is confirmed
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");
            
            if (!user.EmailConfirmed)
                throw new InvalidOperationException("User email is not confirmed");

            var student = await _studentRepository.GetFirstOrDefaultAsync(
                s => s.UserID == userId,
                DefaultIncludes
            );
            if (student == null)
                throw new KeyNotFoundException($"Student with User ID {userId} not found");

            return _mapper.Map<StudentResponseDTO>(student);
        }

        public async Task<IEnumerable<StudentResponseDTO>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync(s => true, DefaultIncludes);
            return students.Select(s => _mapper.Map<StudentResponseDTO>(s));
        }

        public async Task<StudentResponseDTO> CreateStudentAsync(StudentCreateDTO studentDto, string userId)
        {
            // Verify user exists, is confirmed and has Student role
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            if (!user.EmailConfirmed)
                throw new InvalidOperationException("User email is not confirmed");

            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains("Student"))
                throw new InvalidOperationException("User does not have Student role");

            // Check if student profile already exists
            var existingStudent = await _studentRepository.GetFirstOrDefaultAsync(s => s.UserID == userId);
            if (existingStudent != null)
                throw new InvalidOperationException($"Student profile already exists for user {userId}");

            var student = _mapper.Map<Student>(studentDto);
            student.UserID = userId;
            await _studentRepository.InsertAsync(student);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<StudentResponseDTO>(student);
        }

        public async Task<StudentResponseDTO> UpdateStudentAsync(int id, StudentCreateDTO studentDto, string userId)
        {
            // Verify user exists and is confirmed
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            if (!user.EmailConfirmed)
                throw new InvalidOperationException("User email is not confirmed");

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new KeyNotFoundException($"Student with ID {id} not found");

            if (student.UserID != userId)
                throw new UnauthorizedAccessException("You don't have permission to update this student");

            _mapper.Map(studentDto, student);
            _studentRepository.Update(student);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<StudentResponseDTO>(student);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new KeyNotFoundException($"Student with ID {id} not found");

            _studentRepository.Delete(student);
            await _unitOfWork.SaveChangeAsync();

            return true;
        }

        public async Task<IEnumerable<StudentResponseDTO>> GetStudentsBySkillAsync(string skill)
        {
            if (string.IsNullOrWhiteSpace(skill))
                throw new ArgumentException("Tên kỹ năng không được để trống", nameof(skill));

            // Tìm tất cả sinh viên có kỹ năng được chỉ định trong Resume
            var students = await _studentRepository.GetAllAsync(
                s => s.Resumes.Any(r => r.Skills.Contains(skill, StringComparison.OrdinalIgnoreCase)),
                DefaultIncludes
            );

            return students.Select(s => _mapper.Map<StudentResponseDTO>(s));
        }

        public async Task<IEnumerable<StudentResponseDTO>> GetStudentsByUniversityAsync(string university)
        {
            if (string.IsNullOrWhiteSpace(university))
                throw new ArgumentException("Tên trường đại học không được để trống", nameof(university));

            try
            {
                var students = await _studentRepository.GetAllAsync(
                    s => EF.Functions.Like(s.University, $"%{university}%"),
                    "User,Resumes,Applications,JobBookmarks"
                );

                if (!students.Any())
                    return new List<StudentResponseDTO>();

                return students.Select(s => _mapper.Map<StudentResponseDTO>(s));
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm sinh viên theo trường đại học: {ex.Message}");
            }
        }

        public async Task<IEnumerable<StudentResponseDTO>> GetStudentsByMajorAsync(string major)
        {
            if (string.IsNullOrWhiteSpace(major))
                throw new ArgumentException("Tên chuyên ngành không được để trống", nameof(major));

            try
            {
                var students = await _studentRepository.GetAllAsync(
                    s => EF.Functions.Like(s.Major, $"%{major}%"),
                    "User,Resumes,Applications,JobBookmarks"
                );

                if (!students.Any())
                    return new List<StudentResponseDTO>();

                return students.Select(s => _mapper.Map<StudentResponseDTO>(s));
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm sinh viên theo chuyên ngành: {ex.Message}");
            }
        }
    }
}
