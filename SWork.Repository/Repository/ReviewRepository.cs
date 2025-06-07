

namespace SWork.Repository.Repository
{
    public class ReviewRepository(SWorkDbContext context) : GenericRepository<Review>(context), IReviewRepository
    {
    }
}
