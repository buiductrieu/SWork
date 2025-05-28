using Microsoft.EntityFrameworkCore;
using SWork.Data.DTO.SubDTO;
using SWork.Data.Entities;

namespace SWork.Service.Services
{
    public class SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper) : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task CreateSubscriptionAsync(SubDTO subDto)
        {
            var sub = _mapper.Map<Subscription>(subDto);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.GenericRepository<Subscription>().InsertAsync(sub);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                //hub
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteSubscriptionAsync(int subId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var sub = await _unitOfWork.GenericRepository<Subscription>().GetByIdAsync(subId);
                if (sub == null)
                    throw new Exception("Subscription not found");


                _unitOfWork.GenericRepository<Subscription>().Delete(sub);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                //Hub
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<Subscription>> GetAllSubscriptionAsync()
        {
            return await _unitOfWork.GenericRepository<Subscription>().GetAll().ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(int subId)
        {
            var sub = await _unitOfWork.GenericRepository<Subscription>().GetByIdAsync(subId);
            if (sub == null)
                throw new Exception("Subscription not found");
            return sub;
        }

        public async Task<Subscription> GetSubscriptionByName(string subName)
        {
            var listSub = await GetAllSubscriptionAsync();
            if (listSub == null)
                throw new Exception("Do not have any subscription");

            var sub = listSub.FirstOrDefault(s => s.SubscriptionName.Contains(subName, StringComparison.OrdinalIgnoreCase));
            return sub;
        }

        public async Task UpdateSubscriptionAsync(Subscription sub)
        {

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.GenericRepository<Subscription>().Update(sub);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
