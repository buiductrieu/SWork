using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWork.Data.Entities;

namespace SWork.RepositoryContract.Interfaces
{
    public interface IUserRepository
    {
        public Task<ApplicationUser> UpdProfile(ApplicationUser usr);
        public Task<ApplicationUser>? ExistsProfile(string id);
        public Task<ApplicationUser> GetUserByIdAsync(string id);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        public Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    }
}
