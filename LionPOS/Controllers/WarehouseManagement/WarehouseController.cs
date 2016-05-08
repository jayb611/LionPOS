using System;
using System.Web.Mvc;
using LionStartUp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceOperationLayer.WarehouseManagement;
using LionPOSServiceContractModels.ControllerContractModel.warehouseManagement;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using System.Collections.Generic;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels;
using System.Threading;
using LionPOS.Models.ViewModels.warehouseManagement.Warehouse;
using LionPOSServiceContractModels.DomainContractsModel.warehouse;
using LionPOSServiceContractModels.ErrorContactModel;

namespace LionPOS.Controllers.WarehouseManagement
{
    public class WarehouseController : GlobalBaseController
    {
        // GET: Warehouse
        /// <summary>
        /// HC : 14-Apr-2016
        /// Created For Add,Update,Delete The Warehouse
        /// </summary>
        /// <returns></returns>
        public WarehouseController()
        {
        }
        public async Task<ActionResult> Index(int RecordsPerPage = 0, int PageNumber = 1)
        {
            try
            {
                WarehouseManagementServices wms = new WarehouseManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                cm.FilterFieldAndValues = "";
                cm.OrderByFields = "";
                cm.sessionObj = sessionObj;
                cm.LoadAsDefaultFilter = true;
                cm.SaveAsDefaultFilter = false;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                WarehouseCCM model = (await wms.getWarehousesDynamic(new FormDataCollection(lkvp))).Data as WarehouseCCM;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.controllerName = controllerName;
                model.Pagination.actionName = actionName;
                model.Pagination.FilterActionName = "Filter";
                model.bulkActionName = "BulkAction";
                model.InsertActionName = "CreateWarehouse";
                return View("~/Views/WarehouseManagement/Warehouse/Index.cshtml", model);

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
                WarehouseManagementServices wms = new WarehouseManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                cm.FilterFieldAndValues = filterFieldsAndValues;
                cm.OrderByFields = orderByField;
                cm.sessionObj = sessionObj;
                cm.pageNumber = PageNumber;
                cm.SaveAsDefaultFilter = SaveAsDefaultFilter;
                cm.LoadAsDefaultFilter = LoadAsDefaultFilter;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                WarehouseCCM model = (await wms.getWarehousesDynamic(new FormDataCollection(lkvp))).Data as WarehouseCCM;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.FilterActionName = "Filter";
                model.Pagination.SaveAsDefaultURLLink = "SaveAsDefaultFilter";
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/WarehouseManagement/Warehouse/_ListPaneDetail.cshtml", ControllerContext);
                //Thread.Sleep(5000);
                return new JsonResult() { Data = new { TotalPage = model.Pagination.TotalPages, view = dat } };
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = ex.ToString() };
            }
        }

        public async Task<ActionResult> GetWarehouseDetails(string warehousesCode)
        {
            warehouseCrudOperationViewModel model = new warehouseCrudOperationViewModel(sessionObj);
            try
            {
                WarehouseManagementServices wms = new WarehouseManagementServices();

                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(warehousesCode)));
                warehouseDCM wModel = (await wms.getWarehouseByWarehouseCodeAsync(new FormDataCollection(lkvp))).Data as warehouseDCM;
                model.errorLogId = wModel.errorLogId;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.warehouseDCM = wModel;
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                model.SubmitActionName = "UpdateWarehouse";
                model.DeleteActionName = "DeleteWarehouse";
                model.controllerName = controllerName;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/WarehouseManagement/Warehouse/_InputFields.cshtml", ControllerContext);
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
        public JsonResult CreateWarehouse()
        {
            try
            {
                warehouseCrudOperationViewModel model = new warehouseCrudOperationViewModel(sessionObj);
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.warehouseDCM.warehousesCode= "1";
                model.LoadFormFields = true;
                model.controllerName = controllerName;
                model.SubmitActionName = "CreateWarehouseSubmit";
                model.ControllerContext = ControllerContext;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/WarehouseManagement/Warehouse/_Input.cshtml", ControllerContext);
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
        public async Task<JsonResult> CreateWarehouseSubmit(warehouseCrudOperationViewModel model)
        {
            try
            {
                WarehouseManagementServices wms = new WarehouseManagementServices();
                warehouseDCM DCMmodel = new warehouseDCM();
                DCMmodel = model.warehouseDCM;
                DCMmodel.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(DCMmodel)));
                ErrorCM er = (await wms.AddWarehouseDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (er.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + er.errorLogId);
                }
                model.LoadFormFields = false;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/WarehouseManagement/Warehouse/_Input.cshtml", ControllerContext));
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
        public async Task<JsonResult> UpdateWarehouse(warehouseCrudOperationViewModel model)
        {
            try
            {
                WarehouseManagementServices wms = new WarehouseManagementServices();
                model.warehouseDCM.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model.warehouseDCM)));
                ErrorCM cModel = (await wms.UpdateWarehouseDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (cModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + cModel.errorLogId);
                }
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/WarehouseManagement/Warehouse/_Input.cshtml", ControllerContext);
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

        public async Task<JsonResult> DeleteWarehouse(string deletekeys)
        {
            try
            {
                string warehousesCode = deletekeys;
                WarehouseManagementServices ss = new WarehouseManagementServices();
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(warehousesCode)));
                ErrorCM er = (await ss.DeleteWarehouseDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
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

        public async Task<ActionResult> BulkAction(string warehousesCodes, string action)
        {
            try
            {
                WarehouseManagementServices bms = new WarehouseManagementServices();
                WarehouseCCM model = new WarehouseCCM();
                model.warehousesCodes = warehousesCodes;

                if (action == "Delete Selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.deleteMultipleWarehousesAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Active to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.setAsActiveMultipleWarehousesAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Inactive to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.setAsInactiveMultipleWarehousesAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
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