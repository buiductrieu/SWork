
using SWork.RepositoryContract.Basic;
using SWork.RepositoryContract.Interfaces;

namespace SWork.RepositoryContract.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GenericRepository<T>() where T : class;
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
