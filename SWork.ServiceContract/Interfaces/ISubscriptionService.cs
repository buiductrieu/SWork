
using SWork.Data.DTO.SubDTO;

namespace SWork.ServiceContract.Interfaces;

public interface ISubscriptionService
{
    Task<List<Subscription>> GetAllSubscriptionAsync();
    Task<Subscription> GetSubscriptionByName(string subName);
    Task<Subscription> GetSubscriptionByIdAsync(int subId);
    Task CreateSubscriptionAsync(SubDTO subDto);
    Task UpdateSubscriptionAsync(Subscription sub);
    Task DeleteSubscriptionAsync(int subId);
}
