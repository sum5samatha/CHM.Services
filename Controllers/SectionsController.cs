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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Sections")]
    public class SectionsController : ApiController
    {
        private CHMEntities db = new CHMEntities();
        
        [Route("GetActiveSections")]        
        public async Task<IHttpActionResult> GetActiveSections()
        {
            List<Section> lstSections = await db.Sections.Include(obj => obj.Sections_Questions.Select(o => o.Sections_Questions_Answers)).Where(obj => obj.IsActive == true).ToListAsync<Section>();

            


           



            //List<Section> lstSections = await db.Sections.Include("Section.Sections_Questions").Include("Section.Sections_Questions.Sections_Questions_Answers").Include("Section.Sections_Questions.Sections_Questions_Answers.Question_ParentQuestion").ToListAsync<Section>();
            //foreach(Section objSection in lstSections)
            //{
               
            //      // objSection.Sections_Questions = objSection.Sections_Questions.Where(obj => obj.ParentAnswerID == null).ToList<Sections_Questions>();
            //    objSection.Sections_Questions = objSection.Sections_Questions.ToList<Sections_Questions>();
              
            //}
            return Ok(lstSections);
        }

        [Route("GetOnlyThreeSections")]
        public async Task<IHttpActionResult> GetOnlyThreeSections()
        {
            List<Section> lstSections = await db.Sections.Include(obj => obj.Sections_Questions.Select(o => o.Sections_Questions_Answers)).Where(obj => obj.IsActive == true && obj.HasSummary==true).ToListAsync<Section>();       
            return Ok(lstSections);
        }

        [Route("GetOnlyThreeSectionsByOrganizationID")]
        public async Task<IHttpActionResult> GetOnlyThreeSectionsByOrganizationID(Guid OrganizationID)
        {
            List<Sections_Organizations> lstSectionOrganization = db.Sections_Organizations.Where(i => i.OrganizationID == OrganizationID && i.IsActive == true).ToList<Sections_Organizations>();
            List<Section> lstSections = await db.Sections.Include(obj => obj.Sections_Questions.Select(o => o.Sections_Questions_Answers)).Where(obj => obj.IsActive == true && obj.HasSummary==true).ToListAsync<Section>();
            List<Section> lstnewsection = new List<Section>();
            foreach(Section objSection in lstSections)
            {
                foreach(Sections_Organizations objsectionOrganization in lstSectionOrganization)
                {
                    if(objsectionOrganization.SectionID==objSection.ID && objsectionOrganization.OrganizationID==OrganizationID)
                    {
                        lstnewsection.Add(objSection);
                    }
                }
            }
            return Ok(lstnewsection);
        }


         [Route("GetOnlyActiveSections")] 
        public async Task<IHttpActionResult> GetOnlyActiveSections()
        {
            List<Section> lstSections = await db.Sections.Where(obj => obj.IsActive == true).ToListAsync<Section>();
            return Ok(lstSections);
        }

         [HttpGet]
         [Route("GetActiveSectionsByOrganizationID")]
         public async Task<IHttpActionResult> GetActiveSectionsByOrganizationID(Guid OrganizationID)
         {
             List<Sections_Organizations> lstSections_Organizations = await db.Sections_Organizations.Where(obj => obj.IsActive == true && obj.OrganizationID == OrganizationID).Include(i=>i.Section).ToListAsync<Sections_Organizations>();
             List<Section> lstSections = new List<Section>();
             foreach (Sections_Organizations objSectionOrganization in lstSections_Organizations)
             {
                 lstSections.Add(objSectionOrganization.Section);
             }
             return Ok(lstSections);
         }

         [Route("GetActiveSectionById")]
         public async Task<IHttpActionResult> GetActiveSectionById(Guid SectionId)
         {
             List<Section> lstSections = await db.Sections.Include(obj => obj.Sections_Questions.Select(o => o.Sections_Questions_Answers)).Where(obj => obj.IsActive == true && obj.ID==SectionId).ToListAsync<Section>();

             return Ok(lstSections);
         }
         [HttpGet]
         [Route("GetActiveSectionByIdandOrganizationId")]
         public async Task<IHttpActionResult> GetActiveSectionByIdandOrganizationId(Guid SectionId, Guid OrganizationID)
         {
             List<Section> lstSections = await db.Sections.Include(obj => obj.Sections_Questions.Select(o => o.Sections_Questions_Answers)).Where(obj => obj.IsActive == true && obj.ID == SectionId).ToListAsync<Section>();

             List<Sections_Organizations> lstSectionOrganization = db.Sections_Organizations.Where(i => i.OrganizationID == OrganizationID && i.IsActive==true).ToList<Sections_Organizations>();
             List<Section> lstSectionsOrganization = new List<Section>();
             foreach(Section objsection in lstSections)
             {
                 foreach (Sections_Organizations objSectionOrganization in lstSectionOrganization)
                 {
                     if(objSectionOrganization.SectionID==objsection.ID && objSectionOrganization.OrganizationID==OrganizationID)
                     {
                         lstSectionsOrganization.Add(objsection);
                     }
                 }
             }

             return Ok(lstSections);
         }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
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

        [HttpGet]
        [Route("GetActiveSectionsByOrganizationIDBySection")]
        public async Task<IHttpActionResult> GetActiveSectionsByOrganizationIDBySection(Guid OrganizationID)
        {
            List<Sections_Organizations> lstSections_Organizations = await db.Sections_Organizations.Include(i => i.Section).Where(obj => obj.IsActive == true && obj.OrganizationID == OrganizationID && obj.Section.HasSummary==true).ToListAsync<Sections_Organizations>();
            List<Section> lstSections = new List<Section>();
            foreach (Sections_Organizations objSectionOrganization in lstSections_Organizations)
            {
                lstSections.Add(objSectionOrganization.Section);
            }
            return Ok(lstSections);
        }
    }
}
