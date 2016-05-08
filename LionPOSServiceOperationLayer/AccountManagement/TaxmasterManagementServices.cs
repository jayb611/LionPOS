using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LionUtilities.SQLUtilitiesPkg.Models;
using LionUtilities;
using LionPOSDbContracts.DomainModels.Account;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ControllerContractModel.AccountManagement;
using LionPOSServiceContractModels.DomainContractsModel.Account;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSDbContracts.DomainModels.User;
using System.Web;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Transactions;

namespace LionPOSServiceOperationLayer.AccountManagement
{
    public class TaxmasterManagementServices
    {
        public List<FilterFieldsModel> getFieldsToSearch(SessionCM sm, string defaultOrderby = "")
        {
            List<FilterFieldsModel> str = new List<FilterFieldsModel>();
            List<FilterFieldsModel> tmp = new List<FilterFieldsModel>();
            try
            {
                tmp.Add(new FilterFieldsModel("taxMasterTitle", SQLDataTypConversionModel.TextSQLType.name, "Tax Title", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("entryGroupCode", SQLDataTypConversionModel.TextSQLType.name, "Entry Group Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("expiryDate", SQLDataTypConversionModel.DateTimeSQLType.name, "Expiry Date", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("isProductTax", SQLDataTypConversionModel.BitSQLType.name, "Is Product Tax", true, true, defaultOrderby));
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

        public async Task<JsonResult> getTaxmasterDynamic(FormDataCollection JsonParamString)
        {
            TaxmasterCCM model = new TaxmasterCCM();

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BasicQueryContractModel cm = new JavaScriptSerializer().Deserialize<BasicQueryContractModel>(JsonString);
                List<FilterFieldsModel> filters;
                Task<int> taxmasterlistCount_t1_p1;
                Task<List<taxmasterDCM>> taxmasterlist_t1_p2;
                int totalRecords = 0;
                string loadWhere = "";
                string loadOrderby = "";
                if (cm.LoadAsDefaultFilter == true)
                {
                    string defaultSetting = cm.sessionObj.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.TaxmasterFilterSaveAsDefault.title && a.userName == cm.sessionObj.user.userName && a.userEntryBranchCode == cm.sessionObj.user.userEntryBranchCode && a.userEntryGroupCode == cm.sessionObj.user.userEntryGroupCode).Select(a => a.values).SingleOrDefault();
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

                using (accountinglvitsposdbEntities db = new UniversDbContext().accountDbContext(false, false))
                {
                    taxmasterlistCount_t1_p1 = Task.FromResult(
                                                                db.Database.SqlQuery<int>("call getTaxmasterWithDynamicClausesCountRecord(@dynamicWhereClauses)",
                                                                new MySqlParameter("dynamicWhereClauses", loadWhere)
                                                                ).
                                                                Single()
                                                            );
                    taxmasterlist_t1_p2 = Task.FromResult(
                                                            db.Database.SqlQuery<taxmasterDCM>("call getTaxmasterWithDynamicClauses(@dynamicWhereClauses,@dynamicOrderByFields,@skip,@take)",
                                                             new MySqlParameter("dynamicWhereClauses", loadWhere),
                                                              new MySqlParameter("dynamicOrderByFields", loadOrderby),
                                                              new MySqlParameter("skip", skip),
                                                              new MySqlParameter("take", cm.recordPerPage)
                                                              ).ToList()
                                                        );
                }
                await Task.WhenAll(taxmasterlistCount_t1_p1, taxmasterlist_t1_p2);
                totalRecords = taxmasterlistCount_t1_p1.Result;
                model.taxmasterList = taxmasterlist_t1_p2.Result;
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
                        user_settings ss = db.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.TaxmasterFilterSaveAsDefault.title).SingleOrDefault();
                        if (ss == null)
                        {
                            ss = new user_settings();
                            ss.title = ConstantRecordsDictionaryCM.Setting_Seeds.TaxmasterFilterSaveAsDefault.title;
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

        public async Task<JsonResult> AddTaxmasterDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                taxmasterDCM model = (json_serializer.Deserialize<taxmasterDCM>(JsonString));
                model.recordByLion = false;
                model.taxMasterEntryBranchCode = model.SessionCM.user.userEntryBranchCode;
                model.changeByUserName = model.SessionCM.user.userName;
                model.entryByUserName = model.SessionCM.user.userName;
                model.entryDate = DateTime.Now;
                model.lastChangeDate = DateTime.Now;
                string tax = json_serializer.Serialize(model);
                tax_master taxmaster = (json_serializer.Deserialize<tax_master>(tax));

                using (TransactionScope sc = new TransactionScope())
                {
                    using (accountinglvitsposdbEntities adb = new UniversDbContext().accountDbContext(false, false))
                    {
                        adb.tax_master.Add(taxmaster);
                        await adb.SaveChangesAsync();
                        sc.Complete();
                    }
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on Adding New Taxmaster " + System.Reflection.MethodBase.GetCurrentMethod().Name,
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

        public async Task<JsonResult> getTaxmasterByTaxMasterTitleAsync(FormDataCollection JsonParamString)
        {
            taxmasterDCM model = new taxmasterDCM();
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string taxMasterTitle = json_serializer.Deserialize<string>(JsonString);

                Task<taxmasterDCM> tmodel;

                using (accountinglvitsposdbEntities adb = new UniversDbContext().accountDbContext(false, false))
                {
                    tmodel = Task.FromResult((from a in adb.tax_master
                                              where a.taxMasterTitle == taxMasterTitle
                                              select
                                              new taxmasterDCM
                                              {
                                                  taxMasterEntryBranchCode = a.taxMasterEntryBranchCode,
                                                  taxMasterTitle = a.taxMasterTitle,
                                                  entryGroupCode = a.entryGroupCode,
                                             
                                                  isActive = a.isActive,
                                                  isDeleted = a.isDeleted,
                                                  entryDate = a.entryDate,
                                                  lastChangeDate = a.lastChangeDate,
                                                  recordByLion = a.recordByLion,
                                                  entryByUserName = a.entryByUserName,
                                                  changeByUserName = a.changeByUserName

                                              }).SingleOrDefault());
                }
                await Task.WhenAll(tmodel);
                return new JsonResult() { Data = tmodel.Result, MaxJsonLength = Int32.MaxValue };
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

        public async Task<JsonResult> UpdateTaxmasterDynamic(FormDataCollection JsonParamString)
        {

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                taxmasterDCM modeld = (json_serializer.Deserialize<taxmasterDCM>(JsonString));
                tax_master model = (json_serializer.Deserialize<tax_master>(JsonString));
                using (accountinglvitsposdbEntities adb = new UniversDbContext().accountDbContext())
                {
                    //model.isActive = ((model.closeDate != null) ? false : true);
                    //model.isDeleted = ((model.isActive == true) ? false : true);
                    model.changeByUserName = modeld.SessionCM.user.userName;
                    model.lastChangeDate = DateTime.Now;

                    adb.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    await adb.SaveChangesAsync();
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

        public async Task<JsonResult> DeleteTaxmasterDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string taxMasterTitle = json_serializer.Deserialize<string>(JsonString);
                using (accountinglvitsposdbEntities adb = new UniversDbContext().accountDbContext())
                {
                    tax_master model = new tax_master();
                    model = (from a in adb.tax_master where a.taxMasterTitle == taxMasterTitle select a).SingleOrDefault();
                    adb.tax_master.Remove(model);
                    await adb.SaveChangesAsync();
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

        public async Task<JsonResult> deleteMultipleTaxmasterAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                TaxmasterCCM model = json_serializer.Deserialize<TaxmasterCCM>(JsonString);
                using (accountinglvitsposdbEntities adb = new UniversDbContext().accountDbContext())
                {

                    string[] delarr = model.taxMasterTitles.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var tax = (from b in adb.tax_master where b.taxMasterTitle == parameter select b).Single();
                        adb.tax_master.Remove(tax);
                        await adb.SaveChangesAsync();
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

        public async Task<JsonResult> setAsActiveMultipleTaxmasterAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                TaxmasterCCM model = json_serializer.Deserialize<TaxmasterCCM>(JsonString);
                using (accountinglvitsposdbEntities adb = new UniversDbContext().accountDbContext())
                {

                    string[] delarr = model.taxMasterTitles.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var tax = (from b in adb.tax_master where b.taxMasterTitle == parameter select b).Single();
                        tax.isActive = true;
                        await adb.SaveChangesAsync();
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

        public async Task<JsonResult> setAsInactiveMultipleTaxmasterAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                TaxmasterCCM model = json_serializer.Deserialize<TaxmasterCCM>(JsonString);
                using (accountinglvitsposdbEntities adb = new UniversDbContext().accountDbContext())
                {

                    string[] delarr = model.taxMasterTitles.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var tax = (from b in adb.tax_master where b.taxMasterTitle == parameter select b).Single();
                        tax.isActive = false;
                        await adb.SaveChangesAsync();
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
