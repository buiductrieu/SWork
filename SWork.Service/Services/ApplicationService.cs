using SWork.Data.DTO.ApplicationDTO;
using SWork.Data.DTO.AuthDTO;
using SWork.Data.Entities;

namespace SWork.Service.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseApplyDTO> CreateApplicationAsync(RequestApplyDTO apply, string userId)
        {
            var user = await _unitOfWork.GenericRepository<ApplicationUser>().GetFirstOrDefaultAsync(a => a.Id == userId);
            var student = await _unitOfWork.GenericRepository<Student>().GetFirstOrDefaultAsync(a => a.UserID == userId);
            if (student == null || user == null) throw new Exception("Bạn cần đăng nhập hoặc tạo tài khoản trước khi ứng tuyển.");

            var job = await _unitOfWork.GenericRepository<Job>().GetFirstOrDefaultAsync(a => a.JobID == apply.JobID);
            if (job == null) throw new Exception("Công việc không tồn tại. Vui lòng chọn công việc khác để ứng tuyển.");

            if (job.Status == "IsActive") throw new Exception("Công việc đã hết hạn không thể ứng tuyển. Vui lòng chọn công việc khác để ứng tuyển.");

            apply.StudentID = student.StudentID;

            var application = _mapper.Map<Application>(apply);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.GenericRepository<Application>().InsertAsync(application);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                ResponseApplyDTO res = new()
                {
                    STT = application.ApplicationID,
                    StudentName = user.UserName,
                    JobName = job.Title,
                    Status = application.Status,
                    ResumeID = application.ResumeID,
                    Coverletter = application.Coverletter,
                    AppliedAt = application.AppliedAt
                };
                return res;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ResponseApplyDTO> UpdateApplicationAsync(StatusOfApplyDTO applyDto, string userId)
        {
            // check user has exist
            var user = await _unitOfWork.GenericRepository<ApplicationUser>().GetFirstOrDefaultAsync(a => a.Id == userId);
            var employer = await _unitOfWork.GenericRepository<Employer>().GetFirstOrDefaultAsync(a => a.UserID == userId);
            if (employer == null || user == null) throw new Exception("Bạn cần đăng nhập hoặc tạo tài khoản trước khi xét duyệt hồ sơ.");

            // check application has exist
            var application = await _unitOfWork.GenericRepository<Application>().GetFirstOrDefaultAsync(a => a.ApplicationID == applyDto.ApplicationID);
            if (application == null) throw new Exception("Hồ sơ không tồn tại");

            var student = await _unitOfWork.GenericRepository<Student>().GetFirstOrDefaultAsync(a => a.StudentID == application.StudentID);
            var Astudent = await _unitOfWork.GenericRepository<ApplicationUser>().GetFirstOrDefaultAsync(a => a.Id == student.UserID);

            // check this job has exist  
            var job = await _unitOfWork.GenericRepository<Job>().GetFirstOrDefaultAsync(a => a.JobID == application.JobID);
            if (job.Status == "IsActive") throw new Exception("Bài viết đã hết hạn. Không thể thực hiện xét duyệt hồ sơ.");

            if (job.EmployerID != employer.EmployerID) throw new Exception("Bạn không có quyền xét duyệt hồ sơ này");

            if (!IsValidStatusTransition(application.Status, applyDto.Status))
                throw new Exception("Trạng thái xét duyệt không hợp lệ.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.GenericRepository<Application>().Update(application);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                ResponseApplyDTO res = new()
                {
                    STT = application.ApplicationID,
                    StudentName = Astudent.UserName,
                    JobName = job.Title,
                    Status = application.Status,
                    ResumeID = application.ResumeID,
                    Coverletter = application.Coverletter,
                    AppliedAt = application.AppliedAt
                };
                return res;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ResponseApplyDTO> GetApplicationByIdAsync(int applyId, string userId)
        {
            var application = await _unitOfWork.GenericRepository<Application>().GetFirstOrDefaultAsync(a => a.ApplicationID == applyId);
            if (application == null) throw new Exception("Hồ sơ không tồn tại");

            var employer = await _unitOfWork.GenericRepository<Employer>().GetFirstOrDefaultAsync(a => a.UserID == userId);
            var student = await _unitOfWork.GenericRepository<Student>().GetFirstOrDefaultAsync(a => a.UserID == userId);
            var Astudent = await _unitOfWork.GenericRepository<ApplicationUser>().GetFirstOrDefaultAsync(a => a.Id == student.UserID);
            var currentUser = await _unitOfWork.GenericRepository<LoginResponseDTO>().GetFirstOrDefaultAsync(a => a.Role.Contains("Admin"));
            var job = await _unitOfWork.GenericRepository<Job>().GetFirstOrDefaultAsync(a => a.JobID == application.JobID);


            bool isStudentOwner = student != null && student.StudentID == application.StudentID;
            bool isEmployerOwner = employer != null && job.EmployerID == employer.EmployerID;
            bool isAdmin = currentUser != null && currentUser.Role.Contains("Admin");

            if (isStudentOwner || isEmployerOwner || isAdmin)
            {
                ResponseApplyDTO res = new()
                {
                    STT = application.ApplicationID,
                    StudentName = Astudent.UserName,
                    JobName = job.Title,
                    Status = application.Status,
                    ResumeID = application.ResumeID,
                    Coverletter = application.Coverletter,
                    AppliedAt = application.AppliedAt
                };
                return res;
            }
            else
            {
                throw new Exception("Hồ sơ không tồn tại.");
            }
        }

        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            return currentStatus switch
            {
                "PENDING" => newStatus == "REJECTED" || newStatus == "APPROVED",
                "APPROVED" => newStatus == "FINISHED",
                _ => false,
            };
        }


    }
}
