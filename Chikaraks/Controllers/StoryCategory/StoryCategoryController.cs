using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChikaraksServiceOperationLayer.StoryCategoryServices;
using ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using Chikaraks.Models.ViewModels.StoryCategory;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.IO;
using StartUp;
using ChikaraksServiceOperationLayer.Maintenance;
using Chikaraks.ConstantDictionaryViewModel;

namespace Chikaraks.Controllers.StoryCategory
{
    public class StoryCategoryController : GlobalBaseController
    {
        // GET: StoryCategory
        public StoryCategoryController()
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
                StoryCategoryServices ss = new StoryCategoryServices();
                GetStoryCategoryRequestCCM RequestCCM = new GetStoryCategoryRequestCCM();
                GetStoryCategoryResultCCM ResultCCM = JsonConvert.DeserializeObject<GetStoryCategoryResultCCM>(await ss.getStoryCategoryAsync(JsonConvert.SerializeObject(RequestCCM)));
                if (ResultCCM.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultCCM.errorLogId);
                }
                GetStoryCategoryVM ViewModel = JsonConvert.DeserializeObject<GetStoryCategoryVM>(JsonConvert.SerializeObject(ResultCCM));
                ViewModel.Pagination.controllerName = controllerName;
                ViewModel.Pagination.actionName = actionName;
                ViewModel.BulkActionName = BulkActionName;
                ViewModel.InsertActionName = InsertActionName;
                ViewModel.DetailsActionName = DetailsActionName;

