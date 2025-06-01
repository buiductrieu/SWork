

namespace SWork.Repository.Repository;

public class ResumeRepository(SWorkDbContext context) : GenericRepository<Resume>(context), IResumeRepository
{
}
