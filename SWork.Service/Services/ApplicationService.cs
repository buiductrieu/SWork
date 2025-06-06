using Microsoft.AspNetCore.Identity;
using SWork.Data.DTO.ApplicationDTO;

namespace SWork.Service.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
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
            apply.Status = "PENDING";
            apply.AppliedAt = DateTime.Now;
            apply.UpdateAt = null;

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
            application.Status = applyDto.Status;
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
            // get application
            var application = await _unitOfWork.GenericRepository<Application>().GetFirstOrDefaultAsync(a => a.ApplicationID == applyId);
            if (application == null) throw new Exception("Hồ sơ không tồn tại");

            // get job
            var job = await _unitOfWork.GenericRepository<Job>().GetFirstOrDefaultAsync(a => a.JobID == application.JobID);

            // get student
            var student1 = await _unitOfWork.GenericRepository<Student>().GetFirstOrDefaultAsync(a => a.StudentID == application.StudentID);            // get student from application
            var student2 = await _unitOfWork.GenericRepository<ApplicationUser>().GetFirstOrDefaultAsync(a => a.Id == student1.UserID);  //get student from applicationUser

            //get employer
            var employer = await _unitOfWork.GenericRepository<Employer>().GetFirstOrDefaultAsync(a => a.EmployerID == job.EmployerID);

            // get role admin
            var currentUser = await _unitOfWork.GenericRepository<ApplicationUser>().GetFirstOrDefaultAsync(a => a.Id == userId);
            var userRole = await _userManager.GetRolesAsync(currentUser);

            // check valid application
            bool isStudentOwner = student1 != null;
            bool isEmployerOwner = employer != null;
            bool isAdmin = userRole.Contains("Admin");



            if (isStudentOwner || isEmployerOwner || isAdmin)
            {
                ResponseApplyDTO res = new()
                {
                    STT = application.ApplicationID,
                    StudentName = student2.UserName,
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

        public async Task<Pagination<Application>> GetPaginatedApplicationAsync(
             int pageIndex,
             int pageSize,
             Expression<Func<Application, bool>>? predicate = null,
             Expression<Func<Application, object>>? orderBy = null,
             bool isDescending = false)
        {
            try
            {
                var result = await _unitOfWork.GenericRepository<Application>().GetPaginationAsync(
                    predicate = predicate,
                    includeProperties: null,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    orderBy: orderBy ?? (p => p.ApplicationID),
                    isDescending: isDescending);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            return currentStatus switch
            {
                "PENDING" => newStatus == "REJECTED" || newStatus == "APPROVED",
                "APPROVED" => newStatus == "WORKING" || newStatus == "REJECTED",
                "WORKING" => newStatus == "FINISHED",
                _ => false,
            };
        }

    }
}
