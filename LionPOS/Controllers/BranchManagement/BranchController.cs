using System;
using System.Web.Mvc;
using LionStartUp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceOperationLayer.BranchManagement;
using LionPOSServiceContractModels.ControllerContractModel.BranchManagement;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using System.Collections.Generic;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels;
using System.Threading;
using LionPOS.Models.ViewModels.BranchManagement.Branch;
using LionPOSServiceContractModels.DomainContractsModel.Branch;
using LionPOSServiceContractModels.ErrorContactModel;

namespace LionPOS.Controllers.BranchManagement.Branch
{



    public class BranchController : GlobalBaseController
    {
        //
        // GET: /InquiryTypeMaster/
        public BranchController()
        {

        }

        public async Task<ActionResult> Index(int RecordsPerPage = 0, int PageNumber = 1)
        {
            try
            {
                BranchManagementServices ss = new BranchManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                cm.showBranchWise = showBranchWise;
                cm.FilterFieldAndValues = "";
                cm.OrderByFields = "";
                cm.sessionObj = sessionObj;
                cm.LoadAsDefaultFilter = true;
                cm.SaveAsDefaultFilter = false;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                BranchCCM model = (await ss.getBranchesDynamic(new FormDataCollection(lkvp))).Data as BranchCCM;

                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.controllerName = controllerName;
                model.Pagination.actionName = actionName;
                model.Pagination.FilterActionName = "Filter";
                model.bulkActionName = "BulkAction";
                model.InsertActionName = "CreateBranch";
                return View("~/Views/BranchManagement/Branch/Index.cshtml", model);

            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessageForUser,
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
        public async Task<JsonResult> Filter(string filterFieldsAndValues, string orderByField, bool SaveAsDefaultFilter, bool LoadAsDefaultFilter, int PageNumber = 1)
        {
            try
            {
                BranchManagementServices ss = new BranchManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                cm.showBranchWise = showBranchWise;
                cm.FilterFieldAndValues = filterFieldsAndValues;
                cm.OrderByFields = orderByField;
                cm.sessionObj = sessionObj;
                cm.pageNumber = PageNumber;
                cm.SaveAsDefaultFilter = SaveAsDefaultFilter;
                cm.LoadAsDefaultFilter = LoadAsDefaultFilter;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                BranchCCM model = (await ss.getBranchesDynamic(new FormDataCollection(lkvp))).Data as BranchCCM;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.FilterActionName = "Filter";
                model.Pagination.SaveAsDefaultURLLink = "SaveAsDefaultFilter";
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/BranchManagement/Branch/_ListPaneDetail.cshtml", ControllerContext);
                //Thread.Sleep(5000);
                return new JsonResult() { Data = new { TotalPage = model.Pagination.TotalPages, view = dat } };
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = ex.ToString() };
            }
        }


        public async Task<ActionResult> GetBranchDetails(string branchCode)
        {
            branchCrudOperationViewModel model = new branchCrudOperationViewModel(sessionObj);
            try
            {
                BranchManagementServices ss = new BranchManagementServices();

                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(branchCode)));
                getBranchByBranchCodeCCM cModel = (await ss.getBranchByBranchCodeAsync(new FormDataCollection(lkvp))).Data as getBranchByBranchCodeCCM;
                model.errorLogId = cModel.errorLogId;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.branchDCM = cModel.branchDCM;
                model.CountryList = cModel.CountryList;
                model.stateList = cModel.StateList;
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                model.SubmitActionName = "UpdateBranch";
                model.DeleteActionName = "DeleteBranch";
                model.controllerName = controllerName;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/BranchManagement/Branch/_InputFields.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessageForUser,
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


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), ex = ex.ToString() } };
            }
        }



        [HttpPost]
        public JsonResult CreateBranch()
        {

            try
            {

                branchCrudOperationViewModel model = new branchCrudOperationViewModel(sessionObj);
                model.SubmitActionName = "CreateBranchSubmit";
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.branchDCM.branchCode = "-1";
                model.LoadFormFields = true;
                model.controllerName = controllerName;
                model.SubmitActionName = "CreateBranchSubmit";
                model.ControllerContext = ControllerContext;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/BranchManagement/Branch/_Input.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, errorUrl = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };

            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessageForUser,
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
                return new JsonResult() { Data = new { errorLogId = logid, errorUrl = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), view = "" } };
            }
        }



        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> CreateBranchSubmit(branchCrudOperationViewModel model)
        {
            try
            {
                BranchManagementServices ss = new BranchManagementServices();
                branchDCM DCMmodel = new branchDCM();
                DCMmodel = model.branchDCM;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(DCMmodel)));
                ErrorCM er = (await ss.AddBranchDynamic(new FormDataCollection(lkvp), sessionObj)).Data as ErrorCM;
                if (er.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + er.errorLogId);
                }

                model.LoadFormFields = false;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/BranchManagement/Branch/_Input.cshtml", ControllerContext));
                return new JsonResult() { Data = new { errorLogId = 0, errorUrl = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessageForUser,
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
                return new JsonResult() { Data = new { errorLogId = logid, errorUrl = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), view = "" } };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<JsonResult> UpdateBranch(branchCrudOperationViewModel model)
        {
            try
            {
                BranchManagementServices ss = new BranchManagementServices();
                model.branchDCM.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model.branchDCM)));
                ErrorCM cModel = (await ss.UpdateBranchDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (cModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + cModel.errorLogId);
                }
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/BranchManagement/Branch/_Input.cshtml", ControllerContext);
                return new JsonResult() { Data = new { errorLogId = 0, crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update, errorURL = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme), view = dat } };

            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessageForUser,
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


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme), ex = ex.ToString() } };
            }

        }

        public async Task<JsonResult> DeleteBranch(string deletekeys)
        {
            try
            {
                string branchCode = deletekeys;
                BranchManagementServices ss = new BranchManagementServices();
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(branchCode)));
                ErrorCM er = (await ss.DeleteBranchDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (er.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + er.errorLogId);
                }
                return new JsonResult() { Data = new { errorLogId = 0, errorUrl = Url.Action("Index", "ErrorHandler", new { logid = 0 }, Request.Url.Scheme) } };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessageForUser,
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
                return new JsonResult() { Data = new { errorLogId = logid, errorUrl = Url.Action("Index", "ErrorHandler", new { logid = logid }, Request.Url.Scheme) } };
            }

        }

        public async Task<ActionResult> BulkAction(string branchCodes, string action)
        {
            try
            {
                BranchManagementServices bms = new BranchManagementServices();
                BranchCCM model = new BranchCCM();
                model.branchCodes = branchCodes;

                if (action == "Delete Selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.deleteMultipleBranchesAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Active to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.setAsActiveMultipleBranchesAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Inactive to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.setAsInactiveMultipleBranchesAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                        "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                     ConstantDictionaryVM.ErrorMessageForUser,
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


    }
}




