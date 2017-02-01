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
using System.Drawing.Imaging;

namespace CHM.Services.Controllers
{
    //[Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Residents")]
    public class ResidentsController : ApiController
    {
        private CHMEntities db = new CHMEntities();

        [Route("GetActiveResidents")]
        public async Task<IHttpActionResult> GetActiveResidents()
        {
            List<Resident> lstResidents = await db.Residents.Where(obj => obj.IsActive == true).ToListAsync<Resident>();

            List<ResidentWithPhoto> lstResidentsWithPhoto = new List<ResidentWithPhoto>();

            foreach (Resident objResident in lstResidents)
            {
                ResidentWithPhoto objResidentWithPhoto = new ResidentWithPhoto();
                objResidentWithPhoto.Resident = objResident;
                objResidentWithPhoto.PhotoUrl = GetPhotoRelativeUrl(objResident.ID);

                lstResidentsWithPhoto.Add(objResidentWithPhoto);
            }


            return Ok(lstResidentsWithPhoto);
        }
        [HttpGet]
        [Route("GetActiveResidentsByOrganizationID")]
        public async Task<IHttpActionResult> GetActiveResidentsByOrganizationID(Guid OrganizationID)
        {

            try
            {
                List<Resident> lstResidents = await db.Residents.Where(obj => obj.IsActive == true && obj.OrganizationID == OrganizationID).ToListAsync<Resident>();

                List<ResidentWithPhoto> lstResidentsWithPhoto = new List<ResidentWithPhoto>();

                foreach (Resident objResident in lstResidents)
                {
                    ResidentWithPhoto objResidentWithPhoto = new ResidentWithPhoto();
                    objResidentWithPhoto.Resident = objResident;
                    objResidentWithPhoto.PhotoUrl = GetPhotoRelativeUrl(objResident.ID);

                    lstResidentsWithPhoto.Add(objResidentWithPhoto);
                }


                return Ok(lstResidentsWithPhoto);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }



        [HttpGet]
        [Route("GetAllResidentsByOrganizationID")]
        public async Task<IHttpActionResult> GetAllResidentsByOrganizationID(Guid OrganizationID)
        {

            try
            {
                List<Resident> lstResidents = await db.Residents.Where(obj => obj.OrganizationID == OrganizationID).ToListAsync<Resident>();

                List<ResidentWithPhoto> lstResidentsWithPhoto = new List<ResidentWithPhoto>();

                foreach (Resident objResident in lstResidents)
                {
                    ResidentWithPhoto objResidentWithPhoto = new ResidentWithPhoto();
                    objResidentWithPhoto.Resident = objResident;
                    objResidentWithPhoto.PhotoUrl = GetPhotoRelativeUrl(objResident.ID);

                    lstResidentsWithPhoto.Add(objResidentWithPhoto);
                }


                return Ok(lstResidentsWithPhoto);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Route("GetResident")]
        public async Task<IHttpActionResult> GetResident(Guid residentId)
        {
            Resident objResident = await db.Residents.Where(obj => obj.ID == residentId).FirstOrDefaultAsync();

            ResidentWithPhoto objResidentWithPhoto = new ResidentWithPhoto();
            objResidentWithPhoto.Resident = objResident;
            objResidentWithPhoto.PhotoUrl = GetPhotoRelativeUrl(objResident.ID);


            return Ok(objResidentWithPhoto);
        }

        [HttpPost]
        [Route("DeleteResidents")]
        public async Task<IHttpActionResult> DeleteResidents(Resident objResident)
        {
            Resident objDeleteResidents = db.Residents.Find(objResident.ID);

            objDeleteResidents.IsActive = false;
            objDeleteResidents.Modified = DateTime.Now;


            List<CHM.Services.Models.Action> lstActions = db.Actions.Where(i => i.ResidentID == objResident.ID && i.IsActive == true).ToList<CHM.Services.Models.Action>();
            foreach (var objActions in lstActions)
            {
                objActions.IsActive = false;
                objActions.Modified = DateTime.Now;
                db.Entry(objActions).State = EntityState.Modified;
            }

            List<Interventions_Resident_Answers> lstInterventionsResidentAnswers = db.Interventions_Resident_Answers.Where(i => i.ResidentID == objResident.ID && i.IsActive == true).ToList<Interventions_Resident_Answers>();
            foreach (var objInterventionsResidentAnswers in lstInterventionsResidentAnswers)
            {
                objInterventionsResidentAnswers.IsActive = false;
                objInterventionsResidentAnswers.Modified = DateTime.Now;
                db.Entry(objInterventionsResidentAnswers).State = EntityState.Modified;
            }


            List<Resident_Interventions_Questions_Answers> lstResidentInterventionsQuestionsAnswers = db.Resident_Interventions_Questions_Answers.Where(i => i.ResidentID == objResident.ID && i.IsActive == true).ToList<Resident_Interventions_Questions_Answers>();
            foreach (var objResidentInterventionsQuestionsAnswers in lstResidentInterventionsQuestionsAnswers)
            {
                objResidentInterventionsQuestionsAnswers.IsActive = false;
                objResidentInterventionsQuestionsAnswers.Modified = DateTime.Now;
                db.Entry(objResidentInterventionsQuestionsAnswers).State = EntityState.Modified;
            }


            List<Residents_Questions_Answers> lstResidentsQuestionsAnswers = db.Residents_Questions_Answers.Where(i => i.ResidentID == objResident.ID && i.IsActive == true).ToList<Residents_Questions_Answers>();
            foreach (var objResidentsQuestionsAnswers in lstResidentsQuestionsAnswers)
            {
                objResidentsQuestionsAnswers.IsActive = false;
                objResidentsQuestionsAnswers.Modified = DateTime.Now;
                db.Entry(objResidentsQuestionsAnswers).State = EntityState.Modified;
            }


            List<Residents_Relatives> lstResidentsRelatives = db.Residents_Relatives.Where(i => i.ResidentID == objResident.ID && i.IsActive == true).ToList<Residents_Relatives>();
            foreach (var objResidentsRelatives in lstResidentsRelatives)
            {
                objResidentsRelatives.IsActive = false;
                objResidentsRelatives.Modified = DateTime.Now;
                db.Entry(objResidentsRelatives).State = EntityState.Modified;
            }

            List<PainMonitoring> lstPainMonitoring = db.PainMonitorings.Where(i => i.ResidentID == objResident.ID).ToList<PainMonitoring>();
            foreach (var objPainMonitoring in lstPainMonitoring)
            {
                objPainMonitoring.IsActive = false;
                objPainMonitoring.Modified = DateTime.Now;
                db.Entry(objPainMonitoring).State = EntityState.Modified;
            }


            db.Entry(objDeleteResidents).State = EntityState.Modified;
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
        [Route("GetResidentSummary")]
        public async Task<IHttpActionResult> GetResidentSummary(Guid residentId)
        {
            List<Residents_Questions_Answers> lstResidentSummary = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive == true && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();
            //foreach (Residents_Questions_Answers objResidentQuestionAnswers in lstResidentSummary)
            //{

            //    objResidentQuestionAnswers.Sections_Questions_Answers.Sections_Questions_Answers_Summary = objResidentQuestionAnswers.Sections_Questions_Answers.Sections_Questions_Answers_Summary.Where(obj => obj.IsActive).ToList<Sections_Questions_Answers_Summary>();

            //}


            List<Section_Summary> GetlstResidentSectionQuestionAnsTask = new List<Section_Summary>();
            List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
            lstResidentAnsweredQuestions = lstResidentSummary;

            List<Sections_Questions_Answers_Summary> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Summary>();
            lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Summary.Include(obj => obj.Section_Summary).Where(obj => obj.IsActive && obj.Section_Summary.IsActive).ToListAsync<Sections_Questions_Answers_Summary>();





            List<Sections_Questions_Answers_Summary> lstSectionQuestionAnswerHasParentAnswerID = new List<Sections_Questions_Answers_Summary>();
            List<Sections_Questions_Answers_Summary> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Sections_Questions_Answers_Summary>();
            foreach (Sections_Questions_Answers_Summary objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
            {
                if (objSecQuesAnsTask.Section_Question_AnswerID != null)
                    lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                else
                    lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
            }


            foreach (Residents_Questions_Answers objResidentQuestionAnswer in lstResidentAnsweredQuestions)
            {
                foreach (Sections_Questions_Answers_Summary objSectionQuestionAnsTask in lstSectionQuestionAnswerHasParentAnswerID)
                {
                    if (objResidentQuestionAnswer.Section_Question_AnswerID == objSectionQuestionAnsTask.Section_Question_AnswerID)
                    {
                        if (objSectionQuestionAnsTask.Section_Summary.Summary == null)
                        {
                            if (objResidentQuestionAnswer.AnswerText != null)
                            {
                                objSectionQuestionAnsTask.Section_Summary.Summary = objResidentQuestionAnswer.AnswerText;
                                GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask.Section_Summary);
                            }
                        }
                        else
                        {
                            List<Guid> lstQuesParentQuestion = new List<Guid>();
                            lstQuesParentQuestion = db.Question_ParentQuestion.Where(i => i.ParentAnswerID == objResidentQuestionAnswer.Section_Question_AnswerID && i.IsActive == true).Select(i => i.QuestionID).ToList<Guid>();

                            List<Guid> lstSecQuesAnsID = db.Sections_Questions_Answers.Where(i => lstQuesParentQuestion.Contains(i.Section_QuestionID) && i.IsActive == true).Select(i => i.ID).ToList<Guid>();

                            List<Residents_Questions_Answers> lstResQueAns = db.Residents_Questions_Answers.Where(i => lstSecQuesAnsID.Contains(i.Section_Question_AnswerID) && i.IsActive == true && i.ResidentID == residentId).ToList<Residents_Questions_Answers>();

                            if (lstQuesParentQuestion.Count == 0 && lstResQueAns.Count == 0)
                            {
                                lstResQueAns = db.Residents_Questions_Answers.Where(i => i.Section_Question_AnswerID == objResidentQuestionAnswer.Section_Question_AnswerID && i.IsActive == true && i.ResidentID == residentId).ToList<Residents_Questions_Answers>();
                            }


                            string CarrerCount = "1";
                            string AidName = "None";
                            foreach (Residents_Questions_Answers obj in lstResQueAns)
                            {
                                if (obj.AnswerText != null)
                                {
                                    CarrerCount = obj.AnswerText;
                                }
                                else
                                {
                                    AidName = obj.Sections_Questions_Answers.LabelText;
                                }
                            }

                            string carrer = "XYZ";
                            string aid = "XXXX";
                            int hasXYZ = objSectionQuestionAnsTask.Section_Summary.Summary.IndexOf(carrer);
                            int hasXXX = objSectionQuestionAnsTask.Section_Summary.Summary.IndexOf(aid);

                            if (hasXYZ != -1 && hasXXX == -1)
                            {
                                string output = objSectionQuestionAnsTask.Section_Summary.Summary.Replace("XYZ", CarrerCount);
                                objSectionQuestionAnsTask.Section_Summary.Summary = output;
                            }
                            else if (hasXXX != -1 && hasXYZ == -1)
                            {

                                string output = objSectionQuestionAnsTask.Section_Summary.Summary.Replace("XXXX", AidName);
                                objSectionQuestionAnsTask.Section_Summary.Summary = output;
                            }
                            else if (hasXYZ != -1 && hasXXX != -1)
                            {
                                string output1 = objSectionQuestionAnsTask.Section_Summary.Summary.Replace("XYZ", CarrerCount);
                                string output2 = output1.Replace("XXXX", AidName);
                                objSectionQuestionAnsTask.Section_Summary.Summary = output2;
                            }
                            else
                            {
                                string output1 = objSectionQuestionAnsTask.Section_Summary.Summary.Replace("XYZ", "1");
                                string output2 = output1.Replace("XXXX", "None");
                                objSectionQuestionAnsTask.Section_Summary.Summary = output2;
                            }

                            GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask.Section_Summary);


                            //}
                            //else
                            //{
                            //    GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask.Section_Summary);
                            //}
                        }
                    }
                }
            }

            //var UniqueInterventionID = lstSectionQuestionAnswerHasNoParentAnswerID.Select(x => new { x.Section_InterventionID}).Distinct();

            // IEnumerable<Guid> UniqueInterventionID = lstSectionQuestionAnswerHasNoParentAnswerID.Select(x => x.Section_InterventionID).Distinct();


            var SectionQuestionAnserTaskGroupByIntervention = from SectionQuestionAnserTask in lstSectionQuestionAnswerHasNoParentAnswerID
                                                              group SectionQuestionAnserTask by SectionQuestionAnserTask.SectionSummaryID;

            foreach (var group in SectionQuestionAnserTaskGroupByIntervention)
            {
                int score = 0;
                bool hasScore = false;
                Section_Summary objIntervention = new Section_Summary();
                foreach (var SectionQuestionAnserTask in group)
                {


                    foreach (Residents_Questions_Answers objResidentQueAns in lstResidentAnsweredQuestions)
                    {
                        if (objResidentQueAns.Sections_Questions_Answers.Section_QuestionID == SectionQuestionAnserTask.Section_QuestionID)
                        {
                            hasScore = true;
                            if (objResidentQueAns.Sections_Questions_Answers.AnswerType == "FreeText" && objResidentQueAns.Sections_Questions_Answers.Score == 0)
                            {
                                if (objResidentQueAns.AnswerText != null)
                                {
                                    score += Convert.ToInt32(objResidentQueAns.AnswerText) * 5;
                                }
                            }
                            else
                            {
                                score += Convert.ToInt32(objResidentQueAns.Sections_Questions_Answers.Score);
                            }
                        }
                    }
                    objIntervention = SectionQuestionAnserTask.Section_Summary;
                }

                if (hasScore)
                {
                    if (objIntervention.MinScore <= score && (objIntervention.MaxScore >= score || objIntervention.MaxScore == null))
                        GetlstResidentSectionQuestionAnsTask.Add(objIntervention);
                }


            }


            await GetInterventionQuestionSummary(GetlstResidentSectionQuestionAnsTask, residentId);
            string freetxt = "FREETEXT";

            foreach (Section_Summary objSummaryTxt in GetlstResidentSectionQuestionAnsTask)
            {
                int hasFreeText = objSummaryTxt.Summary.IndexOf(freetxt);
                if (hasFreeText > -1)
                {
                    if (objSummaryTxt.Sections_Questions_Answers_Summary != null)
                    {
                        foreach (Sections_Questions_Answers_Summary objSecQueAnsSum in objSummaryTxt.Sections_Questions_Answers_Summary)
                        {
                            if (objSecQueAnsSum.Section_Question_AnswerID != null)
                            {

                                List<Guid> guidAnsIDofResidents = db.Residents_Questions_Answers.Where(i => i.Section_Question_AnswerID == objSecQueAnsSum.Section_Question_AnswerID && i.IsActive == true && i.ResidentID == residentId).Select(i => i.Section_Question_AnswerID).ToList<Guid>();
                                List<string> lstAns = db.Sections_Questions_Answers.Where(i => guidAnsIDofResidents.Contains(i.ID) && i.IsActive == true).Select(i => i.LabelText).ToList<string>();
                                string[] words = lstAns.ToArray();
                                var res = words.Aggregate((current, next) => current + ", " + next);
                                string output1 = objSummaryTxt.Summary.Replace(freetxt, res);
                                objSummaryTxt.Summary = output1;
                            }
                            else
                            {
                                List<Sections_Questions_Answers> objSecQueAns = db.Sections_Questions_Answers.Where(i => i.Section_QuestionID == objSecQueAnsSum.Section_QuestionID).ToList<Sections_Questions_Answers>();
                                List<Guid> guidAnsID = objSecQueAns.Select(i => i.ID).ToList<Guid>();
                                List<Guid> guidAnsIDofResidents = db.Residents_Questions_Answers.Where(i => guidAnsID.Contains(i.Section_Question_AnswerID) && i.IsActive == true && i.ResidentID == residentId).Select(i => i.Section_Question_AnswerID).ToList<Guid>();
                                List<string> lstAns = db.Sections_Questions_Answers.Where(i => guidAnsIDofResidents.Contains(i.ID) && i.IsActive == true).Select(i => i.LabelText).ToList<string>();

                                string[] words = lstAns.ToArray();
                                var res = words.Aggregate((current, next) => current + ", " + next);
                                string output1 = objSummaryTxt.Summary.Replace(freetxt, res);
                                objSummaryTxt.Summary = output1;
                            }
                        }
                    }
                }
            }


            return Ok(GetlstResidentSectionQuestionAnsTask);
        }


        [HttpGet]
        [Route("GetResidentSummaryAlert")]
        public async Task<IHttpActionResult> GetResidentSummaryAlert(Guid residentId)
        {
            List<Residents_Questions_Answers> lstResidentSummary = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive == true && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();
            List<Section_Summary> GetlstResidentSectionQuestionAnsTask = new List<Section_Summary>();
            List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
            lstResidentAnsweredQuestions = lstResidentSummary;

            List<Sections_Questions_Answers_Summary> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Summary>();
            lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Summary.Include(obj => obj.Section_Summary).Where(obj => obj.IsActive && obj.Section_Summary.IsActive).ToListAsync<Sections_Questions_Answers_Summary>();


            List<Sections_Questions_Answers_Summary> lstSectionQuestionAnswerHasParentAnswerID = new List<Sections_Questions_Answers_Summary>();
            List<Sections_Questions_Answers_Summary> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Sections_Questions_Answers_Summary>();
            foreach (Sections_Questions_Answers_Summary objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
            {
                if (objSecQuesAnsTask.Section_Question_AnswerID != null)
                    lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                else
                    lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
            }


            var SectionQuestionAnserTaskGroupByIntervention = from SectionQuestionAnserTask in lstSectionQuestionAnswerHasNoParentAnswerID
                                                              group SectionQuestionAnserTask by SectionQuestionAnserTask.SectionSummaryID;

            foreach (var group in SectionQuestionAnserTaskGroupByIntervention)
            {
                int score = 0;
                bool hasScore = false;
                Section_Summary objIntervention = new Section_Summary();
                foreach (var SectionQuestionAnserTask in group)
                {


                    foreach (Residents_Questions_Answers objResidentQueAns in lstResidentAnsweredQuestions)
                    {
                        if (objResidentQueAns.Sections_Questions_Answers.Section_QuestionID == SectionQuestionAnserTask.Section_QuestionID)
                        {
                            hasScore = true;
                            if (objResidentQueAns.Sections_Questions_Answers.AnswerType == "FreeText" && objResidentQueAns.Sections_Questions_Answers.Score == 0)
                            {
                                if (objResidentQueAns.AnswerText != null)
                                {
                                    score += Convert.ToInt32(objResidentQueAns.AnswerText) * 5;
                                }
                            }
                            else
                            {
                                score += Convert.ToInt32(objResidentQueAns.Sections_Questions_Answers.Score);
                            }
                        }
                    }
                    objIntervention = SectionQuestionAnserTask.Section_Summary;
                }

                if (hasScore)
                {
                    if (objIntervention.MinScore <= score && (objIntervention.MaxScore >= score || objIntervention.MaxScore == null))
                    {
                        if (objIntervention.MinScore == 9 && objIntervention.MaxScore == null)
                            GetlstResidentSectionQuestionAnsTask.Add(objIntervention);
                    }
                }


            }




            return Ok(GetlstResidentSectionQuestionAnsTask);
        }

        private async Task GetInterventionQuestionSummary(List<Section_Summary> lstSectionSummary, Guid ResidentId)
        {
            List<Resident_Interventions_Questions_Answers> lstResidentSummary = await db.Resident_Interventions_Questions_Answers.Include(obj => obj.Intervention_Question_Answer).Where(obj => obj.IsActive && obj.Intervention_Question_Answer.IsActive == true && obj.ResidentID.Equals(ResidentId)).ToListAsync<Resident_Interventions_Questions_Answers>();


            List<Resident_Interventions_Questions_Answers> lstResidentAnsweredQuestions = new List<Resident_Interventions_Questions_Answers>();
            lstResidentAnsweredQuestions = lstResidentSummary;

            List<Interventions_Question_Answer_Summary> lstSectionQuestionsAnswersTask = new List<Interventions_Question_Answer_Summary>();
            lstSectionQuestionsAnswersTask = await db.Interventions_Question_Answer_Summary.Include(obj => obj.Section_Summary).Where(obj => obj.IsActive && obj.Section_Summary.IsActive).ToListAsync<Interventions_Question_Answer_Summary>();





            List<Interventions_Question_Answer_Summary> lstSectionQuestionAnswerHasParentAnswerID = new List<Interventions_Question_Answer_Summary>();
            List<Interventions_Question_Answer_Summary> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Interventions_Question_Answer_Summary>();
            foreach (Interventions_Question_Answer_Summary objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
            {
                if (objSecQuesAnsTask.Intervention_Question_AnswerID != null)
                    lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                else
                    lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
            }


            foreach (Resident_Interventions_Questions_Answers objResidentQuestionAnswer in lstResidentAnsweredQuestions)
            {
                foreach (Interventions_Question_Answer_Summary objSectionQuestionAnsTask in lstSectionQuestionAnswerHasParentAnswerID)
                {
                    if (objResidentQuestionAnswer.Intervention_Question_AnswerID == objSectionQuestionAnsTask.Intervention_Question_AnswerID)
                    {
                        if (objSectionQuestionAnsTask.Section_Summary.Summary == null)
                        {
                            if (objResidentQuestionAnswer.AnswerText != null)
                            {
                                objSectionQuestionAnsTask.Section_Summary.Summary = objResidentQuestionAnswer.AnswerText;
                                lstSectionSummary.Add(objSectionQuestionAnsTask.Section_Summary);
                            }

                        }
                        else
                        {
                            Guid ResidentID = objResidentQuestionAnswer.ResidentID;
                            List<Guid> lstQuesParentQuestion = new List<Guid>();
                            lstQuesParentQuestion = db.Intervention_Question_ParentQuestion.Where(i => i.InterventionParentAnswerID == objResidentQuestionAnswer.Intervention_Question_AnswerID && i.IsActive == true).Select(i => i.InterventionQuestionID).ToList<Guid>();

                            List<Guid> lstSecQuesAnsID = db.Intervention_Question_Answer.Where(i => lstQuesParentQuestion.Contains(i.Intervention_QuestionID) && i.IsActive == true).Select(i => i.ID).ToList<Guid>();

                            List<Resident_Interventions_Questions_Answers> lstResQueAns = db.Resident_Interventions_Questions_Answers.Where(i => lstSecQuesAnsID.Contains(i.Intervention_Question_AnswerID) && i.IsActive == true && i.ResidentID == ResidentID).ToList<Resident_Interventions_Questions_Answers>();

                            string CarrerCount = "1";
                            string AidName = "None";
                            foreach (Resident_Interventions_Questions_Answers obj in lstResQueAns)
                            {
                                if (obj.AnswerText != null)
                                {
                                    CarrerCount = obj.AnswerText;
                                }
                                else
                                {
                                    AidName = obj.Intervention_Question_Answer.LabelText;
                                }
                            }



                            string carrer = "XYZ";
                            string aid = "XXXX";
                            int hasXYZ = objSectionQuestionAnsTask.Section_Summary.Summary.IndexOf(carrer);
                            int hasXXX = objSectionQuestionAnsTask.Section_Summary.Summary.IndexOf(aid);

                            if (hasXYZ != -1 && hasXXX == -1)
                            {
                                string output = objSectionQuestionAnsTask.Section_Summary.Summary.Replace("XYZ", CarrerCount);
                                objSectionQuestionAnsTask.Section_Summary.Summary = output;
                            }
                            else if (hasXXX != -1 && hasXYZ == -1)
                            {

                                string output = objSectionQuestionAnsTask.Section_Summary.Summary.Replace("XXXX", AidName);
                                objSectionQuestionAnsTask.Section_Summary.Summary = output;
                            }
                            else if (hasXYZ != -1 && hasXXX != -1)
                            {
                                string output1 = objSectionQuestionAnsTask.Section_Summary.Summary.Replace("XYZ", CarrerCount);
                                string output2 = output1.Replace("XXXX", AidName);
                                objSectionQuestionAnsTask.Section_Summary.Summary = output2;
                            }
                            else
                            {
                                string output1 = objSectionQuestionAnsTask.Section_Summary.Summary.Replace("XYZ", "1");
                                string output2 = output1.Replace("XXXX", "None");
                                objSectionQuestionAnsTask.Section_Summary.Summary = output2;
                            }

                            lstSectionSummary.Add(objSectionQuestionAnsTask.Section_Summary);

                            //lstSectionSummary.Add(objSectionQuestionAnsTask.Section_Summary);
                        }
                    }
                }
            }

        }

        [HttpGet]
        [Route("GetResidentSummaryData")]
        public async Task<IHttpActionResult> GetResidentSummaryData(Guid residentId, Guid SectionQuestionID)
        {

            List<Question_ParentQuestion> lstQuestionParentQuestion = new List<Question_ParentQuestion>();
            lstQuestionParentQuestion = db.Question_ParentQuestion.Where(i => i.ParentQuestionID == SectionQuestionID).ToList<Question_ParentQuestion>();
            List<Guid> lstQuestionId = lstQuestionParentQuestion.Select(obj => obj.QuestionID).ToList<Guid>();


            List<Residents_Questions_Answers> lstResidentQuestion = db.Residents_Questions_Answers.Where(obj => lstQuestionId.Contains(obj.Sections_Questions_Answers.Section_QuestionID) && obj.ResidentID.Equals(residentId) && obj.IsActive == true).Include(i => i.Sections_Questions_Answers).ToList<Residents_Questions_Answers>();
            return Ok(lstResidentQuestion);
        }


        [HttpPost]
        [Route("SaveResident")]
        public async Task<IHttpActionResult> SaveResident(Resident objResident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid userId = new Guid(User.Identity.GetUserId());

            objResident.ID = Guid.NewGuid();
            objResident.IsActive = true;
            objResident.IsAccepted = false;
            objResident.CreatedBy = userId;
            objResident.ModifiedBy = userId;
            objResident.Created = objResident.Modified = DateTime.Now;


            db.Residents.Add(objResident);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResidentExists(objResident.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(objResident);
        }



        [HttpPost]
        [Route("SaveofflineResident")]
        public async Task<IHttpActionResult> SaveofflineResident(Resident objResident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid userId = new Guid(User.Identity.GetUserId());

            
            objResident.IsActive = true;
            objResident.IsAccepted = false;
            objResident.CreatedBy = userId;
            objResident.ModifiedBy = userId;
            objResident.Created = objResident.Modified = DateTime.Now;


            db.Residents.Add(objResident);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResidentExists(objResident.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(objResident.ID);
        }

        [HttpPost]
        [Route("UpdateResident")]
        public async Task<IHttpActionResult> UpdateResident(Resident objResident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid userId = new Guid(User.Identity.GetUserId());

            objResident.ModifiedBy = userId;
            objResident.Modified = DateTime.Now;


            db.Entry(objResident).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!ResidentExists(objResident.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(objResident);
        }

        [HttpPost]
        [Route("AcceptAsResident")]
        public async Task<IHttpActionResult> AcceptAsResident(Guid residentId)
        {
            if (!ResidentExists(residentId))
            {
                return NotFound();
            }

            Guid userId = new Guid(User.Identity.GetUserId());
            Resident objResident = await db.Residents.FindAsync(residentId);

            objResident.IsAccepted = true;
            objResident.ModifiedBy = userId;
            objResident.Modified = DateTime.Now;


            db.Entry(objResident).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok();
        }

        [HttpPost]
        [Route("GetAssessmentData")]
        public async Task<IHttpActionResult> GetAssessmentData(Guid residentId)
        {
            List<Residents_Questions_Answers> lstAnswersByResident = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers.Sections_Questions).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.Sections_Questions_Answers.Section.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();
            //List<Residents_Questions_Answers> lstAnswersByResident = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive  && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();


            List<ResidentWithFile> lstResidentWithFile = new List<ResidentWithFile>();

            foreach (Residents_Questions_Answers objResidentQuestionAnsweer in lstAnswersByResident)
            {
                ResidentWithFile objResidentWithFile = new ResidentWithFile();
                objResidentWithFile.ResidentQuestionAnswer = objResidentQuestionAnsweer;
                objResidentWithFile.ResidentFile = GetResidentFile(objResidentQuestionAnsweer.ID);

                lstResidentWithFile.Add(objResidentWithFile);
            }


            return Ok(lstResidentWithFile);


            //  return Ok(lstAnswersByResident);
        }


        [HttpPost]
        [Route("GetInterventionAssessmentData")]
        public async Task<IHttpActionResult> GetInterventionAssessmentData(Guid residentId)
        {
            List<Resident_Interventions_Questions_Answers> lstAnswersByResident = await db.Resident_Interventions_Questions_Answers.Include(obj => obj.Intervention_Question_Answer.Intervention_Question).Where(obj => obj.IsActive && obj.Intervention_Question_Answer.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Resident_Interventions_Questions_Answers>();
            //List<Residents_Questions_Answers> lstAnswersByResident = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive  && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();


            List<InterventionResidentWithFile> lstResidentWithFile = new List<InterventionResidentWithFile>();

            foreach (Resident_Interventions_Questions_Answers objResidentQuestionAnsweer in lstAnswersByResident)
            {
                InterventionResidentWithFile objResidentWithFile = new InterventionResidentWithFile();
                objResidentWithFile.ResidentQuestionAnswer = objResidentQuestionAnsweer;
                objResidentWithFile.ResidentFile = GetResidentFile(objResidentQuestionAnsweer.ID);

                lstResidentWithFile.Add(objResidentWithFile);
            }


            return Ok(lstResidentWithFile);


            //  return Ok(lstAnswersByResident);
        }

        [HttpPost]
        [Route("GetInterventionResAnsAssessmentData")]
        public async Task<IHttpActionResult> GetInterventionResAnsAssessmentData(Guid residentId, Guid InterventionId)
        {
            List<Interventions_Resident_Answers> lstAnswersByResident = await db.Interventions_Resident_Answers.Include(obj => obj.Intervention_Question_Answer.Intervention_Question).Where(obj => obj.IsActive && obj.InterventionID == InterventionId && obj.Intervention_Question_Answer.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Interventions_Resident_Answers>();
            //List<Residents_Questions_Answers> lstAnswersByResident = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive  && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();


            List<InterventionResidentAnswerWithFile> lstResidentWithFile = new List<InterventionResidentAnswerWithFile>();

            foreach (Interventions_Resident_Answers objResidentQuestionAnsweer in lstAnswersByResident)
            {
                InterventionResidentAnswerWithFile objResidentWithFile = new InterventionResidentAnswerWithFile();
                objResidentWithFile.ResidentQuestionAnswer = objResidentQuestionAnsweer;
                objResidentWithFile.ResidentFile = GetResidentFile(objResidentQuestionAnsweer.ID);

                lstResidentWithFile.Add(objResidentWithFile);
            }


            return Ok(lstResidentWithFile);


            //  return Ok(lstAnswersByResident);
        }

        [HttpPost]
        [Route("GetRequiredAssessmentSummary")]
        public async Task<IHttpActionResult> GetRequiredAssessmentSummary(Guid residentId)
        {
            //  List<Residents_Questions_Answers> lstAssessmentSummary = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers.Sections_Questions_Answers_Tasks).Include(obj => obj.Sections_Questions_Answers.Sections_Questions.Section).Include(obj => obj.Actions.Select(s => s.Actions_Days)).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.Sections_Questions_Answers.Section.IsActive && obj.ResidentID.Equals(residentId) && obj.Sections_Questions_Answers.Sections_Questions_Answers_Tasks.Count > 0).ToListAsync<Residents_Questions_Answers>();


            //  foreach (Residents_Questions_Answers objResidents_Question_Answers in lstAssessmentSummary)
            //  {
            //      objResidents_Question_Answers.Actions = objResidents_Question_Answers.Actions.Where(obj => obj.IsActive).ToList<CHM.Services.Models.Action>();

            //      foreach (CHM.Services.Models.Action objAction in objResidents_Question_Answers.Actions)
            //      {
            //          objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

            //          foreach (Actions_Days objActions_Days in objAction.Actions_Days)
            //          {
            //              objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
            //          }
            //      }
            //  }
            ////  lstAssessmentSummary.GroupBy(obj => obj.Sections_Questions_Answers.Section.Name);


            List<Residents_Questions_Answers> lstResidentQuestionAnswer = new List<Residents_Questions_Answers>();

            List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
            lstResidentAnsweredQuestions = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers.Sections_Questions).Include(obj => obj.Sections_Questions_Answers.Sections_Questions.Section).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();

            List<Sections_Questions_Answers_Tasks> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Tasks>();
            lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Tasks.Include(obj => obj.Section_Intervention).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Sections_Questions_Answers_Tasks>();

            foreach (Residents_Questions_Answers objResidentQuestion in lstResidentAnsweredQuestions)
            {



                int count1 = lstSectionQuestionsAnswersTask.Where(obj => obj.Section_Question_AnswerID == objResidentQuestion.Section_Question_AnswerID).Count();
                int count2 = 0;
                if (count1 == 0)
                {
                    count2 = lstSectionQuestionsAnswersTask.Where(obj => obj.Section_QuestionID == objResidentQuestion.Sections_Questions_Answers.Section_QuestionID && obj.Section_Question_AnswerID == null).Count();

                    //Get Tasks for particular Question code start
                    if (count2 > 0)
                    {
                        List<Sections_Questions_Answers_Tasks> lstSectionQuestionTask = new List<Sections_Questions_Answers_Tasks>();
                        lstSectionQuestionTask = lstSectionQuestionsAnswersTask.Where(obj => obj.Section_QuestionID == objResidentQuestion.Sections_Questions_Answers.Section_QuestionID && obj.Section_Question_AnswerID == null).ToList<Sections_Questions_Answers_Tasks>();
                        foreach (Sections_Questions_Answers_Tasks objSectionQueTask in lstSectionQuestionTask)
                        {
                            List<Guid> SectionQuestionIDs = lstSectionQuestionsAnswersTask.Where(i => i.Section_InterventionID == objSectionQueTask.Section_InterventionID).Select(i => i.Section_QuestionID).ToList<Guid>();
                            List<Guid> lstSectionQuestionAnswerIds = new List<Guid>();
                            lstSectionQuestionAnswerIds = db.Sections_Questions_Answers.Where(i => SectionQuestionIDs.Contains(i.Section_QuestionID) && i.IsActive == true).Select(i => i.ID).ToList<Guid>();

                            List<Guid> lstSecQueAnsinResidentAnsIDs = lstResidentAnsweredQuestions.Where(i => lstSectionQuestionAnswerIds.Contains(i.Section_Question_AnswerID) && i.IsActive == true).Select(i => i.Section_Question_AnswerID).ToList<Guid>();

                            List<Sections_Questions_Answers> lstSectionQuestionAnsForScore = new List<Sections_Questions_Answers>();
                            lstSectionQuestionAnsForScore = db.Sections_Questions_Answers.Where(i => lstSecQueAnsinResidentAnsIDs.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                            int score = 0;
                            int hasResidentId = 0;
                            bool hastask = false;
                            bool hasScore = false;
                            foreach (Sections_Questions_Answers obj in lstSectionQuestionAnsForScore)
                            {
                                hasScore = true;
                                score += Convert.ToInt32(obj.Score);
                                if (obj.ID == objResidentQuestion.Section_Question_AnswerID)
                                {
                                    hasResidentId = 1;
                                }
                            }
                            if (hasScore)
                            {
                                if (objSectionQueTask.Section_Intervention.MinScore <= score && objSectionQueTask.Section_Intervention.MaxScore >= score)
                                {
                                    hastask = true;
                                }
                            }
                            //Code chneg for bmi issue kept or condition instead of and
                            if (count2 > 0 && (hastask || hasResidentId == 1))
                            {

                                lstResidentQuestionAnswer.Add(objResidentQuestion);
                            }
                        }
                    }
                    //Code End


                }

                if (count1 > 0)
                    lstResidentQuestionAnswer.Add(objResidentQuestion);

            }

            List<Residents_Questions_Answers> DistinctResidentQuestionsAnswers = new List<Residents_Questions_Answers>();

            var DistinctItems = lstResidentQuestionAnswer.GroupBy(x => x.ID).Select(y => y.First());

            foreach (var item in DistinctItems)
            {
                DistinctResidentQuestionsAnswers.Add(item);
            }


            List<AssementSummary> lstAssesSummary = new List<AssementSummary>();
            foreach (Residents_Questions_Answers item in DistinctResidentQuestionsAnswers)
            {
                AssementSummary objAssementSummary = new AssementSummary();
                objAssementSummary.Question = item.Sections_Questions_Answers.Sections_Questions.Question;
                if (item.Sections_Questions_Answers.LabelText != "FreeText" && item.Sections_Questions_Answers.LabelText != "Other")
                {
                    objAssementSummary.LabelTxt = item.Sections_Questions_Answers.LabelText;
                }
                else
                {
                    objAssementSummary.LabelTxt = item.AnswerText;
                }


                objAssementSummary.SectionID = item.Sections_Questions_Answers.Section.ID;
                lstAssesSummary.Add(objAssementSummary);

            }


            return Ok(lstAssesSummary);
        }


        [HttpPost]
        [Route("GetNewRequiredAssessmentSummary")]
        public async Task<IHttpActionResult> GetNewRequiredAssessmentSummary(Guid residentId)
        {
            List<ClsIntervention> lstClsIntervention = new List<ClsIntervention>();

            List<Residents_Questions_Answers> lstResidentQuestionAnswer = new List<Residents_Questions_Answers>();

            List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
            lstResidentAnsweredQuestions = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers.Sections_Questions).Include(obj => obj.Sections_Questions_Answers.Sections_Questions.Section).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();

            List<Sections_Questions_Answers_Tasks> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Tasks>();
            lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Tasks.Include(obj => obj.Section_Intervention).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Sections_Questions_Answers_Tasks>();

            foreach (Residents_Questions_Answers objResidentQuestion in lstResidentAnsweredQuestions)
            {



                int count1 = lstSectionQuestionsAnswersTask.Where(obj => obj.Section_Question_AnswerID == objResidentQuestion.Section_Question_AnswerID).Count();


                int count2 = 0;
                if (count1 == 0)
                {
                    count2 = lstSectionQuestionsAnswersTask.Where(obj => obj.Section_QuestionID == objResidentQuestion.Sections_Questions_Answers.Section_QuestionID && obj.Section_Question_AnswerID == null).Count();


                    if (count2 > 0)
                    {
                        List<Sections_Questions_Answers_Tasks> lstSectionQuestionTask = new List<Sections_Questions_Answers_Tasks>();
                        lstSectionQuestionTask = lstSectionQuestionsAnswersTask.Where(obj => obj.Section_QuestionID == objResidentQuestion.Sections_Questions_Answers.Section_QuestionID && obj.Section_Question_AnswerID == null).ToList<Sections_Questions_Answers_Tasks>();
                        foreach (Sections_Questions_Answers_Tasks objSectionQueTask in lstSectionQuestionTask)
                        {
                            List<Guid> SectionQuestionIDs = lstSectionQuestionsAnswersTask.Where(i => i.Section_InterventionID == objSectionQueTask.Section_InterventionID).Select(i => i.Section_QuestionID).ToList<Guid>();
                            List<Guid> lstSectionQuestionAnswerIds = new List<Guid>();
                            lstSectionQuestionAnswerIds = db.Sections_Questions_Answers.Where(i => SectionQuestionIDs.Contains(i.Section_QuestionID) && i.IsActive == true).Select(i => i.ID).ToList<Guid>();

                            List<Guid> lstSecQueAnsinResidentAnsIDs = lstResidentAnsweredQuestions.Where(i => lstSectionQuestionAnswerIds.Contains(i.Section_Question_AnswerID) && i.IsActive == true).Select(i => i.Section_Question_AnswerID).ToList<Guid>();

                            List<Sections_Questions_Answers> lstSectionQuestionAnsForScore = new List<Sections_Questions_Answers>();
                            lstSectionQuestionAnsForScore = db.Sections_Questions_Answers.Where(i => lstSecQueAnsinResidentAnsIDs.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                            int score = 0;
                            int hasResidentId = 0;
                            bool hastask = false;
                            bool hasScore = false;
                            foreach (Sections_Questions_Answers obj in lstSectionQuestionAnsForScore)
                            {
                                hasScore = true;
                                score += Convert.ToInt32(obj.Score);
                                if (obj.ID == objResidentQuestion.Section_Question_AnswerID)
                                {
                                    hasResidentId = 1;
                                }
                            }
                            if (hasScore)
                            {
                                if (objSectionQueTask.Section_Intervention.MinScore <= score && (objSectionQueTask.Section_Intervention.MaxScore >= score || objSectionQueTask.Section_Intervention.MaxScore == null))
                                {
                                    hastask = true;
                                }
                            }

                            if (count2 > 0 && (hastask || hasResidentId == 1))
                            {
                                if (hasResidentId == 1 && hastask)
                                {
                                    ClsIntervention objclsIntervention = new ClsIntervention();
                                    objclsIntervention.ID = objSectionQueTask.Section_InterventionID;
                                    objclsIntervention.InterventionName = objSectionQueTask.Section_Intervention.InterventionTitle;
                                    objclsIntervention.ResidentQuestionAnsID = objResidentQuestion.ID;
                                    lstClsIntervention.Add(objclsIntervention);
                                }
                                lstResidentQuestionAnswer.Add(objResidentQuestion);
                            }
                        }
                    }

                }

                if (count1 > 0)
                {

                    Sections_Questions_Answers_Tasks obj1 = lstSectionQuestionsAnswersTask.Where(obj => obj.Section_Question_AnswerID == objResidentQuestion.Section_Question_AnswerID).FirstOrDefault();
                    if (obj1 != null)
                    {
                        ClsIntervention objclsIntervention = new ClsIntervention();
                        objclsIntervention.ID = obj1.Section_InterventionID;
                        objclsIntervention.InterventionName = obj1.Section_Intervention.InterventionTitle;
                        objclsIntervention.ResidentQuestionAnsID = objResidentQuestion.ID;
                        lstClsIntervention.Add(objclsIntervention);
                    }

                    lstResidentQuestionAnswer.Add(objResidentQuestion);
                }

            }

            List<Residents_Questions_Answers> DistinctResidentQuestionsAnswers = new List<Residents_Questions_Answers>();

            var DistinctItems = lstResidentQuestionAnswer.GroupBy(x => x.ID).Select(y => y.First());

            foreach (var item in DistinctItems)
            {
                DistinctResidentQuestionsAnswers.Add(item);
            }



            List<GroupAssementSummary> lstGroupAssessment = new List<GroupAssementSummary>();
            foreach (Residents_Questions_Answers item in DistinctResidentQuestionsAnswers)
            {

                GroupAssementSummary objGroupAssementSummary = new GroupAssementSummary();

                objGroupAssementSummary.Question = item.Sections_Questions_Answers.Sections_Questions.Question;
                if (item.Sections_Questions_Answers.LabelText != "FreeText" && item.Sections_Questions_Answers.LabelText != "Other")
                {
                    objGroupAssementSummary.LabelTxt = item.Sections_Questions_Answers.LabelText;
                }
                else
                {
                    objGroupAssementSummary.LabelTxt = item.AnswerText;
                }


                objGroupAssementSummary.SectionID = item.Sections_Questions_Answers.Section.ID;
                List<clsGroupIntervention> lst = new List<clsGroupIntervention>();
                foreach (ClsIntervention obj in lstClsIntervention)
                {
                    clsGroupIntervention objclsGroupIntervention = new clsGroupIntervention();
                    if (obj.ResidentQuestionAnsID == item.ID)
                    {
                        objGroupAssementSummary.InterventionId = obj.ID;
                        objclsGroupIntervention.InterventionId = obj.ID;
                        objclsGroupIntervention.InterventionName = obj.InterventionName;
                        CHM.Services.Models.Action objAction = db.Actions.Where(k => k.Section_InterventionID == obj.ID && k.ResidentID == residentId && k.IsActive == true).FirstOrDefault();
                        if (objAction != null)
                        {
                            objclsGroupIntervention.Ocuurrence = Convert.ToInt32(objAction.Occurrences);
                            objclsGroupIntervention.Type = objAction.Type;
                            objclsGroupIntervention.StartDate = objAction.StartDate;
                        }
                        try
                        {

                            lst.Add(objclsGroupIntervention);

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                objGroupAssementSummary.lstGroupIntervention = lst;
                lstGroupAssessment.Add(objGroupAssementSummary);
            }




            var Groupresults = from p in lstGroupAssessment
                               group p by p.InterventionId into g
                               select new { InterventionID = g.Key, QuestionS = g.ToList() };

            foreach (var item in Groupresults)
            {
                int i = 0;
                foreach (var item1 in item.QuestionS)
                {
                    if (i != item.QuestionS.Count - 1)
                    {
                        item1.lstGroupIntervention = null;
                    }
                    item1.DisplayOrder = i;
                    i++;
                }
            }







            //List<ClsIntervention> DistinctIntervention = new List<ClsIntervention>();

            //var lstDistinctInterventionItem = lstClsIntervention.GroupBy(x => x.ID).Select(y => y.First());

            //foreach (var item in lstDistinctInterventionItem)
            //{
            //    DistinctIntervention.Add(item);
            //}




            //List<ClsIntervention> DistinctClsIntervention = new List<ClsIntervention>();

            //var DistinctClsInterventionItem = lstClsIntervention.GroupBy(x => x.ResidentQuestionAnsID).Select(y => y.First());

            //foreach (var item in DistinctClsInterventionItem)
            //{
            //    DistinctClsIntervention.Add(item);
            //}

            //List<Guid> lstIDs = DistinctClsIntervention.Select(i => i.ID).ToList<Guid>();
            //List<ClsIntervention> clsUnique=new List<ClsIntervention>();

            //clsUnique = DistinctIntervention.Where(i => !lstIDs.Contains(i.ID)).ToList<ClsIntervention>();

            //foreach(ClsIntervention item in clsUnique)
            //{
            //    DistinctClsIntervention.Add(item);
            //}

            //List<NewAssementSummary> lstNewAssesmentSummary = new List<NewAssementSummary>();

            ////List<AssementSummary> lstAssesSummary = new List<AssementSummary>();

            ////foreach (Residents_Questions_Answers item in DistinctResidentQuestionsAnswers)
            ////{

            ////    AssementSummary objAssementSummary = new AssementSummary();
            ////    objAssementSummary.Question = item.Sections_Questions_Answers.Sections_Questions.Question;
            ////    if (item.Sections_Questions_Answers.LabelText != "FreeText" && item.Sections_Questions_Answers.LabelText != "Other")
            ////    {
            ////        objAssementSummary.LabelTxt = item.Sections_Questions_Answers.LabelText;
            ////    }
            ////    else
            ////    {
            ////        objAssementSummary.LabelTxt = item.AnswerText;
            ////    }


            ////    objAssementSummary.SectionID = item.Sections_Questions_Answers.Section.ID;
            ////    lstAssesSummary.Add(objAssementSummary);

            ////}


            //foreach (Residents_Questions_Answers item in DistinctResidentQuestionsAnswers)
            //{
            //    foreach (ClsIntervention obj in DistinctClsIntervention)
            //    {
            //        if (obj.ResidentQuestionAnsID == item.ID)
            //        {
            //            NewAssementSummary objAssementSummary = new NewAssementSummary();
            //            objAssementSummary.Question = item.Sections_Questions_Answers.Sections_Questions.Question;
            //            if (item.Sections_Questions_Answers.LabelText != "FreeText" && item.Sections_Questions_Answers.LabelText != "Other")
            //            {
            //                objAssementSummary.LabelTxt = item.Sections_Questions_Answers.LabelText;
            //            }
            //            else
            //            {
            //                objAssementSummary.LabelTxt = item.AnswerText;
            //            }

            //            objAssementSummary.InterventionId = obj.ID;
            //            objAssementSummary.InterventionName = obj.InterventionName;
            //            objAssementSummary.SectionID = item.Sections_Questions_Answers.Section.ID;
            //            lstNewAssesmentSummary.Add(objAssementSummary);
            //        }
            //    }

            //}

            //var results = from p in lstNewAssesmentSummary
            //              group p by p.InterventionId into g
            //              select new { InterventionID = g.Key, QuestionS = g.ToList() };

            //List<NewAssementSummaryData> lstNewAssementSumamryData=new List<NewAssementSummaryData>();

            //foreach (var item in results)
            //{
            //    int i=0;
            //    CHM.Services.Models.Action objAction = db.Actions.Where(obj => obj.Section_InterventionID == item.InterventionID && obj.ResidentID == residentId && obj.IsActive == true).FirstOrDefault();
            //  foreach(var item1 in item.QuestionS)
            //  {



            //      NewAssementSummaryData objNewAssementSummaryData=new NewAssementSummaryData();
            //      objNewAssementSummaryData.Question=item1.Question;
            //      objNewAssementSummaryData.LabelTxt=item1.LabelTxt;
            //      objNewAssementSummaryData.SectionID = item1.SectionID;
            //      if (i == item.QuestionS.Count - 1)
            //      {
            //          objNewAssementSummaryData.InterventionId = item1.InterventionId;
            //          objNewAssementSummaryData.InterventionName = item1.InterventionName;
            //          if(objAction!=null)
            //          {
            //              objNewAssementSummaryData.Ocuurrence = Convert.ToInt32(objAction.Occurrences);
            //              objNewAssementSummaryData.Type = objAction.Type;
            //              objNewAssementSummaryData.StartDate = objAction.StartDate;
            //          }
            //      }
            //      objNewAssementSummaryData.DisplayOrder = i;
            //      lstNewAssementSumamryData.Add(objNewAssementSummaryData);
            //      i++;
            //  }
            //}


            //return Ok(lstNewAssementSumamryData);

            return Ok(lstGroupAssessment);
        }



        [HttpPost]
        [Route("GetAssessmentSummary")]
        public async Task<IHttpActionResult> GetAssessmentSummary(Guid residentId)
        {
            //List<Residents_Questions_Answers> lstAssessmentSummary =await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers.Sections_Questions_Answers_Tasks).Include(obj => obj.Sections_Questions_Answers.Sections_Questions.Section).Include(obj => obj.Actions.Select(s => s.Actions_Days)).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.Sections_Questions_Answers.Section.IsActive && obj.ResidentID.Equals(residentId) && obj.Sections_Questions_Answers.Sections_Questions_Answers_Tasks.Count > 0).ToListAsync<Residents_Questions_Answers>();


            //foreach (Residents_Questions_Answers objResidents_Question_Answers in lstAssessmentSummary)
            //{
            //    objResidents_Question_Answers.Actions = objResidents_Question_Answers.Actions.Where(obj => obj.IsActive).ToList<CHM.Services.Models.Action>();

            //    foreach (CHM.Services.Models.Action objAction in objResidents_Question_Answers.Actions)
            //    {
            //        objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

            //        foreach (Actions_Days objActions_Days in objAction.Actions_Days)
            //        {
            //            objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
            //        }
            //    }
            //}

            List<Section_Intervention> GetlstResidentSectionQuestionAnsTask = new List<Section_Intervention>();
            List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
            lstResidentAnsweredQuestions = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();

            // List<Sections_Questions_Answers_Tasks> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Tasks>();
            //lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Tasks.Include(obj => obj.Section_Intervention).Include(obj => obj.Section_Intervention.Actions.Select(i => i.Actions_Days)).Include(i => i.Section_Intervention.Intervention_Question).Include(k => k.Section_Intervention.Intervention_Question.Select(o => o.Intervention_Question_Answer)).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Sections_Questions_Answers_Tasks>();

            List<Sections_Questions_Answers_Tasks> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Tasks>();
            lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Tasks.Include(obj => obj.Section_Intervention).Include(i => i.Section_Intervention.Intervention_Question).Include(k => k.Section_Intervention.Intervention_Question.Select(o => o.Intervention_Question_Answer)).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Sections_Questions_Answers_Tasks>();

            //foreach (Sections_Questions_Answers_Tasks objSecQueAnsTask in lstSectionQuestionsAnswersTask)
            //{

            //    objSecQueAnsTask.Section_Intervention.Actions = objSecQueAnsTask.Section_Intervention.Actions.Where(obj => obj.IsActive && obj.ResidentID == residentId).ToList<CHM.Services.Models.Action>();
            //    foreach (CHM.Services.Models.Action objAction in objSecQueAnsTask.Section_Intervention.Actions)
            //    {
            //        objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

            //        foreach (Actions_Days objActions_Days in objAction.Actions_Days)
            //        {
            //            objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
            //        }
            //    }

            //}






            List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
            List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
            foreach (Sections_Questions_Answers_Tasks objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
            {
                if (objSecQuesAnsTask.Section_Question_AnswerID != null)
                    lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                else
                    lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
            }






            foreach (Residents_Questions_Answers objResidentQuestionAnswer in lstResidentAnsweredQuestions)
            {
                foreach (Sections_Questions_Answers_Tasks objSectionQuestionAnsTask in lstSectionQuestionAnswerHasParentAnswerID)
                {


                    if (objResidentQuestionAnswer.Section_Question_AnswerID == objSectionQuestionAnsTask.Section_Question_AnswerID)
                    {
                        GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask.Section_Intervention);
                    }
                }
            }


            //var UniqueInterventionID = lstSectionQuestionAnswerHasNoParentAnswerID.Select(x => new { x.Section_InterventionID}).Distinct();

            // IEnumerable<Guid> UniqueInterventionID = lstSectionQuestionAnswerHasNoParentAnswerID.Select(x => x.Section_InterventionID).Distinct();


            var SectionQuestionAnserTaskGroupByIntervention = from SectionQuestionAnserTask in lstSectionQuestionAnswerHasNoParentAnswerID
                                                              group SectionQuestionAnserTask by SectionQuestionAnserTask.Section_InterventionID;

            foreach (var group in SectionQuestionAnserTaskGroupByIntervention)
            {
                int score = 0;
                bool hasScore = false;
                Section_Intervention objIntervention = new Section_Intervention();
                foreach (var SectionQuestionAnserTask in group)
                {


                    foreach (Residents_Questions_Answers objResidentQueAns in lstResidentAnsweredQuestions)
                    {
                        if (objResidentQueAns.Sections_Questions_Answers.Section_QuestionID == SectionQuestionAnserTask.Section_QuestionID)
                        {
                            hasScore = true;
                            score += Convert.ToInt32(objResidentQueAns.Sections_Questions_Answers.Score);
                        }
                    }
                    objIntervention = SectionQuestionAnserTask.Section_Intervention;
                }

                if (hasScore)
                {
                    if (objIntervention.MinScore <= score && (objIntervention.MaxScore >= score || objIntervention.MaxScore == null))
                        GetlstResidentSectionQuestionAnsTask.Add(objIntervention);
                }


            }

            List<Section_Intervention> DistinctListSectionIntervention = new List<Section_Intervention>();

            var DistinctItems = GetlstResidentSectionQuestionAnsTask.GroupBy(x => x.ID).Select(y => y.First());

            foreach (var item in DistinctItems)
            {
                item.Intervention_Question_Answer = null;
                foreach (var itemInterventionQuestion in item.Intervention_Question)
                {
                    itemInterventionQuestion.Section_Intervention = null;
                }

                DistinctListSectionIntervention.Add(item);
            }


            List<Guid> Section_InterventionIDs = new List<Guid>();
            Section_InterventionIDs = DistinctListSectionIntervention.Select(i => i.ID).ToList<Guid>();

            List<Sections_Questions_Answers_Tasks> sectionQuestionAnswerTask = db.Sections_Questions_Answers_Tasks.Where(i => Section_InterventionIDs.Contains(i.Section_InterventionID)).ToList<Sections_Questions_Answers_Tasks>();

            List<Sections_Questions_Answers_Tasks> DistinctSections_Questions_Answers_Tasks = new List<Sections_Questions_Answers_Tasks>();

            var DistinctItemsSection1 = sectionQuestionAnswerTask.GroupBy(x => x.Section_InterventionID).Select(y => y.First());

            foreach (var item in DistinctItemsSection1)
            {
                DistinctSections_Questions_Answers_Tasks.Add(item);
            }
            List<SectionInterventionSection> lstSection = new List<SectionInterventionSection>();

            foreach (Sections_Questions_Answers_Tasks obj in DistinctSections_Questions_Answers_Tasks)
            {
                SectionInterventionSection objSection = new SectionInterventionSection();
                if (obj.Section_Question_AnswerID == null)
                {
                    objSection.sectionName = db.Sections_Questions.Include(i => i.Section).Where(w => w.ID == obj.Section_QuestionID).FirstOrDefault().Section.Name;
                    objSection.ID = db.Sections_Questions.Include(i => i.Section).Where(w => w.ID == obj.Section_QuestionID).FirstOrDefault().Section.ID;
                }
                else
                {
                    objSection.sectionName = db.Sections_Questions_Answers.Include(i => i.Section).Where(w => w.ID == obj.Section_Question_AnswerID).FirstOrDefault().Section.Name;
                    objSection.ID = db.Sections_Questions_Answers.Include(i => i.Section).Where(w => w.ID == obj.Section_Question_AnswerID).FirstOrDefault().Section.ID;
                }


                objSection.section_InterventionID = obj.Section_InterventionID;
                lstSection.Add(objSection);
            }

            // clsSectionIntervention objSectionIntervention = new clsSectionIntervention();

            clsSectionwithSectionIntervention objSectionWithSectionIntervention = new clsSectionwithSectionIntervention();
            objSectionWithSectionIntervention.SectionInterventionResponse = DistinctListSectionIntervention;
            objSectionWithSectionIntervention.SectionInterventionSection = lstSection;

            return Ok(objSectionWithSectionIntervention);
        }


        [HttpPost]
        [Route("GetOnlyAssessmentSummary")]
        public async Task<IHttpActionResult> GetOnlyAssessmentSummary(Guid residentId, Guid SectionInterventionID)
        {
            //List<Residents_Questions_Answers> lstAssessmentSummary =await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers.Sections_Questions_Answers_Tasks).Include(obj => obj.Sections_Questions_Answers.Sections_Questions.Section).Include(obj => obj.Actions.Select(s => s.Actions_Days)).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.Sections_Questions_Answers.Section.IsActive && obj.ResidentID.Equals(residentId) && obj.Sections_Questions_Answers.Sections_Questions_Answers_Tasks.Count > 0).ToListAsync<Residents_Questions_Answers>();


            //foreach (Residents_Questions_Answers objResidents_Question_Answers in lstAssessmentSummary)
            //{
            //    objResidents_Question_Answers.Actions = objResidents_Question_Answers.Actions.Where(obj => obj.IsActive).ToList<CHM.Services.Models.Action>();

            //    foreach (CHM.Services.Models.Action objAction in objResidents_Question_Answers.Actions)
            //    {
            //        objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

            //        foreach (Actions_Days objActions_Days in objAction.Actions_Days)
            //        {
            //            objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
            //        }
            //    }
            //}

            List<Section_Intervention> GetlstResidentSectionQuestionAnsTask = new List<Section_Intervention>();
            List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
            lstResidentAnsweredQuestions = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();

            List<Sections_Questions_Answers_Tasks> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Tasks>();
            lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Tasks.Include(obj => obj.Section_Intervention).Include(obj => obj.Section_Intervention.Actions.Select(i => i.Actions_Days)).Include(i => i.Section_Intervention.Intervention_Question).Include(k => k.Section_Intervention.Intervention_Question.Select(o => o.Intervention_Question_Answer)).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Sections_Questions_Answers_Tasks>();

            foreach (Sections_Questions_Answers_Tasks objSecQueAnsTask in lstSectionQuestionsAnswersTask)
            {

                objSecQueAnsTask.Section_Intervention.Actions = objSecQueAnsTask.Section_Intervention.Actions.Where(obj => obj.IsActive && obj.ResidentID == residentId).ToList<CHM.Services.Models.Action>();
                foreach (CHM.Services.Models.Action objAction in objSecQueAnsTask.Section_Intervention.Actions)
                {
                    objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

                    foreach (Actions_Days objActions_Days in objAction.Actions_Days)
                    {
                        objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
                    }
                }

            }






            List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
            List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
            foreach (Sections_Questions_Answers_Tasks objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
            {
                if (objSecQuesAnsTask.Section_Question_AnswerID != null)
                    lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                else
                    lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
            }






            foreach (Residents_Questions_Answers objResidentQuestionAnswer in lstResidentAnsweredQuestions)
            {
                foreach (Sections_Questions_Answers_Tasks objSectionQuestionAnsTask in lstSectionQuestionAnswerHasParentAnswerID)
                {


                    if (objResidentQuestionAnswer.Section_Question_AnswerID == objSectionQuestionAnsTask.Section_Question_AnswerID)
                    {
                        GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask.Section_Intervention);
                    }
                }
            }


            //var UniqueInterventionID = lstSectionQuestionAnswerHasNoParentAnswerID.Select(x => new { x.Section_InterventionID}).Distinct();

            // IEnumerable<Guid> UniqueInterventionID = lstSectionQuestionAnswerHasNoParentAnswerID.Select(x => x.Section_InterventionID).Distinct();


            var SectionQuestionAnserTaskGroupByIntervention = from SectionQuestionAnserTask in lstSectionQuestionAnswerHasNoParentAnswerID
                                                              group SectionQuestionAnserTask by SectionQuestionAnserTask.Section_InterventionID;

            foreach (var group in SectionQuestionAnserTaskGroupByIntervention)
            {
                int score = 0;
                bool hasScore = false;
                Section_Intervention objIntervention = new Section_Intervention();
                foreach (var SectionQuestionAnserTask in group)
                {


                    foreach (Residents_Questions_Answers objResidentQueAns in lstResidentAnsweredQuestions)
                    {
                        if (objResidentQueAns.Sections_Questions_Answers.Section_QuestionID == SectionQuestionAnserTask.Section_QuestionID)
                        {
                            hasScore = true;
                            score += Convert.ToInt32(objResidentQueAns.Sections_Questions_Answers.Score);
                        }
                    }
                    objIntervention = SectionQuestionAnserTask.Section_Intervention;
                }

                if (hasScore)
                {
                    if (objIntervention.MinScore <= score && (objIntervention.MaxScore >= score || objIntervention.MaxScore == null))
                        GetlstResidentSectionQuestionAnsTask.Add(objIntervention);
                }


            }

            List<Section_Intervention> DistinctListSectionIntervention = new List<Section_Intervention>();

            var DistinctItems = GetlstResidentSectionQuestionAnsTask.GroupBy(x => x.ID).Select(y => y.First());

            foreach (var item in DistinctItems)
            {
                if (item.ID == SectionInterventionID)
                    DistinctListSectionIntervention.Add(item);
            }

            return Ok(DistinctListSectionIntervention);
        }

        [HttpPost]
        [Route("GetInterventionAssessmentSummary")]
        public async Task<IHttpActionResult> GetInterventionAssessmentSummary(Guid residentId)
        {

            List<Intervention_Question_Answer_Task> GetlstResidentSectionQuestionAnsTask = new List<Intervention_Question_Answer_Task>();
            List<Resident_Interventions_Questions_Answers> lstResidentAnsweredQuestions = new List<Resident_Interventions_Questions_Answers>();
            lstResidentAnsweredQuestions = await db.Resident_Interventions_Questions_Answers.Include(obj => obj.Intervention_Question_Answer).Where(obj => obj.IsActive && obj.Intervention_Question_Answer.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Resident_Interventions_Questions_Answers>();

            List<Intervention_Question_Answer_Task> lstSectionQuestionsAnswersTask = new List<Intervention_Question_Answer_Task>();
            lstSectionQuestionsAnswersTask = await db.Intervention_Question_Answer_Task.Include(obj => obj.Section_Intervention).Include(obj => obj.Section_Intervention.Actions.Select(i => i.Actions_Days)).Include(i => i.Section_Intervention.Intervention_Question).Include(k => k.Section_Intervention.Intervention_Question.Select(o => o.Intervention_Question_Answer)).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Intervention_Question_Answer_Task>();

            //foreach (Intervention_Question_Answer_Task objSecQueAnsTask in lstSectionQuestionsAnswersTask)
            //{

            //    objSecQueAnsTask.Section_Intervention.Actions = objSecQueAnsTask.Section_Intervention.Actions.Where(obj => obj.IsActive && obj.ResidentID == residentId).ToList<CHM.Services.Models.Action>();
            //    foreach (CHM.Services.Models.Action objAction in objSecQueAnsTask.Section_Intervention.Actions)
            //    {
            //        objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

            //        foreach (Actions_Days objActions_Days in objAction.Actions_Days)
            //        {
            //            objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
            //        }
            //    }

            //}






            List<Intervention_Question_Answer_Task> lstSectionQuestionAnswerHasParentAnswerID = new List<Intervention_Question_Answer_Task>();
            List<Intervention_Question_Answer_Task> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Intervention_Question_Answer_Task>();
            foreach (Intervention_Question_Answer_Task objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
            {
                if (objSecQuesAnsTask.InterventionAnswerID != null)
                    lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                else
                    lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
            }


            foreach (Resident_Interventions_Questions_Answers objResidentQuestionAnswer in lstResidentAnsweredQuestions)
            {
                foreach (Intervention_Question_Answer_Task objSectionQuestionAnsTask in lstSectionQuestionAnswerHasParentAnswerID)
                {
                    if (objResidentQuestionAnswer.Intervention_Question_AnswerID == objSectionQuestionAnsTask.InterventionAnswerID)
                    {
                        GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask);
                    }
                }
            }




            //var SectionQuestionAnserTaskGroupByIntervention = from SectionQuestionAnserTask in lstSectionQuestionAnswerHasNoParentAnswerID
            //                                                  group SectionQuestionAnserTask by SectionQuestionAnserTask.Section_InterventionID;

            //foreach (var group in SectionQuestionAnserTaskGroupByIntervention)
            //{
            //    int score = 0;
            //    Section_Intervention objIntervention = new Section_Intervention();
            //    foreach (var SectionQuestionAnserTask in group)
            //    {


            //        foreach (Resident_Interventions_Questions_Answers objResidentQueAns in lstResidentAnsweredQuestions)
            //        {
            //            if (objResidentQueAns.Intervention_Question_Answer.Intervention_QuestionID == SectionQuestionAnserTask.InterventionQuestionID)
            //            {
            //                score += Convert.ToInt32(objResidentQueAns.Intervention_Question_Answer.Score);
            //            }
            //        }
            //        objIntervention = SectionQuestionAnserTask.Section_Intervention;
            //    }

            //    if (score > 0)
            //    {
            //        if (objIntervention.MinScore <= score && objIntervention.MaxScore >= score)
            //            GetlstResidentSectionQuestionAnsTask.Add(objIntervention);
            //    }


            //}





            return Ok(GetlstResidentSectionQuestionAnsTask);
        }



        [HttpPost]
        [Route("GetAssessmentSummaryOnScores")]
        public async Task<IHttpActionResult> GetAssessmentSummaryOnScores(Guid residentId)
        {
            //List<Residents_Questions_Answers> lstAssessmentSummary = new List<Residents_Questions_Answers>();
            //lstAssessmentSummary = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers.Sections_Questions_Answers_Tasks).Include(obj => obj.Sections_Questions_Answers.Sections_Questions.Section).Include(obj => obj.Actions.Select(s => s.Actions_Days)).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.Sections_Questions_Answers.Section.IsActive && obj.ResidentID.Equals(residentId) && obj.Sections_Questions_Answers.Sections_Questions_Answers_Tasks.Count > 0).ToListAsync<Residents_Questions_Answers>();


            //foreach (Residents_Questions_Answers objResidents_Question_Answers in lstAssessmentSummary)
            //{
            //    objResidents_Question_Answers.Actions = objResidents_Question_Answers.Actions.Where(obj => obj.IsActive).ToList<CHM.Services.Models.Action>();

            //    foreach (CHM.Services.Models.Action objAction in objResidents_Question_Answers.Actions)
            //    {
            //        objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

            //        foreach (Actions_Days objActions_Days in objAction.Actions_Days)
            //        {
            //            objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
            //        }
            //    }
            //}






            //return Ok(lstAssessmentSummary);


            List<Section_Intervention> GetlstResidentSectionQuestionAnsTask = new List<Section_Intervention>();
            List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
            lstResidentAnsweredQuestions = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.ResidentID.Equals(residentId)).ToListAsync<Residents_Questions_Answers>();

            List<Sections_Questions_Answers_Tasks> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Tasks>();
            lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Tasks.Include(obj => obj.Section_Intervention).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Sections_Questions_Answers_Tasks>();

            List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
            List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
            foreach (Sections_Questions_Answers_Tasks objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
            {
                if (objSecQuesAnsTask.Section_Question_AnswerID != null)
                    lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                else
                    lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
            }


            foreach (Residents_Questions_Answers objResidentQuestionAnswer in lstResidentAnsweredQuestions)
            {
                foreach (Sections_Questions_Answers_Tasks objSectionQuestionAnsTask in lstSectionQuestionAnswerHasParentAnswerID)
                {
                    if (objResidentQuestionAnswer.Section_Question_AnswerID == objSectionQuestionAnsTask.Section_Question_AnswerID)
                    {
                        GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask.Section_Intervention);
                    }
                }
            }

            //var UniqueInterventionID = lstSectionQuestionAnswerHasNoParentAnswerID.Select(x => new { x.Section_InterventionID}).Distinct();

            // IEnumerable<Guid> UniqueInterventionID = lstSectionQuestionAnswerHasNoParentAnswerID.Select(x => x.Section_InterventionID).Distinct();


            var SectionQuestionAnserTaskGroupByIntervention = from SectionQuestionAnserTask in lstSectionQuestionAnswerHasNoParentAnswerID
                                                              group SectionQuestionAnserTask by SectionQuestionAnserTask.Section_InterventionID;

            foreach (var group in SectionQuestionAnserTaskGroupByIntervention)
            {
                int score = 0;
                bool hasScore = false;
                Section_Intervention objIntervention = new Section_Intervention();
                foreach (var SectionQuestionAnserTask in group)
                {


                    foreach (Residents_Questions_Answers objResidentQueAns in lstResidentAnsweredQuestions)
                    {
                        if (objResidentQueAns.Sections_Questions_Answers.Section_QuestionID == SectionQuestionAnserTask.Section_QuestionID)
                        {
                            hasScore = true;
                            score += Convert.ToInt32(objResidentQueAns.Sections_Questions_Answers.Score);
                        }
                    }
                    objIntervention = SectionQuestionAnserTask.Section_Intervention;
                }

                if (hasScore)
                {
                    if (objIntervention.MinScore <= score && (objIntervention.MaxScore >= score || objIntervention.MaxScore == null))
                        GetlstResidentSectionQuestionAnsTask.Add(objIntervention);
                }


            }

            //code on 6/15/2016
            List<Section_Intervention> DistinctListSectionIntervention = new List<Section_Intervention>();

            var DistinctItems = GetlstResidentSectionQuestionAnsTask.GroupBy(x => x.ID).Select(y => y.First());

            foreach (var item in DistinctItems)
            {
                DistinctListSectionIntervention.Add(item);
            }



            List<Guid> SectionInterventionIDs = new List<Guid>();
            SectionInterventionIDs = DistinctListSectionIntervention.Select(i => i.ID).ToList<Guid>();

            List<Sections_Questions_Answers_Tasks> sectionQuestionAnswerTask = new List<Sections_Questions_Answers_Tasks>();
            sectionQuestionAnswerTask = db.Sections_Questions_Answers_Tasks.Include(i => i.Sections_Questions_Answers.Section).Include(i => i.Sections_Questions.Section).Where(i => SectionInterventionIDs.Contains(i.Section_InterventionID)).ToList<Sections_Questions_Answers_Tasks>();

            List<Sections_Questions_Answers_Tasks> DistinctSections_Questions_Answers_Tasks = new List<Sections_Questions_Answers_Tasks>();

            var DistinctItemsSection = sectionQuestionAnswerTask.GroupBy(x => x.Section_InterventionID).Select(y => y.First());

            foreach (var item in DistinctItemsSection)
            {
                DistinctSections_Questions_Answers_Tasks.Add(item);
            }
            List<SectionInterventionSection> lstSection = new List<SectionInterventionSection>();

            foreach (Sections_Questions_Answers_Tasks obj in DistinctSections_Questions_Answers_Tasks)
            {
                SectionInterventionSection objSection = new SectionInterventionSection();
                if (obj.Section_Question_AnswerID == null)
                {
                    objSection.sectionName = obj.Sections_Questions.Section.Name;
                    objSection.ID = obj.Sections_Questions.Section.ID;
                }
                else
                {
                    objSection.sectionName = obj.Sections_Questions_Answers.Section.Name;
                    objSection.ID = obj.Sections_Questions_Answers.Section.ID;
                }


                objSection.section_InterventionID = obj.Section_InterventionID;
                lstSection.Add(objSection);
            }



            clsSectionwithSectionIntervention objClsSectionWithSectionIntervention = new clsSectionwithSectionIntervention();
            objClsSectionWithSectionIntervention.SectionInterventionResponse = DistinctListSectionIntervention;
            objClsSectionWithSectionIntervention.SectionInterventionSection = lstSection;

            return Ok(objClsSectionWithSectionIntervention);


        }














        [HttpPost]
        [Route("SaveAssessmentData")]
        public async Task<IHttpActionResult> SaveAssessmentData(List<Residents_Questions_Answers> lstAssessmentData)
        {
            await saveAssessmentData(lstAssessmentData);
            return Ok();
        }

        [HttpPost]
        [Route("UpdateAssessmentData")]
        public async Task<IHttpActionResult> UpdateAssessmentData(Guid residentId, [FromBody] List<Residents_Questions_Answers> lstAssessmentData)
        {
            Resident objResident = await db.Residents.FindAsync(residentId);

            if (objResident == null)
            {

            }
            if (lstAssessmentData.Count > 0)
            {
                //Sections_Questions_Answers objSection_Question_Answer = await db.Sections_Questions_Answers.FindAsync(lstAssessmentData[0].Section_Question_AnswerID);

                //code Commented on  6/3/2016
                List<Guid> sections_Questions_AnswersIds = lstAssessmentData.Select(obj => obj.Section_Question_AnswerID).ToList<Guid>();

                sections_Questions_AnswersIds = sections_Questions_AnswersIds.Concat(GetSubQuestions_AnswersIds(sections_Questions_AnswersIds)).ToList<Guid>();

                List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
                List<Guid> residents_Questions_AnswersIds = db.Residents_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Sections_Questions_Answers.Section_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();

                //End


                //Code new code
                //List<Guid> sections_Questions_AnswersIds = lstAssessmentData.Select(obj => obj.Section_Question_AnswerID).ToList<Guid>();
                //List<Sections_Questions_Answers> lstSections_Questions_Answers = db.Sections_Questions_Answers.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).ToList<Sections_Questions_Answers>();
                //List<Sections_Questions_Answers> lstSectionQueAnswer = db.Sections_Questions_Answers.Where(obj => obj.SectionID == lstSections_Questions_Answers[0].SectionID).ToList<Sections_Questions_Answers>();
                //List<Guid> lstGuidAnswerIDs = lstSectionQueAnswer.Select(obj => obj.ID).ToList<Guid>();
                //List<Guid> residents_Questions_AnswersIds = new List<Guid>();
                //residents_Questions_AnswersIds = db.Residents_Questions_Answers.Where(obj => lstGuidAnswerIDs.Contains(obj.Section_Question_AnswerID)).Select(i=>i.ID).ToList<Guid>();


                //End
                if (objResident.IsAccepted == true)
                {
                    await inactivateAssessmentData(residents_Questions_AnswersIds);
                }
                else
                {
                    await deleteAssessmentData(residents_Questions_AnswersIds);
                }

                await saveAssessmentData(lstAssessmentData);
            }
            return Ok();

        }

        [HttpPost]
        [Route("UpdateAssessmentDataWithFiles")]
        public async Task<HttpResponseMessage> UpdateAssessmentDataWithFiles(Guid residentId)
        {
            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers");
            string tempFolder = "temp";
            var provider = new MultipartFormDataStreamProvider(Path.Combine(root, tempFolder));

            try
            {
                List<Residents_Questions_Answers> lstAssessmentData = new List<Residents_Questions_Answers>();
                List<ResidentAnswerAssessment> lstResidentAnswerAssessment = new List<ResidentAnswerAssessment>();
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
                            lstResidentAnswerAssessment = JsonConvert.DeserializeObject<List<ResidentAnswerAssessment>>(val);
                            break;
                        }
                        break;
                    }
                }


                //Start- Updating assessment data

                Resident objResident = await db.Residents.FindAsync(residentId);

                if (objResident == null)
                {

                }
                if (lstResidentAnswerAssessment.Count > 0)
                {
                    //Code 6/7/2016
                    List<ResidentAnswerAssessment> lstNotModifiedOldResidentAns = lstResidentAnswerAssessment.Where(i => i.oldChosenAnswer != i.Section_Question_AnswerId).ToList<ResidentAnswerAssessment>();


                    List<ResidentAnswerAssessment> lstResidentAnswerHasScore = new List<ResidentAnswerAssessment>();
                    lstResidentAnswerHasScore = lstNotModifiedOldResidentAns.Where(i => i.HasScore != null).ToList<ResidentAnswerAssessment>();

                    List<ResidentAnswerAssessment> lstResidentAnswerHasNoScore = new List<ResidentAnswerAssessment>();
                    lstResidentAnswerHasNoScore = lstNotModifiedOldResidentAns.Where(i => i.HasScore == null).ToList<ResidentAnswerAssessment>();

                    List<Guid> residents_Questions_AnswersIds = new List<Guid>();
                    if (lstResidentAnswerHasNoScore.Count > 0)
                    {
                        List<OldChoosenAnswersID> lstoldChoosenAnswerID = new List<OldChoosenAnswersID>();
                        List<ResidentAnswerAssessment> lstSectionQuestionAnsIsNull = lstResidentAnswerHasNoScore.ToList<ResidentAnswerAssessment>();
                        if (lstSectionQuestionAnsIsNull.Count > 0)
                        {
                            foreach (ResidentAnswerAssessment obj in lstSectionQuestionAnsIsNull)
                            {
                                OldChoosenAnswersID objOldChoosenAnswerId = new OldChoosenAnswersID();
                                if (obj.oldChosenAnswer != null)
                                {
                                    if (obj.oldChosenAnswer.HasValue)
                                    {
                                        objOldChoosenAnswerId.InActiveAnswerIDs = new Guid(obj.oldChosenAnswer.Value.ToString());
                                        lstoldChoosenAnswerID.Add(objOldChoosenAnswerId);
                                    }
                                }
                                else
                                {
                                    objOldChoosenAnswerId.InActiveAnswerIDs = new Guid(obj.Section_Question_AnswerId.Value.ToString());
                                    lstoldChoosenAnswerID.Add(objOldChoosenAnswerId);
                                }
                            }
                            List<Guid> lstSectionQuestionAnswerIDs = lstoldChoosenAnswerID.Select(i => i.InActiveAnswerIDs).ToList<Guid>();





                            // residents_Questions_AnswersIds = db.Residents_Questions_Answers.Where(obj => lstSectionQuestionAnswerIDs.Contains(obj.Section_Question_AnswerID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();

                            //code on 6/10/2016
                            lstSectionQuestionAnswerIDs = lstSectionQuestionAnswerIDs.Concat(GetSubQuestions_AnswersIds(lstSectionQuestionAnswerIDs)).ToList<Guid>();

                            List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(obj => lstSectionQuestionAnswerIDs.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
                            List<Guid> residents_Questions_AnswersIdsSub = db.Residents_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Sections_Questions_Answers.Section_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();
                            //
                            foreach (Guid objGuid in residents_Questions_AnswersIdsSub)
                            {
                                residents_Questions_AnswersIds.Add(objGuid);
                            }

                        }
                        List<ResidentAnswerAssessment> lstSectionQuestionAnsIsNotNull = lstResidentAnswerHasNoScore.Where(i => i.Section_Question_AnswerId != null).ToList<ResidentAnswerAssessment>();
                        if (lstSectionQuestionAnsIsNotNull.Count > 0)
                        {
                            foreach (ResidentAnswerAssessment objAns in lstSectionQuestionAnsIsNotNull)
                            {
                                if (objAns.Section_Question_AnswerId != null)
                                {
                                    if (objAns.Section_Question_AnswerId.HasValue)
                                    {
                                        Residents_Questions_Answers objResidentQuestionAnser = new Residents_Questions_Answers();
                                        objResidentQuestionAnser.ResidentID = objAns.ResidentId;
                                        objResidentQuestionAnser.AnswerText = objAns.AnswerText;
                                        objResidentQuestionAnser.Section_Question_AnswerID = Guid.Parse(objAns.Section_Question_AnswerId.Value.ToString());
                                        lstAssessmentData.Add(objResidentQuestionAnser);
                                    }
                                }
                            }
                        }

                    }

                    if (lstResidentAnswerHasScore.Count > 0)
                    {
                        List<OldChoosenAnswersID> lstoldChoosenAnswerID = new List<OldChoosenAnswersID>();
                        List<ResidentAnswerAssessment> lstSectionQuestionAnsIsNull = lstResidentAnswerHasScore.ToList<ResidentAnswerAssessment>();
                        if (lstSectionQuestionAnsIsNull.Count > 0)
                        {
                            foreach (ResidentAnswerAssessment obj in lstSectionQuestionAnsIsNull)
                            {
                                OldChoosenAnswersID objOldChoosenAnswerId = new OldChoosenAnswersID();
                                if (obj.oldChosenAnswer != null && obj.oldChosenAnswer != obj.Section_Question_AnswerId)
                                {
                                    if (obj.oldChosenAnswer.HasValue)
                                    {
                                        objOldChoosenAnswerId.InActiveAnswerIDs = new Guid(obj.oldChosenAnswer.Value.ToString());
                                        lstoldChoosenAnswerID.Add(objOldChoosenAnswerId);
                                    }
                                }

                            }
                            List<Guid> lstSectionQuestionAnswerIDs = lstoldChoosenAnswerID.Select(i => i.InActiveAnswerIDs).ToList<Guid>();
                            List<Guid> lstResidentIDs = db.Residents_Questions_Answers.Where(obj => lstSectionQuestionAnswerIDs.Contains(obj.Section_Question_AnswerID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();
                            //Code To deactivate Answers Based on scores
                            foreach (Guid obj in lstResidentIDs)
                            {
                                residents_Questions_AnswersIds.Add(obj);
                            }

                            if (lstSectionQuestionAnswerIDs.Count > 0)
                            {

                                List<Guid> lstSectionQuestionIDs = new List<Guid>();
                                lstSectionQuestionIDs = db.Sections_Questions_Answers.Where(obj => lstSectionQuestionAnswerIDs.Contains(obj.ID)).Select(i => i.Section_QuestionID).ToList<Guid>();

                                List<Guid> lstDistinctQuestionIDs = lstSectionQuestionIDs.Distinct().ToList<Guid>();

                                List<Question_ParentQuestion> lstQuestionParenQuestion = new List<Question_ParentQuestion>();
                                lstQuestionParenQuestion = db.Question_ParentQuestion.Where(obj => lstDistinctQuestionIDs.Contains(obj.ParentQuestionID)).ToList<Question_ParentQuestion>();
                                //code 6/13/2016
                                List<Guid> lstQuestion = lstQuestionParenQuestion.Select(i => i.QuestionID).ToList<Guid>();

                                List<Sections_Questions_Answers> lstSectionQuestionAnswerScore = db.Sections_Questions_Answers.Where(i => lstQuestion.Contains(i.Section_QuestionID)).ToList<Sections_Questions_Answers>();

                                List<Guid> lstSectionQuestionAnsids = new List<Guid>();
                                lstSectionQuestionAnsids = lstSectionQuestionAnswerScore.Select(i => i.ID).ToList<Guid>();
                                List<Residents_Questions_Answers> lstNewResidentQuestionAns = new List<Residents_Questions_Answers>();
                                lstNewResidentQuestionAns = db.Residents_Questions_Answers.Where(i => lstSectionQuestionAnsids.Contains(i.Section_Question_AnswerID) && i.ResidentID == residentId && i.IsActive == true).ToList<Residents_Questions_Answers>();

                                List<Guid> residentIDsofScoreQuestion = new List<Guid>();
                                residentIDsofScoreQuestion = lstNewResidentQuestionAns.Select(i => i.ID).ToList<Guid>();
                                foreach (Guid objGuid in residentIDsofScoreQuestion)
                                {
                                    residents_Questions_AnswersIds.Add(objGuid);
                                }


                                List<Guid> lstSubQuestionIDs = lstSectionQuestionAnswerIDs.Concat(GetSubQuestions_AnswersIds(lstSectionQuestionAnsids)).ToList<Guid>();

                                List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(obj => lstSubQuestionIDs.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
                                List<Guid> residents_Questions_AnswersIdsSub = db.Residents_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Sections_Questions_Answers.Section_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();
                                //
                                foreach (Guid objGuid in residents_Questions_AnswersIdsSub)
                                {
                                    residents_Questions_AnswersIds.Add(objGuid);
                                }
                            }
                            //    //code

                            //    var results = from objQuestionParentQuestion in lstQuestionParenQuestion
                            //                  group objQuestionParentQuestion.QuestionID by objQuestionParentQuestion.ParentQuestionID into g
                            //                  select new { ParentQuestion = g.Key, Questions = g.ToList() };

                            //    foreach(var obj in results)
                            //    {
                            //       foreach(Guid objQuestion in obj.Questions)
                            //       {

                            //           int Score = -1;
                            //           List<Guid> lstParentQuestion = new List<Guid>();
                            //           lstParentQuestion = db.Question_ParentQuestion.Where(i => i.QuestionID == objQuestion).Select(i => i.ParentQuestionID).ToList<Guid>();

                            //           List<Guid> lstSectionQuestion = new List<Guid>();
                            //           lstSectionQuestion = db.Sections_Questions.Where(i => lstParentQuestion.Contains(i.ID)).Select(i=>i.ID).ToList<Guid>();

                            //           List<Sections_Questions_Answers> lstSectionQuestionAns = new List<Sections_Questions_Answers>();
                            //           lstSectionQuestionAns = db.Sections_Questions_Answers.Where(i => lstSectionQuestion.Contains(i.Section_QuestionID)).ToList<Sections_Questions_Answers>();
                            //           if (lstSectionQuestionAns.Count > 0)
                            //           {
                            //               List<ResidentAnswerAssessment> lstResAnsAssessment = lstResidentAnswerAssessment.Where(i => i.HasScore != null && i.Section_Question_AnswerId != null).ToList<ResidentAnswerAssessment>();
                            //               List<OldChoosenAnswersID> lstNewAnswerIDs = new List<OldChoosenAnswersID>();
                            //               foreach (ResidentAnswerAssessment objResidentAnswer in lstResAnsAssessment)
                            //               {
                            //                   OldChoosenAnswersID objOldChoosenAnswerId = new OldChoosenAnswersID();
                            //                   if (objResidentAnswer.oldChosenAnswer != null)
                            //                   {
                            //                       if (objResidentAnswer.oldChosenAnswer.HasValue)
                            //                       {
                            //                           objOldChoosenAnswerId.InActiveAnswerIDs = new Guid(objResidentAnswer.oldChosenAnswer.Value.ToString());
                            //                           lstNewAnswerIDs.Add(objOldChoosenAnswerId);
                            //                       }
                            //                   }
                            //               }

                            //               List<Guid> guidAnswerIds=lstNewAnswerIDs.Select(i=>i.InActiveAnswerIDs).ToList<Guid>();

                            //               if(guidAnswerIds.Count>0)
                            //               {
                            //                   Score = 0;
                            //                   List<Sections_Questions_Answers> lstSectionQuestionAnswer = lstSectionQuestionAns.Where(i => guidAnswerIds.Contains(i.ID)).ToList<Sections_Questions_Answers>();
                            //                   foreach(Sections_Questions_Answers objSecQueAns in lstSectionQuestionAnswer)
                            //                   {
                            //                       Score +=Convert.ToInt32(objSecQueAns.Score);
                            //                   }

                            //                   //code on  6/12/2016

                            //                   List<Guid> lstAnswerIDs = lstSectionQuestionAnswer.Select(i => i.ID).ToList<Guid>();

                            //                   //code on 6/12/2016

                            //                  lstSectionQuestionAnswerIDs = lstSectionQuestionAnswerIDs.Concat(GetSubQuestions_AnswersIds(lstAnswerIDs)).ToList<Guid>();

                            //                   List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(objsecQue => lstSectionQuestionAnswerIDs.Contains(objsecQue.ID)).Select(objsecQue => objsecQue.Section_QuestionID).ToList<Guid>();
                            //                   List<Guid> residents_Questions_AnswersIdsSub = db.Residents_Questions_Answers.Where(objResAns => section_QuestionIds.Contains(objResAns.Sections_Questions_Answers.Section_QuestionID) && objResAns.ResidentID.Equals(residentId)).Select(objResAns => objResAns.ID).ToList<Guid>();

                            //                   foreach (Guid objGuid in residents_Questions_AnswersIdsSub)
                            //                   {
                            //                       residents_Questions_AnswersIds.Add(objGuid);
                            //                   }

                            //                   //code on 6/12/2016



                            //                   List<Guid> ResidentIds = db.Residents_Questions_Answers.Where(k => lstAnswerIDs.Contains(k.Section_Question_AnswerID) && k.ResidentID.Equals(residentId)).Select(k => k.ID).ToList<Guid>();
                            //                   foreach (Guid objGuid in ResidentIds)
                            //                   {
                            //                       residents_Questions_AnswersIds.Add(objGuid);
                            //                   }
                            //                   //code end on  6/12/2016
                            //               }


                            //           }

                            //           if(Score>=0)
                            //           {
                            //               Sections_Questions objSectionQuestion = new Sections_Questions();
                            //               objSectionQuestion = db.Sections_Questions.Where(i => i.ID == objQuestion).FirstOrDefault();
                            //               if(objSectionQuestion!=null)
                            //               {
                            //                   if(!(objSectionQuestion.MinScore<=Score && (objSectionQuestion.MaxScore>=Score||objSectionQuestion.MaxScore==null))||(objSectionQuestion.MinScore==null))
                            //                   {
                            //                       List<Sections_Questions_Answers> lstSectionQuestionAnswers = new List<Sections_Questions_Answers>();
                            //                       lstSectionQuestionAnswers = db.Sections_Questions_Answers.Where(i => i.Section_QuestionID == objSectionQuestion.ID).ToList<Sections_Questions_Answers>();



                            //                       //code on 6/12/2016
                            //                       List<Guid> lstSecQuestionIdsofScores = lstSectionQuestionAnswers.Select(i => i.ID).ToList<Guid>();
                            //                       lstSectionQuestionAnswerIDs = lstSectionQuestionAnswerIDs.Concat(GetSubQuestions_AnswersIds(lstSecQuestionIdsofScores)).ToList<Guid>();

                            //                       List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(objsecQue => lstSectionQuestionAnswerIDs.Contains(objsecQue.ID)).Select(objsecQue => objsecQue.Section_QuestionID).ToList<Guid>();
                            //                       List<Guid> residents_Questions_AnswersIdsSub = db.Residents_Questions_Answers.Where(objResAns => section_QuestionIds.Contains(objResAns.Sections_Questions_Answers.Section_QuestionID) && objResAns.ResidentID.Equals(residentId)).Select(objResAns => objResAns.ID).ToList<Guid>();

                            //                       foreach (Guid objGuid in residents_Questions_AnswersIdsSub)
                            //                       {
                            //                           residents_Questions_AnswersIds.Add(objGuid);
                            //                       }

                            //                         //code on 6/12/2016


                            //                       if(lstSectionQuestionAnswers.Count>0)
                            //                       {
                            //                           List<Guid> lstGuidAnswer = new List<Guid>();
                            //                           lstGuidAnswer=lstSectionQuestionAnswers.Select(i => i.ID).ToList<Guid>();
                            //                           List<Guid> ResidentIds = db.Residents_Questions_Answers.Where(k => lstGuidAnswer.Contains(k.Section_Question_AnswerID) && k.ResidentID.Equals(residentId)).Select(k => k.ID).ToList<Guid>();
                            //                           foreach (Guid objGuid in ResidentIds)
                            //                           {
                            //                               residents_Questions_AnswersIds.Add(objGuid);
                            //                           }
                            //                       }
                            //                   }


                            //               }
                            //           }

                            //       }
                            //    }
                            //}



                            //End




                        }
                        List<ResidentAnswerAssessment> lstSectionQuestionAnsIsNotNull = lstResidentAnswerHasScore.Where(i => i.Section_Question_AnswerId != null).ToList<ResidentAnswerAssessment>();
                        if (lstSectionQuestionAnsIsNotNull.Count > 0)
                        {
                            foreach (ResidentAnswerAssessment objAns in lstSectionQuestionAnsIsNotNull)
                            {
                                if (objAns.Section_Question_AnswerId != null)
                                {
                                    if (objAns.Section_Question_AnswerId.HasValue)
                                    {
                                        Residents_Questions_Answers objResidentQuestionAnser = new Residents_Questions_Answers();
                                        objResidentQuestionAnser.ResidentID = objAns.ResidentId;
                                        objResidentQuestionAnser.AnswerText = objAns.AnswerText;
                                        objResidentQuestionAnser.Section_Question_AnswerID = Guid.Parse(objAns.Section_Question_AnswerId.Value.ToString());
                                        lstAssessmentData.Add(objResidentQuestionAnser);
                                    }
                                }
                            }
                        }
                    }


                    //code on  6/9/2016

                    if (lstAssessmentData.Count > 0)
                    {
                        List<Guid> lstAnswerTextResidents = new List<Guid>();
                        lstAnswerTextResidents = lstAssessmentData.Where(i => i.AnswerText != null).Select(i => i.Section_Question_AnswerID).ToList<Guid>();
                        List<Residents_Questions_Answers> lstResidentQuestionAns = db.Residents_Questions_Answers.Where(i => lstAnswerTextResidents.Contains(i.Section_Question_AnswerID) && i.ResidentID == residentId).ToList<Residents_Questions_Answers>();
                        foreach (Residents_Questions_Answers obj in lstResidentQuestionAns)
                        {
                            residents_Questions_AnswersIds.Add(obj.ID);
                        }

                        List<Guid> guidSectionQuestionIds = new List<Guid>();
                        guidSectionQuestionIds = db.Sections_Questions_Answers.Where(i => lstAnswerTextResidents.Contains(i.ID)).Select(i => i.Section_QuestionID).ToList<Guid>();

                        List<Guid> lstQuestionIds = new List<Guid>();
                        lstQuestionIds = db.Question_ParentQuestion.Where(i => guidSectionQuestionIds.Contains(i.ParentQuestionID)).Select(i => i.QuestionID).ToList<Guid>();

                        List<Guid> lstAnswerIDs = new List<Guid>();
                        lstAnswerIDs = db.Sections_Questions_Answers.Where(i => lstQuestionIds.Contains(i.Section_QuestionID)).Select(i => i.ID).ToList<Guid>();


                        List<Guid> lstSecQuesAnswerIDs = new List<Guid>();
                        lstSecQuesAnswerIDs = (GetSubQuestions_AnswersIds(lstAnswerIDs)).ToList<Guid>();

                        List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(obj => lstSecQuesAnswerIDs.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
                        List<Guid> residents_Questions_AnswersIdsSub = db.Residents_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Sections_Questions_Answers.Section_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();
                        //
                        foreach (Guid objGuid in residents_Questions_AnswersIdsSub)
                        {
                            residents_Questions_AnswersIds.Add(objGuid);
                        }




                        List<Guid> lstGuidResidentAnser = new List<Guid>();
                        lstGuidResidentAnser = db.Residents_Questions_Answers.Where(i => lstAnswerIDs.Contains(i.Section_Question_AnswerID)).Select(i => i.ID).ToList<Guid>();

                        foreach (Guid objGuid in lstGuidResidentAnser)
                        {
                            residents_Questions_AnswersIds.Add(objGuid);
                        }
                    }
                    //End

                    //End


                    //Sections_Questions_Answers objSection_Question_Answer = await db.Sections_Questions_Answers.FindAsync(lstAssessmentData[0].Section_Question_AnswerID);
                    //Code commented on 6/3/2016

                    // List<Guid> sections_Questions_AnswersIds = lstAssessmentData.Select(obj => obj.Section_Question_AnswerID).ToList<Guid>();

                    // sections_Questions_AnswersIds = sections_Questions_AnswersIds.Concat(GetSubQuestions_AnswersIds(sections_Questions_AnswersIds)).ToList<Guid>();

                    //  List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
                    // List<Guid> residents_Questions_AnswersIds = db.Residents_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Sections_Questions_Answers.Section_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();

                    //End

                    //Start new code on 6/3/2016

                    //List<Guid> sections_Questions_AnswersIds = lstAssessmentData.Select(obj => obj.Section_Question_AnswerID).ToList<Guid>();
                    //List<Sections_Questions_Answers> lstSections_Questions_Answers = db.Sections_Questions_Answers.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).ToList<Sections_Questions_Answers>();
                    //List<Sections_Questions_Answers> lstSectionQueAnswer = db.Sections_Questions_Answers.Where(obj => obj.SectionID == lstSections_Questions_Answers[0].SectionID).ToList<Sections_Questions_Answers>();
                    //List<Guid> lstGuidAnswerIDs = lstSectionQueAnswer.Select(obj => obj.ID).ToList<Guid>();
                    //List<Guid> residents_Questions_AnswersIds = new List<Guid>();
                    //residents_Questions_AnswersIds = db.Residents_Questions_Answers.Where(obj => lstGuidAnswerIDs.Contains(obj.Section_Question_AnswerID)).Select(i => i.ID).ToList<Guid>();

                    //End

                    //code
                    if (objResident.IsAccepted == true)
                    {
                        await inactivateAssessmentData(residents_Questions_AnswersIds);

                    }
                    else
                    {
                        await deleteAssessmentData(residents_Questions_AnswersIds);

                        foreach (Guid objGuid in residents_Questions_AnswersIds)
                        {
                            strFolderPath = Path.Combine(root, Convert.ToString(objGuid));
                            if (Directory.Exists(strFolderPath))
                            {
                                DirectoryInfo di = new DirectoryInfo(strFolderPath);
                                di.Delete(true);
                            }
                        }


                    }

                    Guid userId = new Guid(User.Identity.GetUserId());
                    foreach (Residents_Questions_Answers objResident_Question_Answer in lstAssessmentData)
                    {
                        objResident_Question_Answer.ID = Guid.NewGuid();
                        objResident_Question_Answer.IsActive = true;
                        objResident_Question_Answer.CreatedBy = userId;
                        objResident_Question_Answer.ModifiedBy = userId;
                        objResident_Question_Answer.Created = objResident_Question_Answer.Modified = DateTime.Now;

                        db.Residents_Questions_Answers.Add(objResident_Question_Answer);


                        // await UploadAnswerPhoto(objResident_Question_Answer.ID, objResident_Question_Answer.AnswerText.ToString());
                    }
                    await db.SaveChangesAsync();
                }

                //End - Updating assessment data


                // This illustrates how to get the file names for uploaded files. 
                foreach (var fileData in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(fileData.LocalFileName);
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
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

                    Guid section_Question_AnswerId = JsonConvert.DeserializeObject<Guid>(fileData.Headers.ContentDisposition.Name);
                    Guid resident_Question_AnswerId = lstAssessmentData.Where(obj => obj.Section_Question_AnswerID.Equals(section_Question_AnswerId)).FirstOrDefault().ID;

                    strFolderPath = Path.Combine(root, Convert.ToString(resident_Question_AnswerId));
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }




                    File.Move(fileData.LocalFileName, Path.Combine(strFolderPath, fileName));


                }
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Saved Successfully")
                };
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }




        [HttpPost]
        [Route("CopyUpdateAssessmentDataWithFiles")]
        public async Task<IHttpActionResult> CopyUpdateAssessmentDataWithFiles(Guid residentId)
        {
            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
             List<object> lstresidentFiles = new List<object>();
            string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers");
            string tempFolder = "temp";
            var provider = new MultipartFormDataStreamProvider(Path.Combine(root, tempFolder));

            try
            {
                List<Residents_Questions_Answers> lstAssessmentData = new List<Residents_Questions_Answers>();
                List<ResidentAnswerAssessment> lstResidentAnswerAssessment = new List<ResidentAnswerAssessment>();
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
                            lstResidentAnswerAssessment = JsonConvert.DeserializeObject<List<ResidentAnswerAssessment>>(val);
                            break;
                        }
                        break;
                    }
                }


                //Start- Updating assessment data

                Resident objResident = await db.Residents.FindAsync(residentId);

                //Guid organizationID = objResident.OrganizationID;
                if (objResident == null)
                {

                }
                if (lstResidentAnswerAssessment.Count > 0)
                {
                    //Code 6/7/2016
                    List<ResidentAnswerAssessment> lstNotModifiedOldResidentAns = lstResidentAnswerAssessment.Where(i => i.oldChosenAnswer != i.Section_Question_AnswerId).ToList<ResidentAnswerAssessment>();


                    List<ResidentAnswerAssessment> lstResidentAnswerHasScore = new List<ResidentAnswerAssessment>();
                    lstResidentAnswerHasScore = lstNotModifiedOldResidentAns.Where(i => i.HasScore != null).ToList<ResidentAnswerAssessment>();

                    List<ResidentAnswerAssessment> lstResidentAnswerHasNoScore = new List<ResidentAnswerAssessment>();
                    lstResidentAnswerHasNoScore = lstNotModifiedOldResidentAns.Where(i => i.HasScore == null).ToList<ResidentAnswerAssessment>();


                    List<Guid> residents_Questions_AnswersIds = new List<Guid>();
                    if (lstResidentAnswerHasNoScore.Count > 0)
                    {
                        List<OldChoosenAnswersID> lstoldChoosenAnswerID = new List<OldChoosenAnswersID>();
                        List<ResidentAnswerAssessment> lstSectionQuestionAnsIsNull = lstResidentAnswerHasNoScore.ToList<ResidentAnswerAssessment>();
                        if (lstSectionQuestionAnsIsNull.Count > 0)
                        {
                            foreach (ResidentAnswerAssessment obj in lstSectionQuestionAnsIsNull)
                            {
                                OldChoosenAnswersID objOldChoosenAnswerId = new OldChoosenAnswersID();
                                if (obj.oldChosenAnswer != null)
                                {
                                    if (obj.oldChosenAnswer.HasValue)
                                    {
                                        objOldChoosenAnswerId.InActiveAnswerIDs = new Guid(obj.oldChosenAnswer.Value.ToString());
                                        lstoldChoosenAnswerID.Add(objOldChoosenAnswerId);
                                    }
                                }
                                #region commented on 6222016
                                //else
                                //{
                                //    objOldChoosenAnswerId.InActiveAnswerIDs = new Guid(obj.Section_Question_AnswerId.Value.ToString());
                                //    lstoldChoosenAnswerID.Add(objOldChoosenAnswerId);
                                //}
                                #endregion
                            }
                            List<Guid> lstSectionQuestionAnswerIDs = lstoldChoosenAnswerID.Select(i => i.InActiveAnswerIDs).ToList<Guid>();

                            // residents_Questions_AnswersIds = db.Residents_Questions_Answers.Where(obj => lstSectionQuestionAnswerIDs.Contains(obj.Section_Question_AnswerID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();

                            //code on 6/10/2016
                            lstSectionQuestionAnswerIDs = lstSectionQuestionAnswerIDs.Concat(GetSubQuestions_AnswersIds(lstSectionQuestionAnswerIDs)).ToList<Guid>();
                            #region commented on 6222016
                            // List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(obj => lstSectionQuestionAnswerIDs.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
                            //List<Guid> residents_Questions_AnswersIdsSub = db.Residents_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Sections_Questions_Answers.Section_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();
                            //
                            #endregion
                            List<Guid> residents_Questions_AnswersIdsSub = db.Residents_Questions_Answers.Where(obj => lstSectionQuestionAnswerIDs.Contains(obj.Section_Question_AnswerID) && obj.ResidentID == residentId).Select(i => i.ID).ToList<Guid>();
                            foreach (Guid objGuid in residents_Questions_AnswersIdsSub)
                            {
                                residents_Questions_AnswersIds.Add(objGuid);
                            }

                        }
                        List<ResidentAnswerAssessment> lstSectionQuestionAnsIsNotNull = lstResidentAnswerHasNoScore.Where(i => i.Section_Question_AnswerId != null).ToList<ResidentAnswerAssessment>();
                        if (lstSectionQuestionAnsIsNotNull.Count > 0)
                        {
                            foreach (ResidentAnswerAssessment objAns in lstSectionQuestionAnsIsNotNull)
                            {
                                if (objAns.Section_Question_AnswerId != null)
                                {
                                    if (objAns.Section_Question_AnswerId.HasValue)
                                    {
                                        Residents_Questions_Answers objResidentQuestionAnser = new Residents_Questions_Answers();
                                        objResidentQuestionAnser.ResidentID = objAns.ResidentId;
                                        objResidentQuestionAnser.AnswerText = objAns.AnswerText;
                                        objResidentQuestionAnser.Section_Question_AnswerID = Guid.Parse(objAns.Section_Question_AnswerId.Value.ToString());
                                        lstAssessmentData.Add(objResidentQuestionAnser);
                                    }
                                }
                            }
                        }
                    }


                    //code on  6/9/2016

                    if (lstAssessmentData.Count > 0)
                    {
                        List<Guid> lstAnswerTextResidents = new List<Guid>();
                        lstAnswerTextResidents = lstAssessmentData.Where(i => i.AnswerText != null).Select(i => i.Section_Question_AnswerID).ToList<Guid>();
                        List<Residents_Questions_Answers> lstResidentQuestionAns = db.Residents_Questions_Answers.Where(i => lstAnswerTextResidents.Contains(i.Section_Question_AnswerID) && i.ResidentID == residentId).ToList<Residents_Questions_Answers>();
                        foreach (Residents_Questions_Answers obj in lstResidentQuestionAns)
                        {
                            residents_Questions_AnswersIds.Add(obj.ID);
                        }

                        List<Guid> guidSectionQuestionIds = new List<Guid>();
                        guidSectionQuestionIds = db.Sections_Questions_Answers.Where(i => lstAnswerTextResidents.Contains(i.ID)).Select(i => i.Section_QuestionID).ToList<Guid>();

                        List<Guid> lstQuestionIds = new List<Guid>();
                        lstQuestionIds = db.Question_ParentQuestion.Where(i => guidSectionQuestionIds.Contains(i.ParentQuestionID)).Select(i => i.QuestionID).ToList<Guid>();

                        List<Guid> lstAnswerIDs = new List<Guid>();
                        lstAnswerIDs = db.Sections_Questions_Answers.Where(i => lstQuestionIds.Contains(i.Section_QuestionID)).Select(i => i.ID).ToList<Guid>();


                        List<Guid> lstSecQuesAnswerIDs = new List<Guid>();
                        lstSecQuesAnswerIDs = (GetSubQuestions_AnswersIds(lstAnswerIDs)).ToList<Guid>();

                        List<Guid> section_QuestionIds = db.Sections_Questions_Answers.Where(obj => lstSecQuesAnswerIDs.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
                        List<Guid> residents_Questions_AnswersIdsSub = db.Residents_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Sections_Questions_Answers.Section_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();
                        //
                        foreach (Guid objGuid in residents_Questions_AnswersIdsSub)
                        {
                            residents_Questions_AnswersIds.Add(objGuid);
                        }


                        List<Guid> lstGuidResidentAnser = new List<Guid>();
                        lstGuidResidentAnser = db.Residents_Questions_Answers.Where(i => lstAnswerIDs.Contains(i.Section_Question_AnswerID)).Select(i => i.ID).ToList<Guid>();

                        foreach (Guid objGuid in lstGuidResidentAnser)
                        {
                            residents_Questions_AnswersIds.Add(objGuid);
                        }
                    }
                  
                    if (objResident.IsAccepted == true)
                    {
                        await inactivateAssessmentData(residents_Questions_AnswersIds);

                    }
                    else
                    {
                        await deleteAssessmentData(residents_Questions_AnswersIds);

                        foreach (Guid objGuid in residents_Questions_AnswersIds)
                        {
                            strFolderPath = Path.Combine(root, Convert.ToString(objGuid));
                            if (Directory.Exists(strFolderPath))
                            {
                                DirectoryInfo di = new DirectoryInfo(strFolderPath);
                                di.Delete(true);
                            }
                        }


                    }
                    Guid userId = new Guid(User.Identity.GetUserId());

                    #region OtherText UpdateFro Old has no score 6/24/2016 1.49pm

                    List<ResidentAnswerAssessment> lstOldAnsEqNewAns = lstResidentAnswerAssessment.Where(i => i.oldChosenAnswer == i.Section_Question_AnswerId && i.HasScore == null).ToList<ResidentAnswerAssessment>();
                    foreach (ResidentAnswerAssessment obj in lstOldAnsEqNewAns)
                    {
                        if (obj.oldChosenAnswer == obj.Section_Question_AnswerId && obj.AnswerText != null)
                        {
                            Residents_Questions_Answers objResQueAns = db.Residents_Questions_Answers.Where(i => i.ResidentID == residentId && i.Section_Question_AnswerID == obj.Section_Question_AnswerId && i.IsActive == true).FirstOrDefault();
                            if (objResQueAns != null)
                            {
                                if (objResQueAns.AnswerText != obj.AnswerText)
                                {
                                    objResQueAns.AnswerText = obj.AnswerText;
                                    objResQueAns.Modified = DateTime.Now;
                                    objResQueAns.ModifiedBy = userId;
                                    db.Entry(objResQueAns).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }


                    #endregion


                    foreach (Residents_Questions_Answers objResident_Question_Answer in lstAssessmentData)
                    {
                        objResident_Question_Answer.ID = Guid.NewGuid();
                        objResident_Question_Answer.IsActive = true;
                        objResident_Question_Answer.CreatedBy = userId;
                        objResident_Question_Answer.ModifiedBy = userId;
                        objResident_Question_Answer.Created = objResident_Question_Answer.Modified = DateTime.Now;

                        db.Residents_Questions_Answers.Add(objResident_Question_Answer);
                       // await UploadAnswerPhoto(objResident_Question_Answer.ID, objResident_Question_Answer.AnswerText.ToString());
                    }
                    await db.SaveChangesAsync();

                    //Code for Score start  
                    #region  Scores

                    if (lstResidentAnswerAssessment.Count > 0)
                    {
                        List<Residents_Questions_Answers> lstScoreAssessmentData = new List<Residents_Questions_Answers>();
                        //1)old chosenAnswer equals Section Question Answer
                        List<ResidentAnswerAssessment> lstoldChosenIsEqualSecQuaAnsID = new List<ResidentAnswerAssessment>();
                        lstoldChosenIsEqualSecQuaAnsID = lstResidentAnswerAssessment.Where(i => i.oldChosenAnswer == i.Section_Question_AnswerId && i.HasScore != null).ToList<ResidentAnswerAssessment>();

                        //2)old Chosen not equals Section Question Answerd
                        List<ResidentAnswerAssessment> lstoldChosenIsNotEqualSecQuaAnsID = new List<ResidentAnswerAssessment>();
                        lstoldChosenIsNotEqualSecQuaAnsID = lstResidentAnswerAssessment.Where(i => i.oldChosenAnswer != i.Section_Question_AnswerId && i.HasScore != null && i.oldChosenAnswer != null && i.Section_Question_AnswerId != null).ToList<ResidentAnswerAssessment>();

                        //3)old chosen ans is null
                        List<ResidentAnswerAssessment> lstoldChosenIsNull = new List<ResidentAnswerAssessment>();
                        lstoldChosenIsNull = lstResidentAnswerAssessment.Where(i => i.oldChosenAnswer == null && i.HasScore != null).ToList<ResidentAnswerAssessment>();

                        //4)Section Question Answerid is null
                        List<ResidentAnswerAssessment> lstsecQueAnsIsNull = new List<ResidentAnswerAssessment>();
                        lstsecQueAnsIsNull = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == null && i.HasScore != null).ToList<ResidentAnswerAssessment>();

                        //operation for 2 one to delete  
                        List<Guid> lstGuidAnswerIDs = new List<Guid>();

                        #region lstoldChosenIsNotEqualSecQuaAnsID
                        foreach (ResidentAnswerAssessment objoldAnswerIds in lstoldChosenIsNotEqualSecQuaAnsID)
                        {
                            if (objoldAnswerIds.oldChosenAnswer.HasValue)
                            {
                                lstGuidAnswerIDs.Add(Guid.Parse(objoldAnswerIds.oldChosenAnswer.ToString()));

                                //Change 6/30/2016 9.22pm

                                Guid objAnswerid = Guid.Parse(objoldAnswerIds.oldChosenAnswer.ToString());
                                //Get Question Id From Section Question Answer

                                Sections_Questions_Answers objSectionQuestionAnswer = new Sections_Questions_Answers();
                                objSectionQuestionAnswer = db.Sections_Questions_Answers.Where(obj => obj.ID == objAnswerid).FirstOrDefault();

                                //If objsectionQuestion Answer is Not null
                                if (objSectionQuestionAnswer != null)
                                {
                                    //Get child Question Of this Question  

                                    List<Question_ParentQuestion> objQuestionParentQuestion = new List<Question_ParentQuestion>();
                                    objQuestionParentQuestion = db.Question_ParentQuestion.Where(i => i.ParentQuestionID == objSectionQuestionAnswer.Section_QuestionID && i.IsActive == true).ToList<Question_ParentQuestion>();

                                    //If Count >0 get the childQuestionIDs of this  Parent Question
                                    if (objQuestionParentQuestion.Count > 0)
                                    {
                                        //List guid of child Question
                                        List<Guid> lstChildQuestion = new List<Guid>();
                                        lstChildQuestion = objQuestionParentQuestion.Select(i => i.QuestionID).ToList<Guid>();

                                        Guid objChildQuestionIds = new Guid();
                                        objChildQuestionIds = objQuestionParentQuestion.Where(i => i.IsActive == true).Select(k => k.QuestionID).FirstOrDefault();

                                        //if objChildQuestionIds Count >0
                                        if (objChildQuestionIds != null)
                                        {

                                            //GetChilQuestion 

                                            List<Sections_Questions> lstchildSectionQuestion = new List<Sections_Questions>();
                                            lstchildSectionQuestion = db.Sections_Questions.Where(i => lstChildQuestion.Contains(i.ID)).ToList<Sections_Questions>();


                                            //Get ParentQuestion  of ChildQueastion

                                            List<Guid> lstParentQuestionIds = new List<Guid>();
                                            lstParentQuestionIds = db.Question_ParentQuestion.Where(i => i.QuestionID == objChildQuestionIds && i.IsActive == true).Select(k => k.ParentQuestionID).ToList<Guid>();

                                            //Section_Question Of parent Question
                                            List<Sections_Questions> lstSectionQuestionChild = new List<Sections_Questions>();
                                            lstSectionQuestionChild = db.Sections_Questions.Where(i => lstParentQuestionIds.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions>();



                                            //Get Answers of the Parent Question
                                            List<Sections_Questions_Answers> lstSectionQuestionAnswerofChldQues = new List<Sections_Questions_Answers>();
                                            lstSectionQuestionAnswerofChldQues = db.Sections_Questions_Answers.Where(i => lstParentQuestionIds.Contains(i.Section_QuestionID) && i.IsActive == true).ToList<Sections_Questions_Answers>();

                                            if (lstSectionQuestionAnswerofChldQues.Count > 0)
                                            {
                                                //Get Guid of 1234 Question
                                                List<Guid> guid1 = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj in lstoldChosenIsEqualSecQuaAnsID)
                                                {
                                                    if (obj.oldChosenAnswer.HasValue)
                                                    {
                                                        guid1.Add(Guid.Parse(obj.oldChosenAnswer.ToString()));
                                                    }
                                                }

                                                List<Guid> guid2a = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj2 in lstoldChosenIsNotEqualSecQuaAnsID)
                                                {
                                                    if (obj2.oldChosenAnswer.HasValue)
                                                    {
                                                        guid2a.Add(Guid.Parse(obj2.oldChosenAnswer.ToString()));
                                                    }
                                                }

                                                List<Guid> guid2b = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj2b in lstoldChosenIsNotEqualSecQuaAnsID)
                                                {
                                                    if (obj2b.Section_Question_AnswerId.HasValue)
                                                    {
                                                        guid2b.Add(Guid.Parse(obj2b.Section_Question_AnswerId.ToString()));
                                                    }
                                                }

                                                List<Guid> guid3 = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj3 in lstoldChosenIsNull)
                                                {

                                                    if (obj3.Section_Question_AnswerId.HasValue)
                                                    {
                                                        guid3.Add(Guid.Parse(obj3.Section_Question_AnswerId.ToString()));
                                                    }
                                                }

                                                List<Guid> guid4 = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj4 in lstsecQueAnsIsNull)
                                                {

                                                    if (obj4.oldChosenAnswer.HasValue)
                                                    {
                                                        guid4.Add(Guid.Parse(obj4.oldChosenAnswer.ToString()));
                                                    }
                                                }

                                                List<Sections_Questions_Answers> lst1 = lstSectionQuestionAnswerofChldQues.Where(i => guid1.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();

                                                int Score1 = 0;
                                                bool hasScore1 = false;
                                                //check if lst1 Count greater then 0
                                                if (lst1.Count > 0)
                                                {
                                                    //Loopin threw to get sum of scores
                                                    foreach (Sections_Questions_Answers obj1 in lst1)
                                                    {

                                                        hasScore1 = true;
                                                        if (obj1.AnswerType == "FreeText" && obj1.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj1.ID || i.oldChosenAnswer == obj1.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score1 += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score1 += Convert.ToInt32(obj1.Score);
                                                        }
                                                    }
                                                }


                                                //score for 2a 
                                                int Score2a = 0;
                                                bool hasScore2a = false;
                                                List<Sections_Questions_Answers> lst2a = lstSectionQuestionAnswerofChldQues.Where(i => guid2a.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                                                if (lst2a.Count > 0)
                                                {
                                                    foreach (Sections_Questions_Answers obj2a in lst2a)
                                                    {
                                                        hasScore2a = true;
                                                        if (obj2a.AnswerType == "FreeText" && obj2a.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj2a.ID || i.oldChosenAnswer == obj2a.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score2a += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score2a += Convert.ToInt32(obj2a.Score);
                                                        }
                                                    }
                                                }


                                                //score for 2b
                                                int Score2b = 0;
                                                bool hasScore2b = false;
                                                List<Sections_Questions_Answers> lst2b = lstSectionQuestionAnswerofChldQues.Where(i => guid2b.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                                                if (lst2b.Count > 0)
                                                {
                                                    foreach (Sections_Questions_Answers obj2b in lst2b)
                                                    {
                                                        hasScore2b = true;
                                                        if (obj2b.AnswerType == "FreeText" && obj2b.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj2b.ID || i.oldChosenAnswer == obj2b.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score2b += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score2b += Convert.ToInt32(obj2b.Score);
                                                        }
                                                    }
                                                }

                                                List<Sections_Questions_Answers> lst3 = lstSectionQuestionAnswerofChldQues.Where(i => guid3.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                                                int Score3 = 0;
                                                bool hasScore3 = false;
                                                //check if lst3 Count greater then 0
                                                if (lst3.Count > 0)
                                                {
                                                    //Loopin threw to get sum of scores
                                                    foreach (Sections_Questions_Answers obj3 in lst3)
                                                    {
                                                        hasScore3 = true;
                                                        if (obj3.AnswerType == "FreeText" && obj3.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj3.ID || i.oldChosenAnswer == obj3.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score3 += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score3 += Convert.ToInt32(obj3.Score);
                                                        }
                                                    }
                                                }

                                                List<Sections_Questions_Answers> lst4 = lstSectionQuestionAnswerofChldQues.Where(i => guid4.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                                                int Score4 = 0;
                                                bool hasScore4 = false;
                                                //check if lst3 Count greater then 0
                                                if (lst4.Count > 0)
                                                {
                                                    //Loopin threw to get sum of scores
                                                    foreach (Sections_Questions_Answers obj4 in lst4)
                                                    {
                                                        hasScore4 = true;
                                                        if (obj4.AnswerType == "FreeText" && obj4.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj4.ID || i.oldChosenAnswer == obj4.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score4 += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score4 += Convert.ToInt32(obj4.Score);
                                                        }
                                                    }
                                                }

                                                //Get SumOfTotalScore
                                                int SumofTotalScore = 0;
                                                bool HasSumofScore = false;

                                                if (hasScore1)
                                                {
                                                    HasSumofScore = true;
                                                    SumofTotalScore = Score1;
                                                }

                                                //if(hasScore2a)
                                                //{
                                                //    HasSumofScore = true;
                                                //    SumofTotalScore -= Score2a;
                                                //}

                                                if (hasScore2b)
                                                {
                                                    HasSumofScore = true;
                                                    SumofTotalScore += Score2b;
                                                }
                                                if (hasScore3)
                                                {
                                                    HasSumofScore = true;
                                                    SumofTotalScore += Score3;

                                                }
                                                //if(hasScore4)
                                                //{
                                                //    HasSumofScore = true;
                                                //    SumofTotalScore -= Score4;
                                                //}

                                                //if SumofTotalScore >0
                                                if (SumofTotalScore < 0)
                                                    SumofTotalScore = 0;

                                                if (HasSumofScore)
                                                {

                                                    foreach (Sections_Questions objSectionQuestion in lstchildSectionQuestion)
                                                    {
                                                        if (objSectionQuestion.MinScore != null)
                                                        {
                                                            if (!(objSectionQuestion.MinScore <= SumofTotalScore && (objSectionQuestion.MaxScore >= SumofTotalScore || objSectionQuestion.MaxScore == null)))
                                                            {
                                                                List<Guid> lstAnswerIDs = new List<Guid>();
                                                                lstAnswerIDs = db.Sections_Questions_Answers.Where(i => i.IsActive == true && i.Section_QuestionID == objSectionQuestion.ID).Select(k => k.ID).ToList<Guid>();
                                                                foreach (Guid objGuid in lstAnswerIDs)
                                                                {
                                                                    lstGuidAnswerIDs.Add(objGuid);
                                                                }

                                                            }
                                                        }

                                                    }

                                                }




                                            }
                                        }

                                    }

                                }


                            }


                        }
                        #endregion

                        #region lstsecQueAnsIsNull


                        foreach (ResidentAnswerAssessment objoldAnswerIds in lstsecQueAnsIsNull)
                        {
                            if (objoldAnswerIds.oldChosenAnswer.HasValue)
                            {
                                lstGuidAnswerIDs.Add(Guid.Parse(objoldAnswerIds.oldChosenAnswer.ToString()));

                                Guid objAnswerid = Guid.Parse(objoldAnswerIds.oldChosenAnswer.ToString());
                                //Get Question Id From Section Question Answer

                                Sections_Questions_Answers objSectionQuestionAnswer = new Sections_Questions_Answers();
                                objSectionQuestionAnswer = db.Sections_Questions_Answers.Where(obj => obj.ID == objAnswerid).FirstOrDefault();

                                //If objsectionQuestion Answer is Not null
                                if (objSectionQuestionAnswer != null)
                                {
                                    //Get child Question Of this Question  

                                    List<Question_ParentQuestion> objQuestionParentQuestion = new List<Question_ParentQuestion>();
                                    objQuestionParentQuestion = db.Question_ParentQuestion.Where(i => i.ParentQuestionID == objSectionQuestionAnswer.Section_QuestionID && i.IsActive == true).ToList<Question_ParentQuestion>();

                                    //If Count >0 get the childQuestionIDs of this  Parent Question
                                    if (objQuestionParentQuestion.Count > 0)
                                    {
                                        //List guid of child Question
                                        List<Guid> lstChildQuestion = new List<Guid>();
                                        lstChildQuestion = objQuestionParentQuestion.Select(i => i.QuestionID).ToList<Guid>();

                                        Guid objChildQuestionIds = new Guid();
                                        objChildQuestionIds = objQuestionParentQuestion.Where(i => i.IsActive == true).Select(k => k.QuestionID).FirstOrDefault();

                                        //if objChildQuestionIds Count >0
                                        if (objChildQuestionIds != null)
                                        {

                                            //GetChilQuestion 

                                            List<Sections_Questions> lstchildSectionQuestion = new List<Sections_Questions>();
                                            lstchildSectionQuestion = db.Sections_Questions.Where(i => lstChildQuestion.Contains(i.ID)).ToList<Sections_Questions>();


                                            //Get ParentQuestion  of ChildQueastion

                                            List<Guid> lstParentQuestionIds = new List<Guid>();
                                            lstParentQuestionIds = db.Question_ParentQuestion.Where(i => i.QuestionID == objChildQuestionIds && i.IsActive == true).Select(k => k.ParentQuestionID).ToList<Guid>();

                                            //Section_Question Of parent Question
                                            List<Sections_Questions> lstSectionQuestionChild = new List<Sections_Questions>();
                                            lstSectionQuestionChild = db.Sections_Questions.Where(i => lstParentQuestionIds.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions>();



                                            //Get Answers of the Parent Question
                                            List<Sections_Questions_Answers> lstSectionQuestionAnswerofChldQues = new List<Sections_Questions_Answers>();
                                            lstSectionQuestionAnswerofChldQues = db.Sections_Questions_Answers.Where(i => lstParentQuestionIds.Contains(i.Section_QuestionID) && i.IsActive == true).ToList<Sections_Questions_Answers>();

                                            if (lstSectionQuestionAnswerofChldQues.Count > 0)
                                            {
                                                //Get Guid of 1234 Question
                                                List<Guid> guid1 = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj in lstoldChosenIsEqualSecQuaAnsID)
                                                {
                                                    if (obj.oldChosenAnswer.HasValue)
                                                    {
                                                        guid1.Add(Guid.Parse(obj.oldChosenAnswer.ToString()));
                                                    }
                                                }

                                                List<Guid> guid2a = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj2 in lstoldChosenIsNotEqualSecQuaAnsID)
                                                {
                                                    if (obj2.oldChosenAnswer.HasValue)
                                                    {
                                                        guid2a.Add(Guid.Parse(obj2.oldChosenAnswer.ToString()));
                                                    }
                                                }

                                                List<Guid> guid2b = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj2b in lstoldChosenIsNotEqualSecQuaAnsID)
                                                {
                                                    if (obj2b.Section_Question_AnswerId.HasValue)
                                                    {
                                                        guid2b.Add(Guid.Parse(obj2b.Section_Question_AnswerId.ToString()));
                                                    }
                                                }

                                                List<Guid> guid3 = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj3 in lstoldChosenIsNull)
                                                {

                                                    if (obj3.Section_Question_AnswerId.HasValue)
                                                    {
                                                        guid3.Add(Guid.Parse(obj3.Section_Question_AnswerId.ToString()));
                                                    }
                                                }

                                                List<Guid> guid4 = new List<Guid>();
                                                foreach (ResidentAnswerAssessment obj4 in lstsecQueAnsIsNull)
                                                {

                                                    if (obj4.oldChosenAnswer.HasValue)
                                                    {
                                                        guid4.Add(Guid.Parse(obj4.oldChosenAnswer.ToString()));
                                                    }
                                                }

                                                List<Sections_Questions_Answers> lst1 = lstSectionQuestionAnswerofChldQues.Where(i => guid1.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();

                                                int Score1 = 0;
                                                bool hasScore1 = false;
                                                //check if lst1 Count greater then 0
                                                if (lst1.Count > 0)
                                                {
                                                    //Loopin threw to get sum of scores
                                                    foreach (Sections_Questions_Answers obj1 in lst1)
                                                    {

                                                        hasScore1 = true;
                                                        if (obj1.AnswerType == "FreeText" && obj1.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj1.ID || i.oldChosenAnswer == obj1.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score1 += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score1 += Convert.ToInt32(obj1.Score);
                                                        }
                                                    }
                                                }


                                                //score for 2a 
                                                int Score2a = 0;
                                                bool hasScore2a = false;
                                                List<Sections_Questions_Answers> lst2a = lstSectionQuestionAnswerofChldQues.Where(i => guid2a.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                                                if (lst2a.Count > 0)
                                                {
                                                    foreach (Sections_Questions_Answers obj2a in lst2a)
                                                    {
                                                        hasScore2a = true;
                                                        if (obj2a.AnswerType == "FreeText" && obj2a.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj2a.ID || i.oldChosenAnswer == obj2a.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score2a += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score2a += Convert.ToInt32(obj2a.Score);
                                                        }
                                                    }
                                                }


                                                //score for 2b
                                                int Score2b = 0;
                                                bool hasScore2b = false;
                                                List<Sections_Questions_Answers> lst2b = lstSectionQuestionAnswerofChldQues.Where(i => guid2b.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                                                if (lst2b.Count > 0)
                                                {
                                                    foreach (Sections_Questions_Answers obj2b in lst2b)
                                                    {
                                                        hasScore2b = true;
                                                        if (obj2b.AnswerType == "FreeText" && obj2b.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj2b.ID || i.oldChosenAnswer == obj2b.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score2b += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score2b += Convert.ToInt32(obj2b.Score);
                                                        }
                                                    }
                                                }

                                                List<Sections_Questions_Answers> lst3 = lstSectionQuestionAnswerofChldQues.Where(i => guid3.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                                                int Score3 = 0;
                                                bool hasScore3 = false;
                                                //check if lst3 Count greater then 0
                                                if (lst3.Count > 0)
                                                {
                                                    //Loopin threw to get sum of scores
                                                    foreach (Sections_Questions_Answers obj3 in lst3)
                                                    {
                                                        hasScore3 = true;
                                                        if (obj3.AnswerType == "FreeText" && obj3.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj3.ID || i.oldChosenAnswer == obj3.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score3 += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {

                                                            Score3 += Convert.ToInt32(obj3.Score);
                                                        }
                                                    }
                                                }

                                                List<Sections_Questions_Answers> lst4 = lstSectionQuestionAnswerofChldQues.Where(i => guid4.Contains(i.ID) && i.IsActive == true).ToList<Sections_Questions_Answers>();
                                                int Score4 = 0;
                                                bool hasScore4 = false;
                                                //check if lst3 Count greater then 0
                                                if (lst4.Count > 0)
                                                {
                                                    //Loopin threw to get sum of scores
                                                    foreach (Sections_Questions_Answers obj4 in lst4)
                                                    {
                                                        hasScore4 = true;
                                                        if (obj4.AnswerType == "FreeText" && obj4.Score == 0)
                                                        {
                                                            ResidentAnswerAssessment objResAnsAss = lstResidentAnswerAssessment.Where(i => i.Section_Question_AnswerId == obj4.ID || i.oldChosenAnswer == obj4.ID).FirstOrDefault();
                                                            if (objResAnsAss != null && objResAnsAss.AnswerText != "")
                                                            {
                                                                Score4 += Convert.ToInt32(objResAnsAss.AnswerText);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Score4 += Convert.ToInt32(obj4.Score);
                                                        }
                                                    }
                                                }

                                                //Get SumOfTotalScore
                                                int SumofTotalScore = 0;
                                                bool HasSumofScore = false;

                                                if (hasScore1)
                                                {
                                                    HasSumofScore = true;
                                                    SumofTotalScore = Score1;
                                                }

                                                //if (hasScore2a)
                                                //{
                                                //    HasSumofScore = true;
                                                //    SumofTotalScore -= Score2a;
                                                //}

                                                if (hasScore2b)
                                                {
                                                    HasSumofScore = true;
                                                    SumofTotalScore += Score2b;
                                                }
                                                if (hasScore3)
                                                {
                                                    HasSumofScore = true;
                                                    SumofTotalScore += Score3;

                                                }

                                                #region Change on 7/1/2016
                                                if (hasScore4 && SumofTotalScore == 0)
                                                {
                                                    HasSumofScore = true;
                                                    SumofTotalScore -= Score4;
                                                }
                                                #endregion


                                                if (SumofTotalScore < 0)
                                                    SumofTotalScore = 0;
                                                //if SumofTotalScore >0

                                                if (HasSumofScore)
                                                {

                                                    foreach (Sections_Questions objSectionQuestion in lstchildSectionQuestion)
                                                    {
                                                        //REcentChange 6232016 7:31pm
                                                        if (objSectionQuestion.MinScore != null)
                                                        {
                                                            if (!(objSectionQuestion.MinScore <= SumofTotalScore && (objSectionQuestion.MaxScore >= SumofTotalScore || objSectionQuestion.MaxScore == null)))
                                                            {
                                                                List<Guid> lstAnswerIDs = new List<Guid>();
                                                                lstAnswerIDs = db.Sections_Questions_Answers.Where(i => i.IsActive == true && i.Section_QuestionID == objSectionQuestion.ID).Select(k => k.ID).ToList<Guid>();
                                                                foreach (Guid objGuid in lstAnswerIDs)
                                                                {
                                                                    lstGuidAnswerIDs.Add(objGuid);
                                                                }

                                                            }
                                                        }
                                                    }

                                                }




                                            }
                                        }

                                    }

                                }


                            }


                        }


                        #endregion

                        lstGuidAnswerIDs = lstGuidAnswerIDs.Concat(GetSubQuestions_AnswersIds1(lstGuidAnswerIDs)).ToList<Guid>();

                        #region lstoldChosenIsNotEqualSecQuaAnsIDNewSecQues
                        //Creating new records which have oldrecords and selected newone

                        List<Residents_Questions_Answers> lstNewAssesmentData = new List<Residents_Questions_Answers>();

                        foreach (ResidentAnswerAssessment obj in lstoldChosenIsNotEqualSecQuaAnsID)
                        {
                            if (obj.Section_Question_AnswerId.HasValue)
                            {
                                Residents_Questions_Answers objNewResidentQuestionAnswer = new Residents_Questions_Answers();
                                objNewResidentQuestionAnswer.ID = Guid.NewGuid();
                                objNewResidentQuestionAnswer.IsActive = true;
                                objNewResidentQuestionAnswer.Created = DateTime.Now;
                                objNewResidentQuestionAnswer.CreatedBy = userId;
                                objNewResidentQuestionAnswer.ModifiedBy = userId;
                                objNewResidentQuestionAnswer.ResidentID = residentId;
                                objNewResidentQuestionAnswer.AnswerText = obj.AnswerText;
                                objNewResidentQuestionAnswer.Section_Question_AnswerID = Guid.Parse(obj.Section_Question_AnswerId.Value.ToString());
                                lstNewAssesmentData.Add(objNewResidentQuestionAnswer);
                            }
                        }


                        #endregion

                        #region lstoldChosenIsNull
                        foreach (ResidentAnswerAssessment obj in lstoldChosenIsNull)
                        {
                            if (obj.Section_Question_AnswerId.HasValue)
                            {
                                Residents_Questions_Answers objNewResidentQuestionAnswer = new Residents_Questions_Answers();
                                objNewResidentQuestionAnswer.ID = Guid.NewGuid();
                                objNewResidentQuestionAnswer.IsActive = true;
                                objNewResidentQuestionAnswer.Created = DateTime.Now;
                                objNewResidentQuestionAnswer.CreatedBy = userId;
                                objNewResidentQuestionAnswer.ModifiedBy = userId;
                                objNewResidentQuestionAnswer.ResidentID = residentId;
                                objNewResidentQuestionAnswer.AnswerText = obj.AnswerText;
                                objNewResidentQuestionAnswer.Section_Question_AnswerID = Guid.Parse(obj.Section_Question_AnswerId.Value.ToString());
                                lstNewAssesmentData.Add(objNewResidentQuestionAnswer);
                            }
                        }

                        #endregion

                        #region

                        foreach (ResidentAnswerAssessment obj in lstoldChosenIsEqualSecQuaAnsID)
                        {
                            if (obj.oldChosenAnswer == obj.Section_Question_AnswerId && obj.AnswerText != null)
                            {
                                Residents_Questions_Answers objResQueAns = db.Residents_Questions_Answers.Where(i => i.ResidentID == residentId && i.Section_Question_AnswerID == obj.Section_Question_AnswerId && i.IsActive == true).FirstOrDefault();
                                if (objResQueAns != null)
                                {
                                    if (objResQueAns.AnswerText != obj.AnswerText)
                                    {
                                        objResQueAns.AnswerText = obj.AnswerText;
                                        objResQueAns.Modified = DateTime.Now;
                                        objResQueAns.ModifiedBy = userId;
                                        db.Entry(objResQueAns).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }


                        #endregion

                        #region CreatingNewAnswer


                        foreach (Residents_Questions_Answers objResident_Question_Answer in lstNewAssesmentData)
                        {
                            objResident_Question_Answer.ID = Guid.NewGuid();
                            objResident_Question_Answer.IsActive = true;
                            objResident_Question_Answer.CreatedBy = userId;
                            objResident_Question_Answer.ModifiedBy = userId;
                            objResident_Question_Answer.Created = objResident_Question_Answer.Modified = DateTime.Now;

                            db.Residents_Questions_Answers.Add(objResident_Question_Answer);
                            await db.SaveChangesAsync();
                        }



                        #endregion
                        #region Deactivating

                        //Get ResidentQuestionAnswerIDs by AnswerIds

                        List<Guid> ResidentQuestionAnswerIDs = new List<Guid>();
                        ResidentQuestionAnswerIDs = db.Residents_Questions_Answers.Where(i => lstGuidAnswerIDs.Contains(i.Section_Question_AnswerID) && i.IsActive == true && i.ResidentID == residentId).Select(i => i.ID).ToList<Guid>();


                        if (objResident.IsAccepted == true)
                        {
                            await inactivateAssessmentData(ResidentQuestionAnswerIDs);

                        }
                        else
                        {
                            await deleteAssessmentData(ResidentQuestionAnswerIDs);

                            foreach (Guid objGuid in ResidentQuestionAnswerIDs)
                            {
                                strFolderPath = Path.Combine(root, Convert.ToString(objGuid));
                                if (Directory.Exists(strFolderPath))
                                {
                                    DirectoryInfo di = new DirectoryInfo(strFolderPath);
                                    di.Delete(true);
                                }
                            }

                        }

                        #endregion
                    }

                    #endregion         
                }

                //End - Updating assessment data
               
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

                    Guid section_Question_AnswerId = JsonConvert.DeserializeObject<Guid>(fileData.Headers.ContentDisposition.Name);
                    Guid resident_Question_AnswerId = lstAssessmentData.Where(obj => obj.Section_Question_AnswerID.Equals(section_Question_AnswerId)).FirstOrDefault().ID;

                    strFolderPath = Path.Combine(root, Convert.ToString(resident_Question_AnswerId));
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }
                    var residentFile = new { ID = resident_Question_AnswerId, fileName = fileName};
                    lstresidentFiles.Add(residentFile);
                    File.Move(fileData.LocalFileName, Path.Combine(strFolderPath, fileName));                
                }                             
            }
            catch (System.Exception e)
            {
               // return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                return null;
            }

            List<Residents_Questions_Answers> lstreturnResidentQuestionAnswer = new List<Residents_Questions_Answers>();
            lstreturnResidentQuestionAnswer = db.Residents_Questions_Answers.Where(i => i.IsActive == true).ToList<Residents_Questions_Answers>();
            foreach(Residents_Questions_Answers obj in lstreturnResidentQuestionAnswer)
            {
                obj.Resident = null; obj.Sections_Questions_Answers = null;
                obj.User = null;
                
            }          
            var resFile = new { ResidentQuestionAnswers = lstreturnResidentQuestionAnswer, Files = lstresidentFiles };
            return Ok(resFile);
        }

        [HttpPost]
        [Route("UpdateInterventionAssessmentDataWithFiles")]
        public async Task<IHttpActionResult> UpdateInterventionAssessmentDataWithFiles(Guid residentId)
        {
            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            Resident objResident;
            string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers");
            string tempFolder = "temp";
            var provider = new MultipartFormDataStreamProvider(Path.Combine(root, tempFolder));
            List<object> lstresidentFiles = new List<object>();
            try
            {
                List<Resident_Interventions_Questions_Answers> lstAssessmentData = new List<Resident_Interventions_Questions_Answers>();
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
                            lstAssessmentData = JsonConvert.DeserializeObject<List<Resident_Interventions_Questions_Answers>>(val);
                            break;
                        }
                        break;
                    }
                }


                //Start- Updating assessment data

                objResident = await db.Residents.FindAsync(residentId);

                if (objResident == null)
                {

                }
                if (lstAssessmentData.Count > 0)
                {
                    List<Guid> lst = new List<Guid>();
                    lst = lstAssessmentData.Where(i => i.AnswerText == "Deactive").Select(i => i.Intervention_Question_AnswerID).ToList<Guid>();
                    if (lst.Count > 0)
                    {
                        List<Guid> lstDeactiveIds = new List<Guid>();
                        lstDeactiveIds = db.Resident_Interventions_Questions_Answers.Where(obj => lst.Contains(obj.Intervention_Question_AnswerID) && obj.ResidentID == residentId).Select(i => i.ID).ToList<Guid>();
                        if (lstDeactiveIds.Count > 0)
                            await inactivateIntervetnionAssessmentData(lstDeactiveIds);
                    }

                    lstAssessmentData = lstAssessmentData.Where(i => i.AnswerText != "Deactive").ToList<Resident_Interventions_Questions_Answers>();
                    //Sections_Questions_Answers objSection_Question_Answer = await db.Sections_Questions_Answers.FindAsync(lstAssessmentData[0].Section_Question_AnswerID);
                    List<Guid> sections_Questions_AnswersIds = lstAssessmentData.Select(obj => obj.Intervention_Question_AnswerID).ToList<Guid>();

                    sections_Questions_AnswersIds = sections_Questions_AnswersIds.Concat(GetInterventionSubQuestions_AnswersIds(sections_Questions_AnswersIds)).ToList<Guid>();

                    List<Guid> section_QuestionIds = db.Intervention_Question_Answer.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).Select(obj => obj.Intervention_QuestionID).ToList<Guid>();
                    List<Guid> residents_Questions_AnswersIds = db.Resident_Interventions_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Intervention_Question_Answer.Intervention_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();


                    await inactivateIntervetnionAssessmentData(residents_Questions_AnswersIds);


                    Guid userId = new Guid(User.Identity.GetUserId());
                    foreach (Resident_Interventions_Questions_Answers objResident_Question_Answer in lstAssessmentData)
                    {
                        objResident_Question_Answer.ID = Guid.NewGuid();
                        objResident_Question_Answer.IsActive = true;
                        objResident_Question_Answer.CreatedBy = userId;
                        objResident_Question_Answer.ModifiedBy = userId;
                        objResident_Question_Answer.Created = DateTime.Now;
                        objResident_Question_Answer.Modified = DateTime.Now;

                        db.Resident_Interventions_Questions_Answers.Add(objResident_Question_Answer);


                        // await UploadAnswerPhoto(objResident_Question_Answer.ID, objResident_Question_Answer.AnswerText.ToString());
                    }
                    await db.SaveChangesAsync();
                }

                //End - Updating assessment data


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

                    Guid section_Question_AnswerId = JsonConvert.DeserializeObject<Guid>(fileData.Headers.ContentDisposition.Name);
                    Guid resident_Question_AnswerId = lstAssessmentData.Where(obj => obj.Intervention_Question_AnswerID.Equals(section_Question_AnswerId)).FirstOrDefault().ID;

                    strFolderPath = Path.Combine(root, Convert.ToString(resident_Question_AnswerId));
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }


                    var residentFile = new { ID = resident_Question_AnswerId, fileName = fileName };
                    lstresidentFiles.Add(residentFile);

                    File.Move(fileData.LocalFileName, Path.Combine(strFolderPath, fileName));


                }
                //return new HttpResponseMessage()
                //{
                //    Content = new StringContent("Saved Successfully")
                //};
            }
            catch (System.Exception e)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                return null;
            }

           // List<Intervention_Question_Answer> lstInterventionQuestionAnswer = new List<Intervention_Question_Answer>();
           // lstInterventionQuestionAnswer = db.Intervention_Question_Answer.Where(i => i.IsActive == true).ToList<Intervention_Question_Answer>();
           // return Ok(lstInterventionQuestionAnswer);
            List<Guid> GuidResidents = db.Residents.Where(i => i.IsActive == true && i.OrganizationID == objResident.OrganizationID).Select(i => i.ID).ToList<Guid>();
            List<Resident_Interventions_Questions_Answers> lstInterventionQuestionAnswer = new List<Resident_Interventions_Questions_Answers>();
            lstInterventionQuestionAnswer = db.Resident_Interventions_Questions_Answers.Where(i => i.IsActive == true && GuidResidents.Contains(i.ResidentID)).ToList<Resident_Interventions_Questions_Answers>();


            //foreach (Residents_Questions_Answers obj in lstreturnResidentQuestionAnswer)
            //{
            //    obj.Resident = null; obj.Sections_Questions_Answers = null;
            //    obj.User = null;

            //}
           

            var residentInterventionAnswer = new { ResidentInterventionQuestionAnswer = lstInterventionQuestionAnswer, Files = lstresidentFiles };


            return Ok(residentInterventionAnswer);
            

        }



        [HttpPost]
        [Route("SaveInterventionAnswerAssessmentDataWithFiles")]
        public async Task<IHttpActionResult> SaveInterventionAnswerAssessmentDataWithFiles(Guid residentId)
        {
            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers");
            string tempFolder = "temp";
            var provider = new MultipartFormDataStreamProvider(Path.Combine(root, tempFolder));

            try
            {
                List<Interventions_Resident_Answers> lstAssessmentData = new List<Interventions_Resident_Answers>();
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
                            lstAssessmentData = JsonConvert.DeserializeObject<List<Interventions_Resident_Answers>>(val);
                            break;
                        }
                        break;
                    }
                }

                Guid userId = new Guid(User.Identity.GetUserId());
                //Start- Updating assessment data

                Resident objResident = await db.Residents.FindAsync(residentId);

                if (objResident == null)
                {

                }
                if (lstAssessmentData.Count > 0)
                {
                    ////Sections_Questions_Answers objSection_Question_Answer = await db.Sections_Questions_Answers.FindAsync(lstAssessmentData[0].Section_Question_AnswerID);
                    List<Guid> InterventionIds = lstAssessmentData.Select(obj => obj.InterventionID).ToList<Guid>();

                    //sections_Questions_AnswersIds = sections_Questions_AnswersIds.Concat(GetInterventionSubQuestions_AnswersIds(sections_Questions_AnswersIds)).ToList<Guid>();

                    List<Interventions_Resident_Answers> Interventions_Resident_Answers = db.Interventions_Resident_Answers.Where(obj => InterventionIds.Contains(obj.InterventionID)).ToList<Interventions_Resident_Answers>();
                    //List<Guid> residents_Questions_AnswersIds = db.Resident_Interventions_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Intervention_Question_Answer.Intervention_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();
                    foreach (Interventions_Resident_Answers objInterventionResidentAnswer in Interventions_Resident_Answers)
                    {

                        objInterventionResidentAnswer.IsActive = false;
                        objInterventionResidentAnswer.ModifiedBy = userId;
                        objInterventionResidentAnswer.Modified = DateTime.Now;
                        db.Entry(objInterventionResidentAnswer).State = EntityState.Modified;
                    }
                    await db.SaveChangesAsync();
                    //await inactivateIntervetnionAssessmentData(residents_Questions_AnswersIds);



                    foreach (Interventions_Resident_Answers objResident_Question_Answer in lstAssessmentData)
                    {
                        objResident_Question_Answer.ID = Guid.NewGuid();
                        objResident_Question_Answer.IsActive = true;
                        objResident_Question_Answer.CreatedBy = userId;
                        objResident_Question_Answer.ModifiedBy = userId;
                        objResident_Question_Answer.Created = DateTime.Now;
                        objResident_Question_Answer.Modified = DateTime.Now;

                        db.Interventions_Resident_Answers.Add(objResident_Question_Answer);


                        // await UploadAnswerPhoto(objResident_Question_Answer.ID, objResident_Question_Answer.AnswerText.ToString());
                    }
                    await db.SaveChangesAsync();
                }

                //End - Updating assessment data


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

                    Guid section_Question_AnswerId = JsonConvert.DeserializeObject<Guid>(fileData.Headers.ContentDisposition.Name);
                    Guid resident_Question_AnswerId = lstAssessmentData.Where(obj => obj.Intervention_Question_AnswerID.Equals(section_Question_AnswerId)).FirstOrDefault().ID;

                    strFolderPath = Path.Combine(root, Convert.ToString(resident_Question_AnswerId));
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }




                    File.Move(fileData.LocalFileName, Path.Combine(strFolderPath, fileName));


                }
                //return new HttpResponseMessage()
                //{
                //    Content = new StringContent("Saved Successfully")
                //};
            }
            catch (System.Exception e)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                return null;
            }

            List<Interventions_Resident_Answers> lstInterventionQuestionAns = new List<Interventions_Resident_Answers>();
            lstInterventionQuestionAns = db.Interventions_Resident_Answers.Where(i => i.IsActive == true).ToList<Interventions_Resident_Answers>();
            return Ok(lstInterventionQuestionAns);
        }


        private List<Guid> GetSubQuestions_AnswersIds(List<Guid> sections_Questions_AnswersIds)
        {
            List<Guid> sections_QuestionsIds = db.Sections_Questions_Answers.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
            List<Guid> lstAllAnswerIds = db.Sections_Questions_Answers.Where(obj => sections_QuestionsIds.Contains(obj.Section_QuestionID)).Select(obj => obj.ID).ToList<Guid>();

            // List<Guid> lstSubQuestionIds = db.Sections_Questions.Where(obj => obj.ParentAnswerID != null && lstAllAnswerIds.Contains(obj.ParentAnswerID.Value)).Select(obj => obj.ID).ToList<Guid>();
            // List<Guid> lstSubQuestionIds = db.Question_ParentQuestion.Where(obj => obj.ParentAnswerID != null && lstAllAnswerIds.Contains(obj.ParentAnswerID.Value)).Select(obj => obj.ID).ToList<Guid>();

            //the above is wrong due to obj => obj.ID the right is ==> obj=>obj.QuestionID
            List<Guid> lstSubQuestionIds = db.Question_ParentQuestion.Where(obj => obj.ParentAnswerID != null && lstAllAnswerIds.Contains(obj.ParentAnswerID.Value)).Select(obj => obj.QuestionID).ToList<Guid>();

            // List<Guid> lstSubQuestionIDs = db.Question_ParentQuestion.Where(obj => obj.ParentAnswerID == null && sections_QuestionsIds.Contains(obj.ParentQuestionID)).Select(obj => obj.QuestionID).ToList<Guid>();

            // lstSubQuestionIds.Concat(lstSubQuestionIDs);

            if (lstSubQuestionIds.Count > 0)
            {
                List<Guid> lstQuestions_AnswersIds = db.Sections_Questions_Answers.Where(obj => lstSubQuestionIds.Contains(obj.Section_QuestionID)).Select(obj => obj.ID).ToList<Guid>();

                return lstQuestions_AnswersIds.Concat(GetSubQuestions_AnswersIds(lstQuestions_AnswersIds)).ToList<Guid>();
            }
            else
            {
                return new List<Guid>();
            }
        }

        private List<Guid> newGetSubQuestions_AnswersIds(List<Guid> sections_Questions_AnswersIds)
        {
            List<Guid> sections_QuestionsIds = db.Sections_Questions_Answers.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
            List<Guid> lstAllAnswerIds = db.Sections_Questions_Answers.Where(obj => sections_QuestionsIds.Contains(obj.Section_QuestionID)).Select(obj => obj.ID).ToList<Guid>();

            // List<Guid> lstSubQuestionIds = db.Sections_Questions.Where(obj => obj.ParentAnswerID != null && lstAllAnswerIds.Contains(obj.ParentAnswerID.Value)).Select(obj => obj.ID).ToList<Guid>();
            // List<Guid> lstSubQuestionIds = db.Question_ParentQuestion.Where(obj => obj.ParentAnswerID != null && lstAllAnswerIds.Contains(obj.ParentAnswerID.Value)).Select(obj => obj.ID).ToList<Guid>();

            //the above is wrong due to obj => obj.ID the right is ==> obj=>obj.QuestionID
            List<Guid> lstSubQuestionIds = db.Question_ParentQuestion.Where(obj => obj.ParentAnswerID != null && lstAllAnswerIds.Contains(obj.ParentAnswerID.Value)).Select(obj => obj.QuestionID).ToList<Guid>();

            List<Guid> lstSubQuestionIDs = db.Question_ParentQuestion.Where(obj => obj.ParentAnswerID == null && sections_QuestionsIds.Contains(obj.ParentQuestionID)).Select(obj => obj.QuestionID).ToList<Guid>();

            lstSubQuestionIds.Concat(lstSubQuestionIDs);

            if (lstSubQuestionIds.Count > 0)
            {
                List<Guid> lstQuestions_AnswersIds = db.Sections_Questions_Answers.Where(obj => lstSubQuestionIds.Contains(obj.Section_QuestionID)).Select(obj => obj.ID).ToList<Guid>();

                return lstQuestions_AnswersIds.Concat(newGetSubQuestions_AnswersIds(lstQuestions_AnswersIds)).ToList<Guid>();
            }
            else
            {
                return new List<Guid>();
            }
        }

        private List<Guid> GetSubQuestions_AnswersIds1(List<Guid> sections_Questions_AnswersIds)
        {
            //List<Guid> sections_QuestionsIds = db.Sections_Questions_Answers.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).Select(obj => obj.Section_QuestionID).ToList<Guid>();
            //List<Guid> lstAllAnswerIds = db.Sections_Questions_Answers.Where(obj => sections_QuestionsIds.Contains(obj.Section_QuestionID)).Select(obj => obj.ID).ToList<Guid>();


            List<Guid> lstSubQuestionIds = db.Question_ParentQuestion.Where(obj => obj.ParentAnswerID != null && sections_Questions_AnswersIds.Contains(obj.ParentAnswerID.Value)).Select(obj => obj.QuestionID).ToList<Guid>();

            // List<Guid> lstSubQuestionIDs = db.Question_ParentQuestion.Where(obj => obj.ParentAnswerID == null && sections_QuestionsIds.Contains(obj.ParentQuestionID)).Select(obj => obj.QuestionID).ToList<Guid>();

            // lstSubQuestionIds.Concat(lstSubQuestionIDs);

            if (lstSubQuestionIds.Count > 0)
            {
                List<Guid> lstQuestions_AnswersIds = db.Sections_Questions_Answers.Where(obj => lstSubQuestionIds.Contains(obj.Section_QuestionID)).Select(obj => obj.ID).ToList<Guid>();

                return lstQuestions_AnswersIds.Concat(GetSubQuestions_AnswersIds1(lstQuestions_AnswersIds)).ToList<Guid>();
            }
            else
            {
                return new List<Guid>();
            }
        }


        private List<Guid> GetInterventionSubQuestions_AnswersIds(List<Guid> sections_Questions_AnswersIds)
        {
            List<Guid> sections_QuestionsIds = db.Intervention_Question_Answer.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).Select(obj => obj.Intervention_QuestionID).ToList<Guid>();
            List<Guid> lstAllAnswerIds = db.Intervention_Question_Answer.Where(obj => sections_QuestionsIds.Contains(obj.Intervention_QuestionID)).Select(obj => obj.ID).ToList<Guid>();


            List<Guid> lstSubQuestionIds = db.Intervention_Question_ParentQuestion.Where(obj => obj.InterventionParentAnswerID != null && lstAllAnswerIds.Contains(obj.InterventionParentAnswerID)).Select(obj => obj.InterventionQuestionID).ToList<Guid>();
            if (lstSubQuestionIds.Count > 0)
            {
                List<Guid> lstQuestions_AnswersIds = db.Intervention_Question_Answer.Where(obj => lstSubQuestionIds.Contains(obj.Intervention_QuestionID)).Select(obj => obj.ID).ToList<Guid>();

                return lstQuestions_AnswersIds.Concat(GetInterventionSubQuestions_AnswersIds(lstQuestions_AnswersIds)).ToList<Guid>();
            }
            else
            {
                return new List<Guid>();
            }
        }

        //[HttpPost]
        //[Route("DeleteAssessmentData")]
        //public async Task<IHttpActionResult> DeleteAssessmentData(Guid residentId, [FromBody] Guid sectionId)
        //{
        //    await deleteAssessmentData(residentId, sectionId);
        //    return Ok();
        //}

        //[HttpPost]
        //[Route("InactivateAssessmentData")]
        //public async Task<IHttpActionResult> InactivateAssessmentData(Guid residentId, [FromBody] Guid sectionId)
        //{
        //    await inactivateAssessmentData(residentId, sectionId);
        //    return Ok();
        //}

        [HttpPost]
        [Route("GetTaskTitlesForResident")]
        public async Task<IHttpActionResult> GetTaskTitlesForResident(Guid residentId)
        {
            List<Guid> section_question_answerIds = await db.Residents_Questions_Answers.Include(obj => obj.Sections_Questions_Answers).Where(obj => obj.IsActive && obj.Sections_Questions_Answers.IsActive && obj.Sections_Questions_Answers.Section.IsActive && obj.ResidentID.Equals(residentId)).Select(obj => obj.Sections_Questions_Answers.ID).ToListAsync<Guid>();

            List<String> lstTaskTitles = await db.Sections_Questions_Answers_Tasks.Where(obj => section_question_answerIds.Contains(obj.ID)).Select(s => s.Section_Intervention.InterventionTitle).ToListAsync<String>();

            return Ok(lstTaskTitles);
        }




        private async Task deleteAssessmentData(List<Guid> residents_Questions_AnswerIds)
        {

            List<Residents_Questions_Answers> lstResidents_Questions_Answers = db.Residents_Questions_Answers.Include(i => i.Sections_Questions_Answers).Where(obj => residents_Questions_AnswerIds.Contains(obj.ID)).ToList<Residents_Questions_Answers>();
            Guid userId = new Guid(User.Identity.GetUserId());

            if (lstResidents_Questions_Answers.Count > 0)
            {
                db.Residents_Questions_Answers.RemoveRange(db.Residents_Questions_Answers.Where(obj => residents_Questions_AnswerIds.Contains(obj.ID)));

                //Guid ResidentID = lstResidents_Questions_Answers[0].ResidentID;
                //List<Section_Intervention> GetlstResidentSectionQuestionAnsTask = new List<Section_Intervention>();
                //List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
                //lstResidentAnsweredQuestions = lstResidents_Questions_Answers;

                //List<Sections_Questions_Answers_Tasks> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Tasks>();
                //lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Tasks.Include(obj => obj.Section_Intervention).Include(obj => obj.Section_Intervention.Actions.Select(i => i.Actions_Days)).Include(i => i.Section_Intervention.Intervention_Question).Include(k => k.Section_Intervention.Intervention_Question.Select(o => o.Intervention_Question_Answer)).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Sections_Questions_Answers_Tasks>();

                //foreach (Sections_Questions_Answers_Tasks objSecQueAnsTask in lstSectionQuestionsAnswersTask)
                //{

                //    objSecQueAnsTask.Section_Intervention.Actions = objSecQueAnsTask.Section_Intervention.Actions.Where(obj => obj.IsActive && obj.ResidentID == ResidentID).ToList<CHM.Services.Models.Action>();
                //    foreach (CHM.Services.Models.Action objAction in objSecQueAnsTask.Section_Intervention.Actions)
                //    {
                //        objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

                //        foreach (Actions_Days objActions_Days in objAction.Actions_Days)
                //        {
                //            objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
                //        }
                //    }

                //}






                //List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
                //List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
                //foreach (Sections_Questions_Answers_Tasks objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
                //{
                //    if (objSecQuesAnsTask.Section_Question_AnswerID != null)
                //        lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                //    else
                //        lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
                //}


                //foreach (Residents_Questions_Answers objResidentQuestionAnswer in lstResidentAnsweredQuestions)
                //{
                //    foreach (Sections_Questions_Answers_Tasks objSectionQuestionAnsTask in lstSectionQuestionAnswerHasParentAnswerID)
                //    {
                //        if (objResidentQuestionAnswer.Section_Question_AnswerID == objSectionQuestionAnsTask.Section_Question_AnswerID)
                //        {
                //            GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask.Section_Intervention);
                //        }
                //    }
                //}




                //var SectionQuestionAnserTaskGroupByIntervention = from SectionQuestionAnserTask in lstSectionQuestionAnswerHasNoParentAnswerID
                //                                                  group SectionQuestionAnserTask by SectionQuestionAnserTask.Section_InterventionID;

                //foreach (var group in SectionQuestionAnserTaskGroupByIntervention)
                //{
                //    int score = 0;
                //    Section_Intervention objIntervention = new Section_Intervention();
                //    foreach (var SectionQuestionAnserTask in group)
                //    {


                //        foreach (Residents_Questions_Answers objResidentQueAns in lstResidentAnsweredQuestions)
                //        {
                //            if (objResidentQueAns.Sections_Questions_Answers != null)
                //            {
                //                if (objResidentQueAns.Sections_Questions_Answers.Section_QuestionID == SectionQuestionAnserTask.Section_QuestionID)
                //                {
                //                    score += Convert.ToInt32(objResidentQueAns.Sections_Questions_Answers.Score);
                //                }
                //            }
                //        }
                //        objIntervention = SectionQuestionAnserTask.Section_Intervention;
                //    }

                //    if (score > 0)
                //    {
                //        if (objIntervention.MinScore <= score && (objIntervention.MaxScore >= score || objIntervention.MaxScore == null))
                //            GetlstResidentSectionQuestionAnsTask.Add(objIntervention);
                //    }


                //}

                //List<Guid> lstInterventionIDs = new List<Guid>();
                //lstInterventionIDs = GetlstResidentSectionQuestionAnsTask.Select(obj => obj.ID).ToList<Guid>();
                //comented on 5/23/2016 due to action table residentAnswerDi removel
                // List<Guid> actionIds = db.Actions.Where(obj => lstInterventionIDs.Contains(obj.Section_InterventionID) && obj.ResidentID==ResidentID).Select(obj => obj.ID).ToList<Guid>();
                //  List<Guid> actions_DaysIds = db.Actions_Days.Where(obj => actionIds.Contains(obj.ActionID)).Select(obj => obj.ID).ToList<Guid>();
                // List<Guid> interventionsIds = db.Interventions.Where(obj => actions_DaysIds.Contains(obj.Action_DayID)).Select(obj => obj.ID).ToList<Guid>();

                // db.Interventions.RemoveRange(db.Interventions.Where(obj => interventionsIds.Contains(obj.ID)));

                //db.Actions_Days.RemoveRange(db.Actions_Days.Where(obj => actions_DaysIds.Contains(obj.ID)));

                //db.Actions.RemoveRange(db.Actions.Where(obj => actionIds.Contains(obj.ID)));








                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return;

        }

        private async Task inactivateAssessmentData(List<Guid> residents_Questions_AnswerIds)
        {
            //List<Residents_Questions_Answers> lstResidents_Questions_Answers = db.Residents_Questions_Answers.Where(obj => obj.ResidentID == residentId && obj.Sections_Questions_Answers.Sections_Questions.SectionID == sectionId).ToList<Residents_Questions_Answers>();
            List<Residents_Questions_Answers> lstResidents_Questions_Answers = db.Residents_Questions_Answers.Include(i => i.Sections_Questions_Answers).Where(obj => residents_Questions_AnswerIds.Contains(obj.ID)).ToList<Residents_Questions_Answers>();
            Guid userId = new Guid(User.Identity.GetUserId());

            foreach (Residents_Questions_Answers objResident_Quesion_Answer in lstResidents_Questions_Answers)
            {
                objResident_Quesion_Answer.IsActive = false;
                objResident_Quesion_Answer.ModifiedBy = userId;
                objResident_Quesion_Answer.Modified = DateTime.Now;


                ///Comented on 5/23/2016 due to change in action tabel
                //foreach (CHM.Services.Models.Action objAction in objResident_Quesion_Answer.Actions)
                //{
                //    objAction.IsActive = false;
                //    objAction.ModifiedBy = userId;
                //    objAction.Modified = DateTime.Now;

                //    foreach (Actions_Days objAction_Day in objAction.Actions_Days)
                //    {
                //        objAction_Day.IsActive = false;
                //        objAction_Day.ModifiedBy = userId;
                //        objAction_Day.Modified = DateTime.Now;

                //        foreach (Intervention objIntervention in objAction_Day.Interventions)
                //        {
                //            objIntervention.IsActive = false;
                //            objIntervention.ModifiedBy = userId;
                //            objIntervention.Modified = DateTime.Now;

                //            // db.Entry(objIntervention).State = EntityState.Modified;
                //        }

                //        //db.Entry(objAction_Day).State = EntityState.Modified;
                //    }

                //    //db.Entry(objAction).State = EntityState.Modified;
                //}

                db.Entry(objResident_Quesion_Answer).State = EntityState.Modified;
            }
            await inactivateActionAssessmentData(lstResidents_Questions_Answers);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw;
            }

            return;
        }


        private async Task inactivateActionAssessmentData(List<Residents_Questions_Answers> lsResidentQuestionAnswer)
        {
            if (lsResidentQuestionAnswer.Count > 0)
            {

                Guid userId = new Guid(User.Identity.GetUserId());
                Guid ResidentID = lsResidentQuestionAnswer[0].ResidentID;
                List<Section_Intervention> GetlstResidentSectionQuestionAnsTask = new List<Section_Intervention>();
                List<Residents_Questions_Answers> lstResidentAnsweredQuestions = new List<Residents_Questions_Answers>();
                lstResidentAnsweredQuestions = lsResidentQuestionAnswer;

                List<Sections_Questions_Answers_Tasks> lstSectionQuestionsAnswersTask = new List<Sections_Questions_Answers_Tasks>();
                lstSectionQuestionsAnswersTask = await db.Sections_Questions_Answers_Tasks.Include(obj => obj.Section_Intervention).Include(obj => obj.Section_Intervention.Actions.Select(i => i.Actions_Days)).Include(i => i.Section_Intervention.Intervention_Question).Include(k => k.Section_Intervention.Intervention_Question.Select(o => o.Intervention_Question_Answer)).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Sections_Questions_Answers_Tasks>();

                //foreach (Sections_Questions_Answers_Tasks objSecQueAnsTask in lstSectionQuestionsAnswersTask)
                //{

                //    objSecQueAnsTask.Section_Intervention.Actions = objSecQueAnsTask.Section_Intervention.Actions.Where(obj => obj.IsActive && obj.ResidentID == ResidentID).ToList<CHM.Services.Models.Action>();
                //    foreach (CHM.Services.Models.Action objAction in objSecQueAnsTask.Section_Intervention.Actions)
                //    {
                //        objAction.Actions_Days = objAction.Actions_Days.Where(obj => obj.IsActive).ToList<Actions_Days>();

                //        foreach (Actions_Days objActions_Days in objAction.Actions_Days)
                //        {
                //            objActions_Days.Interventions = objActions_Days.Interventions.Where(obj => obj.IsActive).ToList<Intervention>();
                //        }
                //    }

                //}






                List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
                List<Sections_Questions_Answers_Tasks> lstSectionQuestionAnswerHasNoParentAnswerID = new List<Sections_Questions_Answers_Tasks>();
                foreach (Sections_Questions_Answers_Tasks objSecQuesAnsTask in lstSectionQuestionsAnswersTask)
                {
                    if (objSecQuesAnsTask.Section_Question_AnswerID != null)
                        lstSectionQuestionAnswerHasParentAnswerID.Add(objSecQuesAnsTask);
                    else
                        lstSectionQuestionAnswerHasNoParentAnswerID.Add(objSecQuesAnsTask);
                }


                foreach (Residents_Questions_Answers objResidentQuestionAnswer in lstResidentAnsweredQuestions)
                {
                    foreach (Sections_Questions_Answers_Tasks objSectionQuestionAnsTask in lstSectionQuestionAnswerHasParentAnswerID)
                    {
                        if (objResidentQuestionAnswer.Section_Question_AnswerID == objSectionQuestionAnsTask.Section_Question_AnswerID)
                        {
                            GetlstResidentSectionQuestionAnsTask.Add(objSectionQuestionAnsTask.Section_Intervention);
                        }
                    }
                }




                var SectionQuestionAnserTaskGroupByIntervention = from SectionQuestionAnserTask in lstSectionQuestionAnswerHasNoParentAnswerID
                                                                  group SectionQuestionAnserTask by SectionQuestionAnserTask.Section_InterventionID;

                foreach (var group in SectionQuestionAnserTaskGroupByIntervention)
                {
                    int score = 0;
                    Section_Intervention objIntervention = new Section_Intervention();
                    foreach (var SectionQuestionAnserTask in group)
                    {


                        foreach (Residents_Questions_Answers objResidentQueAns in lstResidentAnsweredQuestions)
                        {
                            if (objResidentQueAns.Sections_Questions_Answers.Section_QuestionID == SectionQuestionAnserTask.Section_QuestionID)
                            {
                                score += Convert.ToInt32(objResidentQueAns.Sections_Questions_Answers.Score);
                            }
                        }
                        objIntervention = SectionQuestionAnserTask.Section_Intervention;
                    }

                    if (score > 0)
                    {
                        if (objIntervention.MinScore <= score && (objIntervention.MaxScore >= score || objIntervention.MaxScore == null))
                            GetlstResidentSectionQuestionAnsTask.Add(objIntervention);
                    }


                }

                List<Guid> SectionIntervetnionID = GetlstResidentSectionQuestionAnsTask.Where(obj => obj.IsActive == true).Select(obj => obj.ID).ToList<Guid>();

                List<Section_Intervention> lstSectionAndInterventions = db.Section_Intervention.Include(i => i.Actions).Where(obj => SectionIntervetnionID.Contains(obj.ID)).ToList<Section_Intervention>();
                foreach (Section_Intervention objSection_Intervention in lstSectionAndInterventions)
                {
                    foreach (CHM.Services.Models.Action objAction in objSection_Intervention.Actions)
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

                                // db.Entry(objIntervention).State = EntityState.Modified;
                            }

                            //db.Entry(objAction_Day).State = EntityState.Modified;
                        }

                        //db.Entry(objAction).State = EntityState.Modified;
                    }
                    db.Entry(objSection_Intervention).State = EntityState.Modified;
                }
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return;
            }
        }



