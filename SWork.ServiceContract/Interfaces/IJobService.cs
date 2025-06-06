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
        Task CreateJobAsync(CreateJobDTO jobDto, string userId);
        Task UpdateJobAsync(int jobId, UpdateJobDTO jobdto, string userId);
        Task DeleteJobAsync(int jobId, string userId);
        Task<Pagination<Job>> SearchJobAsync(JobSearchRequestDTO filter, int jobCategory, int pageIndex, int pageSize);
    }
}
