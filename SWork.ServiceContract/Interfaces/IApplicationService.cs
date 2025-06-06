
using SWork.Data.DTO.ApplicationDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface IApplicationService 
    {
        Task<ResponseApplyDTO> CreateApplicationAsync(RequestApplyDTO apply, string userId);
        Task<ResponseApplyDTO> UpdateApplicationAsync(StatusOfApplyDTO applyDto, string userId);
        Task<ResponseApplyDTO> GetApplicationByIdAsync(int applyId, string userId);

    }
}
