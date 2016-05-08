using LionPOS.Models.ViewModels.SupplierManagement.Supplier;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using LionPOSServiceContractModels.ControllerContractModel.SupplierManagement;
using LionPOSServiceContractModels.DomainContractsModel.Supplier;
using LionPOSServiceContractModels.ErrorContactModel;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceOperationLayer.SupplierManagement;
using LionStartUp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LionPOS.Controllers.SupplierManagement
{
    public class SupplierController : GlobalBaseController
    {
        // GET: Supplier

        public SupplierController()
        { }

        public async Task<ActionResult> Index(int RecordsPerPage = 0, int PageNumber = 1)
        {
            SupplierManagementServices sms = new SupplierManagementServices();
            BasicQueryContractModel cm = new BasicQueryContractModel();
            cm.FilterFieldAndValues = "";
            cm.OrderByFields = "";
            cm.sessionObj = sessionObj;
            cm.LoadAsDefaultFilter = true;
            cm.SaveAsDefaultFilter = false;
            List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
            lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
            SupplierCCM model = (await sms.getSuppliersDynamic(new FormDataCollection(lkvp))).Data as SupplierCCM;
            if (model.errorLogId > 0)
            {
                throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
            }
            model.Pagination.controllerName = controllerName;
            model.Pagination.actionName = actionName;
            model.Pagination.FilterActionName = "Filter";
            model.bulkActionName = "BulkAction";
            model.InsertActionName = "CreateSupplier";
            
            return View("~/Views/SupplierManagement/Supplier/Index.cshtml", model);
            //return View();
        }

        [HttpPost]
        public async Task<JsonResult> Filter(string filterFieldsAndValues, string orderByField, bool SaveAsDefaultFilter, bool LoadAsDefaultFilter, int PageNumber = 1)
        {
            try
            {
                SupplierManagementServices ss = new SupplierManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                
                cm.FilterFieldAndValues = filterFieldsAndValues;
                cm.OrderByFields = orderByField;
                cm.sessionObj = sessionObj;
                cm.pageNumber = PageNumber;
                cm.SaveAsDefaultFilter = SaveAsDefaultFilter;
                cm.LoadAsDefaultFilter = LoadAsDefaultFilter;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                SupplierCCM model = (await ss.getSuppliersDynamic(new FormDataCollection(lkvp))).Data as SupplierCCM;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.FilterActionName = "Filter";
                model.Pagination.SaveAsDefaultURLLink = "SaveAsDefaultFilter";
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/SupplierManagement/Supplier/_ListPaneDetail.cshtml", ControllerContext);
                //Thread.Sleep(5000);
                return new JsonResult() { Data = new { TotalPage = model.Pagination.TotalPages, view = dat } };
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = ex.ToString() };
            }
        }


        public async Task<ActionResult> GetSupplierDetails(string suppliersCode)
        {
            supplierCrudOperationViewModel model = new supplierCrudOperationViewModel(sessionObj);
            try
            {
                SupplierManagementServices sms = new SupplierManagementServices();

                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(suppliersCode)));
                supplierDCM cModel = (await sms.getSupplierBySuppliersCodeAsync(new FormDataCollection(lkvp))).Data as supplierDCM;
                model.errorLogId = cModel.errorLogId;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.supplierDCM = cModel;
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                model.SubmitActionName = "UpdateSupplier";
                model.DeleteActionName = "DeleteSupplier";
                model.controllerName = controllerName;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/SupplierManagement/Supplier/_InputFields.cshtml", ControllerContext);
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
        public JsonResult CreateSupplier()
        {

            try
            {

                supplierCrudOperationViewModel model = new supplierCrudOperationViewModel(sessionObj);
                model.SubmitActionName = "CreateSupplierSubmit";
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.supplierDCM.suppliersCode = "-1";
                model.LoadFormFields = true;
                model.SubmitActionName = "CreateSupplierSubmit";
                model.controllerName = controllerName;
                model.ControllerContext = ControllerContext;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/SupplierManagement/Supplier/_Input.cshtml", ControllerContext);
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
        public async Task<JsonResult> CreateSupplierSubmit(supplierCrudOperationViewModel model)
        {
            try
            {
                SupplierManagementServices ss = new SupplierManagementServices();
                supplierDCM DCMmodel = new supplierDCM();
                DCMmodel = model.supplierDCM;
                DCMmodel.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(DCMmodel)));
                ErrorCM er = (await ss.AddSupplierDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (er.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + er.errorLogId);
                }

                model.LoadFormFields = false;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/SupplierManagement/Supplier/_Input.cshtml", ControllerContext));
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
        public async Task<JsonResult> UpdateSupplier(supplierCrudOperationViewModel model)
        {
            try
            {
                SupplierManagementServices ss = new SupplierManagementServices();
                model.supplierDCM.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model.supplierDCM)));
                ErrorCM cModel = (await ss.UpdateSupplierDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (cModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + cModel.errorLogId);
                }
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/SupplierManagement/Supplier/_Input.cshtml", ControllerContext);
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


    


        public async Task<JsonResult> DeleteSupplier(string deletekeys)
        {
            try
            {
                string supplierCode = deletekeys;
                SupplierManagementServices ss = new SupplierManagementServices();
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(supplierCode)));
                ErrorCM er = (await ss.DeleteSupplierDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
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

       


        public async Task<ActionResult> BulkAction(string suppliersCodes, string action)
        {
            try
            {
                SupplierManagementServices bms = new SupplierManagementServices();
                SupplierCCM model = new SupplierCCM();
                model.supplierCodes= suppliersCodes;

                if (action == "Delete Selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.deleteMultipleSuppliersAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Active to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.setAsActiveMultipleSuppliersAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Inactive to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.setAsInactiveMultipleSuppliersAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
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