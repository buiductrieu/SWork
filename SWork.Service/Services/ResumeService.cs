using Microsoft.AspNetCore.Http;

namespace SWork.Service.Services
{
    public class ResumeService(IUnitOfWork unitOfWork, IMapper mapper) : IResumeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task CreateResumeAsync(CreateResumeDTO resumDto)
        {
            var resum = _mapper.Map<Resume>(resumDto);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.GenericRepository<Resume>().InsertAsync(resum);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error creating resume: {ex.Message}", ex);
            }

        }

        public async Task DeleteResumeAsync(int resumId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var resume = await _unitOfWork.GenericRepository<Resume>().GetByIdAsync(resumId);
                if (resume == null)
                    throw new Exception("Resume not found");

                _unitOfWork.GenericRepository<Resume>().Delete(resume);
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

      

        public async Task<Resume> GetResumeByIdAsync(int resumId)
        {
            var resum  = await _unitOfWork.GenericRepository<Resume>().GetByIdAsync(resumId);
            if (resum == null)
                throw new Exception("Resume not found");
            return resum;
        }

        public async Task<Pagination<Resume>> SearchResumeAsync(string? nameResume, int? studentId, int pageIndex, int pageSize)
        {

            Expression<Func<Resume, bool>> predicate = resume =>
                (string.IsNullOrEmpty(nameResume) || resume.ResumeType.Contains(nameResume)) &&
                 (!studentId.HasValue || resume.StudentID == studentId.Value);

            var result = await _unitOfWork.GenericRepository<Resume>().GetPaginationAsync(
                predicate: predicate,
                includeProperties: "Student,ResumeTemplate",
                pageIndex: pageIndex,
                pageSize: pageSize
           );
            return result;
        }

        public async Task UpdateResumeAsync(Resume resum)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
              
                _unitOfWork.GenericRepository<Resume>().Update(resum);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<Pagination<Resume>> GetPaginatedResumeAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<Resume, bool>>? predicate = null,
            Expression<Func<Resume, object>>? orderBy = null,
            bool isDescending = false)
        {
            try
            {
                var result = await _unitOfWork.GenericRepository<Resume>().GetPaginationAsync(
                    predicate = predicate,
                    includeProperties: "Student,ResumeTemplate",
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    orderBy: r => r.ResumeID,
                    isDescending: isDescending);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
