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
using Microsoft.AspNet.Identity;
using System.Web.Http.Cors;
using CHM.Services.Utils;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Drawing.Imaging;

namespace CHM.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Interventions")]
    public class InterventionsController : ApiController
    {
        private CHMEntities db = new CHMEntities();

        [HttpPost]
        [Route("GenerateInterventions")]
        public async Task<IHttpActionResult> GenerateInterventions(clsGenratedIntervention interventionsWithLimit)
        {
            Guid userId = new Guid(User.Identity.GetUserId());
            Guid ResidentID = interventionsWithLimit.lstActions[0].ResidentID;
            List<Guid> SectionInterventionIds = interventionsWithLimit.lstActions.Select(obj => obj.Section_InterventionID).ToList<Guid>();
            await inactivateActionsAndInterventionsBasedOnActionIds(SectionInterventionIds, ResidentID);
            DateTime dtMaxDateToGenerateInterventions = DateTime.Today.AddDays(Convert.ToDouble(interventionsWithLimit.InterventionLimit));

            foreach (CHM.Services.Models.Action objAction in interventionsWithLimit.lstActions)
            {
                objAction.ID = Guid.NewGuid();
                objAction.IsActive = true;
                objAction.CreatedBy = userId;
                objAction.ModifiedBy = userId;
                objAction.Created = objAction.Modified = DateTime.Now;

                foreach (Actions_Days objAction_Day in objAction.Actions_Days)
                {
                    objAction_Day.ID = Guid.NewGuid();
                    objAction_Day.IsActive = true;
                    objAction_Day.CreatedBy = userId;
                    objAction_Day.ModifiedBy = userId;
                    objAction_Day.Created = objAction_Day.Modified = DateTime.Now;

                    //var startTimeTicks = new DateTime(objAction_Day.StartTime.Ticks);
                    //var utc = startTimeTicks.ToUniversalTime();
                    //objAction_Day.StartTime = new TimeSpan(utc.Ticks);

                    //var endTimeTicks = new DateTime(objAction_Day.EndTime.Ticks);
                    //utc = endTimeTicks.ToUniversalTime();
                    //objAction_Day.EndTime = new TimeSpan(utc.Ticks);

                    DateTime targetEndDate = DateTime.MinValue;
                    int maxOccurrences = 0;

                    if (objAction.EndDate == null && objAction.Occurrences == null)
                    {
                        targetEndDate = dtMaxDateToGenerateInterventions;//DateTime.Now.AddDays(30);
                    }
                    else if (objAction.EndDate != null)
                    {
                        targetEndDate = objAction.EndDate.Value > dtMaxDateToGenerateInterventions ? dtMaxDateToGenerateInterventions : objAction.EndDate.Value;
                    }
                    else
                    {
                        targetEndDate = dtMaxDateToGenerateInterventions;
                        maxOccurrences = objAction.Occurrences.Value;
                    }

                    if (objAction.Type == "Daily")
                    {
                        for (int i = 0; ; i++)
                        {
                            DateTime dt = objAction.StartDate.AddDays(objAction.Interval * i);
                            TimeSpan tsStartTime = objAction_Day.StartTime;
                            TimeSpan tsEndTime = objAction_Day.EndTime;

                            Intervention objIntervention = new Intervention();

                            objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                            objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                            objIntervention.ID = Guid.NewGuid();
                            objIntervention.IsActive = true;
                            objIntervention.CreatedBy = userId;
                            objIntervention.ModifiedBy = userId;
                            objIntervention.Created = objIntervention.Modified = DateTime.Now;



                            if (targetEndDate != DateTime.MinValue)
                            {
                                if (objIntervention.PlannedStartDate >= targetEndDate)
                                    break;
                            }
                            if (objAction.Occurrences != null)
                            {
                                if (maxOccurrences-- <= 0)
                                    break;
                            }

                            objAction_Day.Interventions.Add(objIntervention);
                        }
                    }
                    else if (objAction.Type == "Weekly")
                    {
                        for (int i = 0; ; i++)
                        {

                            //DateTime dt = objAction.StartDate.StartOfWeek((DayOfWeek)objAction_Day.Day).AddDays(objAction.Interval * i * 7);
                            DateTime dt = objAction.StartDate.Next((DayOfWeek)objAction_Day.Day).AddDays(objAction.Interval * i * 7);
                            TimeSpan tsStartTime = objAction_Day.StartTime;
                            TimeSpan tsEndTime = objAction_Day.EndTime;

                            Intervention objIntervention = new Intervention();

                            objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                            objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                            objIntervention.ID = Guid.NewGuid();
                            objIntervention.IsActive = true;
                            objIntervention.CreatedBy = userId;
                            objIntervention.ModifiedBy = userId;
                            objIntervention.Created = objIntervention.Modified = DateTime.Now;



                            if (targetEndDate != DateTime.MinValue)
                            {
                                if (objIntervention.PlannedStartDate >= targetEndDate)
                                    break;
                            }
                            if (objAction.Occurrences != null)
                            {
                                if (maxOccurrences-- <= 0)
                                    break;
                            }

                            objAction_Day.Interventions.Add(objIntervention);


                        }
                    }
                    else if (objAction.Type == "Monthly")
                    {
                        for (int i = 0; ; i++)
                        {
                            DateTime dt = DateTime.MinValue;
                            if (objAction_Day.Date.Value < objAction.StartDate.Day)
                            {
                                dt = new DateTime(objAction.StartDate.Year, objAction.StartDate.Month + 1, objAction_Day.Date.Value).AddMonths(objAction.Interval * i);
                            }
                            else
                            {
                                dt = new DateTime(objAction.StartDate.Year, objAction.StartDate.Month, objAction_Day.Date.Value).AddMonths(objAction.Interval * i);
                            }


                            TimeSpan tsStartTime = objAction_Day.StartTime;
                            TimeSpan tsEndTime = objAction_Day.EndTime;

                            Intervention objIntervention = new Intervention();

                            objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                            objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                            objIntervention.ID = Guid.NewGuid();
                            objIntervention.IsActive = true;
                            objIntervention.CreatedBy = userId;
                            objIntervention.ModifiedBy = userId;
                            objIntervention.Created = objIntervention.Modified = DateTime.Now;



                            if (targetEndDate != DateTime.MinValue)
                            {
                                if (objIntervention.PlannedStartDate >= targetEndDate)
                                    break;
                            }
                            if (objAction.Occurrences != null)
                            {
                                if (maxOccurrences-- <= 0)
                                    break;
                            }

                            objAction_Day.Interventions.Add(objIntervention);


                        }
                    }
                    else if (objAction.Type == "MonthlyNth")
                    {
                        for (int i = 0; ; i++)
                        {
                            DateTime dtTemp = DateTime.MinValue;

                            int currentMonthDate = Utils.DateTimeExtensions.GetDateForWeekDay((DayOfWeek)objAction_Day.Day, objAction.Instance.Value, objAction.StartDate.Month, objAction.StartDate.Year);

                            if (currentMonthDate < objAction.StartDate.Day)
                            {
                                dtTemp = new DateTime(objAction.StartDate.Year, objAction.StartDate.Month + 1, 1).AddMonths(objAction.Interval * i);
                            }
                            else
                            {
                                dtTemp = new DateTime(objAction.StartDate.Year, objAction.StartDate.Month, 1).AddMonths(objAction.Interval * i);
                            }

                            int dateForWeekDay = Utils.DateTimeExtensions.GetDateForWeekDay((DayOfWeek)objAction_Day.Day, objAction.Instance.Value, dtTemp.Month, dtTemp.Year);

                            DateTime dt = new DateTime(dtTemp.Year, dtTemp.Month, dateForWeekDay);

                            TimeSpan tsStartTime = objAction_Day.StartTime;
                            TimeSpan tsEndTime = objAction_Day.EndTime;

                            Intervention objIntervention = new Intervention();

                            objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                            objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                            objIntervention.ID = Guid.NewGuid();
                            objIntervention.IsActive = true;
                            objIntervention.CreatedBy = userId;
                            objIntervention.ModifiedBy = userId;
                            objIntervention.Created = objIntervention.Modified = DateTime.Now;



                            if (targetEndDate != DateTime.MinValue)
                            {
                                if (objIntervention.PlannedStartDate >= targetEndDate)
                                    break;
                            }
                            if (objAction.Occurrences != null)
                            {
                                if (maxOccurrences-- <= 0)
                                    break;
                            }

                            objAction_Day.Interventions.Add(objIntervention);


                        }
                    }
                    else if (objAction.Type == "Yearly")
                    {
                        for (int i = 0; ; i++)
                        {
                            DateTime dt = DateTime.MinValue;
                           
                            if (objAction_Day.Date.Value < objAction.StartDate.Day && objAction.Month.Value + 1 <= objAction.StartDate.Month)
                            {
                                dt = new DateTime(objAction.StartDate.Year + 1, objAction.Month.Value + 1, objAction_Day.Date.Value).AddYears(i);
                            }
                            else
                            {
                                dt = new DateTime(objAction.StartDate.Year, objAction.Month.Value + 1, objAction_Day.Date.Value).AddYears(i);
                            }


                            TimeSpan tsStartTime = objAction_Day.StartTime;
                            TimeSpan tsEndTime = objAction_Day.EndTime;

                            Intervention objIntervention = new Intervention();

                            objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                            objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                            objIntervention.ID = Guid.NewGuid();
                            objIntervention.IsActive = true;
                            objIntervention.CreatedBy = userId;
                            objIntervention.ModifiedBy = userId;
                            objIntervention.Created = objIntervention.Modified = DateTime.Now;



                            if (targetEndDate != DateTime.MinValue)
                            {
                                if (objIntervention.PlannedStartDate >= targetEndDate)
                                    break;
                            }
                            if (objAction.Occurrences != null)
                            {
                                if (maxOccurrences-- <= 0)
                                    break;
                            }

                            objAction_Day.Interventions.Add(objIntervention);


                        }
                    }
                    else if (objAction.Type == "YearlyNth")
                    {
                        for (int i = 0; ; i++)
                        {
                            DateTime dtTemp = DateTime.MinValue;

                            int currentYearDate = Utils.DateTimeExtensions.GetDateForWeekDay((DayOfWeek)objAction_Day.Day, objAction.Instance.Value, objAction.Month.Value + 1, objAction.StartDate.Year);

                            if (new DateTime(objAction.StartDate.Year, objAction.Month.Value + 1, currentYearDate) < objAction.StartDate)
                            {
                                dtTemp = new DateTime(objAction.StartDate.Year + 1, objAction.Month.Value + 1, 1).AddYears(i);
                            }
                            else
                            {
                                dtTemp = new DateTime(objAction.StartDate.Year, objAction.Month.Value + 1, 1).AddYears(i);
                            }

                            int dateForWeekDay = Utils.DateTimeExtensions.GetDateForWeekDay((DayOfWeek)objAction_Day.Day, objAction.Instance.Value, dtTemp.Month, dtTemp.Year);

                            DateTime dt = new DateTime(dtTemp.Year, dtTemp.Month, dateForWeekDay);

                            TimeSpan tsStartTime = objAction_Day.StartTime;
                            TimeSpan tsEndTime = objAction_Day.EndTime;

                            Intervention objIntervention = new Intervention();

                            objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                            objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                            objIntervention.ID = Guid.NewGuid();
                            objIntervention.IsActive = true;
                            objIntervention.CreatedBy = userId;
                            objIntervention.ModifiedBy = userId;
                            objIntervention.Created = objIntervention.Modified = DateTime.Now;



                            if (targetEndDate != DateTime.MinValue)
                            {
                                if (objIntervention.PlannedStartDate >= targetEndDate)
                                    break;
                            }
                            if (objAction.Occurrences != null)
                            {
                                if (maxOccurrences-- <= 0)
                                    break;
                            }

                            objAction_Day.Interventions.Add(objIntervention);


                        }
                    }

                }

                db.Actions.Add(objAction);
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            List<CHM.Services.Models.Action> lstAction = new List<CHM.Services.Models.Action>();
            lstAction = db.Actions.Where(i => i.IsActive == true).ToList<CHM.Services.Models.Action>();

            List<CHM.Services.Models.Actions_Days> lstActions_Days = new List<CHM.Services.Models.Actions_Days>();
            lstActions_Days = db.Actions_Days.Where(i => i.IsActive == true).ToList<CHM.Services.Models.Actions_Days>();

            List<CHM.Services.Models.Intervention> lstIntervention = new List<CHM.Services.Models.Intervention>();
            lstIntervention = db.Interventions.Where(i => i.IsActive == true).ToList<CHM.Services.Models.Intervention>();

            List<Object> InterventionPAtternTables = new List<object>();
            var ActionIntervion = new
            {
                Action = lstAction,
                ActionDays = lstActions_Days,
                lstIntervention = lstIntervention
            };

            InterventionPAtternTables.Add(ActionIntervion);
            return Ok(InterventionPAtternTables);
         
        }

        //Anil
        [HttpGet]
        [Route("GetResidentInterventions")]
        public async Task<IHttpActionResult> GetResidentInterventions(Guid residentId)
        {
            //List<Intervention> lstInterventions = await db.Interventions.Include(obj => obj.Actions_Days.Action.Section_Intervention).Where(obj => obj.Actions_Days.Action.ResidentID.Equals(residentId) && obj.PlannedStartDate >= startDate && obj.PlannedEndDate <= endDate && obj.IsActive).ToListAsync<Intervention>();

            List<Resident_Interventions_Questions_Answers> lstInterventionQuestionAnswers = await db.Resident_Interventions_Questions_Answers.Include(obj => obj.Intervention_Question_Answer.Section_Intervention).Where(obj => obj.ResidentID.Equals(residentId) && obj.IsActive).ToListAsync<Resident_Interventions_Questions_Answers>();

            List<ResidentInterventionsWithFile> lstResidentInterventionQuestionAnswersWithFile = new List<ResidentInterventionsWithFile>();

            foreach (Resident_Interventions_Questions_Answers objInterventionQuestionAnswers in lstInterventionQuestionAnswers)
            {
                ResidentInterventionsWithFile objResidentWithFile = new ResidentInterventionsWithFile();
                objResidentWithFile.ResidentInterventions = objInterventionQuestionAnswers;
                objResidentWithFile.ResidentFile = GetResidentFile(objInterventionQuestionAnswers.ID);
                lstResidentInterventionQuestionAnswersWithFile.Add(objResidentWithFile);
            }
            return Ok(lstResidentInterventionQuestionAnswersWithFile);

            //  return Ok(lstAnswersByResident);
        }

        //Anil
        [HttpGet]
        [Route("GetAdhocInterventionsForResident")]
        public async Task<IHttpActionResult> GetAdhocInterventionsForResident(Guid residentId, DateTime startDate, DateTime endDate)
        {
            TimeSpan tsEndTime = new TimeSpan(23, 59, 59);
            endDate = endDate + tsEndTime;

            // List<Intervention> lstInterventions = await db.Interventions.Include(obj => obj.Actions_Days.Action.Section_Intervention).Where(obj => obj.Actions_Days.Action.ResidentID.Equals(residentId) && obj.Actions_Days.Action.IsAdhocIntervention == true && obj.PlannedStartDate >= startDate && obj.PlannedEndDate <= endDate && obj.IsActive).ToListAsync<Intervention>();

            var lstResidentActionIds = db.Interventions.Where(obj => obj.Actions_Days.Action.ResidentID.Equals(residentId) && obj.Actions_Days.Action.IsAdhocIntervention == true && obj.PlannedStartDate >= startDate && obj.PlannedEndDate <= endDate && obj.IsActive).GroupBy(obj => obj.Actions_Days.ActionID)
            .Select(i => new
            {
                ActionID = i.Key,
                ActionName = i.Select(obj => obj.Actions_Days.Action.Section_Intervention.InterventionTitle).FirstOrDefault()
            }).ToList();

            List<ResidentAdhocInterventionsWithFile> lstResidentAdhocInterventionQuestionAnswersWithFile = new List<ResidentAdhocInterventionsWithFile>();          

            for(var i=0; i< lstResidentActionIds.Count; i++)
            {
                ResidentAdhocInterventionsWithFile objResidentAdhocInterventionWithFile = new ResidentAdhocInterventionsWithFile();
                objResidentAdhocInterventionWithFile.ActionID = lstResidentActionIds[i].ActionID;
                objResidentAdhocInterventionWithFile.ActionName = lstResidentActionIds[i].ActionName;
                objResidentAdhocInterventionWithFile.ResidentFile = GetResidentAdhocInterventionFile(lstResidentActionIds[i].ActionID);
                lstResidentAdhocInterventionQuestionAnswersWithFile.Add(objResidentAdhocInterventionWithFile);
            }
            return Ok(lstResidentAdhocInterventionQuestionAnswersWithFile);
        }

        [HttpGet]
        [Route("GetInterventionsForResident")]
        public async Task<IHttpActionResult> GetInterventionsForResident(Guid residentId, DateTime startDate, DateTime endDate)
        {
            TimeSpan tsEndTime = new TimeSpan(23, 59, 59);
            endDate = endDate + tsEndTime;

            //List<Intervention> lstInterventions = await db.Interventions.Include(obj => obj.Actions_Days.Action.Sections_Questions_Answers_Tasks.Section_Intervention).Where(obj => obj.Actions_Days.Action.ResidentID.Equals(residentId) && obj.PlannedStartDate <= endDate && obj.PlannedEndDate >= startDate && obj.IsActive).ToListAsync<Intervention>();
            List<Intervention> lstInterventions = await db.Interventions.Include(w=>w.Interventions_Resident_Answers).Include(obj => obj.Actions_Days.Action.Section_Intervention).Where(obj => obj.Actions_Days.Action.ResidentID.Equals(residentId) && obj.PlannedStartDate <= endDate && obj.PlannedEndDate >= startDate && obj.IsActive).ToListAsync<Intervention>();


            List<Intervention> lstResdientTotalInterventions = await db.Interventions.Include(w => w.Interventions_Resident_Answers).Include(obj => obj.Actions_Days.Action).Where(obj => obj.Actions_Days.Action.ResidentID.Equals(residentId)).ToListAsync<Intervention>();

            lstResdientTotalInterventions.ToLookup(obj => obj.Action_DayID).Select(g => g.First());


            List<CHM.Services.Models.Action> lstResdientActionDayIds = await db.Actions.Where(obj => obj.ResidentID.Equals(residentId)).ToListAsync<CHM.Services.Models.Action>();


            for (var i = 0; i < lstResdientTotalInterventions.Count; i++)
            {
                for (var j = 0; j < lstResdientActionDayIds.Count; j++)
                {

                }
            }

            return Ok(lstInterventions);
        }

        [HttpGet]
        [Route("GetInterventionsForCurrentDate")]
        public async Task<IHttpActionResult> GetInterventionsForCurrentDate(string starttodaydate, string endtodaydate)
        {
            // TimeSpan tsEndTime = new TimeSpan(23, 59, 59);
            // CurrentDate = new DateTime();
            DateTime df = Convert.ToDateTime(starttodaydate);
            DateTime dt = Convert.ToDateTime(endtodaydate);
            //DateTime dt = new DateTime(); 
            // var newDateTime = dt.AddMinutes(10);
            var newDateTime = df;
            var endDateTime = dt;

            //var lstInterventions = await db.Interventions.Include(obj => obj.Actions_Days.Action.Section_Intervention).Where(obj => newDateTime>=obj.PlannedStartDate<=endDateTime && obj.IsActive).Select(i=>
            var lstInterventions = await db.Interventions.Include(obj => obj.Actions_Days.Action.Section_Intervention).Where(obj => (obj.PlannedStartDate >= newDateTime && obj.PlannedStartDate <= endDateTime) && obj.IsActive).Select(i =>
                       new
                       {
                           InterventionTitle = i.Actions_Days.Action.Section_Intervention.InterventionTitle,
                           PlannedStartDate = i.PlannedStartDate,
                           ResidentName = i.Actions_Days.Action.Resident.FirstName + i.Actions_Days.Action.Resident.LastName
                       }).ToListAsync<object>();
            return Ok(lstInterventions);
        }

        [HttpGet]
        [Route("GetStartedInterventionsForResident")]
        public async Task<IHttpActionResult> GetStartedInterventionsForResident(Guid residentId, DateTime startDate, DateTime endDate)
        {
            TimeSpan tsEndTime = new TimeSpan(23, 59, 59);
            endDate = endDate + tsEndTime;

            List<Intervention> lstInterventions = await db.Interventions.Include(obj => obj.Actions_Days.Action.Section_Intervention).Include(c => c.Interventions_Resident_Answers).Where(obj => obj.Actions_Days.Action.ResidentID.Equals(residentId) && obj.PlannedStartDate <= endDate && obj.PlannedEndDate >= startDate && obj.IsActive && obj.Status != null && obj.IsHandOverNotes == "true").ToListAsync<Intervention>();


            return Ok(lstInterventions);
        }

        private async Task<IHttpActionResult> inactivateActionsAndInterventions(List<Guid> residents_Questions_AnswersId)
        {
            Guid userId = new Guid(User.Identity.GetUserId());


            //comented on 5/23/2016 and uncommented on 5/26/2016
            List<CHM.Services.Models.Action> lstActions = await db.Actions.Include(obj => obj.Actions_Days.Select(s => s.Interventions)).ToListAsync<CHM.Services.Models.Action>();

            foreach (CHM.Services.Models.Action objAction in lstActions)
            {
                objAction.IsActive = false;
                objAction.ModifiedBy = userId;
                objAction.Modified = DateTime.Now;

                foreach (Actions_Days objAction_Day in objAction.Actions_Days)
                {
                    objAction_Day.IsActive = false;
                    objAction_Day.ModifiedBy = userId;
                    objAction_Day.Modified = DateTime.Now;

                    foreach (Intervention objIntervention in objAction_Day.Interventions)
                    {
                        objIntervention.IsActive = false;
                        objIntervention.ModifiedBy = userId;
                        objIntervention.Modified = DateTime.Now;

                        //db.Entry(objIntervention).State = EntityState.Modified;
                    }

                    //db.Entry(objAction_Day).State = EntityState.Modified;
                }

                db.Entry(objAction).State = EntityState.Modified;
            }

            return Ok();
        }

        private async Task<IHttpActionResult> inactivateActionsAndInterventionsBasedOnActionIds(List<Guid> actionIds, Guid ResidenID)
        {

            try
            {
                Guid userId = new Guid(User.Identity.GetUserId());

                List<CHM.Services.Models.Action> lstActions = await db.Actions.Include(obj => obj.Actions_Days.Select(s => s.Interventions)).Where(obj => actionIds.Contains(obj.Section_InterventionID) && obj.ResidentID == ResidenID).ToListAsync<CHM.Services.Models.Action>();

                foreach (CHM.Services.Models.Action objAction in lstActions)
                {
                    //objAction.IsActive = false;
                    objAction.ModifiedBy = userId;
                    objAction.Modified = DateTime.Now;

                    foreach (Actions_Days objAction_Day in objAction.Actions_Days)
                    {
                        // objAction_Day.IsActive = false;
                        objAction_Day.ModifiedBy = userId;
                        objAction_Day.Modified = DateTime.Now;

                        foreach (Intervention objIntervention in objAction_Day.Interventions)
                        {
                            if (objIntervention.PlannedStartDate > DateTime.Now)
                            {
                                objIntervention.IsActive = false;
                                objIntervention.ModifiedBy = userId;
                                objIntervention.Modified = DateTime.Now;
                            }

                            //db.Entry(objIntervention).State = EntityState.Modified;
                        }

                        //db.Entry(objAction_Day).State = EntityState.Modified;
                    }

                    db.Entry(objAction).State = EntityState.Modified;
                }

                return Ok();
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InterventionExists(Guid id)
        {
            return db.Interventions.Count(e => e.ID == id) > 0;
        }

        [HttpPost]
        [Route("UpdateIntervention")]
        public async Task<IHttpActionResult> UpdateIntervention(Intervention objIntervention)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid userId = new Guid(User.Identity.GetUserId());

            objIntervention.ModifiedBy = userId;
            objIntervention.Modified = DateTime.Now;


            db.Entry(objIntervention).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!InterventionExists(objIntervention.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(objIntervention);
        }

        [HttpPost]
        [Route("GetIntervention")]
        public async Task<IHttpActionResult> GetIntervention(Guid InterventionId)
        {
            Intervention objIntervention = await db.Interventions.Where(obj => obj.ID == InterventionId).FirstOrDefaultAsync();

            //ResidentWithPhoto objResidentWithPhoto = new ResidentWithPhoto();
            //objResidentWithPhoto.Resident = objResident;
            //objResidentWithPhoto.PhotoUrl = GetPhotoRelativeUrl(objResident.ID);


            return Ok(objIntervention);
        }

        [HttpGet]
        [ActionName("GetResidentFile")]
        public string GetResidentFile(Guid guidResidentInterventionQuestionAnswerID)
        {
            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers")))
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + guidResidentInterventionQuestionAnswerID + "")))
                {
                    string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + guidResidentInterventionQuestionAnswerID + ""));

                    if (fileEntries.Length > 0)
                    {
                        FileInfo myFileInfo = new FileInfo(fileEntries[0]);
                        return "/Uploads/Residents/Answers/" + guidResidentInterventionQuestionAnswerID + "/" + myFileInfo.Name;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }

        //Anil
        [HttpGet]
        [ActionName("GetResidentAdhocInterventionFile")]
        public string GetResidentAdhocInterventionFile(Guid guidResidentAdhocInterventionActionID)
        {
            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Actions")))
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Actions/" + guidResidentAdhocInterventionActionID + "")))
                {
                    string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Actions/" + guidResidentAdhocInterventionActionID + ""));

                    if (fileEntries.Length > 0)
                    {
                        FileInfo myFileInfo = new FileInfo(fileEntries[0]);
                        return "/Uploads/Residents/Actions/" + guidResidentAdhocInterventionActionID + "/" + myFileInfo.Name;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }

        //Anil
        public class ResidentInterventionsWithFile
        {
            public Resident_Interventions_Questions_Answers ResidentInterventions { get; set; }
            public string ResidentFile { get; set; }
        }

        //Anil
        public class ResidentAdhocInterventionsWithFile
        {
            public Guid ActionID { get; set; }         
            public string ActionName { get;  set;}         
            public string ResidentFile { get; set; }
        }

        #region 7/6/2016
        [HttpPost]
        [Route("GenerateAdhocInterventions")]
        public async Task<IHttpActionResult> GenerateAdhocInterventions(Guid residentId)
        {

            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            List<object> lstresidentAdhocFiles = new List<object>();
            string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Actions");
            string tempFolder = "temp";
            var provider = new MultipartFormDataStreamProvider(Path.Combine(root, tempFolder));

            try
            {               
                List<CHM.Services.Models.Action> lstActions = new List<CHM.Services.Models.Action>();
                string strFolderPath = string.Empty;

                // Read the form data and return an async task. 
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the form data. 
                foreach (var key in provider.FormData.AllKeys)
                {
                    if (key == "Answers")
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            //lstAssessmentData = JsonConvert.DeserializeObject<List<Residents_Questions_Answers>>(val);
                            lstActions = JsonConvert.DeserializeObject<List<CHM.Services.Models.Action>>(val);
                            break;
                        }
                        break;
                    }
                }

                Guid userId = new Guid(User.Identity.GetUserId());
                Guid ResidentID = lstActions[0].ResidentID;

                List<Guid> SectionInterventionIds = lstActions.Select(obj => obj.Section_InterventionID).ToList<Guid>();
                await inactivateActionsAndInterventionsBasedOnActionIds(SectionInterventionIds, ResidentID);

                foreach (CHM.Services.Models.Action objAction in lstActions)
                {
                    objAction.ID = Guid.NewGuid();
                    objAction.IsActive = true;
                    objAction.CreatedBy = userId;
                    objAction.ModifiedBy = userId;
                    objAction.Created = objAction.Modified = DateTime.Now;
                    objAction.IsAdhocIntervention = true;

                    foreach (Actions_Days objAction_Day in objAction.Actions_Days)
                    {
                        objAction_Day.ID = Guid.NewGuid();
                        objAction_Day.IsActive = true;
                        objAction_Day.CreatedBy = userId;
                        objAction_Day.ModifiedBy = userId;
                        objAction_Day.Created = objAction_Day.Modified = DateTime.Now;

                        DateTime targetEndDate = DateTime.MinValue;
                        int maxOccurrences = 0;

                        if (objAction.EndDate == null && objAction.Occurrences == null)
                        {
                            targetEndDate = DateTime.Now.AddDays(30);
                        }
                        else if (objAction.EndDate != null)
                        {
                            targetEndDate = objAction.EndDate.Value;
                        }
                        else
                        {
                            maxOccurrences = objAction.Occurrences.Value;
                        }

                        if (objAction.Type == "Daily")
                        {
                            for (int i = 0; ; i++)
                            {
                                DateTime dt = objAction.StartDate.AddDays(objAction.Interval * i);
                                TimeSpan tsStartTime = objAction_Day.StartTime;
                                TimeSpan tsEndTime = objAction_Day.EndTime;

                                Intervention objIntervention = new Intervention();
                                objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                                objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                                objIntervention.ID = Guid.NewGuid();
                                objIntervention.IsActive = true;
                                objIntervention.CreatedBy = userId;
                                objIntervention.ModifiedBy = userId;
                                objIntervention.Created = objIntervention.Modified = DateTime.Now;

                                if (targetEndDate != DateTime.MinValue)
                                {
                                    if (objIntervention.PlannedStartDate >= targetEndDate)
                                        break;
                                }
                                if (objAction.Occurrences != null)
                                {
                                    if (maxOccurrences-- <= 0)
                                        break;
                                }
                                objAction_Day.Interventions.Add(objIntervention);
                            }
                        }
                        else if (objAction.Type == "Weekly")
                        {
                            for (int i = 0; ; i++)
                            {
                               //DateTime dt = objAction.StartDate.StartOfWeek((DayOfWeek)objAction_Day.Day).AddDays(objAction.Interval * i * 7);
                                DateTime dt = objAction.StartDate.Next((DayOfWeek)objAction_Day.Day).AddDays(objAction.Interval * i * 7);
                                TimeSpan tsStartTime = objAction_Day.StartTime;
                                TimeSpan tsEndTime = objAction_Day.EndTime;
                                Intervention objIntervention = new Intervention();

                                objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                                objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                                objIntervention.ID = Guid.NewGuid();
                                objIntervention.IsActive = true;
                                objIntervention.CreatedBy = userId;
                                objIntervention.ModifiedBy = userId;
                                objIntervention.Created = objIntervention.Modified = DateTime.Now;


                                if (targetEndDate != DateTime.MinValue)
                                {
                                    if (objIntervention.PlannedStartDate >= targetEndDate)
                                        break;
                                }
                                if (objAction.Occurrences != null)
                                {
                                    if (maxOccurrences-- <= 0)
                                        break;
                                }
                                objAction_Day.Interventions.Add(objIntervention);
                            }
                        }
                        else if (objAction.Type == "Monthly")
                        {
                            for (int i = 0; ; i++)
                            {
                                DateTime dt = DateTime.MinValue;
                                if (objAction_Day.Date.Value < objAction.StartDate.Day)
                                {
                                    dt = new DateTime(objAction.StartDate.Year, objAction.StartDate.Month + 1, objAction_Day.Date.Value).AddMonths(objAction.Interval * i);
                                }
                                else
                                {
                                    dt = new DateTime(objAction.StartDate.Year, objAction.StartDate.Month, objAction_Day.Date.Value).AddMonths(objAction.Interval * i);
                                }
                                TimeSpan tsStartTime = objAction_Day.StartTime;
                                TimeSpan tsEndTime = objAction_Day.EndTime;
                                Intervention objIntervention = new Intervention();

                                objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                                objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                                objIntervention.ID = Guid.NewGuid();
                                objIntervention.IsActive = true;
                                objIntervention.CreatedBy = userId;
                                objIntervention.ModifiedBy = userId;
                                objIntervention.Created = objIntervention.Modified = DateTime.Now;

                                if (targetEndDate != DateTime.MinValue)
                                {
                                    if (objIntervention.PlannedStartDate >= targetEndDate)
                                        break;
                                }
                                if (objAction.Occurrences != null)
                                {
                                    if (maxOccurrences-- <= 0)
                                        break;
                                }

                                objAction_Day.Interventions.Add(objIntervention);
                            }
                        }
                        else if (objAction.Type == "MonthlyNth")
                        {
                            for (int i = 0; ; i++)
                            {
                                DateTime dtTemp = DateTime.MinValue;

                                int currentMonthDate = Utils.DateTimeExtensions.GetDateForWeekDay((DayOfWeek)objAction_Day.Day, objAction.Instance.Value, objAction.StartDate.Month, objAction.StartDate.Year);

                                if (currentMonthDate < objAction.StartDate.Day)
                                {
                                    dtTemp = new DateTime(objAction.StartDate.Year, objAction.StartDate.Month + 1, 1).AddMonths(objAction.Interval * i);
                                }
                                else
                                {
                                    dtTemp = new DateTime(objAction.StartDate.Year, objAction.StartDate.Month, 1).AddMonths(objAction.Interval * i);
                                }

                                int dateForWeekDay = Utils.DateTimeExtensions.GetDateForWeekDay((DayOfWeek)objAction_Day.Day, objAction.Instance.Value, dtTemp.Month, dtTemp.Year);

                                DateTime dt = new DateTime(dtTemp.Year, dtTemp.Month, dateForWeekDay);
                                TimeSpan tsStartTime = objAction_Day.StartTime;
                                TimeSpan tsEndTime = objAction_Day.EndTime;

                                Intervention objIntervention = new Intervention();

                                objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                                objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                                objIntervention.ID = Guid.NewGuid();
                                objIntervention.IsActive = true;
                                objIntervention.CreatedBy = userId;
                                objIntervention.ModifiedBy = userId;
                                objIntervention.Created = objIntervention.Modified = DateTime.Now;

                                if (targetEndDate != DateTime.MinValue)
                                {
                                    if (objIntervention.PlannedStartDate >= targetEndDate)
                                        break;
                                }
                                if (objAction.Occurrences != null)
                                {
                                    if (maxOccurrences-- <= 0)
                                        break;
                                }
                                objAction_Day.Interventions.Add(objIntervention);
                            }
                        }
                        else if (objAction.Type == "Yearly")
                        {
                            for (int i = 0; ; i++)
                            {
                                DateTime dt = DateTime.MinValue;
                                if (objAction_Day.Date.Value < objAction.StartDate.Day)
                                {
                                    dt = new DateTime(objAction.StartDate.Year + 1, objAction.Month.Value + 1, objAction_Day.Date.Value).AddYears(i);
                                }
                                else
                                {
                                    dt = new DateTime(objAction.StartDate.Year, objAction.Month.Value + 1, objAction_Day.Date.Value).AddYears(i);
                                }

                                TimeSpan tsStartTime = objAction_Day.StartTime;
                                TimeSpan tsEndTime = objAction_Day.EndTime;

                                Intervention objIntervention = new Intervention();
                                objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                                objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                                objIntervention.ID = Guid.NewGuid();
                                objIntervention.IsActive = true;
                                objIntervention.CreatedBy = userId;
                                objIntervention.ModifiedBy = userId;
                                objIntervention.Created = objIntervention.Modified = DateTime.Now;

                                if (targetEndDate != DateTime.MinValue)
                                {
                                    if (objIntervention.PlannedStartDate >= targetEndDate)
                                        break;
                                }
                                if (objAction.Occurrences != null)
                                {
                                    if (maxOccurrences-- <= 0)
                                        break;
                                }
                                objAction_Day.Interventions.Add(objIntervention);
                            }
                        }
                        else if (objAction.Type == "YearlyNth")
                        {
                            for (int i = 0; ; i++)
                            {
                                DateTime dtTemp = DateTime.MinValue;

                                int currentYearDate = Utils.DateTimeExtensions.GetDateForWeekDay((DayOfWeek)objAction_Day.Day, objAction.Instance.Value, objAction.Month.Value + 1, objAction.StartDate.Year);

                                if (new DateTime(objAction.StartDate.Year, objAction.Month.Value + 1, currentYearDate) < objAction.StartDate)
                                {
                                    dtTemp = new DateTime(objAction.StartDate.Year + 1, objAction.Month.Value + 1, 1).AddYears(i);
                                }
                                else
                                {
                                    dtTemp = new DateTime(objAction.StartDate.Year, objAction.Month.Value + 1, 1).AddYears(i);
                                }

                                int dateForWeekDay = Utils.DateTimeExtensions.GetDateForWeekDay((DayOfWeek)objAction_Day.Day, objAction.Instance.Value, dtTemp.Month, dtTemp.Year);

                                DateTime dt = new DateTime(dtTemp.Year, dtTemp.Month, dateForWeekDay);

                                TimeSpan tsStartTime = objAction_Day.StartTime;
                                TimeSpan tsEndTime = objAction_Day.EndTime;

                                Intervention objIntervention = new Intervention();

                                objIntervention.PlannedStartDate = dt.Date + tsStartTime;
                                objIntervention.PlannedEndDate = dt.Date + tsEndTime;
                                objIntervention.ID = Guid.NewGuid();
                                objIntervention.IsActive = true;
                                objIntervention.CreatedBy = userId;
                                objIntervention.ModifiedBy = userId;
                                objIntervention.Created = objIntervention.Modified = DateTime.Now;

                                if (targetEndDate != DateTime.MinValue)
                                {
                                    if (objIntervention.PlannedStartDate >= targetEndDate)
                                        break;
                                }
                                if (objAction.Occurrences != null)
                                {
                                    if (maxOccurrences-- <= 0)
                                        break;
                                }

                                objAction_Day.Interventions.Add(objIntervention);
                            }
                        }
                    }
                    db.Actions.Add(objAction);
                }
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw;
                }
                // This illustrates how to get the file names for uploaded files. 
                foreach (var fileData in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(fileData.LocalFileName);
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                    }
                    string fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    Guid Section_InterventionID = JsonConvert.DeserializeObject<Guid>(fileData.Headers.ContentDisposition.Name);
                    Guid Action_ID = lstActions.Where(obj => obj.Section_InterventionID.Equals(Section_InterventionID)).FirstOrDefault().ID;

                    strFolderPath = Path.Combine(root, Convert.ToString(Action_ID));
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }
                    var residentFile = new { ID = Action_ID, fileName = fileName };
                    lstresidentAdhocFiles.Add(residentFile);
                    File.Move(fileData.LocalFileName, Path.Combine(strFolderPath, fileName));
                }
            }
            catch (System.Exception e)
            {               
                return null;
            }


            List<CHM.Services.Models.Action> lstAction = new List<CHM.Services.Models.Action>();
            lstAction = db.Actions.ToList<CHM.Services.Models.Action>();

            List<CHM.Services.Models.Actions_Days> lstActions_Days = new List<CHM.Services.Models.Actions_Days>();
            lstActions_Days = db.Actions_Days.ToList<CHM.Services.Models.Actions_Days>();

            List<CHM.Services.Models.Intervention> lstIntervention = new List<CHM.Services.Models.Intervention>();
            lstIntervention = db.Interventions.ToList<CHM.Services.Models.Intervention>();

            List<Object> InterventionPAtternTables = new List<object>();
            var ActionIntervion = new
            {
                Action = lstAction,
                ActionDays = lstActions_Days,
                lstIntervention = lstIntervention,
                Files = lstresidentAdhocFiles
            };

            InterventionPAtternTables.Add(ActionIntervion);
            return Ok(InterventionPAtternTables);
        }

        [HttpPost]
        [Route("DeActiveAdhocIntervention")]
        public async Task<IHttpActionResult> DeActiveAdhocIntervention(CHM.Services.Models.Action objAction)
        {

            Guid userId = new Guid(User.Identity.GetUserId());

            try
            {
                CHM.Services.Models.Action objActiondays = new CHM.Services.Models.Action();
                objActiondays = db.Actions.Include(obj => obj.Actions_Days).Include(obj => obj.Actions_Days.Select(i => i.Interventions)).Where(i => i.ID == objAction.ID && i.IsActive == true).FirstOrDefault();


                objActiondays.IsActive = false;
                objActiondays.ModifiedBy = userId;
                objActiondays.Modified = DateTime.Now;




                foreach (Actions_Days objAction_Day in objActiondays.Actions_Days)
                {
                    objAction_Day.IsActive = false;
                    objAction_Day.ModifiedBy = userId;
                    objAction_Day.Modified = DateTime.Now;

                    foreach (Intervention objIntervention in objAction_Day.Interventions)
                    {

                        objIntervention.IsActive = false;
                        objIntervention.ModifiedBy = userId;
                        objIntervention.Modified = DateTime.Now;
                    }


                }
                db.Entry(objActiondays).State = EntityState.Modified;
                db.SaveChanges();


                return Ok(objActiondays);
            }

            catch (System.Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region updateGenerated intervention    
        [HttpPost]
        [Route("UpdateGeneratedInterventions")]
        public async Task<IHttpActionResult> UpdateGeneratedInterventions(clsUpdateGenratedIntervention objAction)
        {
            if (objAction != null)
            {
                TimeSpan tsStartTime = TimeSpan.Parse(objAction.StartTime);
                TimeSpan tsEndTime = TimeSpan.Parse(objAction.EndTime);



                DateTime PlannedStartDate = objAction.StartDate.Date + tsStartTime;
                DateTime PlannedEndDate = objAction.StartDate.Date + tsEndTime;


                Intervention objUpdateIntervention = new Intervention();
                objUpdateIntervention = db.Interventions.Where(i => i.ID == objAction.Id && i.IsActive == true).FirstOrDefault();

                if (objUpdateIntervention != null)
                {
                    objUpdateIntervention.PlannedStartDate = PlannedStartDate;
                    objUpdateIntervention.PlannedEndDate = PlannedEndDate;
                    db.Entry(objUpdateIntervention).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Ok(objUpdateIntervention);
            }
            else
            {
                return Ok();
            }

        }

        [HttpPost]
        [Route("DeleteGeneratedIntervention")]
        public async Task<IHttpActionResult> DeleteGeneratedIntervention(Guid interventionID)
        {

            Guid userId = new Guid(User.Identity.GetUserId());

            Intervention objUpdateIntervention = new Intervention();
            objUpdateIntervention = db.Interventions.Where(i => i.ID == interventionID && i.IsActive == true).FirstOrDefault();

            if (objUpdateIntervention != null)
            {
                objUpdateIntervention.IsActive = false;
                objUpdateIntervention.ModifiedBy = userId;
                objUpdateIntervention.Modified = DateTime.Now; ;
                db.Entry(objUpdateIntervention).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Ok(objUpdateIntervention);
        }

        public class clsUpdateGenratedIntervention
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }

        }

        public class clsGenratedIntervention
        {
            public virtual List<CHM.Services.Models.Action> lstActions { get; set; }
            public string InterventionLimit { get; set; }
        
        }
        #endregion
    }
}