        private async Task inactivateIntervetnionAssessmentData(List<Guid> residents_Questions_AnswerIds)
        {
            //List<Residents_Questions_Answers> lstResidents_Questions_Answers = db.Residents_Questions_Answers.Where(obj => obj.ResidentID == residentId && obj.Sections_Questions_Answers.Sections_Questions.SectionID == sectionId).ToList<Residents_Questions_Answers>();
            List<Resident_Interventions_Questions_Answers> lstResidents_Questions_Answers = db.Resident_Interventions_Questions_Answers.Include(obj => obj.Intervention_Question_Answer).Where(obj => residents_Questions_AnswerIds.Contains(obj.ID)).ToList<Resident_Interventions_Questions_Answers>();
            Guid userId = new Guid(User.Identity.GetUserId());

            foreach (Resident_Interventions_Questions_Answers objResident_Quesion_Answer in lstResidents_Questions_Answers)
            {
                objResident_Quesion_Answer.IsActive = false;
                objResident_Quesion_Answer.ModifiedBy = userId;
                objResident_Quesion_Answer.Modified = DateTime.Now;
                List<Intervention_Question_Answer_Task> lstIntervention = new List<Intervention_Question_Answer_Task>();
                lstIntervention = await db.Intervention_Question_Answer_Task.Include(obj => obj.Section_Intervention).Include(obj => obj.Section_Intervention.Actions.Select(i => i.Actions_Days)).Include(i => i.Section_Intervention.Intervention_Question).Include(k => k.Section_Intervention.Intervention_Question.Select(o => o.Intervention_Question_Answer)).Where(obj => obj.IsActive && obj.Section_Intervention.IsActive).ToListAsync<Intervention_Question_Answer_Task>();
                //  lstIntervention =await db.Intervention_Question_Answer_Task.Include(i=>i.Section_Intervention.Actions).Include(j=>j.Section_Intervention.Actions.Select(i=>i.Actions_Days)).Where(obj => obj.InterventionAnswerID == objResident_Quesion_Answer.Intervention_Question_AnswerID).ToList<Intervention_Question_Answer_Task>();
                foreach (Intervention_Question_Answer_Task item in lstIntervention)
                {
                    foreach (CHM.Services.Models.Action objAction in item.Section_Intervention.Actions)
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

                                // db.Entry(objIntervention).State = EntityState.Modified;
                            }

                            //db.Entry(objAction_Day).State = EntityState.Modified;
                        }


                    }
                    db.Entry(item).State = EntityState.Modified;
                }



