namespace SWork.Repository.Repository
{
    public class SubscriptionRepository(SWorkDbContext context) : GenericRepository<Subscription>(context), ISubscriptionRepository
    {
    }
}
