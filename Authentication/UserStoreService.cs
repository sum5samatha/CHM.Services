using CHM.Services.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace CHM.Services.Authentication
{
    public class UserStoreService : IUserStore<User>, IUserRoleStore<User>, IUserPasswordStore<User>
    {
        CHMEntities context = new CHMEntities();

        public Task CreateAsync(User user)
        {
            user.ID = Guid.NewGuid();
            context.Users.Add(user);
            return context.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            context.Users.Remove(user);
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            Task<User> task = context.Users.Include(obj => obj.Users_Roles).Where(obj => obj.ID == new Guid(userId)).FirstOrDefaultAsync();
            return task;
        }

        public Task<User> FindByNameAsync(string userName)
        {
            Task<User> task = context.Users.Include(obj => obj.Users_Roles).Where(obj => obj.UserName == userName).FirstOrDefaultAsync();
            return task;
        }

        public Task<User> FindByEmailAsync(string email)
        {
            Task<User> task = context.Users.Include(obj => obj.Users_Roles).Where(obj => obj.Email == email).FirstOrDefaultAsync();
            return task;
        }

        public Task UpdateAsync(User user)
        {
            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }



        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(user.Password != null);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            var userRole = context.Roles.SingleOrDefault(r => r.Name == roleName);

            if (userRole == null)
            {
                throw new InvalidOperationException(string.Format("Role {0} does not exist.", new object[] { roleName }));
            }

            Users_Roles obj = new Users_Roles();
            obj.Role = userRole;
            obj.User = user;

            user.Users_Roles.Add(obj);
            return Task.FromResult(0);


        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<string>>(user.Users_Roles.Join(context.Roles, ur => ur.RoleID, r => r.ID, (ur, r) => r.Name).ToList());
        
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            return Task.FromResult(context.Roles.Any(r => r.Name == roleName && r.Users_Roles.Any(u => u.UserID.Equals(user.ID))));
        
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "roleName");
            }

            var userRole = user.Users_Roles.SingleOrDefault(r => r.Role.Name == roleName);

            if (userRole != null)
            {
                user.Users_Roles.Remove(userRole);
            }

            return Task.FromResult(0);
        
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}