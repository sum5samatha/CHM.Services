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
    [RoutePrefix("api/QuestionParentQuestion")]
    public class QuestionParentQuestionController : ApiController
    {

        private CHMEntities db = new CHMEntities();


        [HttpGet]
        [Route("GetAllQuestionParentQuestion")]
        public async Task<IHttpActionResult> GetAllQuestionParentQuestion()
        {
            List<Question_ParentQuestion> lstQuestionParentQuestion = await db.Question_ParentQuestion.Where(obj => obj.IsActive == true).ToListAsync<Question_ParentQuestion>();

            return Ok(lstQuestionParentQuestion);
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



    }
}
