using DomainModels;
using LionPOSDbContracts.DomainModels.User;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.ControllerContractModel.UserManagement;
using LionPOSServiceContractModels.DomainContractsModel.User;
using LionPOSServiceContractModels.ErrorContactModel;
using LionPOSServiceOperationLayer.Maintenance;
using LionUtilities;
using LionUtilities.SQLUtilitiesPkg.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LionPOSServiceOperationLayer.UserManagement
{
    public class UserManagementServices
    {

        #region Get_User
        public List<FilterFieldsModel> getFieldsToSearch(SessionCM sm, string defaultOrderby = "")
        {
            List<FilterFieldsModel> str = new List<FilterFieldsModel>();
            List<FilterFieldsModel> tmp = new List<FilterFieldsModel>();
            try
            {
                tmp.Add(new FilterFieldsModel("userName", SQLDataTypConversionModel.TextSQLType.name, "User Name", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("employeeCode", SQLDataTypConversionModel.TextSQLType.name, "Employee Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("branchCode", SQLDataTypConversionModel.TextSQLType.name, "Branch Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("userStatus", SQLDataTypConversionModel.TextSQLType.name, "User Status", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("lastLogin", SQLDataTypConversionModel.DateTimeSQLType.name, "Last Login", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("employeeEntryBranchCode", SQLDataTypConversionModel.TextSQLType.name, "Employee Entry Branch Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("userEntryBranchCode", SQLDataTypConversionModel.TextSQLType.name, "User Entry Branch Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("userAccountStatus", SQLDataTypConversionModel.TextSQLType.name, "User Account Status", true, true, defaultOrderby));
                //tmp.Add(new FilterFieldsModel("contactType2", SQLDataTypConversionModel.TextSQLType.name, "Contact Type2", true, true, defaultOrderby));
                //tmp.Add(new FilterFieldsModel("contactNo2", SQLDataTypConversionModel.TextSQLType.name, "Contact Numner 2", true, true, defaultOrderby));
                //tmp.Add(new FilterFieldsModel("email", SQLDataTypConversionModel.TextSQLType.name, "Email", true, true, defaultOrderby));
                //tmp.Add(new FilterFieldsModel("city", SQLDataTypConversionModel.TextSQLType.name, "City", true, true, defaultOrderby));
                //tmp.Add(new FilterFieldsModel("isActive", SQLDataTypConversionModel.BitSQLType.name, "Is Active", true, true, defaultOrderby));
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

        public async Task<JsonResult> getUsersDynamic(FormDataCollection JsonParamString)
        {
            UserCCM model = new UserCCM();

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BasicQueryContractModel cm = new JavaScriptSerializer().Deserialize<BasicQueryContractModel>(JsonString);
                List<FilterFieldsModel> filters;
                Task<int> userListCount_t1_p1;
                Task<List<UserDCM>> userList_t1_p2;
                int totalRecords = 0;
                string loadWhere = "";
                string loadOrderby = "";
                if (cm.LoadAsDefaultFilter == true)
                {

                    string defaultSetting = cm.sessionObj.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.UserFilterSaveAsDefault.title && a.userName == cm.sessionObj.user.userName && a.userEntryBranchCode == cm.sessionObj.user.userEntryBranchCode && a.userEntryGroupCode == cm.sessionObj.user.userEntryGroupCode).Select(a => a.values).SingleOrDefault();
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



                using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext(false, false))
                {




                    userListCount_t1_p1 = Task.FromResult(
                                                              udb.Database.SqlQuery<int>("call getUsersWithDynamicClausesCountRecord(@showBranchWise,@branchCode,@dynamicWhereClauses)",
                                                              new MySqlParameter("showBranchWise", cm.showBranchWise),
                                                              new MySqlParameter("branchCode", cm.sessionObj.branch.branchCode),
                                                              new MySqlParameter("dynamicWhereClauses", loadWhere)
                                                              ).
                                                              Single()
                                                          );
                    userList_t1_p2 = Task.FromResult(
                                                                udb.Database.SqlQuery<UserDCM>("call getUsersWithDynamicClauses(@showBranchWise,@branchCode,@dynamicWhereClauses,@dynamicOrderByFields,@skip,@take)",
                                                              new MySqlParameter("showBranchWise", cm.showBranchWise),
                                                              new MySqlParameter("branchCode", cm.sessionObj.branch.branchCode),
                                                              new MySqlParameter("dynamicWhereClauses", loadWhere),
                                                              new MySqlParameter("dynamicOrderByFields", loadOrderby),
                                                              new MySqlParameter("skip", skip),
                                                              new MySqlParameter("take", cm.recordPerPage)
                                                                ).ToList()
                                                        );
                }

                await Task.WhenAll(userListCount_t1_p1, userList_t1_p2);
                totalRecords = userListCount_t1_p1.Result;
                model.userList = userList_t1_p2.Result;
                model.Pagination.PageNumber = cm.pageNumber;
                model.Pagination.RecordsPerPage = cm.recordPerPage;
                model.Pagination.TotalRecords = totalRecords;
                model.Pagination.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Convert.ToDecimal(model.Pagination.TotalRecords) / Convert.ToDecimal(cm.recordPerPage))));
                model.FilterFieldModelJson = JsonConvert.SerializeObject(filters);
                model.FilterFieldsModel = filters;

                if (cm.SaveAsDefaultFilter == true)
                {
                    using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext(false, false))
                    {
                        user_settings ss = udb.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.BranchFilterSaveAsDefault.title).SingleOrDefault();
                        if (ss == null)
                        {
                            ss = new user_settings();
                            ss.title = ConstantRecordsDictionaryCM.Setting_Seeds.BranchFilterSaveAsDefault.title;
                            ss.values = loadWhere + "|" + loadOrderby;
                            ss.userEntryBranchCode = cm.sessionObj.user.userEntryBranchCode;
                            ss.userEntryGroupCode = cm.sessionObj.user.userEntryGroupCode;
                            ss.userName = cm.sessionObj.user.userName;
                            udb.user_settings.Add(ss);
                        }
                        List<user_settingsSCM> settingsSCM = udb.user_settings.Where(conf => conf.userName == cm.sessionObj.user.userName && conf.userEntryBranchCode == cm.sessionObj.branch.branchCode && conf.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode).Select(a => new user_settingsSCM { userEntryBranchCode = a.userEntryBranchCode, userEntryGroupCode = a.userEntryGroupCode, description = a.description, title = a.title, userName = a.userName, values = a.values }).ToList();
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

        public async Task<JsonResult> getUserByUserNameAsync(FormDataCollection JsonParamString)
        {
            UserDCM model = new UserDCM();
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string userName = json_serializer.Deserialize<string>(JsonString);

                Task<UserDCM> bmodel;

                using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext(false, false))
                {
                    bmodel = Task.FromResult((from a in udb.users
                                              where a.userName == userName
                                              select
new UserDCM
{
    accessRoleTitle = a.accessRoleTitle,
    userName = a.userName,
    userEntryGroupCode = a.userEntryGroupCode,
    userEntryBranchCode = a.userEntryBranchCode,
    employeeEntryBranchCode = a.employeeEntryBranchCode,
    employeeCode = a.employeeCode,
    employeeEntryGroupCode = a.employeeEntryGroupCode,
    branchCode = a.branchCode,
    userStatus = a.userStatus,
    userAccountStatus = a.userAccountStatus,
    passwordEncryptionKey = a.passwordEncryptionKey,
    password = a.password,
    lastLogin = a.lastLogin,
    lastPasswordResetDate = a.lastPasswordResetDate,
    blockExpiry = a.blockExpiry,
    warnOnFailedLoginAfterAtttempt = a.warnOnFailedLoginAfterAtttempt,
    isLion = a.isLion,
    isActive = a.isActive,
    isDeleted = a.isDeleted,
    entryDate = a.entryDate,
    lastChangeDate = a.lastChangeDate,
    recordByLion = a.recordByLion,
    entryByUserName = a.entryByUserName,
    changeByUserName = a.changeByUserName,
    sessionID = a.sessionID,
    sessionValue = a.sessionValue,
    sessionExpireyDateTime = a.sessionExpireyDateTime,
    sessionCreateTime = a.sessionCreateTime,
}).SingleOrDefault());
                }
                await Task.WhenAll(bmodel);
                return new JsonResult() { Data = bmodel.Result, MaxJsonLength = Int32.MaxValue };
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

        #endregion


        #region Create
        public async Task<JsonResult> AddUserDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                UserDCM model = (json_serializer.Deserialize<UserDCM>(JsonString));


                model.userEntryBranchCode = model.SessionCM.user.userEntryBranchCode;
                model.userEntryGroupCode = model.SessionCM.user.userEntryGroupCode;
                model.employeeEntryBranchCode = model.SessionCM.employee.employeeEntryBranchCode;
                model.employeeEntryGroupCode = model.SessionCM.employee.employeeEntryGroupCode;
                model.isLion = false;
                model.lastLogin = model.SessionCM.user.lastLogin;
                model.isDeleted = false;
                model.entryDate = DateTime.Now;
                model.lastChangeDate = DateTime.Now;
                model.recordByLion = false;
                model.entryByUserName = model.SessionCM.user.userName;
                model.changeByUserName = model.SessionCM.user.userName;
                model.sessionID = model.SessionCM.user.sessionID;
                model.sessionValue = model.SessionCM.user.sessionValue;
                model.sessionExpireyDateTime = model.SessionCM.user.sessionExpireyDateTime;
                model.sessionCreateTime = model.SessionCM.user.sessionCreateTime;

                string bras = json_serializer.Serialize(model);
                user usr = (json_serializer.Deserialize<user>(bras));

                using (TransactionScope sc = new TransactionScope())
                {
                    using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext(false, false))
                    {
                        udb.users.Add(usr);
                        await udb.SaveChangesAsync();
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

        #endregion


        #region Update
        public async Task<JsonResult> UpdateUserDynamic(FormDataCollection JsonParamString)
        {

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                UserDCM modeld = (json_serializer.Deserialize<UserDCM>(JsonString));
                user model = (json_serializer.Deserialize<user>(JsonString));
                using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext())
                {
                    //model.isActive = ((model.closeDate != null) ? false : true);
                    //model.isDeleted = ((model.isActive == true) ? false : true);
                    model.changeByUserName = modeld.SessionCM.user.userName;
                    model.lastChangeDate = DateTime.Now;

                    udb.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    await udb.SaveChangesAsync();
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
        #endregion

        #region Delete
        public async Task<JsonResult> DeleteUserDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string userName = json_serializer.Deserialize<string>(JsonString);
                using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext())
                {
                    user model = new user();
                    model = (from a in udb.users where a.userName == userName select a).SingleOrDefault();
                    udb.users.Remove(model);
                    await udb.SaveChangesAsync();
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
        #endregion

        #region Bulk_Action
        public async Task<JsonResult> deleteMultipleUsersAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                UserCCM model = json_serializer.Deserialize<UserCCM>(JsonString);
                using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext())
                {

                    string[] delarr = model.userNames.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var bran = (from b in udb.users where b.userName == parameter select b).Single();
                        udb.users.Remove(bran);
                        await udb.SaveChangesAsync();
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
        public async Task<JsonResult> setAsActiveMultipleUsersAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                UserCCM model = json_serializer.Deserialize<UserCCM>(JsonString);
                using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext())
                {

                    string[] delarr = model.userNames.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var bran = (from b in udb.users where b.userName == parameter select b).Single();
                        bran.isActive = true;
                        await udb.SaveChangesAsync();
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
                UserCCM model = json_serializer.Deserialize<UserCCM>(JsonString);
                using (userlvitsposdbEntities udb = new UniversDbContext().userDbContext())
                {

                    string[] delarr = model.userNames.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var bran = (from b in udb.users where b.userName == parameter select b).Single();
                        bran.isActive = false;
                        await udb.SaveChangesAsync();
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

        #endregion

    }
}
