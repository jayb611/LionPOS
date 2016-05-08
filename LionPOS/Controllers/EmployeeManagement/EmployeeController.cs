using System;
using System.Web.Mvc;
using LionStartUp;
using UtilitiesForAll;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceOperationLayer.EmployeeManagement;
using LionPOSServiceContractModels.ControllerContractModel.EmployeeManagement;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using System.Collections.Generic;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels;
using LionPOS.Models.ViewModels.EmployeeManagement.Employee;
using LionPOSServiceContractModels.DomainContractsModel.Employee;
using LionPOSServiceContractModels.ErrorContactModel;

namespace LionPOS.Controllers.EmployeeManagement
{
    public class EmployeeController : GlobalBaseController
    {
        // GET: Employee
        /// <summary>
        /// HC : 09-Apr-2016
        /// Created For Add,Update,Delete The Employee
        /// </summary>
        /// <returns></returns>
        public EmployeeController()
        { }

        public async Task<ActionResult> Index(int RecordsPerPage = 0, int PageNumber = 1)
        {
            try
            {
                EmployeeManagementServices ems = new EmployeeManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                cm.FilterFieldAndValues = "";
                cm.FilterFieldAndValues = "";
                cm.OrderByFields = "";
                cm.sessionObj = sessionObj;
                cm.LoadAsDefaultFilter = true;
                cm.SaveAsDefaultFilter = false;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                EmployeeCCM model = (await ems.getEmployeeDynamic(new FormDataCollection(lkvp))).Data as EmployeeCCM;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.controllerName = controllerName;
                model.Pagination.actionName = actionName;
                model.Pagination.FilterActionName = "Filter";
                model.InsertActionName = "CreateEmployee";
                model.bulkActionName = "BulkAction";
                return View("~/Views/EmployeeManagement/Employee/Index.cshtml", model);
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
                FormatHelpers rf = new FormatHelpers();
                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Filter(string filterFieldsAndValues, string orderByField, bool SaveAsDefaultFilter, bool LoadAsDefaultFilter, int PageNumber = 1)
        {
            try
            {
                EmployeeManagementServices ems = new EmployeeManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                cm.FilterFieldAndValues = filterFieldsAndValues;
                cm.OrderByFields = orderByField;
                cm.sessionObj = sessionObj;
                cm.pageNumber = PageNumber;
                cm.SaveAsDefaultFilter = SaveAsDefaultFilter;
                cm.LoadAsDefaultFilter = LoadAsDefaultFilter;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                EmployeeCCM model = (await ems.getEmployeeDynamic(new FormDataCollection(lkvp))).Data as EmployeeCCM;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.FilterActionName = "Filter";
                model.Pagination.SaveAsDefaultURLLink = "SaveAsDefaultFilter";
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/EmployeeManagement/Employee/_ListPaneDetail.cshtml", ControllerContext);
                //Thread.Sleep(5000);
                return new JsonResult() { Data = new { TotalPage = model.Pagination.TotalPages, view = dat } };
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = ex.ToString() };
            }
        }

        public async Task<ActionResult> GetEmployeeDetails(string employeeCode)
        {
            employeeCrudOperationViewModel model = new employeeCrudOperationViewModel(sessionObj);
            try
            {
                EmployeeManagementServices ems = new EmployeeManagementServices();

                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(employeeCode)));
                employeeDCM eModel = (await ems.getEmployeeByEmployeeCodeAsync(new FormDataCollection(lkvp))).Data as employeeDCM;
                model.errorLogId = eModel.errorLogId;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.employeeDCM = eModel;
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                model.SubmitActionName = "UpdateEmployee";
                model.DeleteActionName = "DeleteEmployee";
                model.controllerName = controllerName;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/EmployeeManagement/Employee/_InputFields.cshtml", ControllerContext);
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
        public JsonResult CreateEmployee()
        {
            try
            {
                var dateAndTime = DateTime.Now;
                employeeCrudOperationViewModel model = new employeeCrudOperationViewModel(sessionObj);
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.employeeDCM.employeeCode = "Employee-" +dateAndTime.ToShortDateString();
                model.LoadFormFields = true;
                model.controllerName = controllerName;
                model.SubmitActionName = "CreateEmployeeSubmit";
                model.ControllerContext = ControllerContext;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/EmployeeManagement/Employee/_Input.cshtml", ControllerContext);
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
        public async Task<JsonResult> CreateEmployeeSubmit(employeeCrudOperationViewModel model)
        {
            try
            {
                EmployeeManagementServices ems = new EmployeeManagementServices();
                employeeDCM DCMmodel = new employeeDCM();
                DCMmodel = model.employeeDCM;
                DCMmodel.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(DCMmodel)));
                ErrorCM er = (await ems.AddEmployeeDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (er.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + er.errorLogId);
                }
                model.LoadFormFields = false;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/EmployeeManagement/Employee/_Input.cshtml", ControllerContext));
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
        public async Task<JsonResult> UpdateEmployee(employeeCrudOperationViewModel model)
        {
            try
            {
                EmployeeManagementServices ems = new EmployeeManagementServices();
                model.employeeDCM.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model.employeeDCM)));
                ErrorCM cModel = (await ems.UpdateEmployeeDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (cModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + cModel.errorLogId);
                }
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/EmployeeManagement/Employee/_Input.cshtml", ControllerContext);
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

        public async Task<JsonResult> DeleteEmployee(string deletekeys)
        {
            try
            {
                string employeeCode = deletekeys;
                EmployeeManagementServices ems = new EmployeeManagementServices();
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(employeeCode)));
                ErrorCM er = (await ems.DeleteEmployeeDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
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

        public async Task<ActionResult> BulkAction(string employeeCodes, string action)
        {
            try
            {
                EmployeeManagementServices ems = new EmployeeManagementServices();
                EmployeeCCM model = new EmployeeCCM();
                model.employeeCodes = employeeCodes;

                if (action == "Delete Selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await ems.deleteMultipleEmployeeAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Active to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await ems.setAsActiveMultipleEmployeeAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Inactive to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await ems.setAsInactiveMultipleEmployeeAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
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