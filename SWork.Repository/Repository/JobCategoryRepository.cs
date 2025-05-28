
namespace SWork.Repository.Repository
{
    public class JobCategoryRepository(SWorkDbContext context) : GenericRepository<JobCategory>(context), IJobCategoryRepository
    {
    }
}
