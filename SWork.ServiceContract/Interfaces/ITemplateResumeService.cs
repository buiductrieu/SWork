

using SWork.Data.DTO.CVDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface ITemplateResumeService
    {
        Task<Pagination<ResumeTemplate>> GetPaginatedTResumeAsync(
           int pageIndex,
           int pageSize,
           Expression<Func<ResumeTemplate, bool>>? predicate = null,
           Expression<Func<ResumeTemplate, object>>? orderBy = null,
           bool isDescending = false);
        Task<ResumeTemplate> GetTResumeByIdAsync(int tResumId);
        Task CreateTResumeAsync(TemplateResumeDTO tResumDto);
        Task UpdateTResumeAsync(ResumeTemplate tResum);
        Task DeleteTResumeAsync(int tResumId);

        Task<Pagination<ResumeTemplate>> SearchTResumeAsync(string nameTemplate, int pageIndex, int pageSize);
    }
}
