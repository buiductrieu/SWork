

namespace SWork.RepositoryContract.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
    }
}
