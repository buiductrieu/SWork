
using SWork.Data.DTO;

namespace SWork.Service.Services
{
    public class JobCategoryService(IUnitOfWork unitOfWork, IMapper mapper) : IJobCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task CreateJobCategoryAsync(JobCategoryDTO categoryDto)
        {
            var cate = _mapper.Map<JobCategory>(categoryDto);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.GenericRepository<JobCategory>().InsertAsync(cate);
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

        public async Task DeleteJobCategoryAsync(int category)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var cate = await _unitOfWork.GenericRepository<JobCategory>().GetByIdAsync(category);
                if (cate == null)
                    throw new Exception("Category not found");


                _unitOfWork.GenericRepository<JobCategory>().Delete(cate);
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

        public async Task<List<JobCategory>> GetAllJobCategoryAsync()
        {
            return await _unitOfWork.GenericRepository<JobCategory>().GetAll().ToListAsync();
        }

        public async Task<JobCategory> GetCategoryByIdAsync(int categoryId)
        {
            var cate = await _unitOfWork.GenericRepository<JobCategory>().GetByIdAsync(categoryId);
            if (cate == null)
                throw new Exception("Category not found");
            return cate;
        }

        public async Task<JobCategory> GetCategoryByName(string cateName)
        {
            var listCate = await GetAllJobCategoryAsync();
            if (listCate == null)
                throw new Exception("Do not have this category");

            var cate = listCate.FirstOrDefault(s => s.CategoryName.Contains(cateName, StringComparison.OrdinalIgnoreCase));
            return cate;
        }

        public async Task UpdateJobCategoryAsync(JobCategory category)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.GenericRepository<JobCategory>().Update(category);
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
