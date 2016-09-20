using Chikaraks.Models.ViewModels.ContentManagement;
using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using ChikaraksServiceContractModels.ControllerContractModel.ContentCCM;
using ChikaraksServiceContractModels.ErrorContactModel;
using ChikaraksServiceOperationLayer.ContentServices;
using Newtonsoft.Json;
using StartUp;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Chikaraks.Controllers.ChikaraksContentsManagement
{
    public class ChikaraksContentsController : GlobalBaseController
    {
        // GET: ChikaraksContents
        public ChikaraksContentsController()
        {

        }

        string BulkActionName = "BulkAction";
        string InsertActionName = "Create";
        string UpdateActionName = "Update";
        string DeleteActionName = "Delete";
        string DetailsActionName = "GetDetails";

        public async Task<ActionResult> Index()
        {
            try
            {
                ContentServices cs = new ContentServices();
                BasicQueryContractModel RequestModel = new BasicQueryContractModel();
                RequestModel.FilterFieldAndValues = "";
                RequestModel.OrderByFields = "";
                RequestModel.sessionObj = sessionObj;
                RequestModel.LoadAsDefaultFilter = true;
                RequestModel.SaveAsDefaultFilter = false;

                GetContentResultCCM ResultCCM = JsonConvert.DeserializeObject<GetContentResultCCM>(await cs.getContentsAsync(JsonConvert.SerializeObject(RequestModel)));
                if (ResultCCM.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultCCM.errorLogId);
                }

                ResultCCM.Pagination.controllerName = controllerName;
                ResultCCM.Pagination.actionName = actionName;
                ResultCCM.BulkActionName = BulkActionName;
                ResultCCM.DeleteActionName = DeleteActionName;
                ResultCCM.InsertActionName = InsertActionName;
                ResultCCM.DetailsActionName = DetailsActionName;
                return View("~/Views/ChikaraksContents/Index.cshtml", ResultCCM);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "ErrorHandler", new { logid = 1 });
            }

        }


        [HttpPost]
        public async Task<ActionResult> GetDetails(string PrimaryKeys)
        {
            ContentCrudeOperationViewModel model = new ContentCrudeOperationViewModel();
            try
            {
                ContentServices tms = new ContentServices();
                GetContentByContentIDRequestCCM RequestModel = new GetContentByContentIDRequestCCM();
                RequestModel.ID = int.Parse(PrimaryKeys);
                GetContentByContentIDResultCCM ResultModel = JsonConvert.DeserializeObject<GetContentByContentIDResultCCM>((await tms.getContentByContentIDAsync(JsonConvert.SerializeObject(RequestModel))));
                model.errorLogId = ResultModel.errorLogId;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.ContentDCM = ResultModel.ContentDCM;
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                model.SubmitActionName = UpdateActionName;
                model.DeleteActionName = DeleteActionName;
                model.controllerName = controllerName;
                string viewString = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/ChikaraksContents/_InputFields.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = viewString } };
            }
            catch (Exception ex)
            {



                return new JsonResult() { Data = new { errorLogId = 1, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }




        [HttpGet]
        [ValidateInput(false)]
        [AllowAnonymous]
        public JsonResult Create()
        {
            try
            {
                ContentServices tms = new ContentServices();
                ContentCrudeOperationViewModel model = new ContentCrudeOperationViewModel();
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.LoadFormFields = true;
                model.controllerName = controllerName;
                model.SubmitActionName = InsertActionName;
                model.ControllerContext = ControllerContext;
                model.DetailsActionName = DetailsActionName;
                string viewString = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/ChikaraksContents/_Input.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = viewString }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {

                return new JsonResult() { Data = new { errorLogId = 1, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<JsonResult> Create(ContentCrudeOperationViewModel viewModel)
        {
            CreateContentResultCCM ResultCM = new CreateContentResultCCM();
            try
            {
                ContentServices tms = new ContentServices();
                CreateContentRequestCCM RequestModel = new CreateContentRequestCCM();
                RequestModel.ContentDCM = viewModel.ContentDCM;
                if (string.IsNullOrEmpty(viewModel.ContentDCM.contentTag)
                    ||
                    (string.IsNullOrEmpty(viewModel.ContentDCM.contentValue)))
                {
                    return new JsonResult() { Data = new { errorLogId = -1, alertMessage = "you might have missed information of form!Ple. fill all details and then submit.", errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme) } };
                }
                CreateContentResultCCM ResultModel = JsonConvert.DeserializeObject<CreateContentResultCCM>((await tms.CreateContentAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                viewModel.ContentDCM.idContent = ResultModel.ContentDCM.idContent;
                viewModel.LoadFormFields = false;
                viewModel.DetailsActionName = DetailsActionName;
                viewModel.controllerName = controllerName;
                viewModel.DeleteActionName = DeleteActionName;
                viewModel.ControllerContext = ControllerContext;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(viewModel, "~/Views/ChikaraksContents/_Input.cshtml", ControllerContext));
                return new JsonResult() { Data = new { errorLogId = 0, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert, alertMessage = "", errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };
            }
            catch (Exception ex)
            {

                return new JsonResult() { Data = new { errorLogId = 1, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<JsonResult> Update(ContentCrudeOperationViewModel viewModel)
        {
            try
            {
                ContentServices tms = new ContentServices();
                UpdateContentRequestCCM RequestModel = new UpdateContentRequestCCM();
                RequestModel.ContetntDCM = viewModel.ContentDCM;
                if (string.IsNullOrEmpty(viewModel.ContentDCM.contentTag) || string.IsNullOrEmpty(viewModel.ContentDCM.contentValue))
                {
                    return new JsonResult() { Data = new { errorLogId = -1, alertMessage = "you might have missed information of form!Ple. fill all details and then submit.", errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme) } };
                }
                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await tms.UpdateContentAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                viewModel.DetailsActionName = DetailsActionName;
                viewModel.controllerName = controllerName;
                viewModel.DeleteActionName = DeleteActionName;

                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(viewModel, "~/Views/ChikaraksContents/_Input.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };

            }
            catch (Exception ex)
            {


                return new JsonResult() { Data = new { errorLogId = 1, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }



        public async Task<JsonResult> Delete(string PrimaryKeys)
        {
            try
            {
                ContentServices tms = new ContentServices();
                DeleteContentRequestCCM RequestModel = new DeleteContentRequestCCM();
                RequestModel.ID = int.Parse(PrimaryKeys);
                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await tms.DeleteContentAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                return new JsonResult() { Data = new { errorLogId = 0, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme) } };
            }
            catch (Exception ex)
            {

                return new JsonResult() { Data = new { errorLogId = 1, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }


        public async Task<ActionResult> BulkAction(string PrimaryKeys, string action)
        {
            try
            {
                string[] splt = PrimaryKeys.Split(',');
                ContentServices tms = new ContentServices();
                SelectedContentActionRequestCCM RequestModel = new SelectedContentActionRequestCCM();
                foreach (var s in splt)
                {
                    RequestModel.IDs.Add(int.Parse(s));
                }

                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await tms.deleteMultipleContentsAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return new JsonResult() { Data = new { errorLogId = 4, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 4 }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }



        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult CheckContentTitle(string PrimaryKeys)
        {
            int er = 0;
            try
            {
                ContentServices ams = new ContentServices();
                er = ams.checkTitle(PrimaryKeys);
                if (er < 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + er);
                }
                return new JsonResult() { Data = new { result = er, errorLogId = 0 } };
            }
            catch (Exception)
            {


                return new JsonResult() { Data = new { result = er, errorLogId = 1, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }) } };
            }
        }






    }
}