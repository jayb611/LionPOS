using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LionUtilities.SQLUtilitiesPkg.Models;
using LionUtilities;
using LionPOSDbContracts.DomainModels.Product;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ControllerContractModel.ProductManagement;
using LionPOSServiceContractModels.DomainContractsModel.Product;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSDbContracts.DomainModels.User;
using System.Web;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Transactions;

namespace LionPOSServiceOperationLayer.ProductManagement
{
    public class ProductManagementServices
    {
        public List<FilterFieldsModel> getFieldsToSearch(SessionCM sm, string defaultOrderby = "")
        {
            List<FilterFieldsModel> str = new List<FilterFieldsModel>();
            List<FilterFieldsModel> tmp = new List<FilterFieldsModel>();
            try
            {
                tmp.Add(new FilterFieldsModel("sku", SQLDataTypConversionModel.TextSQLType.name, "Sku", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("title", SQLDataTypConversionModel.TextSQLType.name, "Title", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("productDescription", SQLDataTypConversionModel.TextSQLType.name, "Product Description", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("categoryOfPOS", SQLDataTypConversionModel.TextSQLType.name, "Category Of POS", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("canBeSold", SQLDataTypConversionModel.BitSQLType.name, "Can Be Sold", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("isCombo", SQLDataTypConversionModel.BitSQLType.name, "Is Combo", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("canPurchase", SQLDataTypConversionModel.BitSQLType.name, "Can Purchase", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("isActive", SQLDataTypConversionModel.BitSQLType.name, "Is Active", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("entryDate", SQLDataTypConversionModel.DateTimeSQLType.name, "Entry Date", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("entryByUserName", SQLDataTypConversionModel.TextSQLType.name, "Entry By Username", false, false, ""));
                tmp.Add(new FilterFieldsModel("changeByUserName", SQLDataTypConversionModel.TextSQLType.name, "change by Username", false, false, ""));

                str.AddRange(tmp.Where(a => a.currentSortIndex > -1).OrderBy(a => a.currentSortIndex));
                str.AddRange(tmp.Where(a => a.currentSortIndex < 0).OrderBy(a => a.currentSortIndex));


                return str;
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + logid);
            }
        }

        public async Task<JsonResult> getProductsDynamic(FormDataCollection JsonParamString)
        {
            ProductCCM model = new ProductCCM();

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BasicQueryContractModel cm = new JavaScriptSerializer().Deserialize<BasicQueryContractModel>(JsonString);
                List<FilterFieldsModel> filters;
                Task<int> productlistCount_t1_p1;
                Task<List<productDCM>> productlist_t1_p2;
                int totalRecords = 0;
                string loadWhere = "";
                string loadOrderby = "";
                if (cm.LoadAsDefaultFilter == true)
                {

                    string defaultSetting = cm.sessionObj.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.ProductFilterSaveAsDefault.title && a.userName == cm.sessionObj.user.userName && a.userEntryBranchCode == cm.sessionObj.user.userEntryBranchCode && a.userEntryGroupCode == cm.sessionObj.user.userEntryGroupCode).Select(a => a.values).SingleOrDefault();
                    if (defaultSetting != null)
                    {
                        string[] sset = defaultSetting.Split('|');

                        if (sset.Length == 2)
                        {
                            loadWhere = sset[0];
                            loadOrderby = sset[1];
                        }
                    }
                }

                filters = getFieldsToSearch(cm.sessionObj, loadOrderby);
                if (cm.LoadAsDefaultFilter == false)
                {
                    loadWhere = new SQLUtilities().getFormatedWhereClause(cm.FilterFieldAndValues, filters, "dd/MM/yyyy");
                    loadOrderby = cm.OrderByFields;
                }
                int skip = new SQLUtilities().getSkip(cm.recordPerPage, cm.pageNumber);



                using (productlvitsposdbEntities db = new UniversDbContext().productDbContext(false, false))
                {




                    productlistCount_t1_p1 = Task.FromResult(
                                                              db.Database.SqlQuery<int>("call getProductsWithDynamicClausesCountRecord(@dynamicWhereClauses)",
                                                              new MySqlParameter("dynamicWhereClauses", loadWhere)
                                                              ).
                                                              Single()
                                                          );
                    productlist_t1_p2 = Task.FromResult(
                                                                db.Database.SqlQuery<productDCM>("call getProductsWithDynamicClauses(@dynamicWhereClauses,@dynamicOrderByFields,@skip,@take)",
                                                              new MySqlParameter("dynamicWhereClauses", loadWhere),
                                                              new MySqlParameter("dynamicOrderByFields", loadOrderby),
                                                              new MySqlParameter("skip", skip),
                                                              new MySqlParameter("take", cm.recordPerPage)
                                                                ).ToList()
                                                        );
                }

                await Task.WhenAll(productlistCount_t1_p1, productlist_t1_p2);
                totalRecords = productlistCount_t1_p1.Result;
                model.productList = productlist_t1_p2.Result;
                model.Pagination.PageNumber = cm.pageNumber;
                model.Pagination.RecordsPerPage = cm.recordPerPage;
                model.Pagination.TotalRecords = totalRecords;
                model.Pagination.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(model.Pagination.TotalRecords) / Convert.ToDecimal(cm.recordPerPage))));
                model.FilterFieldModelJson = JsonConvert.SerializeObject(filters);
                model.FilterFieldsModel = filters;

                if (cm.SaveAsDefaultFilter == true)
                {
                    using (userlvitsposdbEntities db = new UniversDbContext().userDbContext(false, false))
                    {
                        user_settings ss = db.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.ProductFilterSaveAsDefault.title).SingleOrDefault();
                        if (ss == null)
                        {
                            ss = new user_settings();
                            ss.title = ConstantRecordsDictionaryCM.Setting_Seeds.ProductFilterSaveAsDefault.title;
                            ss.values = loadWhere + "|" + loadOrderby;
                            ss.userEntryBranchCode = cm.sessionObj.user.userEntryBranchCode;
                            ss.userEntryGroupCode = cm.sessionObj.user.userEntryGroupCode;
                            ss.userName = cm.sessionObj.user.userName;
                            db.user_settings.Add(ss);
                        }
                        List<user_settingsSCM> settingsSCM = db.user_settings.Where(conf => conf.userName == cm.sessionObj.user.userName && conf.userEntryBranchCode == cm.sessionObj.branch.branchCode && conf.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode).Select(a => new user_settingsSCM { userEntryBranchCode = a.userEntryBranchCode, userEntryGroupCode = a.userEntryGroupCode, description = a.description, title = a.title, userName = a.userName, values = a.values }).ToList();
                        cm.sessionObj.user_settings = settingsSCM;
                    }

                    RefreshSessionObject(cm.sessionObj);
                }

                return new JsonResult() { Data = model, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                model.errorLogId = logid;
                return new JsonResult() { Data = model, MaxJsonLength = Int32.MaxValue };
            }
        }

        public void RefreshSessionObject(SessionCM sessionObj)
        {

            using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
            {
                user User = dbuser.users.Where(
                            user => user.userName == sessionObj.user.userName &&
                            user.userEntryBranchCode == sessionObj.branch.branchCode &&
                            user.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode
                            ).SingleOrDefault();
                User.sessionValue = HttpUtility.UrlEncode(JsonConvert.SerializeObject(sessionObj));
                dbuser.SaveChanges();
            }

        }

        public async Task<JsonResult> AddProductDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                productDCM model = (json_serializer.Deserialize<productDCM>(JsonString));
                model.recordByLion = false;
                model.productsEntryBranchCode = model.SessionCM.user.userEntryBranchCode;
                model.productsEntryGroupCode = model.SessionCM.user.userEntryGroupCode;
                model.changeByUserName = model.SessionCM.user.userName;
                model.entryByUserName = model.SessionCM.user.userName;
                model.entryDate = DateTime.Now;
                model.lastChangeDate = DateTime.Now;
                string prod = json_serializer.Serialize(model);
                product product = (json_serializer.Deserialize<product>(prod));

                using (TransactionScope sc = new TransactionScope())
                {
                    using (productlvitsposdbEntities pdb = new UniversDbContext().productDbContext(false, false))
                    {
                        pdb.products.Add(product);
                        await pdb.SaveChangesAsync();
                        sc.Complete();
                    }
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on Adding New Product " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                //model.errorLogId = logid;
                return new JsonResult() { Data = new ErrorCM { errorLogId = logid }, MaxJsonLength = Int32.MaxValue };
            }
        }

        public async Task<JsonResult> getProductBySkuAsync(FormDataCollection JsonParamString)
        {
            productDCM model = new productDCM();
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string sku = json_serializer.Deserialize<string>(JsonString);

                Task<productDCM> pmodel;

                using (productlvitsposdbEntities pdb = new UniversDbContext().productDbContext(false, false))
                {
                    pmodel = Task.FromResult((from a in pdb.products
                                              where a.sku == sku
                                              select
new productDCM
{
    sku = a.sku,
    productsEntryGroupCode = a.productsEntryGroupCode,
    productsEntryBranchCode = a.productsEntryBranchCode,
    taxMasterTitle = a.taxMasterTitle,
    title = a.title,
    barcodeNumber = a.barcodeNumber,
    skuParent = a.skuParent,
    productsEntryBranchCodeParent = a.productsEntryBranchCodeParent,
    productsEntryGroupCodeParent = a.productsEntryGroupCodeParent,
    includeInMenu = a.includeInMenu,
    canBeSold = a.canBeSold,
    isCombo = a.isCombo,
    canPurchase = a.canPurchase,
    makeDuration = a.makeDuration,
    unit_of_mesurement_title = a.unit_of_mesurement_title,
    isCategory = a.isCategory,
    productDescription = a.productDescription,
    stockWarningQty = a.stockWarningQty,
  
    availableOnEcommerce = a.availableOnEcommerce,

    isActive = a.isActive,
    isDeleted = a.isDeleted,
    entryDate = a.entryDate,
    lastChangeDate = a.lastChangeDate,
    recordByLion = a.recordByLion,
    entryByUserName = a.entryByUserName,
    changeByUserName = a.changeByUserName
}).SingleOrDefault());
                }
                await Task.WhenAll(pmodel);
                return new JsonResult() { Data = pmodel.Result, MaxJsonLength = Int32.MaxValue };
            }

            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                model.errorLogId = logid;
                return new JsonResult() { Data = model, MaxJsonLength = Int32.MaxValue };
            }

        }

