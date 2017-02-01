using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CHM.Services.Models;
using System.Web.Http.Cors;

namespace CHM.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/InterventionQuestionParentQuestion")]
    public class InterventionQuestionParentQuestionController : ApiController
    {
        private CHMEntities db = new CHMEntities();


        [HttpGet]
        [Route("GetAllInterventionQuestionParentQuestion")]
        public async Task<IHttpActionResult> GetAllInterventionQuestionParentQuestion()
        {
            List<Intervention_Question_ParentQuestion> lstInterventionQuestionParentQuestion = await db.Intervention_Question_ParentQuestion.Where(obj => obj.IsActive == true).ToListAsync<Intervention_Question_ParentQuestion>();

            return Ok(lstInterventionQuestionParentQuestion);
        }


    }
}