                return View("~/Views/StoryCategory/Index.cshtml", ViewModel);
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
        public async Task<ActionResult> GetDetails(int PrimaryKeys)
        {
            CrudOperationStoryCategoryVM ViewModel = new CrudOperationStoryCategoryVM();
            try
            {
                StoryCategoryServices ss = new StoryCategoryServices();
                GetStoryCategoryByPrimaryKeysRequestCCM RequestModel = new GetStoryCategoryByPrimaryKeysRequestCCM();
                RequestModel.idStoryCategory = PrimaryKeys;

                GetStoryCategoryByPrimaryKeysResultCCM ResultModel = JsonConvert.DeserializeObject<GetStoryCategoryByPrimaryKeysResultCCM>((await ss.getStoryCategoryByPrimaryKeysAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                ViewModel = JsonConvert.DeserializeObject<CrudOperationStoryCategoryVM>(JsonConvert.SerializeObject(ResultModel));
                ViewModel.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                ViewModel.SubmitActionName = UpdateActionName;
                ViewModel.DeleteActionName = DeleteActionName;
                ViewModel.controllerName = controllerName;
                ViewModel.ControllerContext = ControllerContext;
                string viewString = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(ViewModel, "~/Views/StoryCategory/_InputFields.cshtml", ControllerContext);
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
        public JsonResult Create()
        {
            try
            {
                CrudOperationStoryCategoryVM model = new CrudOperationStoryCategoryVM();
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.LoadFormFields = true;
                model.controllerName = controllerName;
                model.SubmitActionName = InsertActionName;
                model.ControllerContext = ControllerContext;
                model.StoryCategoryDCM.storyCategoryImageUrl = ConstantDictionaryCM.ProfilePictureViewPath_string + "1_ProfilePicture.png";
                model.StoryCategoryDCM.categoryLogoUrl = ConstantDictionaryCM.ProfilePictureViewPath_string + "1_ProfilePicture.png";
                model.StoryCategoryDCM.backgroundImageUrl = ConstantDictionaryCM.ProfilePictureViewPath_string + "1_ProfilePicture.png";

                string viewString = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/StoryCategory/_Input.cshtml", ControllerContext);
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
        public async Task<JsonResult> Create(CrudOperationStoryCategoryVM viewModel)
        {
            string serverlocation = "";
            try
            {
                StoryCategoryServices ss = new StoryCategoryServices();
                CreateStoryCategoryRequestCCM RequestModel = new CreateStoryCategoryRequestCCM();
                if (viewModel.StoryCategoryDCM.storyCategoryImageUrl != null)
                {
                    if (viewModel.StoryCategoryDCM.storyCategoryImageUrl.ToLower().Contains("base64"))
                    {
                        string fileName = viewModel.StoryCategoryDCM.storyCategoryTitle.Replace("/", "-").Replace("\\", "-") + "storyCategoryImage" + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.StoryCategoryDCM.storyCategoryImageUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryCategoryDCM.storyCategoryImageUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.StoryCategoryDCM.storyCategoryImageUrl = urllocation;
                    }
                }
                if (viewModel.StoryCategoryDCM.categoryLogoUrl != null)
                {
                    if (viewModel.StoryCategoryDCM.categoryLogoUrl.ToLower().Contains("base64"))
                    {
                        string fileName = viewModel.StoryCategoryDCM.storyCategoryTitle.Replace("/", "-").Replace("\\", "-") + "categoryLogo" + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.StoryCategoryDCM.categoryLogoUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryCategoryDCM.categoryLogoUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.StoryCategoryDCM.categoryLogoUrl = urllocation;
                    }
                }
                if (viewModel.StoryCategoryDCM.backgroundImageUrl != null)
                {
                    if (viewModel.StoryCategoryDCM.backgroundImageUrl.ToLower().Contains("base64"))
                    {
                        string fileName = viewModel.StoryCategoryDCM.storyCategoryTitle.Replace("/", "-").Replace("\\","-") + "backgroundImage" + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.StoryCategoryDCM.backgroundImageUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryCategoryDCM.backgroundImageUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.StoryCategoryDCM.backgroundImageUrl = urllocation;
                    }
                }

                RequestModel.StoryCategoryDCM = viewModel.StoryCategoryDCM;
                CreateStoryCategoryResultCCM ResultModel = JsonConvert.DeserializeObject<CreateStoryCategoryResultCCM>((await ss.CreateStoryCategoryAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                viewModel.StoryCategoryDCM.idStoryCategory = ResultModel.StoryCategoryDCM.idStoryCategory;
                viewModel.LoadFormFields = false;
                viewModel.DetailsActionName = DetailsActionName;
                viewModel.controllerName = controllerName;
                viewModel.DeleteActionName = DeleteActionName;
                viewModel.ControllerContext = ControllerContext;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(viewModel, "~/Views/StoryCategory/_Input.cshtml", ControllerContext));
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
        public async Task<JsonResult> Update(CrudOperationStoryCategoryVM viewModel)
        {
            string serverlocation = "";
            try
            {

                StoryCategoryServices ss = new StoryCategoryServices();
                UpdateStoryCategoryRequestCCM RequestModel = new UpdateStoryCategoryRequestCCM();
                if (viewModel.StoryCategoryDCM.storyCategoryImageUrl != null)
                {
                    if (viewModel.StoryCategoryDCM.storyCategoryImageUrl.ToLower().Contains("base64"))
                    {
                        string fileName = viewModel.StoryCategoryDCM.storyCategoryTitle.Replace("/", "-").Replace("\\", "-") + "storyCategoryImage" + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.StoryCategoryDCM.storyCategoryImageUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryCategoryDCM.storyCategoryImageUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.StoryCategoryDCM.storyCategoryImageUrl = urllocation;
                    }
                }
                if (viewModel.StoryCategoryDCM.categoryLogoUrl != null)
                {
                    if (viewModel.StoryCategoryDCM.categoryLogoUrl.ToLower().Contains("base64"))
                    {
                        string fileName = viewModel.StoryCategoryDCM.storyCategoryTitle.Replace("/", "-").Replace("\\", "-") + "categoryLogo" + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.StoryCategoryDCM.categoryLogoUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryCategoryDCM.categoryLogoUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.StoryCategoryDCM.categoryLogoUrl = urllocation;
                    }
                }

                if (viewModel.StoryCategoryDCM.backgroundImageUrl != null)
                {
                    if (viewModel.StoryCategoryDCM.backgroundImageUrl.ToLower().Contains("base64"))
                    {
                        string fileName = viewModel.StoryCategoryDCM.storyCategoryTitle.Replace("/", "-").Replace("\\", "-") + "backgroundImage" + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.StoryCategoryDCM.backgroundImageUrl.IndexOf("base64,") + 7;
                        string base64 = viewModel.StoryCategoryDCM.backgroundImageUrl.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.StoryCategoryDCM.backgroundImageUrl = urllocation;
                    }
                }


                RequestModel.StoryCategoryDCM = viewModel.StoryCategoryDCM;
                GetFormDetailsResultCCM ResultModel = JsonConvert.DeserializeObject<GetFormDetailsResultCCM>((await ss.UpdateStoryCategoryAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                viewModel.DetailsActionName = DetailsActionName;
                viewModel.controllerName = controllerName;
                viewModel.DeleteActionName = DeleteActionName;
                viewModel.ControllerContext = ControllerContext;
                viewModel.LoadFormFields = false;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(viewModel, "~/Views/StoryCategory/_Input.cshtml", ControllerContext));
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
            CrudOperationStoryCategoryVM ViewModel = new CrudOperationStoryCategoryVM();
            try
            {
                StoryCategoryServices ss = new StoryCategoryServices();
                DeleteStoryCategoryRequestCCM RequestModel = new DeleteStoryCategoryRequestCCM();
                RequestModel.idStoryCategory = PrimaryKeys;
                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await ss.DeleteStoryCategoryAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                string serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + PrimaryKeys + ".jpg";
                if (System.IO.File.Exists(serverlocation))
                {
                    System.IO.File.Delete(serverlocation);
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
                StoryCategoryServices tms = new StoryCategoryServices();
                StoryCategoryBulkActionRequestCCM RequestModel = new StoryCategoryBulkActionRequestCCM();
                foreach (var s in splt)
                {
                    RequestModel.idStoryCategory.Add(int.Parse(s));
                }

                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await tms.deleteMultipleStoryCategoryAsync(JsonConvert.SerializeObject(RequestModel))));
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
                                                    "System"
                                                    );


                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult CheckStoryCategoryTitle(string PrimaryKeys)
        {
            int er = 0;
            try
            {
                StoryCategoryServices ams = new StoryCategoryServices();
                er = ams.checkTitle(PrimaryKeys);
                if (er < 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + er);
                }
                return new JsonResult() { Data = new { result = er, errorLogId = 0 } };
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