        public async Task<JsonResult> UpdateProductDynamic(FormDataCollection JsonParamString)
        {

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                productDCM modeld = (json_serializer.Deserialize<productDCM>(JsonString));
                product model = (json_serializer.Deserialize<product>(JsonString));
                using (productlvitsposdbEntities edb = new UniversDbContext().productDbContext())
                {
                    //model.isActive = ((model.closeDate != null) ? false : true);
                    //model.isDeleted = ((model.isActive == true) ? false : true);
                    model.changeByUserName = modeld.SessionCM.user.userName;
                    model.lastChangeDate = DateTime.Now;

                    edb.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    await edb.SaveChangesAsync();
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                //model.errorLogId = logid;
                return new JsonResult() { Data = new ErrorCM { errorLogId = logid }, MaxJsonLength = Int32.MaxValue };
            }

        }

        public async Task<JsonResult> DeleteProductDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string sku = json_serializer.Deserialize<string>(JsonString);
                using (productlvitsposdbEntities pdb = new UniversDbContext().productDbContext())
                {
                    product model = new product();
                    model = (from a in pdb.products where a.sku == sku select a).SingleOrDefault();
                    pdb.products.Remove(model);
                    await pdb.SaveChangesAsync();
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                return new JsonResult() { Data = new ErrorCM { errorLogId = logid }, MaxJsonLength = Int32.MaxValue };
            }
        }

