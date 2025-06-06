using SWork.Data.DTO.EmployerDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface IEmployerService
    {
        Task<EmployerResponseDTO> GetEmployerByIdAsync(int id);
        Task<EmployerResponseDTO> GetEmployerByUserIdAsync(string userId);
        Task<IEnumerable<EmployerResponseDTO>> GetAllEmployersAsync();
        Task<EmployerResponseDTO> CreateEmployerAsync(EmployerCreateDTO employerDto, string userId);
        Task<EmployerResponseDTO> UpdateEmployerAsync(int id, EmployerCreateDTO employerDto, string userId);
        Task<bool> DeleteEmployerAsync(int id);
        Task<IEnumerable<EmployerResponseDTO>> GetEmployersByIndustryAsync(string industry);
        //Task<IEnumerable<EmployerResponseDTO>> GetEmployersByLocationAsync(string location);
        Task<IEnumerable<EmployerResponseDTO>> GetEmployersByCompanySizeAsync(string size);
    }
} 