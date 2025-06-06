namespace SWork.ServiceContract.Interfaces
{
    public interface IJobService
    {
        Task<Pagination<Job>> GetPaginatedJobAsync(
           int pageIndex,
           int pageSize,
           Expression<Func<Job, bool>>? predicate = null,
           Expression<Func<Job, object>>? orderBy = null,
           bool isDescending = false);

        Task<Job> GetJobByIdAsync(int jobId);
        Task CreateJobAsync(CreateJobDTO jobDto);
        Task UpdateJobAsync(Job job, IFormFile newImage);
        Task DeleteJobAsync(int jobId);
     
    }
}