        public async Task<JsonResult> deleteMultipleProductsAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                ProductCCM model = json_serializer.Deserialize<ProductCCM>(JsonString);
                using (productlvitsposdbEntities pdb = new UniversDbContext().productDbContext())
                {

                    string[] delarr = model.skus.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var prod = (from b in pdb.products where b.sku == parameter select b).Single();
                        pdb.products.Remove(prod);
                        await pdb.SaveChangesAsync();
                    }
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                return new JsonResult() { Data = new ErrorCM { errorLogId = logid }, MaxJsonLength = Int32.MaxValue };
            }
        }
        public async Task<JsonResult> setAsActiveMultipleProductsAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                ProductCCM model = json_serializer.Deserialize<ProductCCM>(JsonString);
                using (productlvitsposdbEntities pdb = new UniversDbContext().productDbContext())
                {

                    string[] delarr = model.skus.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var prod = (from b in pdb.products where b.sku == parameter select b).Single();
                        prod.isActive = true;
                        await pdb.SaveChangesAsync();
                    }
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                return new JsonResult() { Data = new ErrorCM { errorLogId = logid }, MaxJsonLength = Int32.MaxValue };
            }
        }

        public async Task<JsonResult> setAsInactiveMultipleProductsAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                ProductCCM model = json_serializer.Deserialize<ProductCCM>(JsonString);
                using (productlvitsposdbEntities pdb = new UniversDbContext().productDbContext())
                {

                    string[] delarr = model.skus.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var prod = (from b in pdb.products where b.sku == parameter select b).Single();
                        prod.isActive = false;
                        await pdb.SaveChangesAsync();
                    }
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                return new JsonResult() { Data = new ErrorCM { errorLogId = logid }, MaxJsonLength = Int32.MaxValue };
            }
        }
    }
}
