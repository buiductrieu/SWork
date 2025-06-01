
namespace SWork.Repository.Repository;

public class TemplateResumeRepository(SWorkDbContext context) : GenericRepository<ResumeTemplate>(context), ITemplateResumeRepository
{
}
