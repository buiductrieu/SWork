using SWork.Data.DTO.CVDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface IResumeService
    {
        Task<Pagination<Resume>> GetPaginatedResumeAsync(
           int pageIndex,
           int pageSize,
           Expression<Func<Resume, bool>>? predicate = null,
           Expression<Func<Resume, object>>? orderBy = null,
           bool isDescending = false);
        Task<Resume> GetResumeByIdAsync(int resumId);
        Task CreateResumeAsync(CreateResumeDTO resumDto);
        Task UpdateResumeAsync(Resume resum);
        Task DeleteResumeAsync(int resumId);

        Task<Pagination<Resume>> SearchResumeAsync(string? nameResume, int? studentId, int pageIndex, int pageSize);
    }

}
