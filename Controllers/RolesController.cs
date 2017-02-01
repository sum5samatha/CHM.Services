using CHM.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using CHM.Services.Authentication;
using System.Web.Http.Cors;


namespace CHM.Services.Controllers
{
    //[Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Roles")]
    public class RolesController : ApiController
    {
        private CHMEntities db = new CHMEntities();
        
       [Route("GetActiveRoles")]
        public async Task<IHttpActionResult> GetActiveRoles()
        {
            List<Role> lstRoles = await db.Roles.Where(obj => obj.IsActive == true).ToListAsync<Role>();

            return Ok(lstRoles);
        }
    }
}