                db.Entry(objResident_Quesion_Answer).State = EntityState.Modified;
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return;
        }

        

        private async Task saveAssessmentData(List<Residents_Questions_Answers> lstAssessmentData)
        {
            Guid userId = new Guid(User.Identity.GetUserId());
            foreach (Residents_Questions_Answers objResident_Question_Answer in lstAssessmentData)
            {
                objResident_Question_Answer.ID = Guid.NewGuid();
                objResident_Question_Answer.IsActive = true;
                objResident_Question_Answer.CreatedBy = userId;
                objResident_Question_Answer.ModifiedBy = userId;
                objResident_Question_Answer.Created = objResident_Question_Answer.Modified = DateTime.Now;
                objResident_Question_Answer.AnswerText = "NULL";
                db.Residents_Questions_Answers.Add(objResident_Question_Answer);


                // await UploadAnswerPhoto(objResident_Question_Answer.ID, objResident_Question_Answer.AnswerText.ToString());
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return;
        }


        [HttpGet]
        [Route("GetSectionInterventionStatements")]
        public async Task<IHttpActionResult> GetSectionInterventionStatements(Guid sectionInterventionID)
        {

            List<Section_Intervention_Statements> lstSectionInterventionStatements = db.Section_Intervention_Statements.Where(obj => obj.IsActive && obj.Section_InterventionID == sectionInterventionID).ToList<Section_Intervention_Statements>();
            return Ok(lstSectionInterventionStatements);
        }




        [HttpGet]
        [Route("GetActiveSectionIntervention")]
        public async Task<IHttpActionResult> GetActiveSectionIntervention()
        {

            List<Section_Intervention> lstSectionIntervention = db.Section_Intervention.Where(obj => obj.IsActive).ToList<Section_Intervention>();
            return Ok(lstSectionIntervention);
        }
        [HttpGet]
        [Route("GetActiveAdhocSectionIntervention")]
        public async Task<IHttpActionResult> GetActiveAdhocSectionIntervention(Guid OrganizationId)
        {

            List<CHM.Services.Models.Action> lstAdhocAction = db.Actions.Include(obj => obj.Resident).Include(obj => obj.Section_Intervention).Where(obj => obj.IsActive && obj.IsAdhocIntervention == true && obj.Resident.OrganizationID == OrganizationId && obj.Resident.IsAccepted==true && obj.Resident.IsActive==true).ToList<CHM.Services.Models.Action>();
            return Ok(lstAdhocAction);
        }

        //[HttpPost]
        //[Route("UpdateInterventionAnswerAssessmentDataWithFiles")]
        //public async Task<HttpResponseMessage> UpdateInterventionAnswerAssessmentDataWithFiles(Guid residentId)
        //{
        //    // Check if the request contains multipart/form-data. 
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers");
        //    string tempFolder = "temp";
        //    var provider = new MultipartFormDataStreamProvider(Path.Combine(root, tempFolder));

        //    try
        //    {
        //        List<Interventions_Resident_Answers> lstAssessmentData = new List<Interventions_Resident_Answers>();
        //        string strFolderPath = string.Empty;

        //        // Read the form data and return an async task. 
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        // This illustrates how to get the form data. 
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            if (key == "Answers")
        //            {
        //                foreach (var val in provider.FormData.GetValues(key))
        //                {
        //                    lstAssessmentData = JsonConvert.DeserializeObject<List<Interventions_Resident_Answers>>(val);
        //                    break;
        //                }
        //                break;
        //            }
        //        }


        //        //Start- Updating assessment data

        //        Resident objResident = await db.Residents.FindAsync(residentId);

        //        if (objResident == null)
        //        {

        //        }
        //        if (lstAssessmentData.Count > 0)
        //        {
        //            //Sections_Questions_Answers objSection_Question_Answer = await db.Sections_Questions_Answers.FindAsync(lstAssessmentData[0].Section_Question_AnswerID);
        //            //List<Guid> sections_Questions_AnswersIds = lstAssessmentData.Select(obj => obj.Intervention_Question_AnswerID).ToList<Guid>();

        //            //sections_Questions_AnswersIds = sections_Questions_AnswersIds.Concat(GetInterventionSubQuestions_AnswersIds(sections_Questions_AnswersIds)).ToList<Guid>();

        //            //List<Guid> section_QuestionIds = db.Intervention_Question_Answer.Where(obj => sections_Questions_AnswersIds.Contains(obj.ID)).Select(obj => obj.Intervention_QuestionID).ToList<Guid>();
        //            //List<Guid> residents_Questions_AnswersIds = db.Resident_Interventions_Questions_Answers.Where(obj => section_QuestionIds.Contains(obj.Intervention_Question_Answer.Intervention_QuestionID) && obj.ResidentID.Equals(residentId)).Select(obj => obj.ID).ToList<Guid>();


        //            //await inactivateIntervetnionAssessmentData(residents_Questions_AnswersIds);


        //            Guid userId = new Guid(User.Identity.GetUserId());
        //            foreach (Interventions_Resident_Answers objResident_Question_Answer in lstAssessmentData)
        //            {
        //                objResident_Question_Answer.ID = Guid.NewGuid();
        //                objResident_Question_Answer.IsActive = true;
        //                objResident_Question_Answer.CreatedBy = userId;
        //                objResident_Question_Answer.ModifiedBy = userId;
        //                objResident_Question_Answer.Created = DateTime.Now;
        //                objResident_Question_Answer.Modified = DateTime.Now;

        //                db.Interventions_Resident_Answers.Add(objResident_Question_Answer);


        //                // await UploadAnswerPhoto(objResident_Question_Answer.ID, objResident_Question_Answer.AnswerText.ToString());
        //            }
        //            await db.SaveChangesAsync();
        //        }

        //        //End - Updating assessment data


        //        // This illustrates how to get the file names for uploaded files. 
        //        foreach (var fileData in provider.FileData)
        //        {
        //            FileInfo fileInfo = new FileInfo(fileData.LocalFileName);
        //            if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
        //            }
        //            string fileName = fileData.Headers.ContentDisposition.FileName;
        //            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
        //            {
        //                fileName = fileName.Trim('"');
        //            }
        //            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
        //            {
        //                fileName = Path.GetFileName(fileName);
        //            }

        //            Guid section_Question_AnswerId = JsonConvert.DeserializeObject<Guid>(fileData.Headers.ContentDisposition.Name);
        //            Guid resident_Question_AnswerId = lstAssessmentData.Where(obj => obj.Intervention_Question_AnswerID.Equals(section_Question_AnswerId)).FirstOrDefault().ID;

        //            strFolderPath = Path.Combine(root, Convert.ToString(resident_Question_AnswerId));
        //            if (!Directory.Exists(strFolderPath))
        //            {
        //                Directory.CreateDirectory(strFolderPath);
        //            }




        //            File.Move(fileData.LocalFileName, Path.Combine(strFolderPath, fileName));


        //        }
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent("Saved Successfully")
        //        };
        //    }
        //    catch (System.Exception e)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //    }

        //}
        //private async Task inactivateActionsAndInterventions(List<Guid> residents_Questions_AnswerIds)
        //{
        //    List<Residents_Questions_Answers> lstResidents_Questions_Answers = db.Residents_Questions_Answers.Where(obj => residents_Questions_AnswerIds.Contains(obj.ID)).ToList<Residents_Questions_Answers>();
        //    Guid userId = new Guid(User.Identity.GetUserId());

        //    foreach (Residents_Questions_Answers objResident_Quesion_Answer in lstResidents_Questions_Answers)
        //    {
        //        objResident_Quesion_Answer.IsActive = false;
        //        objResident_Quesion_Answer.ModifiedBy = userId;
        //        objResident_Quesion_Answer.Modified = DateTime.Now;

        //        db.Entry(objResident_Quesion_Answer).State = EntityState.Modified;
        //    }

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        throw;
        //    }

        //    return;
        //}

        private bool ResidentExists(Guid id)
        {
            return db.Residents.Count(e => e.ID == id) > 0;
        }

        //Photo Actions

        [HttpPost]
        [Route("UploadPhoto")]
        public async Task<HttpResponseMessage> UploadPhoto()
        {
            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/OriginalPhotos");
            string ThumbnailRoot = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos");
            string tempFolder = "temp";
            var provider = new MultipartFormDataStreamProvider(Path.Combine(root, tempFolder));

            try
            {
                string strFolderPath = string.Empty;
                string strFolderThumbnailPath = string.Empty;

                // Read the form data and return an async task. 
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the form data. 
                foreach (var key in provider.FormData.AllKeys)
                {
                    if (key == "ResidentId")
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            strFolderPath = Path.Combine(root, val);
                            strFolderThumbnailPath = Path.Combine(ThumbnailRoot, val);
                            if (!Directory.Exists(strFolderPath))
                            {
                                Directory.CreateDirectory(strFolderPath);
                            }
                            if (!Directory.Exists(strFolderThumbnailPath))
                            {
                                Directory.CreateDirectory(strFolderThumbnailPath);
                            }
                            DeleteAllContentInDirectory(strFolderPath);
                            DeleteAllContentInDirectory(strFolderThumbnailPath);
                            break;
                        }
                        break;
                    }
                }

                // This illustrates how to get the file names for uploaded files. 
                foreach (var fileData in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(fileData.LocalFileName);
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
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
                    File.Move(fileData.LocalFileName, Path.Combine(strFolderPath, fileName));
                    string path = strFolderPath + "\\"+fileName;
                    System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                    using (System.Drawing.Image thumbnail = image.GetThumbnailImage(75, 75, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            thumbnail.Save(memoryStream, ImageFormat.Jpeg);
                            Byte[] bytes = new Byte[memoryStream.Length];
                            memoryStream.Position = 0;
                            memoryStream.Read(bytes, 0, (int)bytes.Length);
                            File.WriteAllBytes(strFolderThumbnailPath+"\\"+fileName, bytes);
                        }
                    }


                }
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Saved Successfully")
                };
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        public bool ThumbnailCallback()
        {
            return false;
        }

        [HttpPost]
        [Route("UploadofflinePhoto")]
        public async Task<HttpResponseMessage> UploadofflinePhoto()
        {
            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos");
            string tempFolder = "temp";
            var provider = new MultipartFormDataStreamProvider(Path.Combine(root, tempFolder));

            try
            {
                string strFolderPath = string.Empty;

                // Read the form data and return an async task. 
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the form data. 
                var base64data = ""; string ResidentId = "";
                foreach (var key in provider.FormData.AllKeys)
                {
                    if (key == "ResidentId")
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            ResidentId = val;
                            strFolderPath = Path.Combine(root, val);
                            if (!Directory.Exists(strFolderPath))
                            {
                                Directory.CreateDirectory(strFolderPath);
                            }
                            DeleteAllContentInDirectory(strFolderPath);

                            break;
                        }
                    }
                    else
                    {
                        base64data = provider.FormData.GetValues(key)[0].Split(';')[1].Substring(7);
                    }
                }


                var bytes = Convert.FromBase64String(base64data);
                using (var imageFile = new FileStream(strFolderPath + "\\fileName" + ResidentId + ".jpg", FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent("Saved Successfully")
                };
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpGet]
        [ActionName("GetPhotoRelativeUrl")]
        public string GetPhotoRelativeUrl(Guid guidResidentId)
        {
            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos")))
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos/" + guidResidentId + "")))
                {
                    string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos/" + guidResidentId + ""));

                    if (fileEntries.Length > 0)
                    {
                        FileInfo myFileInfo = new FileInfo(fileEntries[0]);
                        return "/Uploads/Residents/Photos/" + guidResidentId + "/" + myFileInfo.Name;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }


        [HttpPost]
        [Route("DeletePhotoByName")]
        public HttpResponseMessage DeletePhotoByName([FromBody]string strFileName)
        {
            try
            {
                string strFilePath = HttpContext.Current.Server.MapPath("~/Uploads/Residents/Photos/" + strFileName);
                if (File.Exists(strFilePath))
                {
                    File.Delete(strFilePath);
                }
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Deleted Successfully")
                };
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }


        [HttpPost]
        [Route("DeletePainMonitoringPart")]
        public async Task<IHttpActionResult> DeletePainMonitoringPart(List<object> objPainMonitoringids)
        {

            try
            {
                Guid userId = new Guid(User.Identity.GetUserId());
                //var userProfiles = db.PainMonitorings
                //               .Where(t => objPainMonitoringids.Contains(t.ID));

                foreach (var lstPainMonitoringids in objPainMonitoringids)
                {

                    PainMonitoring objPainMonitorings = db.PainMonitorings.Where(i=>i.ID.ToString()==lstPainMonitoringids.ToString()).FirstOrDefault();
                    objPainMonitorings.IsActive = false;
                    objPainMonitorings.Modified = DateTime.Now;
                    objPainMonitorings.ModifiedBy = userId;
                    db.Entry(objPainMonitorings).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return Ok();

            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }


        [HttpPost]
        [Route("DeleteSelectedPartPainMonitoring")]
        public async Task<IHttpActionResult> DeleteSelectedPartPainMonitoring(List<object> objPainMonitoringids)
        {

            try
            {
                Guid userId = new Guid(User.Identity.GetUserId());
                //var userProfiles = db.PainMonitorings
                //               .Where(t => objPainMonitoringids.Contains(t.ID));

                foreach (var lstPainMonitoringids in objPainMonitoringids)
                {

                    PainMonitoring objPainMonitorings = db.PainMonitorings.Where(i => i.ID.ToString() == lstPainMonitoringids.ToString()).FirstOrDefault();
                    objPainMonitorings.IsActive = false;
                    objPainMonitorings.Modified = DateTime.Now;
                    objPainMonitorings.ModifiedBy = userId;
                    db.Entry(objPainMonitorings).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return Ok();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }






        [HttpGet]
        [Route("GetPainMonitoring")]

        public async Task<IHttpActionResult> GetPainMonitoring(Guid residentID)
        {

            List<PainMonitoring> objPainMonitoring = await db.PainMonitorings.Where(obj => obj.ResidentID == residentID && obj.IsActive==true).ToListAsync<PainMonitoring>();



            return Ok(objPainMonitoring);
        }

        [HttpPost]
        [Route("SavePainMonitoring")]
        public async Task<IHttpActionResult> SavePainMonitoring(List<PainMonitoring> lstParts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<PainMonitoring> lstPainMonitoring = new List<PainMonitoring>();
            Guid userId = new Guid(User.Identity.GetUserId());
            try
            {
                
                foreach (var item in lstParts)
                {
                    if (item.ID == Guid.Empty)
                    {
                        PainMonitoring objPainMonitoring = new PainMonitoring();
                        objPainMonitoring.ID = Guid.NewGuid();
                        objPainMonitoring.IsActive = true;
                        objPainMonitoring.CreatedBy = userId;
                        objPainMonitoring.Created = objPainMonitoring.Modified = DateTime.Now;

                        objPainMonitoring.PartsID = item.PartsID;
                        objPainMonitoring.OrganizationID = item.OrganizationID;
                        objPainMonitoring.ResidentID = item.ResidentID;
                        objPainMonitoring.Description = item.Description;

                        db.PainMonitorings.Add(objPainMonitoring);
                        await db.SaveChangesAsync();
                        lstPainMonitoring.Add(objPainMonitoring);
                    }
                    else
                    {
                        PainMonitoring objPainMonitorings = db.PainMonitorings.Where(i => i.ID.ToString() == item.ID.ToString()).FirstOrDefault();
                        objPainMonitorings.ModifiedBy = userId;
                        objPainMonitorings.Modified = DateTime.Now;
                        objPainMonitorings.Description = item.Description;
                        db.Entry(objPainMonitorings).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        lstPainMonitoring.Add(objPainMonitorings);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(lstPainMonitoring);
        }


        [Route("GetActivePainMonitoring")]
        public async Task<IHttpActionResult> GetActivePainMonitoring()
        {
            List<PainMonitoring> lstPainMonitoring = await db.PainMonitorings.Where(obj => obj.IsActive == true).ToListAsync<PainMonitoring>();

            return Ok(lstPainMonitoring);
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

        //End - Photo Actions




        //Start-photo Answers




        [HttpGet]
        [ActionName("GetResidentFile")]
        public string GetResidentFile(Guid guidResidentQuestionAnswerID)
        {
            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers")))
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + guidResidentQuestionAnswerID + "")))
                {
                    string[] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Uploads/Residents/Answers/" + guidResidentQuestionAnswerID + ""));

                    if (fileEntries.Length > 0)
                    {
                        FileInfo myFileInfo = new FileInfo(fileEntries[0]);
                        return "/Uploads/Residents/Answers/" + guidResidentQuestionAnswerID + "/" + myFileInfo.Name;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }


        //End-Photo
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class ResidentWithPhoto
    {
        public Resident Resident { get; set; }
        public string PhotoUrl { get; set; }
    }


    public class ResidentWithFile
    {
        public Residents_Questions_Answers ResidentQuestionAnswer { get; set; }
        public string ResidentFile { get; set; }
    }
    public class InterventionResidentWithFile
    {
        public Resident_Interventions_Questions_Answers ResidentQuestionAnswer { get; set; }
        public string ResidentFile { get; set; }
    }

    public class InterventionResidentAnswerWithFile
    {
        public Interventions_Resident_Answers ResidentQuestionAnswer { get; set; }
        public string ResidentFile { get; set; }
    }


    public class AssementSummary
    {

        public Guid SectionID { get; set; }
        public string Question { get; set; }
        public string LabelTxt { get; set; }
    }


    public class SectionQuestionAnswerTask
    {
        public Guid IntervtionID { get; set; }
        public virtual List<Sections_Questions_Answers_Tasks> Sections_Questions_Answers_Tasks { get; set; }
    }

    public class ResidentAnswerAssessment
    {
        public Guid ResidentId { get; set; }
        public Guid? Section_Question_AnswerId { get; set; }
        public int? HasScore { get; set; }
        public Guid? oldChosenAnswer { get; set; }

        public string AnswerText { get; set; }
    }

    public class OldChoosenAnswersID
    {
        public Guid InActiveAnswerIDs { get; set; }
    }

    public class SectionInterventionSection
    {
        public Guid ID { get; set; }
        public string sectionName { get; set; }
        public Guid section_InterventionID { get; set; }
    }

    public class clsSectionwithSectionIntervention
    {
        public virtual List<SectionInterventionSection> SectionInterventionSection { get; set; }
        public virtual List<Section_Intervention> SectionInterventionResponse { get; set; }
    }


    public class clsInterventioSectionIntervention
    {
        public virtual List<SectionInterventionSection> SectionInterventionSection { get; set; }
        public virtual List<Section_Intervention> SectionInterventionResponse { get; set; }
    }

    public class ClsIntervention
    {
        public Guid ID { get; set; }
        public string InterventionName { get; set; }
        public Guid ResidentQuestionAnsID { get; set; }
    }

    public class NewAssementSummary
    {

        public Guid SectionID { get; set; }
        public Guid InterventionId { get; set; }
        public string InterventionName { get; set; }
        public string Question { get; set; }
        public string LabelTxt { get; set; }


    }

    public class NewAssementSummaryData
    {

        public Guid SectionID { get; set; }
        public Guid InterventionId { get; set; }
        public string InterventionName { get; set; }
        public string Question { get; set; }
        public string LabelTxt { get; set; }
        public int DisplayOrder { get; set; }
        public int Ocuurrence { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
    }


    public class GroupAssementSummary
    {

        public Guid SectionID { get; set; }
        public Guid InterventionId { get; set; }
        public string Question { get; set; }
        public string LabelTxt { get; set; }

        public int DisplayOrder { get; set; }
        public List<clsGroupIntervention> lstGroupIntervention { get; set; }

    }

    public class clsGroupIntervention
    {
        public Guid InterventionId { get; set; }
        public string InterventionName { get; set; }

        public int Ocuurrence { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
    }


}
