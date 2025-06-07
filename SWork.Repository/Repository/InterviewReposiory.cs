

namespace SWork.Repository.Repository
{
    public class InterviewReposiory(SWorkDbContext context) : GenericRepository<Interview>(context), IInterviewRepository
    {
    }
}
