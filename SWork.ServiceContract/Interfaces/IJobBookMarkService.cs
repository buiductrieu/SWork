
using SWork.Data.DTO.JobBookMarkDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface IJobBookMarkService
    {
        Task<bool> AddJobBookMark(string userId , MarkDTO dto);
        Task<bool> RemoveJobBookMark(string userId, int jobId);
    }
}
