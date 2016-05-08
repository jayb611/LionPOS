using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LionUtilities.SQLUtilitiesPkg.Models;
using LionUtilities;
using LionPOSDbContracts.DomainModels.warehouse;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ControllerContractModel.warehouseManagement;
using LionPOSServiceContractModels.DomainContractsModel.warehouse;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSDbContracts.DomainModels.User;
using System.Web;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Transactions;

namespace LionPOSServiceOperationLayer.WarehouseManagement
{
    public class WarehouseManagementServices
    {
        public List<FilterFieldsModel> getFieldsToSearch(SessionCM sm, string defaultOrderby = "")
        {
            List<FilterFieldsModel> str = new List<FilterFieldsModel>();
            List<FilterFieldsModel> tmp = new List<FilterFieldsModel>();
            try
            {
                tmp.Add(new FilterFieldsModel("warehousesCode", SQLDataTypConversionModel.TextSQLType.name, "Warehouses Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("name", SQLDataTypConversionModel.TextSQLType.name, "Name", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("CoveringAreas", SQLDataTypConversionModel.TextSQLType.name, "Covering Area", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("warehouseType", SQLDataTypConversionModel.TextSQLType.name, "Warehouse Type", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactType1", SQLDataTypConversionModel.TextSQLType.name, "Contact Type1", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactNo1", SQLDataTypConversionModel.TextSQLType.name, "Contact Numner 1", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactType2", SQLDataTypConversionModel.TextSQLType.name, "Contact Type2", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactNo2", SQLDataTypConversionModel.TextSQLType.name, "Contact Numner 2", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("email", SQLDataTypConversionModel.TextSQLType.name, "Email", true, true, defaultOrderby));
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

        public async Task<JsonResult> getWarehousesDynamic(FormDataCollection JsonParamString)
        {
            WarehouseCCM model = new WarehouseCCM();

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                BasicQueryContractModel cm = new JavaScriptSerializer().Deserialize<BasicQueryContractModel>(JsonString);
                List<FilterFieldsModel> filters;
                Task<int> warehouselistCount_t1_p1;
                Task<List<warehouseDCM>> warehouselist_t1_p2;
                int totalRecords = 0;
                string loadWhere = "";
                string loadOrderby = "";
                if (cm.LoadAsDefaultFilter == true)
                {

                    string defaultSetting = cm.sessionObj.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.WarehouseFilterSaveAsDefault.title && a.userName == cm.sessionObj.user.userName && a.userEntryBranchCode == cm.sessionObj.user.userEntryBranchCode && a.userEntryGroupCode == cm.sessionObj.user.userEntryGroupCode).Select(a => a.values).SingleOrDefault();
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



                using (warehouselvitsposdbEntities wdb = new UniversDbContext().warehouseDbContext(false, false))
                {




                    warehouselistCount_t1_p1 = Task.FromResult(
                                                              wdb.Database.SqlQuery<int>("call getWarehousesWithDynamicClausesCountRecord(@dynamicWhereClauses)",
                                                              new MySqlParameter("dynamicWhereClauses", loadWhere)
                                                              ).
                                                              Single()
                                                          );
                    warehouselist_t1_p2 = Task.FromResult(
                                                                wdb.Database.SqlQuery<warehouseDCM>("call getWarehousesWithDynamicClauses(@dynamicWhereClauses,@dynamicOrderByFields,@skip,@take)",
                                                              new MySqlParameter("dynamicWhereClauses", loadWhere),
                                                              new MySqlParameter("dynamicOrderByFields", loadOrderby),
                                                              new MySqlParameter("skip", skip),
                                                              new MySqlParameter("take", cm.recordPerPage)
                                                                ).ToList()
                                                        );
                }

                await Task.WhenAll(warehouselistCount_t1_p1, warehouselist_t1_p2);
                totalRecords = warehouselistCount_t1_p1.Result;
                model.warehouseList = warehouselist_t1_p2.Result;
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
                        user_settings ss = udb.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.WarehouseFilterSaveAsDefault.title).SingleOrDefault();
                        if (ss == null)
                        {
                            ss = new user_settings();
                            ss.title = ConstantRecordsDictionaryCM.Setting_Seeds.WarehouseFilterSaveAsDefault.title;
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

        public async Task<JsonResult> AddWarehouseDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                warehouseDCM model = (json_serializer.Deserialize<warehouseDCM>(JsonString));
                model.recordByLion = false;
                model.warehousesEntryBranchCode = model.SessionCM.user.userEntryBranchCode;
                model.warehousesEntryGroupCode = model.SessionCM.user.userEntryGroupCode;
                model.changeByUserName = model.SessionCM.user.userName;
                model.entryByUserName = model.SessionCM.user.userName;
                model.entryDate = DateTime.Now;
                model.lastChangeDate = DateTime.Now;
                string war = json_serializer.Serialize(model);
                warehouse warehouse = (json_serializer.Deserialize<warehouse>(war));

                using (TransactionScope sc = new TransactionScope())
                {
                    using (warehouselvitsposdbEntities wdb = new UniversDbContext().warehouseDbContext(false, false))
                    {
                        wdb.warehouses.Add(warehouse);
                        await wdb.SaveChangesAsync();
                        sc.Complete();
                    }
                }
                return new JsonResult() { Data = new ErrorCM { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on Adding New Warehouse " + System.Reflection.MethodBase.GetCurrentMethod().Name,
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

        public async Task<JsonResult> getWarehouseByWarehouseCodeAsync(FormDataCollection JsonParamString)
        {
            warehouseDCM model = new warehouseDCM();
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string warehousesCode = json_serializer.Deserialize<string>(JsonString);

                Task<warehouseDCM> wmodel;

                using (warehouselvitsposdbEntities wdb = new UniversDbContext().warehouseDbContext(false, false))
                {
                    wmodel = Task.FromResult((from a in wdb.warehouses
                                              where a.warehousesCode == warehousesCode
                                              select
new warehouseDCM
{
    warehousesCode = a.warehousesCode,
    warehousesEntryBranchCode = a.warehousesEntryBranchCode,
    warehousesEntryGroupCode = a.warehousesEntryGroupCode,
    name = a.name,
    description = a.description,
    address = a.address,
    CoveringAreas = a.CoveringAreas,
    logitudeLocation = a.logitudeLocation,
    latitudeLoaction = a.latitudeLoaction,
    warehouseType = a.warehouseType,
    remarks = a.remarks,
    contactType2 = a.contactType2,
    contactNo2 = a.contactNo2,
    contactType1 = a.contactType1,
    contactNo1 = a.contactNo1,
    email = a.email,
    isActive = a.isActive,
    isDeleted = a.isDeleted,
    entryDate = a.entryDate,
    lastChangeDate = a.lastChangeDate,
    recordByLion = a.recordByLion,
    entryByUserName = a.entryByUserName,
    changeByUserName = a.changeByUserName
}).SingleOrDefault());
                }
                await Task.WhenAll(wmodel);
                return new JsonResult() { Data = wmodel.Result, MaxJsonLength = Int32.MaxValue };
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

        public async Task<JsonResult> UpdateWarehouseDynamic(FormDataCollection JsonParamString)
        {

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                warehouseDCM modeld = (json_serializer.Deserialize<warehouseDCM>(JsonString));
                warehouse model = (json_serializer.Deserialize<warehouse>(JsonString));
                using (warehouselvitsposdbEntities wdb = new UniversDbContext().warehouseDbContext())
                {
                    model.changeByUserName = modeld.SessionCM.user.userName;
                    model.lastChangeDate = DateTime.Now;

                    wdb.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    await wdb.SaveChangesAsync();
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

        public async Task<JsonResult> DeleteWarehouseDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string warehousesCode = json_serializer.Deserialize<string>(JsonString);
                using (warehouselvitsposdbEntities wdb = new UniversDbContext().warehouseDbContext())
                {
                    warehouse model = new warehouse();
                    model = (from a in wdb.warehouses where a.warehousesCode == warehousesCode select a).SingleOrDefault();
                    wdb.warehouses.Remove(model);
                    await wdb.SaveChangesAsync();
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

        public async Task<JsonResult> deleteMultipleWarehousesAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                WarehouseCCM model = json_serializer.Deserialize<WarehouseCCM>(JsonString);
                using (warehouselvitsposdbEntities wdb = new UniversDbContext().warehouseDbContext())
                {

                    string[] delarr = model.warehousesCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var ware = (from b in wdb.warehouses where b.warehousesCode == parameter select b).Single();
                        wdb.warehouses.Remove(ware);
                        await wdb.SaveChangesAsync();
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

        public async Task<JsonResult> setAsActiveMultipleWarehousesAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                WarehouseCCM model = json_serializer.Deserialize<WarehouseCCM>(JsonString);
                using (warehouselvitsposdbEntities wdb = new UniversDbContext().warehouseDbContext())
                {

                    string[] delarr = model.warehousesCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var ware = (from b in wdb.warehouses where b.warehousesCode == parameter select b).Single();
                        ware.isActive = true;
                        await wdb.SaveChangesAsync();
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

        public async Task<JsonResult> setAsInactiveMultipleWarehousesAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                WarehouseCCM model = json_serializer.Deserialize<WarehouseCCM>(JsonString);
                using (warehouselvitsposdbEntities wdb = new UniversDbContext().warehouseDbContext())
                {

                    string[] delarr = model.warehousesCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var ware = (from b in wdb.warehouses where b.warehousesCode == parameter select b).Single();
                        ware.isActive = false;
                        await wdb.SaveChangesAsync();
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
