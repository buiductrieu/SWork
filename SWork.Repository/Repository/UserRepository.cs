using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWork.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWork.Data.Models;
using SWork.RepositoryContract.Interfaces;

namespace SWork.Repository.Repository
{

    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly SWorkDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(SWorkDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> ExistsProfile(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            return await GetAsync(filter: b => b.Id.Equals(id)) ?? throw new KeyNotFoundException("User is not found");
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            return user == null ? false : true;
        }

        public async Task<ApplicationUser> UpdProfile(ApplicationUser usr)
        {
            return await UpdateAsync(usr);
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return false;
            }
            return true;

        }
    }
}
