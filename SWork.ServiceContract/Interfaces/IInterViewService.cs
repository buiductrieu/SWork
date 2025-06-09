
using SWork.Data.DTO.InterviewDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface IInterViewService
    {
        Task<InterviewDTO> UpdateInterviewAsync(UpdateInterviewDTO interDto, string userId);
        Task<InterviewDTO> GetInterviewByIdAsync(int interId, string userId);
        Task<Pagination<Interview>> GetPaginatedInterviewAsync(
             int pageIndex,
             int pageSize,
             Expression<Func<Interview, bool>>? predicate = null,
             Expression<Func<Interview, object>>? orderBy = null,
             bool isDescending = false);

        Task<Pagination<InterviewDTO>> GetInterviewRelatedApplicationForEmployer(string userId, int interId, int pageIndex, int pageSize);
        Task<Pagination<InterviewDTO>> GetInterviewJobForStudent(string userId, int pageIndex, int pageSize);
    }
}
