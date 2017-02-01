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
    public class RoleStoreService : IRoleStore<Role>
    {
        CHMEntities context = new CHMEntities();
        public Task CreateAsync(Role role) 
        {
            role.ID = Guid.NewGuid();
            context.Roles.Add(role);
            return context.SaveChangesAsync();
        }

        public Task DeleteAsync(Role role)
        {
            context.Roles.Remove(role);
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }

        public Task<Role> FindByIdAsync(string roleId)
        {
            Task<Role> task = context.Roles.Where(obj => obj.ID == new Guid(roleId)).FirstOrDefaultAsync();
            return task;
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            Task<Role> task = context.Roles.Where(obj => obj.Name == roleName).FirstOrDefaultAsync();
            return task;
        }

        public Task UpdateAsync(Role role)
        {
            context.Roles.Attach(role);
            context.Entry(role).State = EntityState.Modified;
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}