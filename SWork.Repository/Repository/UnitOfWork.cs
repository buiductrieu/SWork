using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SWork.Data.Entities;
using SWork.Data.Models;
using SWork.RepositoryContract.Interfaces;

namespace SWork.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SWorkDbContext _context;

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
        private bool disposed = false;
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
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
