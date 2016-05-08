using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LionUtilities.SQLUtilitiesPkg.Models;
using LionUtilities;
using LionPOSDbContracts.DomainModels.Employee;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ControllerContractModel.EmployeeManagement;
using LionPOSServiceContractModels.DomainContractsModel.Employee;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSDbContracts.DomainModels.Configuration;
using LionPOSDbContracts.DomainModels.User;
using System.Web;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Transactions;

namespace LionPOSServiceOperationLayer.EmployeeManagement
{
    public class EmployeeManagementServices
    {

        public List<FilterFieldsModel> getFieldsToSearch(SessionCM sm, string defaultOrderby = "")
        {
            List<FilterFieldsModel> str = new List<FilterFieldsModel>();
            List<FilterFieldsModel> tmp = new List<FilterFieldsModel>();
            try
            {
                tmp.Add(new FilterFieldsModel("employeeCode", SQLDataTypConversionModel.TextSQLType.name, "Employee Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("title", SQLDataTypConversionModel.TextSQLType.name, "Title", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("firstName", SQLDataTypConversionModel.TextSQLType.name, "First Name", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("middleName", SQLDataTypConversionModel.TextSQLType.name, "Middle Name", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("sureName", SQLDataTypConversionModel.TextSQLType.name, "Sure Name", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("joiningdate", SQLDataTypConversionModel.DateTimeSQLType.name, "Joining Date", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactType1", SQLDataTypConversionModel.TextSQLType.name, "Contact Type1", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactNo1", SQLDataTypConversionModel.TextSQLType.name, "Contact Numner 1", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactType2", SQLDataTypConversionModel.TextSQLType.name, "Contact Type2", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactNo2", SQLDataTypConversionModel.TextSQLType.name, "Contact Numner 2", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("emialAddress", SQLDataTypConversionModel.TextSQLType.name, "Email Address", true, true, defaultOrderby));
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

        public async Task<JsonResult> getEmployeeDynamic(FormDataCollection JsonParamString)
        {
            EmployeeCCM model = new EmployeeCCM();

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BasicQueryContractModel cm = new JavaScriptSerializer().Deserialize<BasicQueryContractModel>(JsonString);
                List<FilterFieldsModel> filters;
                Task<int> employeelistCount_t1_p1;
                Task<List<employeeDCM>> employeelist_t1_p2;
                int totalRecords = 0;
                string loadWhere = "";
                string loadOrderby = "";
                if (cm.LoadAsDefaultFilter == true)
                {
                    string defaultSetting = cm.sessionObj.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.EmployeeFilterSaveAsDefault.title && a.userName == cm.sessionObj.user.userName && a.userEntryBranchCode == cm.sessionObj.user.userEntryBranchCode && a.userEntryGroupCode == cm.sessionObj.user.userEntryGroupCode).Select(a => a.values).SingleOrDefault();
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

                using (employeelvitsposdbEntities db = new UniversDbContext().employeeDbContext(false, false))
                {
                    employeelistCount_t1_p1 = Task.FromResult(
                                                                db.Database.SqlQuery<int>("call getEmployeeWithDynamicClausesCountRecord(@dynamicWhereClauses)",
                                                                new MySqlParameter("dynamicWhereClauses", loadWhere)
                                                                ).
                                                                Single()
                                                            );
                    employeelist_t1_p2 = Task.FromResult(
                                                            db.Database.SqlQuery<employeeDCM>("call getEmployeeWithDynamicClauses(@dynamicWhereClauses,@dynamicOrderByFields,@skip,@take)",
                                                             new MySqlParameter("dynamicWhereClauses", loadWhere),
                                                              new MySqlParameter("dynamicOrderByFields", loadOrderby),
                                                              new MySqlParameter("skip", skip),
                                                              new MySqlParameter("take", cm.recordPerPage)
                                                              ).ToList()
                                                        );
                }
                await Task.WhenAll(employeelistCount_t1_p1, employeelist_t1_p2);
                totalRecords = employeelistCount_t1_p1.Result;
                model.employeeList = employeelist_t1_p2.Result;
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
                        user_settings ss = db.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.EmployeeFilterSaveAsDefault.title).SingleOrDefault();
                        if (ss == null)
                        {
                            ss = new user_settings();
                            ss.title = ConstantRecordsDictionaryCM.Setting_Seeds.EmployeeFilterSaveAsDefault.title;
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

        public async Task<JsonResult> AddEmployeeDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                employeeDCM model = (json_serializer.Deserialize<employeeDCM>(JsonString));
                model.recordByLion = false;
                model.employeeEntryBranchCode = model.SessionCM.user.userEntryBranchCode;
                model.employeeEntryGroupCode = model.SessionCM.user.userEntryGroupCode;
                model.changeByUserName = model.SessionCM.user.userName;
                model.entryByUserName = model.SessionCM.user.userName;
                model.entryDate = DateTime.Now;
                model.lastChangeDate = DateTime.Now;
                string emp = json_serializer.Serialize(model);
                employee employee = (json_serializer.Deserialize<employee>(emp));

                using (TransactionScope sc = new TransactionScope())
                {
                    using (employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext(false, false))
                    {
                        edb.employees.Add(employee);
                        await edb.SaveChangesAsync();
                        sc.Complete();
                    }
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on Adding New Employee " + System.Reflection.MethodBase.GetCurrentMethod().Name,
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

        public async Task<JsonResult> getEmployeeByEmployeeCodeAsync(FormDataCollection JsonParamString)
        {
            employeeDCM model = new employeeDCM();
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string employeeCode = json_serializer.Deserialize<string>(JsonString);

                Task<employeeDCM> emodel;

                using (employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext(false, false))
                {
                    emodel = Task.FromResult((from a in edb.employees
                                              where a.employeeCode == employeeCode
                                              select
                                              new employeeDCM
                                              {
                                                  employeeEntryBranchCode = a.employeeEntryBranchCode,
                                                  employeeCode = a.employeeCode,
                                                  employeeEntryGroupCode = a.employeeEntryGroupCode,
                                                  contactNo1 = a.contactNo1,
                                                  contactType1 = a.contactType1,
                                                  contactNo2 = a.contactNo2,
                                                  contactType2 = a.contactType2,
                                                  title = a.title,
                                                  firstName = a.firstName,
                                                  middleName = a.middleName,
                                                  sureName = a.sureName,
                                                  profilePicture = a.profilePicture,
                                                  idproof = a.idproof,
                                                  oneTimePassword = a.oneTimePassword,
                                                  isActiveOneTimePassword = a.isActiveOneTimePassword,
                                                  oneTimePasswordTimeOut = a.oneTimePasswordTimeOut,
                                                  designation = a.designation,
                                                  gender = a.gender,
                                                  emialAddress = a.emialAddress,
                                                  married = a.married,
                                                  employmentStatus = a.employmentStatus,
                                                  joiningdate = a.joiningdate,
                                                  currentSalary = a.currentSalary,
                                                  castCategory = a.castCategory,
                                                  dateOfBirth = a.dateOfBirth,
                                                  dateOfAniversary = a.dateOfAniversary,
                                                  address = a.address,
                                                  licenseNo = a.licenseNo,
                                                  pancardNo = a.pancardNo,
                                                  employeeWorkShift = a.employeeWorkShift,
                                                  bankName = a.bankName,
                                                  bankAcNo = a.bankAcNo,
                                                  ifsccode = a.ifsccode,
                                                  keyResponsibleArea = a.keyResponsibleArea,
                                                  salary = a.salary,
                                                  remarks = a.remarks,
                                                  leavingdate = a.leavingdate,
                                                  isActive = a.isActive,
                                                  isDeleted = a.isDeleted,
                                                  entryDate = a.entryDate,
                                                  lastChangeDate = a.lastChangeDate,
                                                  recordByLion = a.recordByLion,
                                                  entryByUserName = a.entryByUserName,
                                                  changeByUserName = a.changeByUserName

                                              }).SingleOrDefault());
                }
                await Task.WhenAll(emodel);
                return new JsonResult() { Data = emodel.Result, MaxJsonLength = Int32.MaxValue };
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

        public async Task<JsonResult> UpdateEmployeeDynamic(FormDataCollection JsonParamString)
        {

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                employeeDCM modeld = (json_serializer.Deserialize<employeeDCM>(JsonString));
                employee model = (json_serializer.Deserialize<employee>(JsonString));
                using (employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext())
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

        public async Task<JsonResult> DeleteEmployeeDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string employeeCode = json_serializer.Deserialize<string>(JsonString);
                using (employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext())
                {
                    employee model = new employee();
                    model = (from a in edb.employees where a.employeeCode == employeeCode select a).SingleOrDefault();
                    edb.employees.Remove(model);
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
                return new JsonResult() { Data = new ErrorCM { errorLogId = logid }, MaxJsonLength = Int32.MaxValue };
            }
        }

        public async Task<JsonResult> deleteMultipleEmployeeAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                EmployeeCCM model = json_serializer.Deserialize<EmployeeCCM>(JsonString);
                using (employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext())
                {

                    string[] delarr = model.employeeCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var emp = (from b in edb.employees where b.employeeCode == parameter select b).Single();
                        edb.employees.Remove(emp);
                        await edb.SaveChangesAsync();
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
        public async Task<JsonResult> setAsActiveMultipleEmployeeAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                EmployeeCCM model = json_serializer.Deserialize<EmployeeCCM>(JsonString);
                using (employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext())
                {

                    string[] delarr = model.employeeCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var emp = (from b in edb.employees where b.employeeCode == parameter select b).Single();
                        emp.isActive = true;
                        await edb.SaveChangesAsync();
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

        public async Task<JsonResult> setAsInactiveMultipleEmployeeAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                EmployeeCCM model = json_serializer.Deserialize<EmployeeCCM>(JsonString);
                using (employeelvitsposdbEntities edb = new UniversDbContext().employeeDbContext())
                {

                    string[] delarr = model.employeeCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var emp = (from b in edb.employees where b.employeeCode == parameter select b).Single();
                        emp.isActive = false;
                        await edb.SaveChangesAsync();
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
