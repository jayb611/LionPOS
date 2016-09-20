using Chikaraks.ConstantDictionaryViewModel;
using Chikaraks.Models.ViewModels.StoryScenePages;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using ChikaraksServiceContractModels.ControllerContractModel;
using ChikaraksServiceContractModels.ControllerContractModel.StoryScenePagesCCM;
using ChikaraksServiceContractModels.ErrorContactModel;
using ChikaraksServiceOperationLayer.Maintenance;
using ChikaraksServiceOperationLayer.StoryScenePageServices;
using Newtonsoft.Json;
using StartUp;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Chikaraks.Controllers.StoryScenePages
{
    public class StoryScenePagesController : GlobalBaseController
    {
        // GET: StoryScenePages

        public StoryScenePagesController()
        { }

        string BulkActionName = "BulkAction";
        string InsertActionName = "Create";
        string UpdateActionName = "Update";
        string DeleteActionName = "Delete";
        string DetailsActionName = "GetDetails";

        public async Task<ActionResult> Index(int idStoryCategory)
        {
            try
            {
                StoryScenePageServices scs = new StoryScenePageServices();

                GetStoryScenePagesRequestCCM RequestCCM = new GetStoryScenePagesRequestCCM();
                RequestCCM.idstorycategory = idStoryCategory;
                GetStoryScenePagesResultCCM ResultCCM = JsonConvert.DeserializeObject<GetStoryScenePagesResultCCM>(await scs.getStoryScenePagesAsync(JsonConvert.SerializeObject(RequestCCM)));
                if (ResultCCM.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultCCM.errorLogId);
                }
                StoryScenePagesVM ViewModel = JsonConvert.DeserializeObject<StoryScenePagesVM>(JsonConvert.SerializeObject(ResultCCM));
                ViewModel.idStoryCategoryPK = idStoryCategory;
                ViewModel.Pagination.controllerName = controllerName;
                ViewModel.Pagination.actionName = actionName;
                ViewModel.BulkActionName = BulkActionName;
                ViewModel.InsertActionName = InsertActionName;
                ViewModel.DetailsActionName = DetailsActionName;
                return View("~/Views/StoryScenePages/Index.cshtml", ViewModel);
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessage,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    "System"
                                                    );


                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetDetails(string PrimaryKeys)
        {
            StoryScenePagesCrudeVM ViewModel = new StoryScenePagesCrudeVM();
            try
            {
                StoryScenePageServices ss = new StoryScenePageServices();
                GetStoryScenePageByPrimaryKeysRequestCCM RequestModel = new GetStoryScenePageByPrimaryKeysRequestCCM();
                string[] keys = PrimaryKeys.Split(Convert.ToChar(ConstantDictionaryCM.keysSeparater_string));
                RequestModel.idStoryScenePage = int.Parse(keys[0]);
                RequestModel.idStoryCategory = int.Parse(keys[1]);

                GetStoryScenePageByPrimaryKeysResultCCM ResultModel = JsonConvert.DeserializeObject<GetStoryScenePageByPrimaryKeysResultCCM>((await ss.getStoryScenePageByPrimaryKeysAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                ViewModel = JsonConvert.DeserializeObject<StoryScenePagesCrudeVM>(JsonConvert.SerializeObject(ResultModel));
                ViewModel.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                ViewModel.SubmitActionName = UpdateActionName;
                ViewModel.DeleteActionName = DeleteActionName;
                ViewModel.controllerName = controllerName;
                ViewModel.ControllerContext = ControllerContext;
                string viewString = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(ViewModel, "~/Views/StoryScenePages/_InputFields.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = viewString } };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessage,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    sessionObj.user.userName
                                                    );


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        [AllowAnonymous]
        public JsonResult Create(int idstorycategory)
        {
            try
            {
                StoryScenePagesCrudeVM model = new StoryScenePagesCrudeVM();
                model.StoryScenePagesDCM.idStoryCategory = idstorycategory;
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.LoadFormFields = true;
                model.controllerName = controllerName;
                model.SubmitActionName = InsertActionName;
                model.ControllerContext = ControllerContext;
                model.StoryScenePagesDCM.imageUrl = ConstantDictionaryCM.ProfilePictureViewPath_string + "1_ProfilePicture.png";
                //model.StoryScenePagesDCM.audioUrl = ConstantDictionaryCM.ProfilePictureViewPath_string + "browse.png";

                string viewString = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/StoryScenePages/_Input.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = viewString }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessage,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    sessionObj.user.userName
                                                    );


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<JsonResult> Create(StoryScenePagesCrudeVM viewModel)
        {
            string serverlocation = "";
            try
            {
                StoryScenePageServices ss = new StoryScenePageServices();
                int max = ss.GetMaxIDStoryScenePageAsync();
                CreateStoryScenePageRequestCCM RequestModel = new CreateStoryScenePageRequestCCM();
                if (viewModel.StoryScenePagesDCM.imageUrl != null)
                {
                    if (viewModel.StoryScenePagesDCM.imageUrl.ToLower().Contains("base64"))
                    {
                        string fileName = "StoryScenePageImage_" + viewModel.StoryScenePagesDCM.idStoryCategory + ConstantDictionaryCM.keysSeparater_string + max + ConstantDictionaryCM.keysSeparater_string + viewModel.StoryScenePagesDCM.indexOrder + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.StoryScenePagesDCM.imageUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryScenePagesDCM.imageUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.StoryScenePagesDCM.imageUrl = urllocation;
                    }
                }
                if (viewModel.StoryScenePagesDCM.audioUrl != null)
                {
                    if (viewModel.StoryScenePagesDCM.audioUrl.ToLower().Contains("base64"))
                    {
                        string fileName = "StoryScenePageAudio_" + viewModel.StoryScenePagesDCM.idStoryCategory + ConstantDictionaryCM.keysSeparater_string + max + ConstantDictionaryCM.keysSeparater_string + viewModel.StoryScenePagesDCM.indexOrder + "Audio.mp3";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.AudioServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.AudioViewPath_string + fileName;
                        int index = viewModel.StoryScenePagesDCM.audioUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryScenePagesDCM.audioUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var AudFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            AudFile.Write(bytes, 0, bytes.Length);
                            AudFile.Flush();
                        }
                        viewModel.StoryScenePagesDCM.audioUrl = urllocation;

                    }

                }

                RequestModel.StoryScenePagesDCM = viewModel.StoryScenePagesDCM;
                CreateStoryScenePageResultCCM ResultModel = JsonConvert.DeserializeObject<CreateStoryScenePageResultCCM>((await ss.CreateStoryScenePageAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                viewModel.StoryScenePagesDCM.idStoryScenePages = ResultModel.idStoryScenePages;
                viewModel.LoadFormFields = false;
                viewModel.DetailsActionName = DetailsActionName;
                viewModel.controllerName = controllerName;
                viewModel.DeleteActionName = DeleteActionName;
                viewModel.ControllerContext = ControllerContext;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(viewModel, "~/Views/StoryScenePages/_Input.cshtml", ControllerContext));
                return new JsonResult() { Data = new { errorLogId = 0, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert, alertMessage = "", errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessage,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    sessionObj.user.userName
                                                    );


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<JsonResult> Update(StoryScenePagesCrudeVM viewModel)
        {
            string serverlocation = "";
            try
            {
                StoryScenePageServices tms = new StoryScenePageServices();
                UpdateStoryScenePageRequestCCM RequestModel = new UpdateStoryScenePageRequestCCM();
                if (viewModel.StoryScenePagesDCM.imageUrl != null)
                {
                    if (viewModel.StoryScenePagesDCM.imageUrl.ToLower().Contains("base64"))
                    {
                        string fileName = "StoryScenePageImage_" + viewModel.StoryScenePagesDCM.idStoryCategory + ConstantDictionaryCM.keysSeparater_string + viewModel.StoryScenePagesDCM.idStoryScenePages + ConstantDictionaryCM.keysSeparater_string + viewModel.StoryScenePagesDCM.indexOrder + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.StoryScenePagesDCM.imageUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryScenePagesDCM.imageUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.StoryScenePagesDCM.imageUrl = urllocation;
                    }
                }
                if (viewModel.StoryScenePagesDCM.audioUrl != null)
                {
                    if (viewModel.StoryScenePagesDCM.audioUrl.ToLower().Contains("base64"))
                    {
                        string fileName = "StoryScenePageAudio_" + viewModel.StoryScenePagesDCM.idStoryCategory + ConstantDictionaryCM.keysSeparater_string + viewModel.StoryScenePagesDCM.idStoryScenePages + ConstantDictionaryCM.keysSeparater_string + viewModel.StoryScenePagesDCM.indexOrder + "Audio.mp3";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.AudioServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.AudioViewPath_string + fileName;
                        int index = viewModel.StoryScenePagesDCM.audioUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryScenePagesDCM.audioUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var AudFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            AudFile.Write(bytes, 0, bytes.Length);
                            AudFile.Flush();
                        }
                        viewModel.StoryScenePagesDCM.audioUrl = urllocation;

                    }

                }


                RequestModel.StoryScenePagesDCM = viewModel.StoryScenePagesDCM;

                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await tms.UpdateStoryScenePageAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                viewModel.DetailsActionName = DetailsActionName;
                viewModel.controllerName = controllerName;
                viewModel.DeleteActionName = DeleteActionName;
                viewModel.ControllerContext = ControllerContext;

                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(viewModel, "~/Views/StoryScenePages/_Input.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };

            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessage,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    sessionObj.user.userName
                                                    );


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }



        public async Task<JsonResult> Delete(int PrimaryKeys)
        {
            try
            {
                StoryScenePageServices tms = new StoryScenePageServices();
                DeleteStoryScenePageRequestCCM RequestModel = new DeleteStoryScenePageRequestCCM();
                RequestModel.IDStoryScenePage = PrimaryKeys;
                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await tms.DeleteStoryScenePageAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }

                return new JsonResult() { Data = new { errorLogId = 0, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme) } };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessage,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    sessionObj.user.userName
                                                    );


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }


        public async Task<ActionResult> BulkAction(string PrimaryKeys, string action)
        {
            try
            {
                string[] splt = PrimaryKeys.Split(',');
                StoryScenePageServices tms = new StoryScenePageServices();
                SelectedStoryScenePageActionRequestCCM RequestModel = new SelectedStoryScenePageActionRequestCCM();
                foreach (var s in splt)
                {
                    RequestModel.IDStoryCategoryScene.Add(int.Parse(s));
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
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessage,
                                                    new LionUtilities.ConversionUtilitise().ObjectToString(ex),
                                                    null,
                                                    "",
                                                    this.GetType().Name,
                                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
                                                    new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
                                                    true,
                                                    sessionObj.user.userName
                                                    );


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }



    }
}