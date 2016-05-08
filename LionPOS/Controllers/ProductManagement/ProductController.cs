using System;
using System.Web.Mvc;
using LionStartUp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceOperationLayer.ProductManagement;
using LionPOSServiceContractModels.ControllerContractModel.ProductManagement;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using System.Collections.Generic;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels;
using System.Threading;
using LionPOS.Models.ViewModels.ProductManagement.Product;
using LionPOSServiceContractModels.DomainContractsModel.Product;
using LionPOSServiceContractModels.ErrorContactModel;

namespace LionPOS.Controllers.ProductManagement
{
    public class ProductController : GlobalBaseController
    {
        // GET: Product
        /// <summary>
        /// HC : 13-Apr-2016
        /// Created For Add,Update,Delete The Product
        /// </summary>
        /// <returns></returns>
        public ProductController()
        {
        }

        public async Task<ActionResult> Index(int RecordsPerPage = 0, int PageNumber = 1)
        {
            try
            {
                ProductManagementServices ss = new ProductManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                cm.FilterFieldAndValues = "";
                cm.OrderByFields = "";
                cm.sessionObj = sessionObj;
                cm.LoadAsDefaultFilter = true;
                cm.SaveAsDefaultFilter = false;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                ProductCCM model = (await ss.getProductsDynamic(new FormDataCollection(lkvp))).Data as ProductCCM;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.controllerName = controllerName;
                model.Pagination.actionName = actionName;
                model.Pagination.FilterActionName = "Filter";
                model.bulkActionName = "BulkAction";
                model.InsertActionName = "CreateProduct";
                return View("~/Views/ProductManagement/Product/Index.cshtml", model);

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
                ProductManagementServices ss = new ProductManagementServices();
                BasicQueryContractModel cm = new BasicQueryContractModel();
                cm.FilterFieldAndValues = filterFieldsAndValues;
                cm.OrderByFields = orderByField;
                cm.sessionObj = sessionObj;
                cm.pageNumber = PageNumber;
                cm.SaveAsDefaultFilter = SaveAsDefaultFilter;
                cm.LoadAsDefaultFilter = LoadAsDefaultFilter;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                ProductCCM model = (await ss.getProductsDynamic(new FormDataCollection(lkvp))).Data as ProductCCM;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.Pagination.FilterActionName = "Filter";
                model.Pagination.SaveAsDefaultURLLink = "SaveAsDefaultFilter";
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/ProductManagement/Product/_ListPaneDetail.cshtml", ControllerContext);
                //Thread.Sleep(5000);
                return new JsonResult() { Data = new { TotalPage = model.Pagination.TotalPages, view = dat } };
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = ex.ToString() };
            }
        }

        public async Task<ActionResult> GetProductDetails(string sku)
        {
            productCrudOperationViewModel model = new productCrudOperationViewModel(sessionObj);
            try
            {
                ProductManagementServices pms = new ProductManagementServices();

                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(sku)));
                productDCM pModel = (await pms.getProductBySkuAsync(new FormDataCollection(lkvp))).Data as productDCM;
                model.errorLogId = pModel.errorLogId;
                if (model.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + model.errorLogId);
                }
                model.productDCM = pModel;
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Update;
                model.SubmitActionName = "UpdateProduct";
                model.DeleteActionName = "DeleteProduct";
                model.controllerName = controllerName;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/ProductManagement/Product/_InputFields.cshtml", ControllerContext);
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
        public JsonResult CreateProduct()
        {

            try
            {

                productCrudOperationViewModel model = new productCrudOperationViewModel(sessionObj);
                model.crudOprationType = ConstantDictionaryCM.crudOprationTypes.Insert;
                model.productDCM.sku= "-1";
                model.LoadFormFields = true;
                model.controllerName = controllerName;
                model.SubmitActionName = "CreateProductSubmit";
                model.ControllerContext = ControllerContext;
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/ProductManagement/Product/_Input.cshtml", ControllerContext);
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
        public async Task<JsonResult> CreateProductSubmit(productCrudOperationViewModel model)
        {
            try
            {
                ProductManagementServices pms = new ProductManagementServices();
                productDCM DCMmodel = new productDCM();
                DCMmodel = model.productDCM;
                DCMmodel.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(DCMmodel)));
                ErrorCM er = (await pms.AddProductDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (er.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + er.errorLogId);
                }
                model.LoadFormFields = false;
                string dat = await Task.FromResult(LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/ProductManagement/Product/_Input.cshtml", ControllerContext));
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
        public async Task<JsonResult> UpdateProduct(productCrudOperationViewModel model)
        {
            try
            {
                ProductManagementServices pms = new ProductManagementServices();
                model.productDCM.SessionCM = sessionObj;
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model.productDCM)));
                ErrorCM cModel = (await pms.UpdateProductDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
                if (cModel.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + cModel.errorLogId);
                }
                string dat = LionUtilities.MVCViewUtilitiesPkg.Helper.GetRazorViewAsString(model, "~/Views/ProductManagement/Product/_Input.cshtml", ControllerContext);
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

        public async Task<JsonResult> DeleteProduct(string deletekeys)
        {
            try
            {
                string sku = deletekeys;
                ProductManagementServices pms = new ProductManagementServices();
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(sku)));
                ErrorCM er = (await pms.DeleteProductDynamic(new FormDataCollection(lkvp))).Data as ErrorCM;
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

        public async Task<ActionResult> BulkAction(string skus, string action)
        {
            try
            {
                ProductManagementServices bms = new ProductManagementServices();
                ProductCCM model = new ProductCCM();
                model.skus = skus;

                if (action == "Delete Selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.deleteMultipleProductsAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Active to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.setAsActiveMultipleProductsAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                    if (ex.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ex.errorLogId);
                    }
                }
                else if (action == "Set Inactive to selected")
                {
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(model)));
                    ErrorCM ex = (await bms.setAsInactiveMultipleProductsAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
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