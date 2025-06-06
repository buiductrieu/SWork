
using SWork.Data.DTO.StudentDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface IStudentService
    {
        Task<StudentResponseDTO> GetStudentByIdAsync(int id);
        Task<StudentResponseDTO> GetStudentByUserIdAsync(string userId);
        Task<IEnumerable<StudentResponseDTO>> GetAllStudentsAsync();
        Task<StudentResponseDTO> CreateStudentAsync(StudentCreateDTO studentDto, string userId);
        Task<StudentResponseDTO> UpdateStudentAsync(int id, StudentCreateDTO studentDto, string userId);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<StudentResponseDTO>> GetStudentsBySkillAsync(string skill);
        Task<IEnumerable<StudentResponseDTO>> GetStudentsByUniversityAsync(string university);
        Task<IEnumerable<StudentResponseDTO>> GetStudentsByMajorAsync(string major);
    }
}
