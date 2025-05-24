

namespace SWork.Repository.Repository
{
    public class JobRepository(SWorkDbContext context) : GenericRepository<Job>(context), IJobRepository
    {
    }
}
