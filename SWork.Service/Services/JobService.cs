using SWork.Data.DTO.JobDTO;
using SWork.ServiceContract.ICloudinaryService;

namespace SWork.Service.Services
{
    public class JobService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryImageService cloudImageService) : IJobService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICloudinaryImageService _cloudinaryImageService = cloudImageService;
        public async Task<Pagination<Job>> GetPaginatedJobAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<Job, bool>>? predicate = null,
            Expression<Func<Job, object>>? orderBy = null,
            bool isDescending = false)
        {
            try
            {
                var result = await _unitOfWork.GenericRepository<Job>().GetPaginationAsync(
                    predicate = predicate,
                    includeProperties: "Subscription", 
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    orderBy: orderBy ?? (p => p.JobID),
                    isDescending: isDescending);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task CreateJobAsync(CreateJobDTO jobDto, string userId)
        {

            var subscription = await _unitOfWork.GenericRepository<Subscription>().GetFirstOrDefaultAsync(a => a.SubscriptionID == jobDto.SubscriptionID);
            if (subscription == null) throw new Exception("Gói bài viết không tồn tại.Vui lòng chọn lại!");

            var employer = await _unitOfWork.GenericRepository<Employer>().GetFirstOrDefaultAsync(a => a.UserID == userId);
            if (employer == null) throw new Exception("Bạn không có quyền tạo mới công việc.");

            jobDto.EmployerID = employer.EmployerID;
            var job = _mapper.Map<Job>(jobDto);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if(jobDto.Image != null)
                {
                    string imageUrl = await _cloudinaryImageService.UploadImageAsync(jobDto.Image, "job-images");
                    job.ImageUrl = imageUrl;
                }
                await _unitOfWork.GenericRepository<Job>().InsertAsync(job);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateJobAsync(int jobId, UpdateJobDTO jobdto, string userId)
        {

            var subscription = await _unitOfWork.GenericRepository<Subscription>().GetFirstOrDefaultAsync(a => a.SubscriptionID == jobdto.SubscriptionID);
            if (subscription == null) throw new Exception("Gói bài viết không tồn tại.Vui lòng chọn lại!");

            var job = await _unitOfWork.GenericRepository<Job>().GetFirstOrDefaultAsync(a => a.JobID == jobId);
            if (job == null) throw new Exception("Bài viết không tồn tại.");

            if (job.Status == "IsActive") throw new Exception("Bài viết đã hết hạn.");

            var employer = await _unitOfWork.GenericRepository<Employer>().GetFirstOrDefaultAsync(a => a.UserID == userId);

            if (job.EmployerID != employer.EmployerID) throw new Exception("Bạn không có quyền chỉnh sửa bài viết này.");


            await _unitOfWork.BeginTransactionAsync();
            
            try
            {
                // update properties 
                if (!string.IsNullOrWhiteSpace(jobdto.Title)) job.Title = jobdto.Title;
                if (!string.IsNullOrWhiteSpace(jobdto.Description)) job.Description = jobdto.Description;
                if (!string.IsNullOrWhiteSpace(jobdto.Requirements)) job.Requirements = jobdto.Requirements;
                if (!string.IsNullOrWhiteSpace(jobdto.Location)) job.Location = jobdto.Location;
                if (jobdto.Salary.HasValue) job.Salary = jobdto.Salary.Value;
                if (!string.IsNullOrWhiteSpace(jobdto.Status)) job.Status = jobdto.Status;
                if (!string.IsNullOrWhiteSpace(jobdto.WorkingHours)) job.WorkingHours = jobdto.WorkingHours;
                if (jobdto.StartDate.HasValue) job.StartDate = jobdto.StartDate.Value;
                if (jobdto.EndDate.HasValue) job.EndDate = jobdto.EndDate.Value;
                if (jobdto.SubscriptionID.HasValue) job.SubscriptionID = jobdto.SubscriptionID.Value;
                //update image
                if (jobdto.Image != null)
                {
                    if (!string.IsNullOrEmpty(job.ImageUrl))
                    {
                        string publicId = _cloudinaryImageService.ExtractPublicIdFromUrl(job.ImageUrl);
                        //  Console.WriteLine("Extracted publicId: " + publicId);
                        await _cloudinaryImageService.DeleteImageAsync(publicId);
                    }
                    string imageUrl = await _cloudinaryImageService.UploadImageAsync(jobdto.Image, "job-images");
                    job.ImageUrl = imageUrl;
                }
                _unitOfWork.GenericRepository<Job>().Update(job);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task DeleteJobAsync(int jobId, string userId)
        {
            var job = await _unitOfWork.GenericRepository<Job>().GetFirstOrDefaultAsync(a => a.JobID == jobId);
            if (job == null) throw new Exception("Bài viết không tồn tại.");

            var employer = await _unitOfWork.GenericRepository<Employer>().GetFirstOrDefaultAsync(a => a.UserID == userId);
            if (job.EmployerID != employer.EmployerID) throw new Exception("Bạn không có xóa bài viết này.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (!string.IsNullOrEmpty(job.ImageUrl))
                {
                    string publicId = _cloudinaryImageService.ExtractPublicIdFromUrl(job.ImageUrl);
                    await _cloudinaryImageService.DeleteImageAsync(publicId);
                }

                _unitOfWork.GenericRepository<Job>().Delete(job);
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
        public async Task<Job> GetJobByIdAsync(int jobId)
        {
           var job = await _unitOfWork.GenericRepository<Job>().GetByIdAsync(jobId);
            if(job == null)
                throw new Exception("Job not found");
            return job;
        }
        public async Task<Pagination<Job>> SearchJobAsync(JobSearchRequestDTO filter , int jobCategory, int pageIndex, int pageSize)
        {
            Expression<Func<Job, bool>> predicate = job =>
            (string.IsNullOrEmpty(filter.Keyword) ||
            job.Title.Contains(filter.Keyword) ||
            job.Description.Contains(filter.Keyword) ||
            job.Requirements.Contains(filter.Keyword)) &&
            (string.IsNullOrEmpty(filter.Location) || job.Location.Contains(filter.Location)) &&
            (!filter.MinSalary.HasValue || job.Salary >= filter.MinSalary.Value) &&
            (!filter.MaxSalary.HasValue || job.Salary <= filter.MaxSalary.Value);

            var result = await _unitOfWork.GenericRepository<Job>().GetPaginationAsync(
                predicate: predicate,
                includeProperties: " Subscription",
                pageIndex: pageIndex,
                pageSize: pageSize,
                orderBy: job => new
                {
                    Type = job.Subscription.SubscriptionName,
                    CreateAt = job.CreatedAt
                },
                isDescending: true
           );
            return result;
        }
    }
}
