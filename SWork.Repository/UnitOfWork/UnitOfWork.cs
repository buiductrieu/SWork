using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using SWork.RepositoryContract.Basic;
using SWork.RepositoryContract.IUnitOfWork;

namespace SWork.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SWorkDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private bool disposed = false;
        private IDbContextTransaction? _transaction;
        public IUserRepository UserRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public UserManager<ApplicationUser> UserManager { get; }

        public UnitOfWork(SWorkDbContext context, IUserRepository userRepository, UserManager<ApplicationUser> userManager, IRefreshTokenRepository refreshTokenRepository)
        {
            _context = context;
            UserRepository = userRepository;
            UserManager = userManager;
            RefreshTokenRepository = refreshTokenRepository;
        }

        // Implement repository properties here

        public void Save()
        {
            _context.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IGenericRepository<T>)repository;
            }

            var newRepository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), newRepository);
            return newRepository;
        }

        public IJobCategoryRepository JobCategoryRepository { get; }
        public IJobRepository JobRepository { get; }
        public ISubscriptionRepository SubscriptionRepository { get; }

        public Task<int> SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
        public async Task BeginTransactionAsync() //Ensure multiple operations complete simultaneously or rollback completely if there is an error
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync() //End the transaction and commit the changes.
        {
            if(_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync() //Restore the state before the transaction if there is an error.
        {
            if( _transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }

      
    }
}
