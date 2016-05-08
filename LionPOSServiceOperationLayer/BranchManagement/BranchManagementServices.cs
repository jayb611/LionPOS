using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LionUtilities.SQLUtilitiesPkg.Models;
using LionUtilities;
using LionPOSDbContracts.DomainModels.Branch;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ControllerContractModel.BranchManagement;
using LionPOSServiceContractModels.DomainContractsModel.Branch;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSDbContracts.DomainModels.User;
using System.Web;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Transactions;
using LionPOSDbContracts.DomainModels.Configuration;
using LionPOSServiceContractModels.DomainContractsModel.Configuration;
using System.Data.Entity;

namespace LionPOSServiceOperationLayer.BranchManagement
{
    public class BranchManagementServices
    {


        public List<FilterFieldsModel> getFieldsToSearch(SessionCM sm, string defaultOrderby = "")
        {
            List<FilterFieldsModel> str = new List<FilterFieldsModel>();
            List<FilterFieldsModel> tmp = new List<FilterFieldsModel>();
            try
            {
                tmp.Add(new FilterFieldsModel("branchCode", SQLDataTypConversionModel.TextSQLType.name, "Branch Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("branchName", SQLDataTypConversionModel.TextSQLType.name, "Branch Name", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("CoveringAreas", SQLDataTypConversionModel.TextSQLType.name, "Covering Area", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("branchType", SQLDataTypConversionModel.TextSQLType.name, "Branch Type", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("openDate", SQLDataTypConversionModel.DateTimeSQLType.name, "Opened Date", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("responsibilityarea", SQLDataTypConversionModel.TextSQLType.name, "Responsibility Area", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactType1", SQLDataTypConversionModel.TextSQLType.name, "Contact Type1", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactNo1", SQLDataTypConversionModel.TextSQLType.name, "Contact Numner 1", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactType2", SQLDataTypConversionModel.TextSQLType.name, "Contact Type2", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactNo2", SQLDataTypConversionModel.TextSQLType.name, "Contact Numner 2", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("email", SQLDataTypConversionModel.TextSQLType.name, "Email", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("city", SQLDataTypConversionModel.TextSQLType.name, "City", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("isActive", SQLDataTypConversionModel.BitSQLType.name, "Is Active", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("entryDate", SQLDataTypConversionModel.DateTimeSQLType.name, "Entry Date", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("entryByUserName", SQLDataTypConversionModel.TextSQLType.name, "Entry By Username", false, false, ""));
                tmp.Add(new FilterFieldsModel("changeByUserName", SQLDataTypConversionModel.TextSQLType.name, "Cahnge by Username", false, false, ""));

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

        public async Task<JsonResult> getBranchesDynamic(FormDataCollection JsonParamString)
        {
            BranchCCM model = new BranchCCM();

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BasicQueryContractModel cm = new JavaScriptSerializer().Deserialize<BasicQueryContractModel>(JsonString);
                List<FilterFieldsModel> filters;
                Task<int> branchlistCount_t1_p1;
                Task<List<branchDCM>> branchlist_t1_p2;
                int totalRecords = 0;
                string loadWhere = "";
                string loadOrderby = "";
                if (cm.LoadAsDefaultFilter == true)
                {

                    string defaultSetting = cm.sessionObj.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.BranchFilterSaveAsDefault.title && a.userName == cm.sessionObj.user.userName && a.userEntryBranchCode == cm.sessionObj.user.userEntryBranchCode && a.userEntryGroupCode == cm.sessionObj.user.userEntryGroupCode).Select(a => a.values).SingleOrDefault();
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


              
                    using (branchlvitsposdbEntities db = new UniversDbContext().branchDbContext(false, false))
                {




                    branchlistCount_t1_p1 = Task.FromResult(
                                                              db.Database.SqlQuery<int>("call getBranchesWithDynamicClausesCountRecord(@showBranchWise,@branchCode,@dynamicWhereClauses)",
                                                              new MySqlParameter("showBranchWise", cm.showBranchWise),
                                                              new MySqlParameter("branchCode", cm.sessionObj.branch.branchCode),
                                                              new MySqlParameter("dynamicWhereClauses", loadWhere)
                                                              ).
                                                              Single()
                                                          );
                    branchlist_t1_p2 = Task.FromResult(
                                                                db.Database.SqlQuery<branchDCM>("call getBranchesWithDynamicClauses(@showBranchWise,@branchCode,@dynamicWhereClauses,@dynamicOrderByFields,@skip,@take)",
                                                              new MySqlParameter("showBranchWise", cm.showBranchWise),
                                                              new MySqlParameter("branchCode", cm.sessionObj.branch.branchCode),
                                                              new MySqlParameter("dynamicWhereClauses", loadWhere),
                                                              new MySqlParameter("dynamicOrderByFields", loadOrderby),
                                                              new MySqlParameter("skip", skip),
                                                              new MySqlParameter("take", cm.recordPerPage)
                                                                ).ToList()
                                                        );
                }

                await Task.WhenAll(branchlistCount_t1_p1, branchlist_t1_p2);
                totalRecords = branchlistCount_t1_p1.Result;
                model.branchList = branchlist_t1_p2.Result;
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
                        user_settings ss = db.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.BranchFilterSaveAsDefault.title).SingleOrDefault();
                        if (ss == null)
                        {
                            ss = new user_settings();
                            ss.title = ConstantRecordsDictionaryCM.Setting_Seeds.BranchFilterSaveAsDefault.title;
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


        public async Task<JsonResult> AddBranchDynamic(FormDataCollection JsonParamString, SessionCM sessionObj)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                branchDCM model = (json_serializer.Deserialize<branchDCM>(JsonString));
                //model.isActive = ((model.closeDate != null) ? false : true);
                //model.isDeleted = ((model.isActive == true) ? false : true);
                model.recordByLion = false;
                model.branchCodeEntryBranchCode = sessionObj.branch.branchCode;
                model.changeByUserName = sessionObj.user.userName;
                model.entryByUserName = sessionObj.user.userName;
                model.entryDate = DateTime.Now;
                model.lastChangeDate = DateTime.Now;
                string bras = json_serializer.Serialize(model);
                branch branch = (json_serializer.Deserialize<branch>(bras));

                using (TransactionScope sc = new TransactionScope())
                {
                    using (branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext(false, false))
                    {
                        bdb.branches.Add(branch);
                        await bdb.SaveChangesAsync();
                        sc.Complete();
                    }
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on Adding New Branch " + System.Reflection.MethodBase.GetCurrentMethod().Name,
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


        public async Task<JsonResult> getBranchByBranchCodeAsync(FormDataCollection JsonParamString)
        {
            getBranchByBranchCodeCCM modelcm = new getBranchByBranchCodeCCM();
            
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string branchCode = json_serializer.Deserialize<string>(JsonString);

                Task<branchDCM> bmodel;
                Task<List<StateDCM>> stateList;
                Task<List<CountryDCM>> countryList;
                using (configurationlvitsposdbEntities db = new UniversDbContext().configurationDbContext(false, false))
                {
                    countryList = db.countries.Select(a => new CountryDCM()
                    {
                        a2 = a.a2,
                        a3 = a.a3,
                        capitols = a.capitols,
                        countryName = a.countryName,
                        currencyCode = a.currencyCode,
                        currencyName = a.currencyName,
                        isoUnm49Code = a.isoUnm49Code,
                        latitude = a.latitude,
                        logitude = a.logitude,
                        phoneCode = a.phoneCode
                    }).ToListAsync();

                    stateList = db.states.Select(a => new StateDCM()
                    {
                        a2 = a.a2,
                        countryA2 = a.countryA2,
                        countryName = a.countryName,
                        stateName = a.stateName
                    }).ToListAsync();
                }

                using (branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext(false, false))
                {
                    bmodel = Task.FromResult((from a in bdb.branches
                                              where a.branchCode == branchCode
                                              select
new branchDCM
{
    branchCode = a.branchCode,
    branchName = a.branchName,
    branchType = a.branchType,
    address = a.address,
    branchCodeEntryBranchCode = a.branchCodeEntryBranchCode,
    changeByUserName = a.changeByUserName,
    city = a.city,
    closeDate = a.closeDate,
    contactNo1 = a.contactNo1,
    contactNo2 = a.contactNo2,
    contactType1 = a.contactType1,
    contactType2 = a.contactType2,
    country = a.country,
    CoveringAreas = a.CoveringAreas,
    deposit = a.deposit,
    description = a.description,
    email = a.email,
    entryByUserName = a.entryByUserName,
    entryDate = a.entryDate,
    experiance = a.experiance,
    investment = a.investment,
    isActive = a.isActive,
    isDeleted = a.isDeleted,
    lastChangeDate = a.lastChangeDate,
    latitudeLoaction = a.logitudeLocation,
    logitudeLocation = a.logitudeLocation,
    recordByLion = a.recordByLion,
    remarks = a.remarks,
    openDate = a.openDate,
    state = a.state,
    staticIpAddress = a.staticIpAddress
}).SingleOrDefault());
                }
                await Task.WhenAll(bmodel,countryList,stateList);
                modelcm.branchDCM = bmodel.Result;
                modelcm.StateList = stateList.Result;
                modelcm.CountryList = countryList.Result;
                return new JsonResult() { Data = modelcm, MaxJsonLength = Int32.MaxValue };
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
                modelcm.errorLogId = logid;
                return new JsonResult() { Data = modelcm, MaxJsonLength = Int32.MaxValue };
            }

        }


        public async Task<JsonResult> UpdateBranchDynamic(FormDataCollection JsonParamString)
        {

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                branchDCM modeld = (json_serializer.Deserialize<branchDCM>(JsonString));
                branch model = (json_serializer.Deserialize<branch>(JsonString));
                using (branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext())
                {
                    //model.isActive = ((model.closeDate != null) ? false : true);
                    //model.isDeleted = ((model.isActive == true) ? false : true);
                    model.changeByUserName = modeld.SessionCM.user.userName;
                    model.lastChangeDate = DateTime.Now;

                    bdb.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    await bdb.SaveChangesAsync();
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


        public async Task<JsonResult> DeleteBranchDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string branchCode = json_serializer.Deserialize<string>(JsonString);
                using (branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext())
                {
                    branch model = new branch();
                    model = (from a in bdb.branches where a.branchCode == branchCode select a).SingleOrDefault();
                    bdb.branches.Remove(model);
                    await bdb.SaveChangesAsync();
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



        public async Task<JsonResult> deleteMultipleBranchesAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BranchCCM model = json_serializer.Deserialize<BranchCCM>(JsonString);
                using (branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext())
                {

                    string[] delarr = model.branchCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var bran = (from b in bdb.branches where b.branchCode == parameter select b).Single();
                        bdb.branches.Remove(bran);
                        await bdb.SaveChangesAsync();
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
        public async Task<JsonResult> setAsActiveMultipleBranchesAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BranchCCM model = json_serializer.Deserialize<BranchCCM>(JsonString);
                using (branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext())
                {

                    string[] delarr = model.branchCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var bran = (from b in bdb.branches where b.branchCode == parameter select b).Single();
                        bran.isActive = true;
                        await bdb.SaveChangesAsync();
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

        public async Task<JsonResult> setAsInactiveMultipleBranchesAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BranchCCM model = json_serializer.Deserialize<BranchCCM>(JsonString);
                using (branchlvitsposdbEntities bdb = new UniversDbContext().branchDbContext())
                {

                    string[] delarr = model.branchCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var bran = (from b in bdb.branches where b.branchCode == parameter select b).Single();
                        bran.isActive = false;
                        await bdb.SaveChangesAsync();
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
