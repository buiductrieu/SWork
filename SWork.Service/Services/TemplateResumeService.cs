

using MailKit.Search;

namespace SWork.Service.Services
{
    public class TemplateResumeService(IUnitOfWork unitOfWork, IMapper mapper) : ITemplateResumeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task CreateTResumeAsync(TemplateResumeDTO tResumDto)
        {
            var tResum = _mapper.Map<ResumeTemplate>(tResumDto);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.GenericRepository<ResumeTemplate>().InsertAsync(tResum);
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

        public async Task DeleteTResumeAsync(int tResumId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var tResum = await _unitOfWork.GenericRepository<ResumeTemplate>().GetByIdAsync(tResumId);
                if (tResum == null)
                    throw new Exception("CV template not found");


                _unitOfWork.GenericRepository<ResumeTemplate>().Delete(tResum);
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

        public async Task<Pagination<ResumeTemplate>> GetPaginatedTResumeAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<ResumeTemplate, bool>>? predicate = null,
            Expression<Func<ResumeTemplate, object>>? orderBy = null,
            bool isDescending = false)
        {
            try
            {

                var result = await _unitOfWork.GenericRepository<ResumeTemplate>().GetPaginationAsync(
                    predicate = predicate,
                    includeProperties: null,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    orderBy: orderBy ?? (p => p.TemplateID),
                    isDescending: isDescending
                    );

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResumeTemplate> GetTResumeByIdAsync(int tResumId)
        {
            var tResum = await _unitOfWork.GenericRepository<ResumeTemplate>().GetByIdAsync(tResumId);
            if (tResum == null)
                throw new Exception("CV template not found");
            return tResum;
        }

        public async Task<Pagination<ResumeTemplate>> SearchTResumeAsync(string nameTemplate, int pageIndex, int pageSize)
        {
            Expression<Func<ResumeTemplate, bool>> predicate = tResum =>
                string.IsNullOrEmpty(nameTemplate) || tResum.TemplateName.Contains(nameTemplate);

            var result = await _unitOfWork.GenericRepository<ResumeTemplate>().GetPaginationAsync(
            predicate: predicate,
            includeProperties: null,
            pageIndex: pageIndex,
            pageSize: pageSize,
            orderBy: p => p.TemplateName,
            isDescending: false);

            return result;

        }

        public async Task UpdateTResumeAsync(ResumeTemplate tResum)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.GenericRepository<ResumeTemplate>().Update(tResum);
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
