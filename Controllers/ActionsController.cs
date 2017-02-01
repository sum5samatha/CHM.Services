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

namespace CHM.Services.Controllers
{
    public class ActionsController : ApiController
    {
        private CHMEntities db = new CHMEntities();

        // GET: api/Actions
        public IQueryable<CHM.Services.Models.Action> GetActions()
        {
            return db.Actions;
        }

        // GET: api/Actions/5
        [ResponseType(typeof(CHM.Services.Models.Action))]
        public async Task<IHttpActionResult> GetAction(Guid id)
        {
            CHM.Services.Models.Action action = await db.Actions.FindAsync(id);
            if (action == null)
            {
                return NotFound();
            }

            return Ok(action);
        }

        // PUT: api/Actions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAction(Guid id, CHM.Services.Models.Action action)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != action.ID)
            {
                return BadRequest();
            }

            db.Entry(action).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Actions
        [ResponseType(typeof(CHM.Services.Models.Action))]
        public async Task<IHttpActionResult> PostAction(CHM.Services.Models.Action action)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Actions.Add(action);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActionExists(action.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = action.ID }, action);
        }

        // DELETE: api/Actions/5
        [ResponseType(typeof(CHM.Services.Models.Action))]
        public async Task<IHttpActionResult> DeleteAction(Guid id)
        {
            CHM.Services.Models.Action action = await db.Actions.FindAsync(id);
            if (action == null)
            {
                return NotFound();
            }

            db.Actions.Remove(action);
            await db.SaveChangesAsync();

            return Ok(action);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActionExists(Guid id)
        {
            return db.Actions.Count(e => e.ID == id) > 0;
        }
    }
}