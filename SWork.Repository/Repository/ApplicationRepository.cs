
namespace SWork.Repository.Repository
{
    public class ApplicationRepository(SWorkDbContext context) : GenericRepository<Application>(context), IApplicationRepository
    {
    }
}
