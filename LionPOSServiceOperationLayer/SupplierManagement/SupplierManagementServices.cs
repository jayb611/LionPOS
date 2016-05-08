using DomainModels;
using LionPOSDbContracts.DomainModels.Supplier;
using LionPOSDbContracts.DomainModels.User;
using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.ControllerContractModel.SupplierManagement;
using LionPOSServiceContractModels.DomainContractsModel.Supplier;
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
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LionPOSServiceOperationLayer.SupplierManagement
{
    public class SupplierManagementServices
    {
        public string JsonString;
        JavaScriptSerializer json_serializer = new JavaScriptSerializer();

        public List<FilterFieldsModel> getFieldsToSearch(SessionCM sm, string defaultOrderby = "")
        {
            List<FilterFieldsModel> str = new List<FilterFieldsModel>();
            List<FilterFieldsModel> tmp = new List<FilterFieldsModel>();
            try
            {

                tmp.Add(new FilterFieldsModel("suppliersCode", SQLDataTypConversionModel.TextSQLType.name, "Supplier Code", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("title", SQLDataTypConversionModel.TextSQLType.name, "Title", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("contactPersonName", SQLDataTypConversionModel.TextSQLType.name, "Contact Person Name", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("status", SQLDataTypConversionModel.TextSQLType.name, "Status", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("registrationDate", SQLDataTypConversionModel.DateTimeSQLType.name, "Registration Date", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("registrationNo", SQLDataTypConversionModel.TextSQLType.name, "Registration Number", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("bankName", SQLDataTypConversionModel.TextSQLType.name, "Bank Name", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("mobileNo", SQLDataTypConversionModel.TextSQLType.name, "Mobile Numner", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("telephoneNo", SQLDataTypConversionModel.TextSQLType.name, "Telephone Numner", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("bstNo", SQLDataTypConversionModel.TextSQLType.name, "BST Number", true, true, defaultOrderby));
                tmp.Add(new FilterFieldsModel("state", SQLDataTypConversionModel.TextSQLType.name, "State", true, true, defaultOrderby));
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

        public async Task<JsonResult> getSuppliersDynamic(FormDataCollection JsonParamString)
        {
            SupplierCCM model = new SupplierCCM();
            try
            {
                JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                BasicQueryContractModel bqcm = json_serializer.Deserialize<BasicQueryContractModel>(JsonString);
                List<FilterFieldsModel> filters;
                Task<int> supplierListCount_t1_p1;
                Task<List<supplierDCM>> supplierList_t1_p2;
                int totalRecords = 0;
                string loadWhere = "";
                string loadOrderby = "";
                if (bqcm.LoadAsDefaultFilter == true)
                {
                    string defaultSetting = bqcm.sessionObj.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.SupplierFilterSaveAsDefault.title && a.userName == bqcm.sessionObj.user.userName && a.userEntryBranchCode == bqcm.sessionObj.user.userEntryBranchCode && a.userEntryGroupCode == bqcm.sessionObj.user.userEntryGroupCode).Select(a => a.values).SingleOrDefault();
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
                filters = getFieldsToSearch(bqcm.sessionObj, loadOrderby);
                if (bqcm.LoadAsDefaultFilter == false)
                {
                    loadWhere = new SQLUtilities().getFormatedWhereClause(bqcm.FilterFieldAndValues, filters, "dd/MM/yyyy");
                    loadOrderby = bqcm.OrderByFields;
                }
                int skip = new SQLUtilities().getSkip(bqcm.recordPerPage, bqcm.pageNumber);

                using (supplierlvitsposdbEntities sdb = new UniversDbContext().supplierDbContext(false, false))
                {
                    supplierListCount_t1_p1 = Task.FromResult(
                                                                sdb.Database.SqlQuery<int>("call getSuppliersWithDynamicClausesCountRecord(@dynamicWhereClauses)",
                                                               new MySqlParameter("dynamicWhereClauses", loadWhere)
                                                              ).
                                                              Single()
                                                          );

                    supplierList_t1_p2 = Task.FromResult(
                                                            sdb.Database.SqlQuery<supplierDCM>("call getSuppliersWithDynamicClauses(@dynamicWhereClauses,@dynamicOrderByFields,@skip,@take)",
                                                            new MySqlParameter("dynamicWhereClauses", loadWhere),
                                                            new MySqlParameter("dynamicOrderByFields", loadOrderby),
                                                            new MySqlParameter("skip", skip),
                                                            new MySqlParameter("take", bqcm.recordPerPage)
                                                            ).ToList()
                                                        );
                }
                await Task.WhenAll(supplierListCount_t1_p1, supplierList_t1_p2);
                totalRecords = supplierListCount_t1_p1.Result;
                model.supplierList = supplierList_t1_p2.Result;
                model.Pagination.PageNumber = bqcm.pageNumber;
                model.Pagination.TotalRecords = totalRecords;
                model.Pagination.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(model.Pagination.TotalRecords) / Convert.ToDecimal(bqcm.recordPerPage)));
                model.FilterFieldModelJson = JsonConvert.SerializeObject(filters);
                model.FilterFieldsModel = filters;


                if (bqcm.SaveAsDefaultFilter == true)
                {
                    using (userlvitsposdbEntities db = new UniversDbContext().userDbContext(false, false))
                    {
                        user_settings ss = db.user_settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.SupplierFilterSaveAsDefault.title).SingleOrDefault();
                        if (ss == null)
                        {
                            ss = new user_settings();
                            ss.title = ConstantRecordsDictionaryCM.Setting_Seeds.SupplierFilterSaveAsDefault.title;
                            ss.values = loadWhere + "|" + loadOrderby;
                            ss.userEntryBranchCode = bqcm.sessionObj.user.userEntryBranchCode;
                            ss.userEntryGroupCode = bqcm.sessionObj.user.userEntryGroupCode;
                            ss.userName = bqcm.sessionObj.user.userName;
                            db.user_settings.Add(ss);
                        }
                        List<user_settingsSCM> settingsSCM = db.user_settings.Where(conf => conf.userName == bqcm.sessionObj.user.userName && conf.userEntryBranchCode == bqcm.sessionObj.branch.branchCode && conf.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode).Select(a => new user_settingsSCM { userEntryBranchCode = a.userEntryBranchCode, userEntryGroupCode = a.userEntryGroupCode, description = a.description, title = a.title, userName = a.userName, values = a.values }).ToList();
                        bqcm.sessionObj.user_settings = settingsSCM;
                    }
                    RefreshSessionObject(bqcm.sessionObj);
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





        public async Task<JsonResult> AddSupplierDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                supplierDCM model = (json_serializer.Deserialize<supplierDCM>(JsonString));
                model.isDeleted = false;
                model.isActive = true;
                model.recordByLion = false;
                model.suppliersEntryGroupCode = model.SessionCM.user.userEntryGroupCode;
                model.suppliersEntryBranchCode = model.SessionCM.user.userEntryBranchCode;
                model.changeByUserName = model.SessionCM.user.userName;
                model.entryByUserName = model.SessionCM.user.userName;
                model.entryDate = DateTime.Now;
                model.lastChangeDate = DateTime.Now;

                string splyr = json_serializer.Serialize(model);
                supplier supplier = (json_serializer.Deserialize<supplier>(splyr));

                using (TransactionScope sc = new TransactionScope())
                {
                    using (supplierlvitsposdbEntities sdb = new UniversDbContext().supplierDbContext(false, false))
                    {
                        sdb.suppliers.Add(supplier);
                        await sdb.SaveChangesAsync();
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



        public async Task<JsonResult> getSupplierBySuppliersCodeAsync(FormDataCollection JsonParamString)
        {
            supplierDCM model = new supplierDCM();
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string supplierCode = json_serializer.Deserialize<string>(JsonString);

                Task<supplierDCM> smodel;

                using (supplierlvitsposdbEntities sdb = new UniversDbContext().supplierDbContext(false, false))
                {
                    smodel = Task.FromResult((from a in sdb.suppliers
                                              where a.suppliersCode == supplierCode
                                              select
    new supplierDCM
    {
        suppliersEntryBranchCode = a.suppliersEntryBranchCode,

        suppliersCode = a.suppliersCode,

        suppliersEntryGroupCode = a.suppliersEntryGroupCode,

        title = a.title,

        shortTitle = a.shortTitle,

        status = a.status,

        creditLimitDays = a.creditLimitDays,

        creditLimitAmount = a.creditLimitAmount,

        permanentAccountNumberDate = a.permanentAccountNumberDate,

        contactPersonName = a.contactPersonName,

        contactPersonDesignation = a.contactPersonDesignation,

        pincode = a.pincode,

        mobileNo = a.mobileNo,

        telephoneNo = a.telephoneNo,

        city = a.city,

        state = a.state,

        country = a.country,

        registreredAddress = a.registreredAddress,

        correspondenceAddress = a.correspondenceAddress,

        billingAddress = a.billingAddress,

        serviceTaxNo = a.serviceTaxNo,

        valueAddedTaxNo = a.valueAddedTaxNo,

        valueAddedTaxDate = a.valueAddedTaxDate,

        permanentAccountNumber = a.permanentAccountNumber,

        centralSalesTaxNo = a.centralSalesTaxNo,

        centralSalesTaxDate = a.centralSalesTaxDate,

        taxpayerIdentificationNumber = a.taxpayerIdentificationNumber,

        taxpayerIdentificationNumberDate = a.taxpayerIdentificationNumberDate,

        corporateIdentityNumber = a.corporateIdentityNumber,

        corporateIdentityNumberDate = a.corporateIdentityNumberDate,

        bstNo = a.bstNo,

        bstDate = a.bstDate,
        registrationNo = a.registrationNo,
        registrationDate = a.registrationDate,
        legalStatus = a.legalStatus,
        bankName = a.bankName,
        bankAcNo = a.bankAcNo,
        ifsccode = a.ifsccode,
        overDueLockingDays = a.overDueLockingDays,
        isActive = a.isActive,
        isDeleted = a.isDeleted,
        entryDate = a.entryDate,
        lastChangeDate = a.lastChangeDate,
        recordByLion = a.recordByLion,
        entryByUserName = a.entryByUserName,
        changeByUserName = a.changeByUserName
    }).SingleOrDefault());
                }
                await Task.WhenAll(smodel);
                return new JsonResult() { Data = smodel.Result, MaxJsonLength = Int32.MaxValue };
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



        public async Task<JsonResult> UpdateSupplierDynamic(FormDataCollection JsonParamString)
        {

            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                supplierDCM modeld = (json_serializer.Deserialize<supplierDCM>(JsonString));
                supplier model = (json_serializer.Deserialize<supplier>(JsonString));
                using (supplierlvitsposdbEntities sdb = new UniversDbContext().supplierDbContext())
                {
                    model.isDeleted = false;
                    model.changeByUserName = modeld.SessionCM.user.userName;
                    model.lastChangeDate = DateTime.Now;

                    sdb.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    await sdb.SaveChangesAsync();
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

        public async Task<JsonResult> DeleteSupplierDynamic(FormDataCollection JsonParamString)
        {
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                string SuppliersCode = json_serializer.Deserialize<string>(JsonString);
                using (supplierlvitsposdbEntities sdb = new UniversDbContext().supplierDbContext())
                {
                    supplier model = new supplier();
                    model = (from a in sdb.suppliers where a.suppliersCode== SuppliersCode select a).SingleOrDefault();
                    sdb.suppliers.Remove(model);
                    await sdb.SaveChangesAsync();
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



        public async Task<JsonResult> deleteMultipleSuppliersAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                SupplierCCM model = json_serializer.Deserialize<SupplierCCM>(JsonString);
                using (supplierlvitsposdbEntities sdb = new UniversDbContext().supplierDbContext())
                {

                    string[] delarr = model.supplierCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var splyr = (from b in sdb.suppliers where b.suppliersCode== parameter select b).Single();
                        sdb.suppliers.Remove(splyr);
                        await sdb.SaveChangesAsync();
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
        public async Task<JsonResult> setAsActiveMultipleSuppliersAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                SupplierCCM model = json_serializer.Deserialize<SupplierCCM>(JsonString);
                using (supplierlvitsposdbEntities sdb = new UniversDbContext().supplierDbContext())
                {

                    string[] delarr = model.supplierCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var splyr = (from b in sdb.suppliers where b.suppliersCode == parameter select b).Single();
                        splyr.isActive = true;
                        await sdb.SaveChangesAsync();
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

        public async Task<JsonResult> setAsInactiveMultipleSuppliersAsync(FormDataCollection JsonParamString)
        {
            try
            {

                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                SupplierCCM model = json_serializer.Deserialize<SupplierCCM>(JsonString);
                using (supplierlvitsposdbEntities sdb = new UniversDbContext().supplierDbContext())
                {

                    string[] delarr = model.supplierCodes.Split(',');
                    for (int i = 0; i < delarr.Length; i++)
                    {
                        string parameter = delarr[i];
                        var splyr = (from b in sdb.suppliers where b.suppliersCode == parameter select b).Single();
                        splyr.isActive = false;
                        await sdb.SaveChangesAsync();
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
