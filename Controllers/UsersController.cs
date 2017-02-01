using CHM.Services.Authentication;
using CHM.Services.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Mail;
using System.Configuration;
using System.Text;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace CHM.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private AuthRepository _repo = null;
        private CHMEntities db = new CHMEntities();

        public UsersController() 
        {
            _repo = new AuthRepository();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        [Route("GetAllActiveUsers")]
        public async Task<IHttpActionResult> GetAllActiveUsers()
        {
            List<User> lstUsers = await db.Users.Include(obj=>obj.Users_Roles.Select(o=>o.Role)).Include(obj=>obj.Users_Organizations).Where(obj => obj.IsActive == true).ToListAsync<User>();

            foreach (User objuser in lstUsers)
            {
                objuser.Actions = null;
                objuser.Actions_Days = null;
                objuser.Intervention_Question = null;
                objuser.Intervention_Question_Answer = null;
                objuser.Interventions = null;
                objuser.Residents = null;
                objuser.Question_ParentQuestion = null;
                objuser.Residents_Relatives = null;
                objuser.Section_Intervention = null;
                objuser.Section_Summary = null;
                objuser.Sections_Questions_Answers_Tasks = null;
                objuser.Residents_Questions_Answers = null;
                objuser.Resident_Interventions_Questions_Answers = null;
                objuser.Intervention_Question_Answer_Task = null;
                objuser.Interventions_Question_Answer_Summary = null;
                objuser.Sections_Questions = null;
                objuser.Sections_Questions_Answers = null;
                objuser.Users_Roles = objuser.Users_Roles.Where(obj => obj.IsActive==true).ToList<CHM.Services.Models.Users_Roles>();
               
                foreach(Users_Roles objUserRoles in objuser.Users_Roles)
                {
                  objUserRoles.User=null;
                }
            }
      
            return Ok(lstUsers);
        }

        [HttpGet]
        [Route("GetAllActiveUsersByOrganizationId")]
        public async Task<IHttpActionResult> GetAllActiveUsersByOrganizationId(Guid OrganizationID)
        {
            List<User> lstUsers = await db.Users.Include(obj => obj.Users_Roles.Select(o => o.Role)).Include(obj => obj.Residents_Relatives2).Where(obj => obj.IsActive == true).ToListAsync<User>();

            foreach (User objuser in lstUsers)
            {

                objuser.Users_Roles = objuser.Users_Roles.Where(obj => obj.IsActive).ToList<CHM.Services.Models.Users_Roles>();

                foreach (Users_Roles objUserRoles in objuser.Users_Roles)
                {
                    objUserRoles.User = null;
                }
            }
            List<Users_Organizations> lstUserOrganization = db.Users_Organizations.Where(i => i.IsActive == true && i.OrganizationID == OrganizationID).ToList<Users_Organizations>();

            List<User> lstUserByOrganization = new List<User>();
            foreach (User objUser in lstUsers)
            {
                foreach(Users_Organizations objUserOrganization in lstUserOrganization)
                {
                    if(objUserOrganization.UserID==objUser.ID && objUserOrganization.OrganizationID==OrganizationID)
                    {
                        lstUserByOrganization.Add(objUser);
                    }

                }
            }

            return Ok(lstUserByOrganization);
        }
    
        [Route("GetUser")]
        public async Task<IHttpActionResult> GetUsers(Guid UserID)
        {
            User objUsers = await db.Users.Include(obj => obj.Users_Roles).Include(i=>i.Residents_Relatives2).Where(obj => obj.ID == UserID && obj.IsActive == true).FirstOrDefaultAsync();
            objUsers.Users_Roles = objUsers.Users_Roles.Where(obj => obj.IsActive == true && obj.UserID == UserID).ToList<CHM.Services.Models.Users_Roles>();
           objUsers.Residents_Relatives2 = objUsers.Residents_Relatives2.Where(obj => obj.IsActive == true && obj.UserID == UserID).ToList<CHM.Services.Models.Residents_Relatives>();
            return Ok(objUsers);
        }

        [Route("GetUserOrganizations")]
        public async Task<IHttpActionResult> GetUserOrganizations(Guid UserID)
        {
            User objUsers = await db.Users.Include(obj => obj.UserType).Include(i => i.Users_Organizations).Include(i=>i.UserType).Include(j=>j.Organizations.Select(i=>i.OrganizationGroups_Organizations.Select(k=>k.OrganizationGroup))).Where(obj => obj.ID == UserID && obj.IsActive == true).FirstOrDefaultAsync();
            objUsers.Users_Organizations = objUsers.Users_Organizations.Where(obj => obj.IsActive == true && obj.UserID == UserID).ToList<CHM.Services.Models.Users_Organizations>();
            objUsers.Organizations = objUsers.Organizations.Where(obj => obj.IsActive == true).ToList<CHM.Services.Models.Organization>();
            objUsers.Actions = null;
            objUsers.Actions_Days = null;
            objUsers.Intervention_Question = null;
            objUsers.Intervention_Question_Answer = null;
            objUsers.Interventions = null;
            objUsers.Residents = null;
            objUsers.Question_ParentQuestion = null;
            objUsers.Residents_Relatives = null;
            objUsers.Section_Intervention = null;
            objUsers.Section_Summary = null;
            objUsers.Sections_Questions_Answers_Tasks = null;
            objUsers.Residents_Questions_Answers = null;
            objUsers.Resident_Interventions_Questions_Answers = null;
            objUsers.Intervention_Question_Answer_Task = null;
            objUsers.Interventions_Question_Answer_Summary = null;
            objUsers.Sections_Questions = null;
            objUsers.Sections_Questions_Answers = null;
            return Ok(objUsers);
        }

        [Route("GetUserOrganizationByUserID")]
        public async Task<IHttpActionResult> GetUserOrganizationByUserID(Guid UserID)
        {
            var Users_Organizations = await db.Users_Organizations.Where(i => i.UserID == UserID && i.IsActive == true).FirstOrDefaultAsync();
            return Ok(Users_Organizations);
        }

        [HttpGet]
        [Route("GetOrganization")]
        public async Task<IHttpActionResult> GetOrganization()
        {
            List<Organization> lstOrganization = db.Organizations.Where(i => i.IsActive == true).Include(i=>i.OrganizationGroups_Organizations.Select(k=>k.OrganizationGroup)).ToList<Organization>();
            return Ok(lstOrganization);
        }
        [HttpGet]
        [Route("GetOrganizationWRTUserID")]
        public async Task<IHttpActionResult> GetOrganizationWRTUserID(Guid UserID)
        {
            var  objUsersOrganization= db.Users_Organizations.Where(i => i.IsActive == true && i.UserID == UserID).FirstOrDefault();
            return Ok(objUsersOrganization);
        }

        [HttpPost]
        [Route("GetUserName")]
        public async Task<IHttpActionResult> GetUserName(string Username)
        {

            User objUsers = await db.Users.Where(obj => obj.UserName == Username && obj.IsActive == true).FirstOrDefaultAsync();
            if (objUsers != null)
            {


                string smtpAddress = ConfigurationManager.AppSettings["SmtpAddress"];
                int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                bool enableSSL = true;

                string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                string password = ConfigurationManager.AppSettings["Password"];
                string emailTo = objUsers.Email;
                string subject = "Forgot password details";

                StringBuilder sbMailBody = new StringBuilder();

                // Start - Generate the body of the email.

                sbMailBody.Append("<html>");
                sbMailBody.Append("<body>");
       
                //sbMailBody.Append("<a href = '" + objUsers.UserName + "'>Click here to Change the Password</a>");
                sbMailBody.Append("<b>Your UserName Is : </b>" + objUsers.UserName);
                sbMailBody.Append("<br/><b>Your Password Is : </b>" + objUsers.Password);
                sbMailBody.Append("</body>");
                sbMailBody.Append("</html>");

           

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = sbMailBody.ToString();
                    mail.IsBodyHtml = true;
                  

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }
               

            }
          
     

            return Ok(objUsers);
        }

        [HttpPost]
        [Route("GetUserEmail")]
        public async Task<IHttpActionResult> GetUserEmail(string UserEmail)
        {

            User objUsers = await db.Users.Where(obj => obj.Email == UserEmail && obj.IsActive == true).FirstOrDefaultAsync();
            if (objUsers != null)
            {


                string smtpAddress = ConfigurationManager.AppSettings["SmtpAddress"];
                int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                bool enableSSL = false;

                string emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                string userName = ConfigurationManager.AppSettings["userName"];
                string password = ConfigurationManager.AppSettings["Password"];
                string emailTo = objUsers.Email;
                string subject = "Forgot password details";

                StringBuilder sbMailBody = new StringBuilder();

                // Start - Generate the body of the email.

                sbMailBody.Append("<html>");
                sbMailBody.Append("<body>");

                //sbMailBody.Append("<a href = '" + objUsers.UserName + "'>Click here to Change the Password</a>");
                sbMailBody.Append("<b>Your UserName Is : </b>" + objUsers.UserName);
                sbMailBody.Append("<br/><b>Your Password Is : </b>" + objUsers.Password);
                sbMailBody.Append("</body>");
                sbMailBody.Append("</html>");



                using (MailMessage mail = new MailMessage())
                {
                   
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = sbMailBody.ToString();
                    mail.IsBodyHtml = true;

                    try
                    {
                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            System.Net.NetworkCredential Credentials = new System.Net.NetworkCredential(userName, password);
                            smtp.Credentials = Credentials;
                            smtp.EnableSsl = enableSSL;
                            smtp.Send(mail);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    
                }


            }



            return Ok(objUsers);
        }

        [HttpGet]
        [Route("ViewUser")]
        public async Task<IHttpActionResult> ViewUser(Guid UserID)
        {
            User objUsers = await db.Users.Include(obj => obj.Users_Roles.Select(o=>o.Role)).Include(i => i.Residents_Relatives2.Select(o=>o.Resident)).Where(obj => obj.ID == UserID && obj.IsActive == true).FirstOrDefaultAsync();

            objUsers.Users_Roles = objUsers.Users_Roles.Where(obj => obj.IsActive==true && obj.UserID==UserID).ToList<CHM.Services.Models.Users_Roles>();
            objUsers.Residents_Relatives2 = objUsers.Residents_Relatives2.Where(obj => obj.IsActive == true && obj.UserID == UserID).ToList<CHM.Services.Models.Residents_Relatives>();

            return Ok(objUsers);
        }

        //[HttpGet]
        //[Route("CheckOldPassword")]
        //public async Task<IHttpActionResult> CheckOldPassword(Guid UserID)
        //{
        //    //Guid UserID = new Guid(UserID);
        //    //Guid UserID = new Guid(User.Identity.GetUserId());
        //    User objUser = await db.Users.Where(o => o.ID == UserID && o.IsActive == true).FirstOrDefaultAsync();
        //    return Ok(objUser);
        //}

        [HttpPost]
        [Route("UpdateUserDetails")]
        public async Task<IHttpActionResult> UpdateUserDetails(User objUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid userId = new Guid(User.Identity.GetUserId());
          
            objUser.ModifiedBy = userId;
            objUser.Modified = DateTime.Now;


            db.Entry(objUser).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                throw ex;
            }

            return Ok();
        }

       [HttpPost]
        [Route("SaveUser")]
        public async Task<IHttpActionResult>SaveUser(User objUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid userId = new Guid(User.Identity.GetUserId());

            objUser.ID = Guid.NewGuid();
            objUser.IsActive = true;
           
            objUser.CreatedBy = userId;
            objUser.ModifiedBy = userId;
            objUser.Created = objUser.Modified = DateTime.Now;


            foreach (Users_Roles objUser_Role in objUser.Users_Roles)
            {
                objUser_Role.ID = Guid.NewGuid();
                objUser_Role.IsActive = true;
            }

            foreach (Users_Organizations objUsers_Organizations in objUser.Users_Organizations)
            {
                objUsers_Organizations.ID = Guid.NewGuid();
                objUsers_Organizations.UserID = objUser.ID;
                objUsers_Organizations.IsActive = true;
                objUsers_Organizations.Created = DateTime.Now;
                objUsers_Organizations.CreatedBy = userId;
                objUsers_Organizations.Modified = DateTime.Now;
                objUsers_Organizations.ModifiedBy = userId;
            }
           
            foreach (Residents_Relatives objResidents_Relatives in objUser.Residents_Relatives2)
            {

                objResidents_Relatives.ID = Guid.NewGuid();
                objResidents_Relatives.IsActive = true;
                objResidents_Relatives.CreatedBy = userId;
                objResidents_Relatives.ModifiedBy = userId;
                objResidents_Relatives.Created = objResidents_Relatives.Modified = DateTime.Now;
            }




            db.Users.Add(objUser);
           

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
              
                throw ex;
            }

            return Ok(objUser.ID);
        }

       [HttpPost]
       [Route("UpdateUser")]
       public async Task<IHttpActionResult> UpdateUser(User objUser)
       {
           //if (!ModelState.IsValid)
           //{
           //    return BadRequest(ModelState);
           //}

           CHMEntities dbCHM1 = new CHMEntities();
           Guid userId = new Guid(User.Identity.GetUserId());

           
           objUser.ModifiedBy = userId;
           objUser.Modified = DateTime.Now;
        



           foreach (Users_Roles objUserRole in objUser.Users_Roles)
           {
               List<Users_Roles> lstUsers_Roles = new List<Users_Roles>();
               lstUsers_Roles = dbCHM1.Users_Roles.Where(i => i.UserID == objUserRole.UserID).ToList<Users_Roles>();
               //Users_Roles objUser_Role = dbCHM1.Users_Roles.Find(objUserRole.ID);
               if (lstUsers_Roles != null)
               {
                   foreach (Users_Roles obj in lstUsers_Roles)
                   {
                       obj.IsActive = false;
                       dbCHM1.Entry(obj).State = EntityState.Modified;
                       dbCHM1.SaveChanges();
                   }

               }
                   Users_Roles objUsers_Roles = new Users_Roles();
                   objUsers_Roles.ID = Guid.NewGuid();
                   objUsers_Roles.UserID = objUserRole.UserID;
                   objUsers_Roles.RoleID = objUserRole.RoleID;
                   objUsers_Roles.IsActive = true;
                   dbCHM1.Users_Roles.Add(objUsers_Roles);
                   dbCHM1.SaveChanges();
                   
                   //dbCHM1.Entry(objUser_Role).State = EntityState.Modified;
               
           }

           foreach (Residents_Relatives objResidents_Relatives in objUser.Residents_Relatives2)
           {
              List<Residents_Relatives> objResidentsRelatives=new List<Residents_Relatives>();
              objResidentsRelatives = dbCHM1.Residents_Relatives.Where(i => i.UserID == objResidents_Relatives.UserID).ToList<Residents_Relatives>();
              if (objResidentsRelatives != null)
              {
                  foreach (Residents_Relatives obj in objResidentsRelatives)
                  {
                      obj.IsActive = false;
                      obj.ModifiedBy = userId;
                      obj.Modified = DateTime.Now;
                      dbCHM1.Entry(obj).State = EntityState.Modified;
                      dbCHM1.SaveChanges();
                  }
                 
              }


                  Residents_Relatives objNewResidentRelatives = new Residents_Relatives();
                  objNewResidentRelatives.ID = Guid.NewGuid();
                  objNewResidentRelatives.UserID = objUser.ID;
                  objNewResidentRelatives.ResidentID = objResidents_Relatives.ResidentID;
                  objNewResidentRelatives.IsActive = true;
                  objNewResidentRelatives.CreatedBy = userId;
                  objNewResidentRelatives.ModifiedBy = userId;
                  objNewResidentRelatives.Created = DateTime.Now;
                  objNewResidentRelatives.Modified = DateTime.Now;
                  dbCHM1.Residents_Relatives.Add(objNewResidentRelatives);
                  dbCHM1.SaveChanges();

                 
           }
           objUser.Users_Roles = null;
           objUser.Residents_Relatives = null;
           objUser.Residents_Relatives1 = null;
           objUser.Residents_Relatives2 = null;
           
           dbCHM1.Entry(objUser).State = EntityState.Modified;

           try
            {
                await dbCHM1.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
              
                throw ex;
            }

            return Ok();
        
       }


       [HttpPost]
       [Route("DeleteUser")]
       public async Task<IHttpActionResult> DeleteUser(User objUser)
       {

           CHMEntities dbCHM = new CHMEntities();

           //if (!ModelState.IsValid)
           //{
           //    return BadRequest(ModelState);
           //}
          // Guid userId = new Guid(User.Identity.GetUserId());

           User  objDeleteUser = dbCHM.Users.Find(objUser.ID);

           objDeleteUser.IsActive = false;
           objDeleteUser.Modified = DateTime.Now;


           foreach (Users_Roles objUser_Role in objUser.Users_Roles)
           {
               Users_Roles objDeleteUserRoles = dbCHM.Users_Roles.Find(objUser_Role.ID);
               objDeleteUserRoles.IsActive = false;             
               dbCHM.Entry(objDeleteUserRoles).State = EntityState.Modified;
           }

           foreach (Users_Organizations objUserOrganization  in objUser.Users_Organizations)
           {
                Users_Organizations objDeleteUsers_Organizations= dbCHM.Users_Organizations.Find(objUserOrganization.ID);
               objDeleteUsers_Organizations.IsActive = false;
               objDeleteUsers_Organizations.ModifiedBy = objUser.ID;
               objDeleteUsers_Organizations.Modified = DateTime.Now;
               dbCHM.Entry(objDeleteUsers_Organizations).State = EntityState.Modified;
           }

           foreach (Residents_Relatives objResidents_Relatives in objUser.Residents_Relatives2)
           {
               Residents_Relatives objDeleteResidents_Relatives = dbCHM.Residents_Relatives.Find(objResidents_Relatives.ID);
               objDeleteResidents_Relatives.IsActive = false;
               objDeleteResidents_Relatives.ModifiedBy = objUser.ID;
               objDeleteResidents_Relatives.Modified = DateTime.Now;
               dbCHM.Entry(objDeleteResidents_Relatives).State = EntityState.Modified;
           }
           dbCHM.Entry(objDeleteUser).State = EntityState.Modified;
           try
           {
               await dbCHM.SaveChangesAsync();
           }
           catch (DbUpdateException ex)
           {

               throw ex;
           }

           return Ok();
        
       }

       [HttpPost]
       [Route("DeleteUserOrganizations")]
       public async Task<IHttpActionResult> DeleteUserOrganizations(User objUser)
       {

           CHMEntities dbCHM = new CHMEntities();

           //if (!ModelState.IsValid)
           //{
           //    return BadRequest(ModelState);
           //}
           // Guid userId = new Guid(User.Identity.GetUserId());

           User objDeleteUser = dbCHM.Users.Find(objUser.ID);

           objDeleteUser.IsActive = false;
           objDeleteUser.Modified = DateTime.Now;

           List<Users_Roles> lstUserRoles=db.Users_Roles.Where(i=>i.UserID==objUser.ID && i.IsActive==true).ToList<Users_Roles>();

           foreach (Users_Roles objUser_Role in lstUserRoles)
           {
             
               objUser_Role.IsActive = false;
               dbCHM.Entry(objUser_Role).State = EntityState.Modified;
           }

           List<Users_Organizations> lstUsers_Organizations = db.Users_Organizations.Where(i => i.UserID == objUser.ID && i.IsActive == true).ToList<Users_Organizations>();
           foreach (Users_Organizations objUserOrganization in lstUsers_Organizations)
           {

               objUserOrganization.IsActive = false;
               objUserOrganization.ModifiedBy = objUser.ID;
               objUserOrganization.Modified = DateTime.Now;
               dbCHM.Entry(objUserOrganization).State = EntityState.Modified;
           }
           List<Residents_Relatives> lstResidents_Relatives = db.Residents_Relatives.Where(i => i.UserID == objUser.ID && i.IsActive == true).ToList<Residents_Relatives>();
           foreach (Residents_Relatives objResidents_Relatives in lstResidents_Relatives)
           {

               objResidents_Relatives.IsActive = false;
               objResidents_Relatives.ModifiedBy = objUser.ID;
               objResidents_Relatives.Modified = DateTime.Now;
               dbCHM.Entry(objResidents_Relatives).State = EntityState.Modified;
           }
           dbCHM.Entry(objDeleteUser).State = EntityState.Modified;
           try
           {
               await dbCHM.SaveChangesAsync();
           }
           catch (DbUpdateException ex)
           {

               throw ex;
           }

           return Ok();

       }



        // Insertion Master tables

        [Route("GetActiveUsersRoles")]
        public async Task<IHttpActionResult> GetActiveUsersRoles()
        {
            List<Users_Roles> lstUsers_Roles = await db.Users_Roles.Where(obj => obj.IsActive == true).ToListAsync<Users_Roles>();

            return Ok(lstUsers_Roles);
        }


        [Route("GetActiveOrganizations")]
        public async Task<IHttpActionResult> GetActiveOrganizations()
        {
            List<Organization> lstOrganization = await db.Organizations.Where(obj => obj.IsActive == true).ToListAsync<Organization>();

            return Ok(lstOrganization);
        }


        [Route("GetUsersOrganizations")]
        public async Task<IHttpActionResult> GetUsersOrganizations()
        {
            List<Users_Organizations> lstUsersOrganization = await db.Users_Organizations.Where(obj => obj.IsActive == true).ToListAsync<Users_Organizations>();

            return Ok(lstUsersOrganization);
        }

        [Route("GetSections_Questions_Answers_Widget")]
        public async Task<IHttpActionResult> GetSections_Questions_Answers_Widget()
        {
            List<Sections_Questions_Answers_Widget> lstSections_Questions_Answers_Widget = await db.Sections_Questions_Answers_Widget.Where(obj => obj.IsActive == true).ToListAsync<Sections_Questions_Answers_Widget>();

            return Ok(lstSections_Questions_Answers_Widget);
        }

        [Route("GetSections_Questions_Answers_Tasks")]
        public async Task<IHttpActionResult> GetSections_Questions_Answers_Tasks()
        {
            List<Sections_Questions_Answers_Tasks> lstSections_Questions_Answers_Tasks = await db.Sections_Questions_Answers_Tasks.Where(obj => obj.IsActive == true).ToListAsync<Sections_Questions_Answers_Tasks>();

            return Ok(lstSections_Questions_Answers_Tasks);
        }

        [Route("GetSections_Questions_Answers_Summary")]
        public async Task<IHttpActionResult> GetSections_Questions_Answers_Summary()
        {
            List<Sections_Questions_Answers_Summary> lstSections_Questions_Answers_Summary = await db.Sections_Questions_Answers_Summary.Where(obj => obj.IsActive == true).ToListAsync<Sections_Questions_Answers_Summary>();

            return Ok(lstSections_Questions_Answers_Summary);
        }

        [Route("GetSections_Questions_Answers")]
        public async Task<IHttpActionResult> GetSections_Questions_Answers()
        {
            List<Sections_Questions_Answers> lstSections_Questions_Answers = await db.Sections_Questions_Answers.Where(obj => obj.IsActive == true).ToListAsync<Sections_Questions_Answers>();

            return Ok(lstSections_Questions_Answers);
        }

        [Route("GetSections_Questions")]
        public async Task<IHttpActionResult> GetSections_Questions()
        {
            List<Sections_Questions> lstSections_Questions = await db.Sections_Questions.Where(obj => obj.IsActive == true).ToListAsync<Sections_Questions>();

            return Ok(lstSections_Questions);
        }

        [Route("GetSections_Organizations")]
        public async Task<IHttpActionResult> GetSections_Organizations()
        {
            List<Sections_Organizations> lstSections_Organizations = await db.Sections_Organizations.Where(obj => obj.IsActive == true).ToListAsync<Sections_Organizations>();

            return Ok(lstSections_Organizations);
        }

        [Route("GetSection_Summary")]
       public async Task<IHttpActionResult> GetSection_Summary()
        {
        List<Section_Summary> lstSection_Summary = await db.Section_Summary.Where(obj => obj.IsActive == true).ToListAsync<Section_Summary>();

        return Ok(lstSection_Summary);
    }


        [Route("GetSectionInterventionStatements")]
        public async Task<IHttpActionResult> GetSectionInterventionStatements()
        {
            List<Section_Intervention_Statements> lstSection_Intervention_Statements = await db.Section_Intervention_Statements.Where(obj => obj.IsActive == true).ToListAsync<Section_Intervention_Statements>();

            return Ok(lstSection_Intervention_Statements);
        }

        [Route("GetSectionIntervention")]
        public async Task<IHttpActionResult> GetSectionIntervention()
        {
            List<Section_Intervention> lstSection_Intervention = await db.Section_Intervention.Where(obj => obj.IsActive == true).ToListAsync<Section_Intervention>();

            return Ok(lstSection_Intervention);
        }

        [Route("GetOrganizationGroups_Organizations")]
        public async Task<IHttpActionResult> GetOrganizationGroups_Organizations()
        {
            List<OrganizationGroups_Organizations> lstOrganizationGroups_Organizations = await db.OrganizationGroups_Organizations.Where(obj => obj.IsActive == true).ToListAsync<OrganizationGroups_Organizations>();

            return Ok(lstOrganizationGroups_Organizations);
        }


        [Route("GetOrganizationGroups")]
        public async Task<IHttpActionResult> GetOrganizationGroups()
        {
            List<OrganizationGroup> lstOrganizationGroups = await db.OrganizationGroups.Where(obj => obj.IsActive == true).ToListAsync<OrganizationGroup>();

            return Ok(lstOrganizationGroups);
        }

        [Route("GetInterventions_Question_Answer_Summary")]
        public async Task<IHttpActionResult> GetInterventions_Question_Answer_Summary()
        {
            List<Interventions_Question_Answer_Summary> lstInterventions_Question_Answer_Summary = await db.Interventions_Question_Answer_Summary.Where(obj => obj.IsActive == true).ToListAsync<Interventions_Question_Answer_Summary>();

            return Ok(lstInterventions_Question_Answer_Summary);
        }

        [Route("GetInterventions")]
        public async Task<IHttpActionResult> GetInterventions()
        {
            List<Intervention> lstInterventions = await db.Interventions.Where(obj => obj.IsActive == true).ToListAsync<Intervention>();

            return Ok(lstInterventions);
        }


        [Route("GetIntervention_Question_ParentQuestion")]
        public async Task<IHttpActionResult> GetIntervention_Question_ParentQuestion()
        {
            List<Intervention_Question_ParentQuestion> lstIntervention_Question_ParentQuestion = await db.Intervention_Question_ParentQuestion.Where(obj => obj.IsActive == true).ToListAsync<Intervention_Question_ParentQuestion>();

            return Ok(lstIntervention_Question_ParentQuestion);
        }


        [Route("GetIntervention_Question_Answer_Task")]
        public async Task<IHttpActionResult> GetIntervention_Question_Answer_Task()
        {
            List<Intervention_Question_Answer_Task> lstIntervention_Question_Answer_Task = await db.Intervention_Question_Answer_Task.Where(obj => obj.IsActive == true).ToListAsync<Intervention_Question_Answer_Task>();

            return Ok(lstIntervention_Question_Answer_Task);
        }



        [Route("GetIntervention_Question_Answer")]
        public async Task<IHttpActionResult> GetIntervention_Question_Answer()
        {
            List<Intervention_Question_Answer> lstIntervention_Question_Answer = await db.Intervention_Question_Answer.Where(obj => obj.IsActive == true).ToListAsync<Intervention_Question_Answer>();

            return Ok(lstIntervention_Question_Answer);
        }



        [Route("GetIntervention_Question")]
        public async Task<IHttpActionResult> GetIntervention_Question()
        {
            List<Intervention_Question> lstIntervention_Question_Answer = await db.Intervention_Question.Where(obj => obj.IsActive == true).ToListAsync<Intervention_Question>();

            return Ok(lstIntervention_Question_Answer);
        }


        [Route("GetActions")]
        public async Task<IHttpActionResult> GetActions()
        {
            List<CHM.Services.Models.Action> lstGetActions = await db.Actions.Where(obj => obj.IsActive == true).ToListAsync<CHM.Services.Models.Action>();

            return Ok(lstGetActions);
        }



        [Route("GetActions_Days")]
        public async Task<IHttpActionResult> GetActions_Days()
        {
            List<CHM.Services.Models.Actions_Days> lstGetActions_Days = await db.Actions_Days.Where(obj => obj.IsActive == true).ToListAsync<CHM.Services.Models.Actions_Days>();

            return Ok(lstGetActions_Days);
        }

        [Route("GetInterventionsData")]
        public async Task<IHttpActionResult> GetInterventionsData()
        {
            List<Intervention> lstGetInterventions = await db.Interventions.Where(obj => obj.IsActive == true).ToListAsync<Intervention>();

            return Ok(lstGetInterventions);
        }


        [Route("GetInterventions_Resident_Answers")]
        public async Task<IHttpActionResult> GetInterventions_Resident_Answers()
        {
            List<Interventions_Resident_Answers> lstGetInterventions_Resident_Answers = await db.Interventions_Resident_Answers.Where(obj => obj.IsActive == true).ToListAsync<Interventions_Resident_Answers>();

            return Ok(lstGetInterventions_Resident_Answers);
        }


        //[Route("GetResident_Interventions_Questions_Answers")]
        //public async Task<IHttpActionResult> GetResident_Interventions_Questions_Answers()
        //{
        //    List<Resident_Interventions_Questions_Answers> lstGetResident_Interventions_Questions_Answers = await db.Resident_Interventions_Questions_Answers.Where(obj => obj.IsActive == true).ToListAsync<Resident_Interventions_Questions_Answers>();

        //    return Ok(lstGetResident_Interventions_Questions_Answers);
        //}


        [Route("GetResident_Interventions_Questions_Answers")]
        public async Task<IHttpActionResult> GetResident_Interventions_Questions_Answers()
        {
            List<Resident_Interventions_Questions_Answers> lstGetResident_Interventions_Questions_Answers = await db.Resident_Interventions_Questions_Answers.Where(obj => obj.IsActive == true).ToListAsync<Resident_Interventions_Questions_Answers>();

            List<object> lstResidentInterventionsQuestionsAnswers = new List<object>();
            foreach (var item in lstGetResident_Interventions_Questions_Answers)
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers")))
                {
                    if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + item.ID + "")))
                    {
                        string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + item.ID + ""));

                        if (fileEntries.Length > 0)
                        {
                            var objResidentInterventionDoc = new
                            {
                                objResident_Interventions_Questions_Answers = item,
                                Filedata = File.ReadAllBytes(fileEntries[0]),
                                Extention=Path.GetExtension(fileEntries[0]),
                                FileName=Path.GetFileName(fileEntries[0])
                            };
                            lstResidentInterventionsQuestionsAnswers.Add(objResidentInterventionDoc);
                        }
                        else
                        {
                            var objResidentInterventionDoc = new
                            {
                                objResident_Interventions_Questions_Answers = item
                            };
                            lstResidentInterventionsQuestionsAnswers.Add(objResidentInterventionDoc);
                        }

                    }
                    else
                    {
                        var objResidentInterventionDoc = new
                        {
                            objResident_Interventions_Questions_Answers = item
                        };
                        lstResidentInterventionsQuestionsAnswers.Add(objResidentInterventionDoc);
                    }
                }
            }
            return Ok(lstResidentInterventionsQuestionsAnswers);
        }



        [Route("GetResidents")]
        public async Task<IHttpActionResult> GetResidents()
        {
            List<Resident> lstGetResidents = await db.Residents.Where(obj => obj.IsActive == true).ToListAsync<Resident>();

            return Ok(lstGetResidents);
        }


        //[Route("GetResidents_Questions_Answers")]
        //public async Task<IHttpActionResult> GetResidents_Questions_Answers()
        //{
        //    List<Residents_Questions_Answers> lstGetResidents_Questions_Answers = await db.Residents_Questions_Answers.Where(obj => obj.IsActive == true).ToListAsync<Residents_Questions_Answers>();
           
        //    return Ok(lstGetResidents_Questions_Answers);
        //}


        [Route("GetResidents_Questions_Answers")]
        public async Task<IHttpActionResult> GetResidents_Questions_Answers()
        {
            List<Residents_Questions_Answers> lstGetResidents_Questions_Answers = await db.Residents_Questions_Answers.Where(obj => obj.IsActive == true).ToListAsync<Residents_Questions_Answers>();

            List<object> lstResidentAnswersDocuments = new List<object>();
            foreach (var item in lstGetResidents_Questions_Answers)
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers")))
                {
                    if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + item.ID + "")))
                    {
                        string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + item.ID + ""));

                        if (fileEntries.Length > 0)
                        {
                            FileInfo myFileInfo = new FileInfo(fileEntries[0]);
                            var objResidentDoc = new
                            {
                                objResidents_Questions_Answers = item,
                                Filedata = "/Uploads/Residents/Answers/" + item.ID.ToString() + "/" + myFileInfo.Name
                            };
                            lstResidentAnswersDocuments.Add(objResidentDoc);
                        }
                        else
                        {
                            var objResidentDoc = new
                            {
                                objResidents_Questions_Answers = item
                            };
                            lstResidentAnswersDocuments.Add(objResidentDoc);
                        }

                    }
                    else
                    {
                        var objResidentDoc = new
                        {
                            objResidents_Questions_Answers = item
                        };
                        lstResidentAnswersDocuments.Add(objResidentDoc);
                    }
                }
            }
            return Ok(lstResidentAnswersDocuments);
        }


        [Route("GetResidents_Relatives")]
        public async Task<IHttpActionResult> GetResidents_Relatives()
        {
            List<Residents_Relatives> lstGetResidents_Relatives = await db.Residents_Relatives.Where(obj => obj.IsActive == true).ToListAsync<Residents_Relatives>();

            return Ok(lstGetResidents_Relatives);
        }


        [HttpPost]
        [Route("SaveOrganization")]
        public async Task<IHttpActionResult> SaveOrganization(Organization objOrganization)
        {
            objOrganization.ID =Guid.NewGuid();
            objOrganization.IsActive = true;
            objOrganization.Created = DateTime.Now;
            objOrganization.Modified = DateTime.Now;
            foreach(OrganizationGroups_Organizations objOrganizationGroup in objOrganization.OrganizationGroups_Organizations)
            {
                objOrganizationGroup.ID = Guid.NewGuid();
                objOrganizationGroup.IsActive = true;
                objOrganizationGroup.Created = DateTime.Now;
                objOrganizationGroup.Modified = DateTime.Now;
                //objOrganizationGroup.OrganizationID = objOrganization.ID;
            }

            Users_Organizations objUserOrganization = new Users_Organizations();
            objUserOrganization.ID = Guid.NewGuid();
            objUserOrganization.UserID = objOrganization.CreatedBy;
            objUserOrganization.OrganizationID = objOrganization.ID;
            objUserOrganization.Created = DateTime.Now;
            objUserOrganization.CreatedBy = objOrganization.CreatedBy;
            objUserOrganization.Modified = DateTime.Now;
            objUserOrganization.ModifiedBy = objOrganization.ModifiedBy;
            objUserOrganization.IsActive = true;

            
            CHM.Services.Models.Configuration objConfiguration = new CHM.Services.Models.Configuration();
            objConfiguration.ID = Guid.NewGuid();
            objConfiguration.OrganizationID = objOrganization.ID;
            objConfiguration.ConfigurationKey = "GenerateInterventionLimit";
            objConfiguration.ConfigurationValue = "30";

            try
            {
                db.Organizations.Add(objOrganization);
                db.Configurations.Add(objConfiguration);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                
                throw;
            }
           

            List<Section> lstSection = db.Sections.Where(i => i.IsActive == true).ToList<Section>();
            List<Sections_Organizations> lstSectionOrganization = new List<Sections_Organizations>();

            foreach (Section objSection  in lstSection)
            {
                Sections_Organizations objSectionOrganization = new Sections_Organizations();
                objSectionOrganization.ID = Guid.NewGuid();
                objSectionOrganization.OrganizationID = objOrganization.ID;
                objSectionOrganization.SectionID = objSection.ID;
                objSectionOrganization.IsActive = true;
                objSectionOrganization.Modified = DateTime.Now;
                objSectionOrganization.ModifiedBy = objOrganization.CreatedBy;
                objSectionOrganization.Created = DateTime.Now;
                objSectionOrganization.CreatedBy = objOrganization.CreatedBy;

                db.Sections_Organizations.Add(objSectionOrganization);
              
            }
            try
            {
                await db.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
            

            
           
            db.Users_Organizations.Add(objUserOrganization);
            try
            {
               
                await db.SaveChangesAsync();
            }
            catch (Exception )
            {
                
                throw;
            }
             
            return Ok(objOrganization);
        }

        [HttpPost]
        [Route("DeleteOrganizations")]
        public async Task<IHttpActionResult> DeleteOrganizations(Organization objOrganization)
        {
            Organization objDeleteOrganization = db.Organizations.Find(objOrganization.ID);

            objDeleteOrganization.IsActive = false;
            objDeleteOrganization.Modified = DateTime.Now;

            List<Users_Organizations> lstUsers_Organizations = db.Users_Organizations.Where(i => i.OrganizationID == objOrganization.ID && i.IsActive == true).ToList<Users_Organizations>();
            foreach (Users_Organizations objUserOrganization in lstUsers_Organizations)
            {
                objUserOrganization.IsActive = false;
                objUserOrganization.Modified = DateTime.Now;
                db.Entry(objUserOrganization).State = EntityState.Modified;
              
            }
            List<OrganizationGroups_Organizations> lstOrganizationGroups_Organizations = db.OrganizationGroups_Organizations.Where(i => i.OrganizationID == objOrganization.ID && i.IsActive == true).ToList<OrganizationGroups_Organizations>();
            foreach (OrganizationGroups_Organizations objOrgnizationGroup in lstOrganizationGroups_Organizations)
            {
                objOrgnizationGroup.IsActive = false;
                objOrgnizationGroup.Modified=DateTime.Now;
                db.Entry(objOrgnizationGroup).State = EntityState.Modified;
            }
            List<Sections_Organizations> lstSectionOrganization = db.Sections_Organizations.Where(i => i.OrganizationID == objOrganization.ID && i.IsActive == true).ToList<Sections_Organizations>();

            foreach(Sections_Organizations objSectionOrganization in lstSectionOrganization)
            {
                objSectionOrganization.IsActive = false;
                objSectionOrganization.Modified = DateTime.Now;
                db.Entry(objSectionOrganization).State = EntityState.Modified;

            }


            db.Entry(objDeleteOrganization).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                throw ex;
            }

            return Ok();

        }

        [HttpGet]
        [Route("GetOrganization")]
        public async Task<IHttpActionResult> GetOrganization(Guid organizationID)
        {
            Organization objOrganization = await db.Organizations.Where(obj => obj.ID == organizationID && obj.IsActive == true).FirstOrDefaultAsync();

            return Ok(objOrganization);
        }
        [HttpPost]
        [Route("UpdateOrganization")]
        public async Task<IHttpActionResult> UpdateOrganization(Organization objOrganization)
        {
            db.Entry(objOrganization).State = EntityState.Modified;        
            try
            {

                await db.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(objOrganization);
        }

        [HttpGet]
        [Route("GetUserTypes")]
        public async Task<IHttpActionResult> GetUserTypes()
        {
            List<UserType> lstGetUserTypes= await db.UserTypes.ToListAsync<UserType>();

            return Ok(lstGetUserTypes);
        }

        [HttpGet]
        [Route("GetAllEmails")]
        public async Task<IHttpActionResult> GetAllEmails()
        {
            List<User> lstGetUsers = await db.Users.Where(i=>i.IsActive==true).ToListAsync<User>();
            List<string> lstEmails = lstGetUsers.Select(i => i.Email).ToList<string>();

            return Ok(lstEmails);
        }
        [HttpGet]
        [Route("GetAllUserName")]
        public async Task<IHttpActionResult> GetAllUserName()
        {
            List<Users_Organizations> lstUsers_Organizations = await db.Users_Organizations.Where(i => i.IsActive == true).Include(i => i.User).ToListAsync<Users_Organizations>();

            List<object> lstUserName = new List<object>();
            foreach (CHM.Services.Models.Users_Organizations objuser in lstUsers_Organizations)
            {
                var UserNameOrganization = new { UserName = objuser.User.UserName, OrganizationID = objuser.OrganizationID };
                lstUserName.Add(UserNameOrganization);
            }

            return Ok(lstUserName);
        }

        [HttpGet]
        [Route("GetAllUserNameByOrganizationID")]
        public async Task<IHttpActionResult> GetAllUserNameByOrganizationID(Guid guidOrganizationID)
        {
            List<Users_Organizations> lstGetUsersorg = await db.Users_Organizations.Where(i => i.IsActive == true && i.OrganizationID == guidOrganizationID).Include(i => i.User).ToListAsync<Users_Organizations>();

            List<string> lstUserName = new List<string>();
            foreach (CHM.Services.Models.Users_Organizations objuser in lstGetUsersorg)
            {
                lstUserName.Add(objuser.User.UserName);
            }

            return Ok(lstUserName);
        }

        [HttpGet]
        [Route("GetConfigurationsValues")]
        public async Task<IHttpActionResult> GetConfigurationsValues()
        {
            List<CHM.Services.Models.Configuration> lstConfigValues = await db.Configurations.ToListAsync<CHM.Services.Models.Configuration>();

            return Ok(lstConfigValues);
        }

        [HttpPost]
        [Route("CheckValidEmail")]
        public async Task<IHttpActionResult> CheckValidEmail(User objUser)
        {
            User obj =new User();
            obj = await db.Users.Where(i => i.Email == objUser.Email && i.Password == objUser.Password && i.IsActive == true).FirstOrDefaultAsync();
            if(obj!=null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }            
        }

        [HttpPost]
        [Route("CheckValidUser")]
        public async Task<IHttpActionResult> CheckValidUser(User objUser)
        {
            User obj = new User();
            obj = await db.Users.Where(i => i.UserName == objUser.UserName && i.Password == objUser.Password && i.IsActive == true).FirstOrDefaultAsync();

            if (obj != null)
            {
                return Ok(obj.Email);
            }
            else
            {
                return Ok();
            }
        }

        //public class AllUserName
        //{
        //    public string UserName { get; set; }
        //    public string OrganizationId { get; set; }
        //}
    }
}
