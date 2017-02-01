using CHM.Services.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CHM.Services.Authentication
{
    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private CHMEntities context;
 
        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<User>(new UserStoreService()) { PasswordHasher = new CustomPasswordHasher() };
            _roleManager = new ApplicationRoleManager(new RoleStoreService());
            context = new CHMEntities();
        }

        public async Task<IdentityResult> RegisterUser(User userModel)
        {
            //User user = new User
            //{
            //    UserName = userModel.UserName
            //};

            var result = await _userManager.CreateAsync(userModel, userModel.Password);

            return result;
        }

        public async Task<User> FindUser(string userName, string password)
        {
            User user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<User> FindUserByEmailAndPassword(string email, string password)
        {
            var objUser = context.Users.Include("Users_Roles").Include("Users_Organizations").Include("Users_Roles.Role").Where(obj => obj.Email.Equals(email) && obj.Password.Equals(password)).FirstOrDefault();

            if (objUser != null)
            {
                DateTime? oldLogin = objUser.LastLogin;

                var LastLogin = Convert.ToDateTime(DateTime.Now);
                objUser.LastLogin = LastLogin.ToUniversalTime();

                context.Users.Attach(objUser);
                context.Entry(objUser).State = System.Data.Entity.EntityState.Modified;
                await context.SaveChangesAsync();

                objUser.LastLogin = oldLogin;

                return objUser;
            }

            return null;
        }

        public async Task<IdentityResult> AddRole(Role roleModel)
        {
            Role role = new Role
            {
                Name = roleModel.Name
            };

            var result = await _roleManager.CreateAsync(role);

            return result;
        }

        public async Task<Role> FindRole(string roleName)
        {
            Role role = await _roleManager.FindByNameAsync(roleName);

            return role;
        }

        public async Task<IdentityResult> DeleteRole(Role role)
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);
            return result;
        }

        public async Task<IdentityResult> AddUserToRole(string userName, string roleName)
        {
            User user = _userManager.Users.Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
           var result = await _userManager.AddToRoleAsync(user.Id, roleName);
           return result;
        }

        public IList<string> GetUserRoles(User user)
        {
            IList<string> roles =  _userManager.GetRoles(Convert.ToString(user.ID));
            return roles;

        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}