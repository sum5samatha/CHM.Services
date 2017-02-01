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
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace CHM.Services.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Syncronization")]
    public class SyncronizationController : ApiController
    {
        private CHMEntities db = new CHMEntities();

        [Route("SaveofflineResident")]
        public async Task<IHttpActionResult> SaveofflineResident(List<Resident> lstResident)
        {
            foreach (Resident objResident in lstResident)
            {
                Resident objChkIsExistingRes = await db.Residents.FindAsync(objResident.ID);
                if (objChkIsExistingRes == null)
                {
                    db.Residents.Add(objResident);
                    await db.SaveChangesAsync();
                }
            }

            return Ok();
        }

        [Route("UpdateofflineResident")]
        public async Task<IHttpActionResult> UpdateofflineResident(List<Resident> lstResident)
        {
            // Guid userId = new Guid(User.Identity.GetUserId());
            foreach (Resident objRes in lstResident)
            {
                Resident objResident = await db.Residents.FindAsync(objRes.ID);
                //objResident.ModifiedBy = userId;
                if (objResident != null)
                {
                    objResident.FirstName = objRes.FirstName;
                    objResident.LastName = objRes.LastName;
                    objResident.NickName = objRes.NickName;
                    objResident.Gender = objRes.Gender;
                    objResident.DOB = objRes.DOB;
                    objResident.DOJ = objRes.DOJ;
                    objResident.AdmittedFrom = objRes.AdmittedFrom;
                    objResident.Telephone = objRes.Telephone;
                    objResident.Mobile = objRes.Mobile;
                    objResident.GPDetails = objRes.GPDetails;
                    objResident.IsActive = objRes.IsActive.ToString().ToUpper() == "TRUE" ? true : false;
                    if (objRes.Nok != null)
                    {
                        objResident.Nok = objRes.Nok;
                    }
                    if (objRes.NokTelephoneNumber != null)
                    {
                        objResident.NokTelephoneNumber = objRes.NokTelephoneNumber;
                    }
                    if (objRes.NokAddress != null)
                    {
                        objResident.NokAddress = objRes.NokAddress;
                    }
                    if (objRes.NokPreferred != null)
                    {
                        objResident.NokPreferred = objRes.NokPreferred;
                    }
                    if (objRes.SocialWorker != null)
                    {
                        objResident.SocialWorker = objRes.SocialWorker;
                    }
                    if (objRes.ReasonForAdmission != null)
                    {
                        objResident.ReasonForAdmission = objRes.ReasonForAdmission;
                    }

                    objResident.Modified = DateTime.Now;
                    objResident.IsAccepted = objRes.IsAccepted;
                    db.Entry(objResident).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
            return Ok();
        }


        [Route("UpdateofflineUserLastLogin")]
        public async Task<IHttpActionResult> UpdateofflineUserLastLogin(List<User> lstUser)
        {
            // Guid userId = new Guid(User.Identity.GetUserId());
            foreach (User objUser in lstUser)
            {
                User obj = await db.Users.FindAsync(objUser.ID);

                if (obj != null)
                {
                    obj.LastLogin = Convert.ToDateTime(objUser.LastLogin).ToUniversalTime();
                    obj.Modified = DateTime.Now;
                    db.Entry(obj).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
            return Ok();
        }

        [Route("SaveofflineActions")]
        public async Task<IHttpActionResult> SaveofflineActions(List<CHM.Services.Models.Action> lstActions)
        {
            try
            {
                foreach (CHM.Services.Models.Action objAction in lstActions)
                {
                    CHM.Services.Models.Action objChkIsExistingAction = await db.Actions.FindAsync(objAction.ID);
                    if (objChkIsExistingAction == null)
                    {
                        objAction.Created = DateTime.Now;
                        objAction.Modified = DateTime.Now;
                        db.Actions.Add(objAction);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }

        [Route("UpdateofflineActions")]

        public async Task<IHttpActionResult> UpdateofflineActions(List<CHM.Services.Models.Action> lstActions)
        {
            //  Guid userId = new Guid(User.Identity.GetUserId());
            foreach (CHM.Services.Models.Action objAction in lstActions)
            {
                CHM.Services.Models.Action objActions = await db.Actions.FindAsync(objAction.ID);
                //   objActions.ModifiedBy = userId;
                if (objActions != null)
                {
                    objActions.IsActive = objAction.IsActive;
                    objActions.Modified = DateTime.Now;
                    db.Entry(objActions).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return Ok();
        }

        [Route("SaveofflineAction_Days")]
        public async Task<IHttpActionResult> SaveofflineAction_Days(List<Actions_Days> lstActions_Days)
        {
            try
            {
                foreach (Actions_Days objActions_Days in lstActions_Days)
                {
                    Actions_Days objChkIsExistingActions_Days = await db.Actions_Days.FindAsync(objActions_Days.ID);
                    if (objChkIsExistingActions_Days == null)
                    {
                        objActions_Days.Created = DateTime.Now;
                        objActions_Days.Modified = DateTime.Now;

                        db.Actions_Days.Add(objActions_Days);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }

        [Route("UpdateofflineActions_Days")]

        public async Task<IHttpActionResult> UpdateofflineActions_Days(List<Actions_Days> lstActions_Days)
        {
            // Guid userId = new Guid(User.Identity.GetUserId());
            foreach (Actions_Days objActions_Days in lstActions_Days)
            {
                CHM.Services.Models.Actions_Days objActions_Day = await db.Actions_Days.FindAsync(objActions_Days.ID);
                //  objActions_Days.ModifiedBy = userId;
                if (objActions_Day != null)
                {
                    objActions_Days.IsActive = objActions_Day.IsActive;
                    objActions_Days.Modified = DateTime.Now;
                    db.Entry(objActions_Day).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return Ok();
        }


        [Route("SaveofflineInterventions")]
        public async Task<IHttpActionResult> SaveofflineInterventions(List<CHM.Services.Models.Intervention> lstIntervention)
        {
            try
            {
                foreach (CHM.Services.Models.Intervention objIntervention in lstIntervention)
                {
                    CHM.Services.Models.Intervention objChkIsExistingIntervention = await db.Interventions.FindAsync(objIntervention.ID);
                    if (objChkIsExistingIntervention == null)
                    {
                        objIntervention.Created = DateTime.Now;
                        objIntervention.Modified = DateTime.Now;
                        db.Interventions.Add(objIntervention);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

        [Route("UpdateofflineInterventions")]

        public async Task<IHttpActionResult> UpdateofflineInterventions(List<CHM.Services.Models.Intervention> lstIntervention)
        {
            // Guid userId = new Guid(User.Identity.GetUserId());
            foreach (CHM.Services.Models.Intervention objIntervention in lstIntervention)
            {
                CHM.Services.Models.Intervention objInterventions = await db.Interventions.FindAsync(objIntervention.ID);
                //  objInterventions.ModifiedBy = userId;
                if (objInterventions != null)
                {
                    objInterventions.PlannedStartDate = objIntervention.PlannedStartDate;
                    objInterventions.PlannedEndDate = objIntervention.PlannedEndDate;
                    if (objIntervention.Status != null)
                    {
                        objInterventions.Status = objIntervention.Status;
                    }
                    if (objIntervention.Comments != null)
                    {
                        objInterventions.Comments = objIntervention.Comments;
                    }
                    if (objIntervention.MoodAfter != null)
                    {
                        objInterventions.MoodAfter = objIntervention.MoodAfter;
                    }
                    if (objIntervention.MoodDuring != null)
                    {
                        objInterventions.MoodDuring = objIntervention.MoodDuring;
                    }
                    if (objIntervention.MoodBefore != null)
                    {
                        objInterventions.MoodBefore = objIntervention.MoodBefore;
                    }
                    if (objIntervention.OutCome != null)
                    {
                        objInterventions.OutCome = objIntervention.OutCome;
                    }
                    if (objIntervention.Exception != null)
                    {
                        objInterventions.Exception = objIntervention.Exception;
                    }
                    if (objIntervention.Time_Span != null)
                    {
                        objInterventions.Time_Span = objIntervention.Time_Span;
                    }
                    objInterventions.IsActive = objIntervention.IsActive;
                    objInterventions.Modified = DateTime.Now;
                    db.Entry(objInterventions).State = EntityState.Modified;

                    await db.SaveChangesAsync();
                }
            }

            return Ok();
        }

        [Route("SaveofflineInterventionsResidentAnswers")]
        public async Task<IHttpActionResult> SaveofflineInterventionsResidentAnswers(List<Interventions_Resident_Answers> lstInterventions_Resident_Answers)
        {
            foreach (Interventions_Resident_Answers objInterventions_Resident_Answers in lstInterventions_Resident_Answers)
            {
                Interventions_Resident_Answers objChkIsExistingInterventions_Resident_Answers = await db.Interventions_Resident_Answers.FindAsync(objInterventions_Resident_Answers.ID);
                if (objChkIsExistingInterventions_Resident_Answers == null)
                {
                    db.Interventions_Resident_Answers.Add(objInterventions_Resident_Answers);
                    await db.SaveChangesAsync();
                }
            }
            return Ok();
        }

        [Route("UpdateofflineInterventionsResidentAnswers")]

        public async Task<IHttpActionResult> UpdateofflineInterventionsResidentAnswers(List<Interventions_Resident_Answers> lstInterventions_Resident_Answers)
        {
            // Guid userId = new Guid(User.Identity.GetUserId());
            foreach (Interventions_Resident_Answers objIntervention in lstInterventions_Resident_Answers)
            {
                CHM.Services.Models.Interventions_Resident_Answers objInterventions = await db.Interventions_Resident_Answers.FindAsync(objIntervention.ID);
                //objInterventions.ModifiedBy = userId;
                if (objInterventions != null)
                {
                    if (objIntervention.AnswerText != null)
                    {
                        objInterventions.AnswerText = objIntervention.AnswerText;
                    }
                    objInterventions.IsActive = objIntervention.IsActive;
                    objInterventions.Modified = DateTime.Now;
                    db.Entry(objInterventions).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
            return Ok();
        }


        [Route("SaveofflineResidentInterventionsQuestions_Answers")]
        public async Task<IHttpActionResult> SaveofflineResidentInterventionsQuestions_Answers(List<Resident_Interventions_Questions_Answers> lstResident_Interventions_Questions_Answers)
        {
            foreach (Resident_Interventions_Questions_Answers objResident_Interventions_Questions_Answers in lstResident_Interventions_Questions_Answers)
            {
                Resident_Interventions_Questions_Answers objChkIsExistingResident_Interventions_Questions_Answers = await db.Resident_Interventions_Questions_Answers.FindAsync(objResident_Interventions_Questions_Answers.ID);
                if (objChkIsExistingResident_Interventions_Questions_Answers == null)
                {

                    db.Resident_Interventions_Questions_Answers.Add(objResident_Interventions_Questions_Answers);
                    await db.SaveChangesAsync();
                }
            }

            return Ok();
        }



        [Route("UpdateofflineResidentInterventionsQuestions_Answers")]
        public async Task<IHttpActionResult> UpdateofflineResidentInterventionsQuestions_Answers(List<Resident_Interventions_Questions_Answers> lstResident_Interventions_Questions_Answers)
        {
            //  Guid userId = new Guid(User.Identity.GetUserId());
            foreach (Resident_Interventions_Questions_Answers objResident_Interventions_Questions_Answers in lstResident_Interventions_Questions_Answers)
            {
                CHM.Services.Models.Resident_Interventions_Questions_Answers objResident_Interventions_Questions_Answer = await db.Resident_Interventions_Questions_Answers.FindAsync(objResident_Interventions_Questions_Answers.ID);
                //  objResident_Interventions_Questions_Answer.ModifiedBy = userId;
                if (objResident_Interventions_Questions_Answer != null)
                {
                    if (objResident_Interventions_Questions_Answers.AnswerText != null)
                    {
                        objResident_Interventions_Questions_Answer.AnswerText = objResident_Interventions_Questions_Answers.AnswerText;
                    }
                    objResident_Interventions_Questions_Answer.IsActive = objResident_Interventions_Questions_Answers.IsActive;
                    objResident_Interventions_Questions_Answer.Modified = DateTime.Now;
                    db.Entry(objResident_Interventions_Questions_Answer).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
            return Ok();
        }

        [Route("SaveofflineResidentsQuestionsAnswers")]
        public async Task<IHttpActionResult> SaveofflineResidentsQuestionsAnswers(List<Residents_Questions_Answers> lstResidents_Questions_Answers)
        {
            foreach (Residents_Questions_Answers objResidents_Questions_Answers in lstResidents_Questions_Answers)
            {
                Residents_Questions_Answers objChkIsExistingobjResidents_Questions_Answers = await db.Residents_Questions_Answers.FindAsync(objResidents_Questions_Answers.ID);
                if (objChkIsExistingobjResidents_Questions_Answers == null)
                {
                    db.Residents_Questions_Answers.Add(objResidents_Questions_Answers);
                    await db.SaveChangesAsync();
                }
            }

            return Ok();
        }

        [Route("UpdateofflineResidentsQuestionsAnswers")]

        public async Task<IHttpActionResult> UpdateofflineResidentsQuestionsAnswers(List<Residents_Questions_Answers> lstResidents_Questions_Answers)
        {
            //Guid userId = new Guid(User.Identity.GetUserId());
            foreach (Residents_Questions_Answers objResidents_Questions_Answers in lstResidents_Questions_Answers)
            {
                CHM.Services.Models.Residents_Questions_Answers objResidents_Questions_Answer = await db.Residents_Questions_Answers.FindAsync(objResidents_Questions_Answers.ID);
                // objResidents_Questions_Answer.ModifiedBy = userId;
                if (objResidents_Questions_Answer != null)
                {
                    if (objResidents_Questions_Answers.AnswerText != null)
                    {
                        objResidents_Questions_Answer.AnswerText = objResidents_Questions_Answers.AnswerText;
                    }
                    objResidents_Questions_Answer.IsActive = objResidents_Questions_Answers.IsActive;
                    objResidents_Questions_Answer.Modified = DateTime.Now;
                    db.Entry(objResidents_Questions_Answer).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
            return Ok();
        }


        [Route("SaveofflineResidentPhotos")]
        public string SaveofflineResidentPhotos(List<ResidentPhotos> lstResidentPhotos)
        {

            foreach (var item in lstResidentPhotos)
            {
                string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos");
                string tempFolder = "temp";
                var provider = Path.Combine(root, tempFolder);

                string strFolderPath = string.Empty;
                try
                {
                    var base64data = item.PhotoUrl;
                    strFolderPath = Path.Combine(root, item.ResidentID.ToString());
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }
                    DeleteAllContentInDirectory(strFolderPath);

                    var bytes = Convert.FromBase64String(base64data.Split(';')[1].Substring(7));
                    using (var imageFile = new FileStream(strFolderPath + "\\fileName" + item.ResidentID.ToString() + ".jpg", FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    return null;

                }
                catch (System.Exception e)
                {
                    return null;
                }
            }
            return null;

        }



        [HttpPost]
        [Route("SaveResidentAnswerDocuments")]
        public string SaveResidentAnswerDocuments(List<ResidentAnswerDocuments> lstResidentAnswerDocuments)
        {
            foreach (ResidentAnswerDocuments item in lstResidentAnswerDocuments)
            {
                string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers");
                string tempFolder = "temp";
                var provider = Path.Combine(root, tempFolder);

                string strFolderPath = string.Empty;
                try
                {
                    strFolderPath = Path.Combine(root, item.ResidentQuestionAnswerID.ToString());
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }
                    DeleteAllContentInDirectory(strFolderPath);

                    var base64dataParts = item.ResidentFile.Split(';');
                    var base64data = base64dataParts[base64dataParts.Length - 1].Substring(7);
                    var bytes = Convert.FromBase64String(base64data);

                    using (var imageFile = new FileStream(strFolderPath + "/" + item.FileName, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                        //File.Move(imageFile, Path.Combine(strFolderPath));
                    }
                }
                catch (System.Exception e)
                {
                    return null;
                }
            }
            return "Success";
        }


        //public string SaveResidentAnswerDocuments(ResidentAnswerDocuments objResidentAnswerDocuments)
        //{



        //       List<ResidentAnswerDocuments> lstResidentAnswerDocuments = new List<ResidentAnswerDocuments>();
        //        string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers");
        //        string tempFolder = "temp";
        //        var provider = Path.Combine(root, tempFolder);
        //        var item = objResidentAnswerDocuments;
        //        //foreach (var key in provider.FormData.AllKeys)
        //        //{
        //        //    if (key == "OfflineResidentDocuments")
        //        //    {
        //        //        foreach (var val in provider.FormData.GetValues(key))
        //        //        {
        //        //            lstResidentAnswerDocuments = JsonConvert.DeserializeObject<List<ResidentAnswerDocuments>>(val);
        //        //            break;
        //        //        }
        //        //        break;
        //        //    }
        //        //}


        //        string strFolderPath = string.Empty;
        //        try
        //        {
        //            var base64data = item.ResidentFile;
        //            strFolderPath = Path.Combine(root, item.ResidentQuestionAnswerID.ToString());
        //            if (!Directory.Exists(strFolderPath))
        //            {
        //                Directory.CreateDirectory(strFolderPath);
        //            }
        //            DeleteAllContentInDirectory(strFolderPath);

        //            var bytes = Convert.FromBase64String(base64data.Split(';')[1].Substring(7));
        //            using (var imageFile = new FileStream(strFolderPath + "\\fileName" + item.ResidentQuestionAnswerID.ToString() + ".jpg", FileMode.Create))
        //            {
        //                imageFile.Write(bytes, 0, bytes.Length);
        //                imageFile.Flush();
        //            }
        //            return null;

        //        }
        //        catch (System.Exception e)
        //        {
        //            return null;
        //        }

        //    return null;

        //}

        [HttpPost]
        [Route("SaveInterventionResidentAnswerDocument")]
        public string SaveInterventionResidentAnswerDocument(InterventionResidentAnswerDocument lstInterventionResidentAnswerDocument)
        {
            var item = lstInterventionResidentAnswerDocument;


            string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers");
            string tempFolder = "temp";
            var provider = Path.Combine(root, tempFolder);

            string strFolderPath = string.Empty;
            try
            {
                var base64data = item.InterventionAnsFile;
                strFolderPath = Path.Combine(root, item.InterventionResidentQuestionAnswerID.ToString());
                if (!Directory.Exists(strFolderPath))
                {
                    Directory.CreateDirectory(strFolderPath);
                }
                DeleteAllContentInDirectory(strFolderPath);

                var bytes = Convert.FromBase64String(base64data.Split(';')[1].Substring(7));
                using (var imageFile = new FileStream(strFolderPath + "\\fileName" + item.InterventionResidentQuestionAnswerID.ToString() + ".jpg", FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                return null;

            }
            catch (System.Exception e)
            {
                return null;
            }

            return null;

        }


        [HttpPost]
        [Route("SaveResidentAdhocInterventionDocuments")]
        public string SaveResidentAdhocInterventionDocuments(List<ResidentAdhocInterventionDocument> lstResidentAdhocInterventionDocuments)
        {
            foreach (ResidentAdhocInterventionDocument item in lstResidentAdhocInterventionDocuments)
            {
                string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Actions");
                string tempFolder = "temp";
                var provider = Path.Combine(root, tempFolder);

                string strFolderPath = string.Empty;
                try
                {
                    strFolderPath = Path.Combine(root, item.ActionID.ToString());
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }
                    DeleteAllContentInDirectory(strFolderPath);

                    var base64dataParts = item.AdhocInterventionFile.Split(';');
                    var base64data = base64dataParts[base64dataParts.Length - 1].Substring(7);
                    var bytes = Convert.FromBase64String(base64data);

                    using (var imageFile = new FileStream(strFolderPath + "/" + item.FileName, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                        //File.Move(imageFile, Path.Combine(strFolderPath));
                    }
                }
                catch (System.Exception e)
                {
                    return null;
                }
            }
            return "Success";
        }
       

        private void DeleteAllContentInDirectory(string strPath)
        {
            DirectoryInfo di = new DirectoryInfo(strPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        [HttpGet]
        [Route("GetMasterDataBasedonOrganization")]
        public async Task<IHttpActionResult> GetSectionBasedonOrganization(Guid guidOrganizationID)
        {

            List<Sections_Organizations> lstSectionOrganization = db.Sections_Organizations.Where(i => i.IsActive == true && i.OrganizationID == guidOrganizationID).ToList<Sections_Organizations>();
            List<Guid> GuidSectionIds = lstSectionOrganization.Select(i => i.SectionID).ToList<Guid>();


            foreach (Sections_Organizations item in lstSectionOrganization)
            {
                item.Organization = null; item.Section = null;
                item.User = null;

            }
            //sections 2
            List<Section> lstSection = db.Sections.Where(i => i.IsActive == true).ToList<Section>();
            foreach (Section item in lstSection)
            {
                item.Sections_Organizations = null;
            }
            List<Guid> GuidSectionID = lstSection.Select(i => i.ID).ToList<Guid>();

            ////sectionQuestion 3
            List<Sections_Questions> lstSectionQuestion = db.Sections_Questions.Where(i => GuidSectionID.Contains(i.SectionID) && i.IsActive == true).ToList<Sections_Questions>();
            List<Guid> GuidSectionQuestionIds = lstSectionQuestion.Select(i => i.ID).ToList<Guid>();


            foreach (Sections_Questions item in lstSectionQuestion)
            {
                item.Question_ParentQuestion1 = null;
                item.Question_ParentQuestion = null;
                item.User = null;
                item.Section = null;
                item.Sections_Questions_Answers = null;
                item.Sections_Questions_Answers_Summary = null;
                item.Sections_Questions_Answers_Tasks = null;
                item.Sections_Questions_Answers_Widget = null;
            }
            ////SEctionQuestion Answer 4
            List<Sections_Questions_Answers> lstSectionQuestionAnswer = db.Sections_Questions_Answers.Where(i => GuidSectionQuestionIds.Contains(i.Section_QuestionID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
            List<Guid> GuidSectionQuestionAnswerIDs = lstSectionQuestionAnswer.Select(i => i.ID).ToList<Guid>();


            foreach (Sections_Questions_Answers item in lstSectionQuestionAnswer)
            {
                item.Section = null;
                item.Sections_Questions = null;
                item.Sections_Questions_Answers_Summary = null;
                item.Sections_Questions_Answers_Tasks = null;
                item.Sections_Questions_Answers_Widget = null;
            }
            ////Sections_Questions_Answers_Tasks 5
            List<Sections_Questions_Answers_Tasks> lstSections_Questions_Answers_Tasks = db.Sections_Questions_Answers_Tasks.Where(i => GuidSectionQuestionIds.Contains(i.Section_QuestionID) && i.IsActive == true).ToList<Sections_Questions_Answers_Tasks>();
            List<Guid> GuidSectionInterventionsIDs = lstSections_Questions_Answers_Tasks.Select(i => i.Section_InterventionID).ToList<Guid>();

            foreach (Sections_Questions_Answers_Tasks item in lstSections_Questions_Answers_Tasks)
            {
                item.Section_Intervention = null;
                item.Sections_Questions_Answers = null;
                item.Sections_Questions = null;

            }

            //Sections_Questions_Answers_Summary 6
            List<Sections_Questions_Answers_Summary> lstSections_Questions_Answers_Summary = db.Sections_Questions_Answers_Summary.Where(i => GuidSectionQuestionIds.Contains(i.Section_QuestionID) && i.IsActive == true).ToList<Sections_Questions_Answers_Summary>();
            List<Guid> GuidSectionSeummaryId = lstSections_Questions_Answers_Summary.Select(i => i.SectionSummaryID).ToList<Guid>();

            foreach (Sections_Questions_Answers_Summary item in lstSections_Questions_Answers_Summary)
            {
                item.Sections_Questions = null;
                item.Sections_Questions_Answers = null;
                item.Section_Summary = null;

            }




            List<object> lstMasterData = new List<object>();
            var arrmasterData = new
            {
                objsectionOrganization = lstSectionOrganization,
                objSections = lstSection,
                SectionsQuestions = lstSectionQuestion,
                SectionsQuestionsAnswers = lstSectionQuestionAnswer,
                SectionsQuestions_AnswersTasks = lstSections_Questions_Answers_Tasks,
                SectionsQuestionsAnswersSummary = lstSections_Questions_Answers_Summary,
                //SectionsQuestionsAnswersWidget = lstSections_Questions_Answers_Widget,
                //QuestionParentQuestion = lstQuestion_ParentQuestion,
                //SectionIntervention = lstSection_Intervention,
                //SectionInterventionStatements = lstSection_Intervention_Statements,
                //SectionSummary = lstSection_Summary,
                //InterventionQuestion = lstIntervention_Question,
                //InterventionQuestionAnswer = lstIntervention_Question_Answer,
                //InterventionQuestionAnswerTask = lstIntervention_Question_Answer_Task,
                //InterventionsQuestionAnswerSummary = lstInterventions_Question_Answer_Summary,
                //InterventionQuestionParentQuestion = lstIntervention_Question_ParentQuestion
            };

            lstMasterData.Add(arrmasterData);



            return Ok(lstMasterData);
        }


        [HttpPost]
        [Route("MasterDatabasedonQuestionIds")]
        public async Task<IHttpActionResult> MasterDatabasedonQuestionIds(List<AllQuestionds> lstAllQuestions)
        {
            List<Guid> GuidSectionQuestionIds = lstAllQuestions.Select(i => i.ID).ToList<Guid>();


            List<Sections_Questions_Answers_Tasks> lstSections_Questions_Answers_Tasks = db.Sections_Questions_Answers_Tasks.Where(i => GuidSectionQuestionIds.Contains(i.Section_QuestionID) && i.IsActive == true).ToList<Sections_Questions_Answers_Tasks>();
            List<Guid> GuidSectionInterventionsIDs = lstSections_Questions_Answers_Tasks.Select(i => i.Section_InterventionID).ToList<Guid>();

            foreach (Sections_Questions_Answers_Tasks item in lstSections_Questions_Answers_Tasks)
            {
                item.Section_Intervention = null;
                item.Sections_Questions_Answers = null;
                item.Sections_Questions = null;

            }

            //Sections_Questions_Answers_Summary 6
            List<Sections_Questions_Answers_Summary> lstSections_Questions_Answers_Summary = db.Sections_Questions_Answers_Summary.Where(i => GuidSectionQuestionIds.Contains(i.Section_QuestionID) && i.IsActive == true).ToList<Sections_Questions_Answers_Summary>();
            List<Guid> GuidSectionSeummaryId = lstSections_Questions_Answers_Summary.Select(i => i.SectionSummaryID).ToList<Guid>();

            foreach (Sections_Questions_Answers_Summary item in lstSections_Questions_Answers_Summary)
            {
                item.Sections_Questions = null;
                item.Sections_Questions_Answers = null;
                item.Section_Summary = null;

            }

            //Sections_Questions_Answers_Widget 7
            List<Sections_Questions_Answers_Widget> lstSections_Questions_Answers_Widget = await db.Sections_Questions_Answers_Widget.Where(i => GuidSectionQuestionIds.Contains(i.Section_QuestionID) && i.IsActive == true).ToListAsync<Sections_Questions_Answers_Widget>();

            foreach (Sections_Questions_Answers_Widget item in lstSections_Questions_Answers_Widget)
            {
                item.Sections_Questions = null;
                item.Sections_Questions_Answers = null;

            }

            //Question_ParentQuestion 8
            List<Question_ParentQuestion> lstQuestion_ParentQuestion = db.Question_ParentQuestion.Where(i => GuidSectionQuestionIds.Contains(i.QuestionID) && i.IsActive == true).ToList<Question_ParentQuestion>();

            foreach (Question_ParentQuestion item in lstQuestion_ParentQuestion)
            {
                item.Sections_Questions = null;
                item.Sections_Questions_Answers = null;
            }

            //Section_Intervention 9
            List<Section_Intervention> lstSection_Intervention = db.Section_Intervention.Where(i => GuidSectionInterventionsIDs.Contains(i.ID) && i.IsActive == true).ToList<Section_Intervention>();
            List<Guid> GuidSectionInterventionIDs = lstSection_Intervention.Select(i => i.ID).ToList<Guid>();
            foreach (Section_Intervention item in lstSection_Intervention)
            {

                item.Section_Intervention_Statements = null;
                item.Sections_Questions_Answers_Tasks = null;
                item.Section_Intervention1 = null;
                item.Section_Intervention2 = null;

            }


            //Section_Intervention_Statements 10
            List<Section_Intervention_Statements> lstSection_Intervention_Statements = db.Section_Intervention_Statements.Where(i => GuidSectionInterventionIDs.Contains(i.Section_InterventionID) && i.IsActive == true).ToList<Section_Intervention_Statements>();

            foreach (Section_Intervention_Statements item in lstSection_Intervention_Statements)
            {
                item.Section_Intervention = null;

            }

            //Section_Summary 11
            List<Section_Summary> lstSection_Summary = db.Section_Summary.Where(i => GuidSectionSeummaryId.Contains(i.ID) && i.IsActive == true).ToList<Section_Summary>();

            foreach (Section_Summary item in lstSection_Summary)
            {
                item.Sections_Questions_Answers_Summary = null;
                item.Interventions_Question_Answer_Summary = null;
            }

            //Intervention_Question 12
            List<Intervention_Question> lstIntervention_Question = db.Intervention_Question.Where(i => GuidSectionInterventionIDs.Contains(i.Section_InterventionID)).ToList<Intervention_Question>();
            List<Guid> GuidInterventionQuestionIDs = lstIntervention_Question.Select(i => i.ID).ToList<Guid>();

            foreach (Intervention_Question item in lstIntervention_Question)
            {
                item.Section_Intervention = null;
                item.Intervention_Question_Answer = null;
                item.Intervention_Question_Answer_Task = null;
                item.Intervention_Question_ParentQuestion = null;
                item.Intervention_Question_ParentQuestion1 = null;
                item.Interventions_Question_Answer_Summary = null;

            }
            //Intervention_Question_Answer 13
            List<Intervention_Question_Answer> lstIntervention_Question_Answer = db.Intervention_Question_Answer.Where(i => GuidInterventionQuestionIDs.Contains(i.Intervention_QuestionID) && i.IsActive == true).ToList<Intervention_Question_Answer>();

            foreach (Intervention_Question_Answer item in lstIntervention_Question_Answer)
            {
                item.Intervention_Question = null;
                item.Intervention_Question_Answer_Task = null;
                item.Intervention_Question_ParentQuestion = null;
                item.Interventions_Question_Answer_Summary = null;
                item.Interventions_Resident_Answers = null;
                item.Section_Intervention = null;
                item.Resident_Interventions_Questions_Answers = null;

            }

            //Intervention_Question_Answer_Task 14
            List<Intervention_Question_Answer_Task> lstIntervention_Question_Answer_Task = db.Intervention_Question_Answer_Task.Where(i => GuidInterventionQuestionIDs.Contains(i.InterventionQuestionID) && i.IsActive == true).ToList<Intervention_Question_Answer_Task>();
            foreach (Intervention_Question_Answer_Task item in lstIntervention_Question_Answer_Task)
            {
                item.Intervention_Question = null;
                item.Intervention_Question_Answer = null;
                item.Section_Intervention = null;

            }

            //Interventions_Question_Answer_Summary 15
            List<Interventions_Question_Answer_Summary> lstInterventions_Question_Answer_Summary = db.Interventions_Question_Answer_Summary.Where(i => GuidInterventionQuestionIDs.Contains(i.InterventionQuestionID) && i.IsActive == true).ToList<Interventions_Question_Answer_Summary>();

            foreach (Interventions_Question_Answer_Summary item in lstInterventions_Question_Answer_Summary)
            {
                item.Intervention_Question = null;
                item.Intervention_Question_Answer = null;
                item.Section_Summary = null;

            }

            //Intervention_Question_ParentQuestion 16
            List<Intervention_Question_ParentQuestion> lstIntervention_Question_ParentQuestion = db.Intervention_Question_ParentQuestion.Where(i => GuidInterventionQuestionIDs.Contains(i.InterventionQuestionID) && i.IsActive == true).ToList<Intervention_Question_ParentQuestion>();

            foreach (Intervention_Question_ParentQuestion item in lstIntervention_Question_ParentQuestion)
            {
                item.Intervention_Question = null;
                item.Intervention_Question_Answer = null;

            }

            List<object> lstMasterData = new List<object>();
            var arrmasterData = new
            {
                //objSections = lstSection,
                //SectionsQuestions = lstSectionQuestion,
                //SectionsQuestionsAnswers = lstSectionQuestionAnswer,
                //SectionsQuestions_AnswersTasks = lstSections_Questions_Answers_Tasks,
                //SectionsQuestionsAnswersSummary = lstSections_Questions_Answers_Summary,
                SectionsQuestionsAnswersWidget = lstSections_Questions_Answers_Widget,
                QuestionParentQuestion = lstQuestion_ParentQuestion,
                SectionIntervention = lstSection_Intervention,
                SectionInterventionStatements = lstSection_Intervention_Statements,
                SectionSummary = lstSection_Summary,
                InterventionQuestion = lstIntervention_Question,
                InterventionQuestionAnswer = lstIntervention_Question_Answer,
                InterventionQuestionAnswerTask = lstIntervention_Question_Answer_Task,
                InterventionsQuestionAnswerSummary = lstInterventions_Question_Answer_Summary,
                InterventionQuestionParentQuestion = lstIntervention_Question_ParentQuestion
            };

            lstMasterData.Add(arrmasterData);
            return Ok(lstMasterData);
        }



        [HttpGet]
        [Route("GetResidentsDataBasedonOrganization")]
        public async Task<IHttpActionResult> GetResidentsDataBasedonOrganization(Guid guidOrganizationID, DateTime startDate, DateTime endDate)
        {

            List<Resident> lstResident = db.Residents.Where(i => i.OrganizationID == guidOrganizationID).ToList<Resident>();
            List<Guid> GuidResidentIds = lstResident.Select(i => i.ID).ToList<Guid>();
            List<object> lstFiles = new List<object>();
            foreach (var item in lstResident)
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos")))
                {
                    if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos/" + item.ID + "")))
                    {
                        string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos/" + item.ID + ""));

                        if (fileEntries.Length > 0)
                        {
                            FileInfo myFileInfo = new FileInfo(fileEntries[0]);
                            var objResidentDoc = new
                            {
                                objResident = item,
                                Filedata = "/Uploads/Residents/Photos/" + item.ID + "/" + myFileInfo.Name
                            };
                            lstFiles.Add(objResidentDoc);
                        }
                        else
                        {
                            var objResidentDoc = new
                            {
                                objResident = item
                            };
                            lstFiles.Add(objResidentDoc);
                        }
                    }
                    else
                    {
                        var objResidentDoc = new
                        {
                            objResident = item
                        };
                        lstFiles.Add(objResidentDoc);
                    }
                }
            }
            foreach (Resident item in lstResident)
            {
                item.Interventions_Resident_Answers = null;
                item.Resident_Interventions_Questions_Answers = null;
                item.Residents_Questions_Answers = null;
                item.Organization = null;
                item.Residents_Relatives = null;
                item.User = null;
            }

            //cHM Actions 1
            List<CHM.Services.Models.Action> lstActions = db.Actions.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<CHM.Services.Models.Action>();
            List<Guid> GuidActionIds = lstActions.Select(i => i.ID).ToList<Guid>();

            foreach (CHM.Services.Models.Action item in lstActions)
            {
                item.Resident = null;
                item.Section_Intervention = null;

            }

            //CHM Actions_Days 2
            List<Actions_Days> lstActions_Days = db.Actions_Days.Where(i => GuidActionIds.Contains(i.ActionID) && i.IsActive == true).ToList<Actions_Days>();
            List<Guid> GuidActionDays = lstActions_Days.Select(i => i.ID).ToList<Guid>();

            foreach (Actions_Days item in lstActions_Days)
            {
                item.Interventions = null;
                item.User = null;

            }
            //Interventions 3
            List<Intervention> lstInterventions = db.Interventions.Where(i => GuidActionDays.Contains(i.Action_DayID) && i.IsActive == true && (startDate >= i.PlannedStartDate) || (i.PlannedStartDate <= endDate)).ToList<Intervention>();
            List<Guid> GuidInterventionsID = lstInterventions.Select(i => i.ID).ToList<Guid>();

            foreach (Intervention item in lstInterventions)
            {
                item.Interventions_Resident_Answers = null;
                item.User = null;
            }


            //Interventions_Resident_Answers 4

            List<Interventions_Resident_Answers> lstInterventions_Resident_Answers = db.Interventions_Resident_Answers.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<Interventions_Resident_Answers>();


            foreach (Interventions_Resident_Answers item in lstInterventions_Resident_Answers)
            {
                item.Intervention_Question_Answer = null;
                item.Resident = null;
                item.User = null;
                item.Intervention = null;

            }
            ////Residents_Questions_Answers 5
            //List<Residents_Questions_Answers> lstResidents_Questions_Answers = db.Residents_Questions_Answers.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<Residents_Questions_Answers>();

            //List<object> lstResidentAnswersDocuments = new List<object>();
            //foreach (var item in lstResidents_Questions_Answers)
            //{
            //    if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers")))
            //    {
            //        if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + item.ID + "")))
            //        {
            //            string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + item.ID + ""));

            //            if (fileEntries.Length > 0)
            //            {
            //                var objResidentDoc = new
            //                {
            //                    objResidents_Questions_Answers = item,
            //                    Filedata = fileEntries[0]
            //                    //Filedata = File.ReadAllBytes(fileEntries[0]),
            //                    //Extention = Path.GetExtension(fileEntries[0]),
            //                    //FileName = Path.GetFileName(fileEntries[0])
            //                };
            //                lstResidentAnswersDocuments.Add(objResidentDoc);
            //            }
            //            else
            //            {
            //                var objResidentDoc = new
            //                {
            //                    objResidents_Questions_Answers = item
            //                };
            //                lstResidentAnswersDocuments.Add(objResidentDoc);
            //            }

            //        }
            //        else
            //        {
            //            var objResidentDoc = new
            //            {
            //                objResidents_Questions_Answers = item
            //            };
            //            lstResidentAnswersDocuments.Add(objResidentDoc);
            //        }
            //    }
            //}

            //foreach (Residents_Questions_Answers item in lstResidents_Questions_Answers)
            //{
            //    item.Resident = null;
            //    item.Sections_Questions_Answers = null;
            //    item.User = null;

            //}
            //Residents_Relatives 6

            List<Residents_Relatives> lstResidents_Relatives = db.Residents_Relatives.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<Residents_Relatives>();


            foreach (Residents_Relatives item in lstResidents_Relatives)
            {
                item.Resident = null;
                item.User = null;

            }

            //Pain Monitoring 7
            List<PainMonitoring> lstPainMonitoring = db.PainMonitorings.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<PainMonitoring>();


            foreach (PainMonitoring item in lstPainMonitoring)
            {
                item.Resident = null;
                item.User = null;

            }
            ////Interventions_Resident_Answers 7
            //List<Resident_Interventions_Questions_Answers> lstResident_Interventions_Questions_Answers = db.Resident_Interventions_Questions_Answers.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<Resident_Interventions_Questions_Answers>();

            //List<object> lstResidentInterventionsQuestionsAnswers = new List<object>();
            //foreach (var item in lstResident_Interventions_Questions_Answers)
            //{
            //    if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers")))
            //    {
            //        if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + item.ID + "")))
            //        {
            //            string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + item.ID + ""));

            //            if (fileEntries.Length > 0)
            //            {
            //                var objResidentInterventionDoc = new
            //                {
            //                    objResident_Interventions_Questions_Answers = item,
            //                    Filedata = File.ReadAllBytes(fileEntries[0]),
            //                    Extention = Path.GetExtension(fileEntries[0]),
            //                    FileName = Path.GetFileName(fileEntries[0])
            //                };
            //                lstResidentInterventionsQuestionsAnswers.Add(objResidentInterventionDoc);
            //            }
            //            else
            //            {
            //                var objResidentInterventionDoc = new
            //                {
            //                    objResident_Interventions_Questions_Answers = item
            //                };
            //                lstResidentInterventionsQuestionsAnswers.Add(objResidentInterventionDoc);
            //            }

            //        }
            //        else
            //        {
            //            var objResidentInterventionDoc = new
            //            {
            //                objResident_Interventions_Questions_Answers = item
            //            };
            //            lstResidentInterventionsQuestionsAnswers.Add(objResidentInterventionDoc);
            //        }
            //    }
            //}


            //foreach (Resident_Interventions_Questions_Answers item in lstResident_Interventions_Questions_Answers)
            //{
            //    item.Intervention_Question_Answer = null;
            //    item.Resident = null;
            //    item.User = null;
            //}
            //8

            List<object> lstResidentsData = new List<object>();

            var ResidentData = new
            {
                Resident = lstFiles,
                Actions = lstActions,
                ActionDays = lstActions_Days,
                Intervention = lstInterventions,
                InterventionResidentAnswers = lstInterventions_Resident_Answers,
                // ResidentQuestionAnswer = lstResidentAnswersDocuments,
                ResidentRelatives = lstResidents_Relatives,
                PainMonitoring = lstPainMonitoring

                // ResidentInterventionQuestionAnswers = lstResidentInterventionsQuestionsAnswers
            };

            lstResidentsData.Add(ResidentData);
            return Ok(lstResidentsData);
        }

        [HttpGet]
        [Route("GetResidentsAnswerDocumentsBasedonOrganization")]
        public async Task<IHttpActionResult> GetResidentsAnswerDocumentsBasedonORganization(Guid guidOrganizationID)
        {

            List<Resident> lstResident = db.Residents.Where(i => i.OrganizationID == guidOrganizationID && i.IsActive == true).ToList<Resident>();
            List<Guid> GuidResidentIds = lstResident.Select(i => i.ID).ToList<Guid>();

            foreach (Resident item in lstResident)
            {
                item.Interventions_Resident_Answers = null;
                item.Resident_Interventions_Questions_Answers = null;
                item.Residents_Questions_Answers = null;
                item.Organization = null;
                item.Residents_Relatives = null;
                item.User = null;
            }

            //Residents_Questions_Answers 5
            List<Residents_Questions_Answers> lstResidents_Questions_Answers = db.Residents_Questions_Answers.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<Residents_Questions_Answers>();

            List<object> lstResidentAnswersDocuments = new List<object>();
            foreach (var item in lstResidents_Questions_Answers)
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
                                Filedata = "/Uploads/Residents/Answers/" + item.ID + "/" + myFileInfo.Name
                                //Filedata = File.ReadAllBytes(fileEntries[0]),
                                //Extention = Path.GetExtension(fileEntries[0]),
                                //FileName = Path.GetFileName(fileEntries[0])
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

            foreach (Residents_Questions_Answers item in lstResidents_Questions_Answers)
            {
                item.Resident = null;
                item.Sections_Questions_Answers = null;
                item.User = null;

            }

            return Ok(lstResidentAnswersDocuments);
        }


        [HttpGet]
        [Route("GetInterventionAnswerDocumentsBasedonOrganization")]
        public async Task<IHttpActionResult> GetInterventionAnswerDocumentsBasedonOrganization(Guid guidOrganizationID)
        {

            List<Resident> lstResident = db.Residents.Where(i => i.OrganizationID == guidOrganizationID && i.IsActive == true).ToList<Resident>();
            List<Guid> GuidResidentIds = lstResident.Select(i => i.ID).ToList<Guid>();

            foreach (Resident item in lstResident)
            {
                item.Interventions_Resident_Answers = null;
                item.Resident_Interventions_Questions_Answers = null;
                item.Residents_Questions_Answers = null;
                item.Organization = null;
                item.Residents_Relatives = null;
                item.User = null;
            }


            //Interventions_Resident_Answers 7
            List<Resident_Interventions_Questions_Answers> lstResident_Interventions_Questions_Answers = db.Resident_Interventions_Questions_Answers.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<Resident_Interventions_Questions_Answers>();

            List<object> lstResidentInterventionsQuestionsAnswers = new List<object>();
            foreach (var item in lstResident_Interventions_Questions_Answers)
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
                                Extention = Path.GetExtension(fileEntries[0]),
                                FileName = Path.GetFileName(fileEntries[0])
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


            foreach (Resident_Interventions_Questions_Answers item in lstResident_Interventions_Questions_Answers)
            {
                item.Intervention_Question_Answer = null;
                item.Resident = null;
                item.User = null;
            }


            return Ok(lstResidentInterventionsQuestionsAnswers);
        }

        [HttpGet]
        [Route("GetAdhocInterventionAnswerDocumentsBasedonOrganization")]
        public async Task<IHttpActionResult> GetAdhocInterventionAnswerDocumentsBasedonOrganization(Guid guidOrganizationID)
        {

            List<Resident> lstResident = db.Residents.Where(i => i.OrganizationID == guidOrganizationID && i.IsActive == true).ToList<Resident>();
            List<Guid> GuidResidentIds = lstResident.Select(i => i.ID).ToList<Guid>();

            foreach (Resident item in lstResident)
            {
                item.Interventions_Resident_Answers = null;
                item.Resident_Interventions_Questions_Answers = null;
                item.Residents_Questions_Answers = null;
                item.Organization = null;
                item.Residents_Relatives = null;
                item.User = null;
            }


            //Interventions_Resident_Answers 7
            List<CHM.Services.Models.Action> lstActions = db.Actions.Where(i => GuidResidentIds.Contains(i.ResidentID) && i.IsActive == true).ToList<CHM.Services.Models.Action>();

            List<object> lstAction = new List<object>();
            foreach (var item in lstActions)
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Actions")))
                {
                    if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Actions/" + item.ID + "")))
                    {
                        string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Actions/" + item.ID + ""));

                        if (fileEntries.Length > 0)
                        {
                            var objResidentInterventionDoc = new
                            {
                                objResident_AdhocInterventions_FileData = item,
                                Filedata = File.ReadAllBytes(fileEntries[0]),
                                Extention = Path.GetExtension(fileEntries[0]),
                                FileName = Path.GetFileName(fileEntries[0])
                            };
                            lstAction.Add(objResidentInterventionDoc);
                        }
                        else
                        {
                            var objResidentInterventionDoc = new
                            {
                                objResident_AdhocInterventions_FileData = item
                            };
                            lstAction.Add(objResidentInterventionDoc);
                        }

                    }
                    else
                    {
                        var objResidentInterventionDoc = new
                        {
                            objResident_AdhocInterventions_FileData = item
                        };
                        lstAction.Add(objResidentInterventionDoc);
                    }
                }
            }


            foreach (var item in lstActions)

            {
                //item.Intervention_Question_Answer = null;
                item.Resident = null;
                item.User = null;
            }


            return Ok(lstAction);
        }


        [Route("SaveofflinePainMonitoring")]
        public async Task<IHttpActionResult> SaveofflinePainMonitoring(List<PainMonitoring> lstPainMonitoring)
        {

            foreach (PainMonitoring objPainMonitoring in lstPainMonitoring)
            {
                PainMonitoring objChkIsExistingPainMonitoring = await db.PainMonitorings.FindAsync(objPainMonitoring.ID);
                if (objChkIsExistingPainMonitoring == null)
                {
                    db.PainMonitorings.Add(objPainMonitoring);
                    await db.SaveChangesAsync();
                }
            }

            return Ok();
        }

        [Route("UpdateofflinePainMonitoring")]

        public async Task<IHttpActionResult> UpdateofflinePainMonitoring(List<PainMonitoring> lstPainMonitoring)
        {
            // Guid userId = new Guid(User.Identity.GetUserId());
            foreach (PainMonitoring objPain in lstPainMonitoring)
            {
                PainMonitoring objPainMonitoring = await db.PainMonitorings.FindAsync(objPain.ID);
                //objResident.ModifiedBy = userId;
                if (objPainMonitoring != null)
                {
                    objPainMonitoring.IsActive = objPain.IsActive.ToString().ToUpper() == "TRUE" ? true : false;
                    objPainMonitoring.Description = objPain.Description;
                    objPainMonitoring.Modified = DateTime.Now;
                    db.Entry(objPainMonitoring).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

            }

            return Ok();
        }

        public class ResidentPhotos
        {
            public Guid ResidentID { get; set; }
            public string PhotoUrl { get; set; }


        }

        public class ResidentAnswerDocuments
        {
            public string FileName { get; set; }
            public Guid ResidentQuestionAnswerID { get; set; }
            public string ResidentFile { get; set; }


        }

        public class InterventionResidentAnswerDocument
        {
            public Guid ResidentID { get; set; }
            public Guid InterventionResidentQuestionAnswerID { get; set; }
            public string InterventionAnsFile { get; set; }
        }

        public class ResidentAdhocInterventionDocument
        {
            public string FileName { get; set; }
            public Guid ActionID { get; set; }
            public string AdhocInterventionFile { get; set; }
        }

        public class AllQuestionds
        {
            public Guid ID { get; set; }
        }
        public class clsMasterdataofOrganization
        {
            public virtual List<Section> Section { get; set; }
            public virtual List<Sections_Questions> Sections_Questions { get; set; }
            public virtual List<Sections_Questions_Answers> Sections_Questions_Answers { get; set; }
            public virtual List<Sections_Questions_Answers_Tasks> Sections_Questions_Answers_Tasks { get; set; }
            public virtual List<Sections_Questions_Answers_Summary> Sections_Questions_Answers_Summary { get; set; }
            public virtual List<Question_ParentQuestion> Question_ParentQuestion { get; set; }
            public virtual List<Sections_Questions_Answers_Widget> Sections_Questions_Answers_Widget { get; set; }
            public virtual List<Section_Intervention> Section_Intervention { get; set; }
            public virtual List<Section_Intervention_Statements> Section_Intervention_Statements { get; set; }
            public virtual List<Section_Summary> Section_Summary { get; set; }

            public virtual List<Intervention_Question> Intervention_Question { get; set; }
            public virtual List<Intervention_Question_Answer> Intervention_Question_Answer { get; set; }
            public virtual List<Intervention_Question_Answer_Task> Intervention_Question_Answer_Task { get; set; }
            public virtual List<Interventions_Question_Answer_Summary> Interventions_Question_Answer_Summary { get; set; }
            public virtual List<Intervention_Question_ParentQuestion> Intervention_Question_ParentQuestion { get; set; }

        }
    }
}
