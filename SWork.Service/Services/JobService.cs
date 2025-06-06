using Microsoft.AspNetCore.Http;
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
                    includeProperties: "JobCategory,Subscription", 
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
        public async Task CreateJobAsync(CreateJobDTO jobDto)
        {
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
                //hub
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateJobAsync(Job job, IFormFile newImage)
        {
            await _unitOfWork.BeginTransactionAsync();
           // Console.WriteLine("job.ImageUrl: " + job.ImageUrl);
            try
            {
                if (newImage != null)
                {
                 if(!string.IsNullOrEmpty(job.ImageUrl))
                    {
                        string publicId = _cloudinaryImageService.ExtractPublicIdFromUrl(job.ImageUrl);
                      //  Console.WriteLine("Extracted publicId: " + publicId);
                        await _cloudinaryImageService.DeleteImageAsync(publicId);
                    }
                    string imageUrl = await _cloudinaryImageService.UploadImageAsync(newImage, "job-images");
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
        public async Task DeleteJobAsync(int jobId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var job = await _unitOfWork.GenericRepository<Job>().GetByIdAsync(jobId);
                if (job == null)
                    throw new Exception("Job not found");

                //delete image in cloudinary
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
        //public async Task<Pagination<Job>> SearchJobAsync(JobSearchRequestDTO filter, string category, int pageIndex, int pageSize)
        //{
        //    Expression<Func<Job, bool>> predicate = job =>
        //        (string.IsNullOrEmpty(filter.Keyword) ||
        //            job.Title.Contains(filter.Keyword) ||
        //            job.Description.Contains(filter.Keyword) ||
        //            job.Requirements.Contains(filter.Keyword)) &&
        //        (string.IsNullOrEmpty(filter.Location) || job.Location.Contains(filter.Location)) &&
        //        (!filter.MinSalary.HasValue || job.Salary >= filter.MinSalary.Value) &&
        //        (!filter.MaxSalary.HasValue || job.Salary <= filter.MaxSalary.Value) &&
        //        (string.IsNullOrEmpty(category) || job.Category == category);

        //    return await GetPaginatedJobAsync(pageIndex, pageSize, predicate);
        //}
    }
}
