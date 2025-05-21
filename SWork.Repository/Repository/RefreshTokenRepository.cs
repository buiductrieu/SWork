using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWork.Data.Entities;
using SWork.Data.Models;
using SWork.RepositoryContract.Interfaces;

namespace SWork.Repository.Repository
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly SWorkDbContext _context;
        public RefreshTokenRepository(SWorkDbContext db) : base(db)
        {
            _context = db;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
        }


    }
}
