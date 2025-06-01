using SWork.Data.DTO;

namespace SWork.ServiceContract.Interfaces;

public interface IJobCategoryService
{
    Task<List<JobCategory>> GetAllJobCategoryAsync();
    Task<JobCategory> GetCategoryByName(string cateName);
    Task<JobCategory> GetCategoryByIdAsync(int categoryId);
    Task CreateJobCategoryAsync(JobCategoryDTO categoryDto);
    Task UpdateJobCategoryAsync(JobCategory category);
    Task DeleteJobCategoryAsync(int category);
}
