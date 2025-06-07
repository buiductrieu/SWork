

namespace SWork.Repository.Repository
{
    public class EmployerRepository(SWorkDbContext context) : GenericRepository<Employer>(context), IEmployerRepository
    {
    }
}
