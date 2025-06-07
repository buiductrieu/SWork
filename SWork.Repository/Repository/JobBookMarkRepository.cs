

namespace SWork.Repository.Repository
{
    public class JobBookMarkRepository(SWorkDbContext context) : GenericRepository<JobBookmark>(context), IJobBookMarkRepository
    {
    }
}
