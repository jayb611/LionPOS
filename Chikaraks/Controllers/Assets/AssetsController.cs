using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChikaraksServiceOperationLayer.AssetsServices;
using ChikaraksServiceContractModels.ControllerContractModel.AssetsCCM;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using ChikaraksServiceContractModels;
using Chikaraks.Models.ViewModels.Assets;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.IO;
using StartUp;

namespace Chikaraks.Controllers.Assets
{
    public class AssetsController : GlobalBaseController
    {
        // GET: Assets
        public AssetsController()
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
                AssetsServices ss = new AssetsServices();
                BasicQueryContractModel RequestModel = new BasicQueryContractModel();
                RequestModel.FilterFieldAndValues = "";
                RequestModel.OrderByFields = "";
                RequestModel.sessionObj = sessionObj;
                RequestModel.LoadAsDefaultFilter = true;
                RequestModel.SaveAsDefaultFilter = false;
                GetAssetsResultCCM ResultModel = JsonConvert.DeserializeObject<GetAssetsResultCCM>((await ss.getAssetsAsync(JsonConvert.SerializeObject(RequestModel))));

                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                ResultModel.Pagination.controllerName = controllerName;
                ResultModel.Pagination.actionName = actionName;
                ResultModel.BulkActionName = BulkActionName;
                ResultModel.InsertActionName = InsertActionName;
                ResultModel.DetailsActionName = DetailsActionName;
                return View("~/Views/Assets/Index.cshtml", ResultModel);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "ErrorHandler", new { logid = 1 });
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        [AllowAnonymous]
        public JsonResult Create()
        {
            try
            {
                AssetsCrudOperationViewModel model = new AssetsCrudOperationViewModel();
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.LoadFormFields = true;
                model.controllerName = controllerName;
                model.SubmitActionName = InsertActionName;
                model.ControllerContext = ControllerContext;
                model.AssetsDCM.homeBackground = ConstantDictionaryCM.ProfilePictureViewPath_string + "1_ProfilePicture.png";
                model.AssetsDCM.homeAudio = ConstantDictionaryCM.ProfilePictureViewPath_string + "browse.png";
                string viewString = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/Assets/_Input.cshtml", ControllerContext);
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
        public async Task<JsonResult> Create(AssetsCrudOperationViewModel viewModel)
        {
            string serverlocation = "";
            try
            {
                AssetsServices ss = new AssetsServices();
                //int max = ss.getAssetMaxID().Result;
                CreateAssetsRequestCCM RequestModel = new CreateAssetsRequestCCM();
                RequestModel.AssetsDCM = viewModel.AssetsDCM;
                if (viewModel.AssetsDCM.homeBackground != null)
                {
                    if (viewModel.AssetsDCM.homeBackground.ToLower().Contains("base64"))
                    {
                        string fileName = "AssetsHomeBackground_" + viewModel.AssetsDCM.title + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.AssetsDCM.homeBackground.IndexOf("base64,") + 7;
                        string base64 = viewModel.AssetsDCM.homeBackground.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.AssetsDCM.homeBackground = urllocation;
                    }
                }
                if (viewModel.AssetsDCM.homeAudio != null)
                {
                    if (viewModel.AssetsDCM.homeAudio.ToLower().Contains("base64"))
                    {

                        string fileName = "AssetsHomeAudio_" + viewModel.AssetsDCM.title + ".mp3";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.AudioServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.AudioViewPath_string + fileName;
                        int index = viewModel.AssetsDCM.homeAudio.IndexOf("base64,") + 7;
                        string base64 = viewModel.AssetsDCM.homeAudio.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var AudFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            AudFile.Write(bytes, 0, bytes.Length);
                            AudFile.Flush();
                        }
                        viewModel.AssetsDCM.homeAudio = urllocation;

                    }

                }
                RequestModel.AssetsDCM = viewModel.AssetsDCM;
                CreateAssetsResultCCM ResultModel = JsonConvert.DeserializeObject<CreateAssetsResultCCM>((await ss.CreateAssetsAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                viewModel.AssetsDCM.idAssets = ResultModel.AssetsDCM.idAssets;
                viewModel.LoadFormFields = false;
                viewModel.DetailsActionName = DetailsActionName;
                viewModel.controllerName = controllerName;
                viewModel.DeleteActionName = DeleteActionName;
                viewModel.ControllerContext = ControllerContext;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(viewModel, "~/Views/Assets/_Input.cshtml", ControllerContext));
                return new JsonResult() { Data = new { errorLogId = 0, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert, alertMessage = "", errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(serverlocation))
                {
                    System.IO.File.Delete(serverlocation);
                }
                return new JsonResult() { Data = new { errorLogId = 1, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }

        [HttpPost]

        public async Task<ActionResult> GetDetails(string PrimaryKeys)
        {
            AssetsCrudOperationViewModel model = new AssetsCrudOperationViewModel();
            try
            {
                string[] keys = PrimaryKeys.Split(Convert.ToChar(ConstantDictionaryCM.keysSeparater_string));
                AssetsServices ss = new AssetsServices();
                GetAssetsByAssetsIDRequestCCM RequestModel = new GetAssetsByAssetsIDRequestCCM();
                RequestModel.idAssets = Convert.ToInt32(keys[0]);
                RequestModel.title = keys[1];
                GetAssetsByAssetsIDResultCCM ResultModel = JsonConvert.DeserializeObject<GetAssetsByAssetsIDResultCCM>((await ss.getAssetsByAssetsIDAsync(JsonConvert.SerializeObject(RequestModel))));
                model.errorLogId = ResultModel.errorLogId;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.AssetsDCM = ResultModel.AssetsDCM;
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                model.SubmitActionName = UpdateActionName;
                model.DeleteActionName = DeleteActionName;
                model.controllerName = controllerName;
                string viewString = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/Assets/_InputFields.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = viewString } };
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = new { errorLogId = 1, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<JsonResult> Update(AssetsCrudOperationViewModel viewModel)
        {
            string serverlocation = "";
            try
            {
                AssetsServices ss = new AssetsServices();
                UpdateAssetsRequestCCM RequestModel = new UpdateAssetsRequestCCM();
                RequestModel.Assets = viewModel.AssetsDCM;
                if (viewModel.AssetsDCM.homeBackground != null)
                {
                    if (viewModel.AssetsDCM.homeBackground.ToLower().Contains("base64"))
                    {
                        string fileName = "AssetsHomeBackground_" + viewModel.AssetsDCM.title + ".jpg";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.ProfilePictureViewPath_string + fileName;
                        int index = viewModel.AssetsDCM.homeBackground.IndexOf("base64,") + 7;
                        string base64 = viewModel.AssetsDCM.homeBackground.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var imageFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }
                        viewModel.AssetsDCM.homeBackground = urllocation;
                    }
                }
                if (viewModel.AssetsDCM.homeAudio != null)
                {
                    if (viewModel.AssetsDCM.homeAudio.ToLower().Contains("base64"))
                    {

                        string fileName = "AssetsHomeAudio_" + viewModel.AssetsDCM.title + ".mp3";
                        serverlocation = Server.MapPath(ConstantDictionaryCM.AudioServerMapPath_string) + fileName;
                        string urllocation = ConstantDictionaryCM.AudioViewPath_string + fileName;
                        int index = viewModel.AssetsDCM.homeAudio.IndexOf("base64,") + 7;
                        string base64 = viewModel.AssetsDCM.homeAudio.Substring(index);
                        var bytes = Convert.FromBase64String(base64);
                        using (var AudFile = new FileStream(serverlocation, FileMode.Create))
                        {
                            AudFile.Write(bytes, 0, bytes.Length);
                            AudFile.Flush();
                        }
                        viewModel.AssetsDCM.homeAudio = urllocation;

                    }

                }

                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await ss.UpdateAssetsAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }

                viewModel.DetailsActionName = DetailsActionName;
                viewModel.controllerName = controllerName;
                viewModel.DeleteActionName = DeleteActionName;


                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(viewModel, "~/Views/Assets/_Input.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };

            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(serverlocation))
                {
                    System.IO.File.Delete(serverlocation);
                }
                return new JsonResult() { Data = new { errorLogId = 1, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 1 }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }

        public async Task<JsonResult> Delete(string PrimaryKeys)
        {
            try
            {
                string[] keys = PrimaryKeys.Split(Convert.ToChar(ConstantDictionaryCM.keysSeparater_string));
                AssetsServices ss = new AssetsServices();
                DeleteAssetsRequestCCM RequestModel = new DeleteAssetsRequestCCM();
                RequestModel.idAssets = Convert.ToInt32(keys[0]);
                RequestModel.title = keys[1];
                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await ss.DeleteAssetsAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                string serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + "AssetsHomeBackground_" + RequestModel.title + ".jpg";
                if (System.IO.File.Exists(serverlocation))
                {
                    System.IO.File.Delete(serverlocation);
                }
                string serverlocationAudio = Server.MapPath(ConstantDictionaryCM.AudioServerMapPath_string) + "AssetsHomeAudio_" + RequestModel.title + ".mp3";
                if (System.IO.File.Exists(serverlocationAudio))
                {
                    System.IO.File.Delete(serverlocationAudio);
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

                AssetsServices tms = new AssetsServices();
                SelectedAssetsActionRequestCCM RequestModel = new SelectedAssetsActionRequestCCM();

                var PrimaryKeysdata = PrimaryKeys.Split(Convert.ToChar(ConstantDictionaryCM.keysListSeparater_string));
                foreach (var a in PrimaryKeysdata)
                {
                    string[] PrimaryKeysSplit = a.Split(Convert.ToChar(ConstantDictionaryCM.keysSeparater_string));
                    RequestModel.idAssets.Add(Convert.ToInt32(PrimaryKeysSplit[0]));
                    RequestModel.title.Add(PrimaryKeysSplit[1]);
                }

                ErrorCM ResultModel = JsonConvert.DeserializeObject<ErrorCM>((await tms.deleteMultipleAssetsAsync(JsonConvert.SerializeObject(RequestModel))));
                if (ResultModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultModel.errorLogId);
                }
                string serverlocation = Server.MapPath(ConstantDictionaryCM.ProfilePictureServerMapPath_string) + "AssetsHomeBackground_" + RequestModel.title + ".jpg";
                if (System.IO.File.Exists(serverlocation))
                {
                    System.IO.File.Delete(serverlocation);
                }
                string serverlocationAudio = Server.MapPath(ConstantDictionaryCM.AudioServerMapPath_string) + "AssetsHomeAudio_" + RequestModel.title + ".mp3";
                if (System.IO.File.Exists(serverlocationAudio))
                {
                    System.IO.File.Delete(serverlocationAudio);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = new { errorLogId = 4, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 4 }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }
    }
}