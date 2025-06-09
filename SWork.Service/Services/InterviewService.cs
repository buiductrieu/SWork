
using SWork.Data.DTO.InterviewDTO;

namespace SWork.Service.Services
{
    public class InterviewService : IInterViewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InterviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<InterviewDTO> GetInterviewByIdAsync(int interId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<InterviewDTO>> GetInterviewJobForStudent(string userId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<InterviewDTO>> GetInterviewRelatedApplicationForEmployer(string userId, int interId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Pagination<Interview>> GetPaginatedInterviewAsync(int pageIndex, int pageSize, Expression<Func<Interview, bool>>? predicate = null, Expression<Func<Interview, object>>? orderBy = null, bool isDescending = false)
        {
            throw new NotImplementedException();
        }

        public async Task<InterviewDTO> UpdateInterviewAsync(UpdateInterviewDTO dto, string userId)
        {
            var employer = await _unitOfWork.GenericRepository<Employer>().GetFirstOrDefaultAsync(e => e.UserID == userId);
            var interview = await _unitOfWork.GenericRepository<Interview>().GetFirstOrDefaultAsync(i => i.InterviewID == dto.InterviewID);

            if (interview == null || interview.SenderID != employer.EmployerID)
                throw new Exception("Bạn không có quyền cập nhật lịch phỏng vấn này.");
            if (interview.Status == "REJECTED") throw new Exception("Bạn không thể cập nhật lịch phỏng vấn này.");

            interview.ScheduledTime = dto.ScheduledTime;
            interview.Duration_minutes = dto.Duration_minutes;
            interview.MeetingLink = dto.MeetingLink;
            interview.Location = dto.Location;
            interview.Note = dto.Note;
            interview.Status = dto.Status;

            _unitOfWork.GenericRepository<Interview>().Update(interview);
            await _unitOfWork.SaveChangeAsync();

            InterviewDTO res = new()
            {
                ScheduledTime = interview.ScheduledTime,
                Duration_minutes = interview.Duration_minutes,
                Location = interview.Location,
                MeetingLink = interview.MeetingLink,
                Note = interview.Note,
                Status = interview.Status
            };
            return res;
        }
    }
}
