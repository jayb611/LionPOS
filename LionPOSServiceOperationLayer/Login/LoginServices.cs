using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using LionPOSDbContracts.DomainModels.User;
using LionPOSDbContracts.DomainModels.Configuration;
using DomainModels;
using LionPOSServiceContractModels;
using System.Data.Entity;
using LionUtilities.SecurityPkg;
using LionPOSDbContracts.DomainModels.Employee;
using LionPOSDbContracts.DomainModels.Branch;

using System.Web;
using Newtonsoft.Json;
using LionPOSServiceOperationLayer.InitiateSystemStartUp;
using LionPOSServiceContractModels.ControllerContractModel.Login.Login;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using System.Web.Mvc;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Net.Http.Formatting;
using System.Web.Script.Serialization;
using LionPOSServiceContractModels.DomainContractsModel.Branch;

namespace LionPOSServiceOperationLayer.Login
{

    /// <summary>
    /// Login Services 
    /// Last Modifed by JB 31-03-2016
    /// </summary>
    public class LoginServices
    {
        public LoginServices()
        { }
        /// <summary>
        /// Async
        /// Returns Session Object to store session.
        /// Identifies Person authorisation.
        /// Store Session in Database and returns Sesure Session Identifier as Encrypted.
        /// </summary>
        /// <param name="JsonParamString.GetSessionDetailsSubmitCCM"></param>
        /// <returns>JsonResult.Data.SessionCM</returns>
        public async Task<JsonResult> GetSessionDetailsAsync(FormDataCollection JsonParamString)
        {

            SessionCM sm = new SessionCM();
            try
            {
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                GetSessionDetailsSubmitCCM getSessionCM = new JavaScriptSerializer().Deserialize<GetSessionDetailsSubmitCCM>(JsonString);

                Task<List<settingSCM>> taskSettings_t1_p1;
                Task<user> taskUser_t1_p2;
                
                //Fetch All Settings
                using (configurationlvitsposdbEntities dbconf = new UniversDbContext().configurationDbContext(false, false))
                {
                    taskSettings_t1_p1 = dbconf.settings.Where(conf => conf.branchCode == getSessionCM.branchCode).Select(a => new settingSCM { branchCode = a.branchCode, description = a.description, title = a.title, values = a.values }).ToListAsync<settingSCM>();
                }
                //Fetch user details with access role and accessible areas
                using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
                {
                    taskUser_t1_p2 = dbuser.users.
                        Where(
                        user => user.userName == getSessionCM.username &&
                        user.userEntryBranchCode == getSessionCM.branchCode &&
                        user.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode
                        )
                        .Include(
                        accessRole => accessRole.access_role_master.access_role_in_access_area
                        )
                        .Include(
                        accessOverride => accessOverride.override_access_role
                        )
                        .SingleOrDefaultAsync();

                }
                //Run both query parellel
                await Task.WhenAll(taskSettings_t1_p1, taskUser_t1_p2);
                //Store Parellel thread results
                sm.settings = taskSettings_t1_p1.Result;
                if (taskUser_t1_p2.Result != null)
                {
                    sm.user = new userSCM();
                    sm.user.accessRoleTitle = taskUser_t1_p2.Result.accessRoleTitle;
                    sm.user.blockExpiry = taskUser_t1_p2.Result.blockExpiry;
                    sm.user.employeeCode = taskUser_t1_p2.Result.employeeCode;
                    sm.user.employeeEntryBranchCode = taskUser_t1_p2.Result.employeeEntryBranchCode;
                    sm.user.employeeEntryGroupCode = taskUser_t1_p2.Result.employeeEntryGroupCode;
                    sm.user.lastLogin = taskUser_t1_p2.Result.lastLogin;
                    sm.user.lastPasswordResetDate = taskUser_t1_p2.Result.lastPasswordResetDate;
                    sm.user.sessionCreateTime = taskUser_t1_p2.Result.sessionCreateTime;
                    sm.user.sessionExpireyDateTime = taskUser_t1_p2.Result.sessionExpireyDateTime;
                    sm.user.sessionID = taskUser_t1_p2.Result.sessionID;
                    sm.user.sessionValue = taskUser_t1_p2.Result.sessionValue;
                    sm.user.userAccountStatus = taskUser_t1_p2.Result.userAccountStatus;
                    sm.user.userEntryBranchCode = taskUser_t1_p2.Result.userEntryBranchCode;
                    sm.user.userEntryGroupCode = taskUser_t1_p2.Result.userEntryGroupCode;
                    sm.user.userName = taskUser_t1_p2.Result.userName;
                    sm.user.userStatus = taskUser_t1_p2.Result.userStatus;
                    sm.user.password = taskUser_t1_p2.Result.password;
                    sm.user.passwordEncryptionKey = taskUser_t1_p2.Result.passwordEncryptionKey;

                    sm.access_role_in_access_area = taskUser_t1_p2.Result.access_role_master.access_role_in_access_area.Select(a => new access_role_in_access_areaSCM
                    {
                        canAccess = a.canAccess,
                        domain = a.domain,
                        controller = a.controller,
                        showBranchWise = a.showBranchWise,
                        underMaintenance = a.underMaintenance,
                        view = a.view,
                        visible = a.visible
                    }).ToList();
                    sm.override_access_role = taskUser_t1_p2.Result.override_access_role.Select(a => new override_access_roleSCM
                    {
                        canAccess = a.canAccess,
                        domain = a.domain,
                        controller = a.controller,
                        overrideTill = a.overrideTill,
                        permanent = a.permanent,
                        title = a.title,
                        view = a.view,
                        visible = a.visible
                    }).ToList();

                }



                //Check is not null or not found
                if (sm.user.userName != null)
                {
                    //Reencrypt with same AES Key stored in user data
                    string currentEncPass = new AESAlgoritham().Encrypt(getSessionCM.password, sm.user.passwordEncryptionKey);

                    //Unblock user if account is blocked by unblockable time
                    if (sm.user.blockExpiry != null)
                    {
                        if (sm.user.blockExpiry < DateTime.Now)
                        {
                            sm.user.userAccountStatus = ConstantDictionaryCM.AccountStatus.Unblocked;
                        }
                    }

                    //Match Encrypted with encrypted stored password 
                    if (currentEncPass == sm.user.password && sm.user.userAccountStatus == ConstantDictionaryCM.AccountStatus.Unblocked)
                    {
                        Task<employeeSCM> taskEmployee_t2_p1;
                        Task<branchSCM> taskBranch_t2_p2;
                        Task<List<access_areaSCM>> taskAccessArea_t1_p4;
                        Task<List<user_settingsSCM>> taskUser_settings_t1_p3; 
                        DateTime sessionExpiryDateTime = DateTime.Now.AddMinutes(Convert.ToInt32(sm.settings.Where(a => a.title == InitConstantRecordsDictionaryServices.Setting_Seeds.Session_Expiry_Time_In_Minutes.title && a.branchCode == getSessionCM.branchCode).Select(a => a.values).Single()));
                        sm.user.sessionExpireyDateTime = sessionExpiryDateTime;
                        AESAlgoritham aes = new AESAlgoritham();
                        using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
                        {
                            taskAccessArea_t1_p4 = dbuser.access_area.Select(a => new access_areaSCM
                            {
                                isForHeadOffice = a.isForHeadOffice,
                                domain = a.domain,
                                controller = a.controller,
                                view = a.view,
                                visible = a.visible,
                                isForOutlet = a.isForOutlet
                            }).ToListAsync();

                            taskUser_settings_t1_p3 = dbuser.user_settings.Where(a => a.userName == getSessionCM.username && a.userEntryBranchCode == getSessionCM.branchCode && a.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode)
                                .Select(a => new user_settingsSCM()
                                {
                                    description = a.description,
                                    userEntryBranchCode = a.userEntryBranchCode,
                                    title= a.title,
                                    userEntryGroupCode= a.userEntryGroupCode,
                                    userName= a.userName,
                                    values = a.values
                                }).ToListAsync();

                        }
                        using (employeelvitsposdbEntities dbemp = new UniversDbContext().employeeDbContext(false, false))
                        {
                            taskEmployee_t2_p1 = dbemp.employees.Where(employee => employee.employeeEntryBranchCode == sm.user.employeeEntryBranchCode && employee.employeeCode == sm.user.employeeCode && employee.employeeEntryGroupCode == sm.user.employeeEntryGroupCode).Select(employee => new employeeSCM
                            {
                                firstName = employee.firstName,
                                middleName = employee.middleName,
                                profilePicture = employee.profilePicture,
                                sureName = employee.sureName,
                                title = employee.title,
                                employeeCode = employee.employeeCode,
                                employeeEntryBranchCode = employee.employeeEntryBranchCode,
                                employeeEntryGroupCode = employee.employeeEntryGroupCode,
                                contactNo = employee.contactNo1,
                                emialAddress = employee.emialAddress
                            }).SingleOrDefaultAsync();
                        }
                        using (branchlvitsposdbEntities dbbran = new UniversDbContext().branchDbContext(false, false))
                        {
                            taskBranch_t2_p2 = dbbran.branches.Where(branche => branche.branchCode == sm.user.userEntryBranchCode).Select(branche => new branchSCM
                            {
                                branchCode = branche.branchCode,
                                branchName = branche.branchName,
                                branchType = branche.branchType,
                                latitudeLoaction = branche.latitudeLoaction,
                                logitudeLocation = branche.logitudeLocation,
                                GroupCode = ConstantDictionaryCM.ApplicationGroupCode
                            }).SingleOrDefaultAsync();
                        }

                        await Task.WhenAll(taskEmployee_t2_p1, taskBranch_t2_p2, taskAccessArea_t1_p4, taskUser_settings_t1_p3);
                        sm.employee = taskEmployee_t2_p1.Result;
                        sm.branch = taskBranch_t2_p2.Result;
                        sm.user_settings = taskUser_settings_t1_p3.Result;
                        sm.branch.GroupCode = ConstantDictionaryCM.ApplicationGroupCode;
                        sm.access_areas = taskAccessArea_t1_p4.Result;
                        sm.user.password = "";
                        sm.user.passwordEncryptionKey = "";
                        sm.isAuthorised = true;
                        sm.user.sessionCreateTime = DateTime.Now;
                        sm.user.userAccountStatus = ConstantDictionaryCM.AccountStatus.Unblocked;
                        using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
                        {
                            user User = dbuser.users.Where(
                                        user => user.userName == getSessionCM.username &&
                                        user.userEntryBranchCode == getSessionCM.branchCode &&
                                        user.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode
                                        ).SingleOrDefault();
                            User.userAccountStatus = ConstantDictionaryCM.AccountStatus.Unblocked;
                            User.sessionCreateTime = DateTime.Now;
                            User.sessionExpireyDateTime = sm.user.sessionExpireyDateTime;
                            User.sessionID = User.userName + User.userEntryBranchCode + User.userEntryGroupCode + sm.user.sessionExpireyDateTime.Value.ToShortDateString() + sm.user.sessionExpireyDateTime.Value.ToShortTimeString();
                            User.sessionID = aes.Encrypt(User.sessionID, User.passwordEncryptionKey);
                            User.sessionValue = "";
                            sm.user.sessionID = User.sessionID;
                            sm.user.sessionValue = "";
                            User.sessionValue = HttpUtility.UrlEncode(JsonConvert.SerializeObject(sm));
                            await dbuser.SaveChangesAsync();
                        }

                    }
                    else
                    {
                        bool block = ((sm.settings.Where(a => a.title == InitConstantRecordsDictionaryServices.Setting_Seeds.Block_Acount_After_Attempt.title).Select(a => a.values).Single() == "Yes") ? true : false);
                        int blockCount = Convert.ToInt32(sm.settings.Where(a => a.title == InitConstantRecordsDictionaryServices.Setting_Seeds.Block_Account_After_Attempt_Count.title).Select(a => a.values).Single());
                        if (blockCount == getSessionCM.requestCount && block == true)
                        {
                            using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext())
                            {
                                user User = dbuser.users.Where(
                                            user => user.userName == getSessionCM.username &&
                                            user.userEntryBranchCode == getSessionCM.branchCode &&
                                            user.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode
                                            ).SingleOrDefault();
                                User.userAccountStatus = ConstantDictionaryCM.AccountStatus.Blocked;
                                dbuser.SaveChanges();
                            }
                        }
                        sm.isAuthorised = false;
                    }

                }
                return new JsonResult() { Data = sm, MaxJsonLength = Int32.MaxValue };
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
                sm.errorLogId = logid;
                return new JsonResult() { Data = sm, MaxJsonLength = Int32.MaxValue };
            }
        }

        /// <summary>
        /// Async
        /// Retrives Session Object by Secure Session identifier.
        /// </summary>
        /// <param name="JsonParamString.string"></param>
        /// <returns>JsonResult.Data.SessionCM</returns>
        public async Task<JsonResult> GetSessionValueBySSIDAsync(FormDataCollection JsonParamString)
        {

            SessionCM cm = new SessionCM();
            try
            {
                //FormDataCollection JsonParamString
                string ssid = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);

                Task<string> task_get;
                AESAlgoritham aes = new AESAlgoritham();
                //Fetch user details with access role and accessible areas
                DateTime now = DateTime.Now;
                using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
                {
                    task_get = dbuser.users.Where(ses => ses.sessionExpireyDateTime != null && ses.sessionExpireyDateTime > now && ses.sessionID == ssid).Select(ses => ses.sessionValue).SingleOrDefaultAsync();
                }
                //Run both query parellel
                await Task.WhenAll(task_get);
                string sessionValue = task_get.Result;

                if (!string.IsNullOrWhiteSpace(sessionValue))
                {
                    cm = JsonConvert.DeserializeObject<SessionCM>(HttpUtility.UrlDecode(sessionValue));
                    return new JsonResult() { Data = cm, MaxJsonLength = Int32.MaxValue };
                }
                else
                {
                    return new JsonResult() { Data = null, MaxJsonLength = Int32.MaxValue }; ;
                }
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
                cm.errorLogId = logid;
                return new JsonResult() { Data = cm, MaxJsonLength = Int32.MaxValue };
            }
        }



        /// <summary>
        /// Async
        /// Retrives Branches list.
        /// </summary>
        /// <param name="JsonParamString.string"></param>
        /// <returns>JsonResult.Data.SessionCM</returns>
        public async Task<JsonResult> GetBranchesAsync()
        {
            BranchLoginListCCM dcm = new BranchLoginListCCM();
            try
            {
                //FormDataCollection JsonParamString

                Task<List<branchDCM>> task_get;
                AESAlgoritham aes = new AESAlgoritham();
                //Fetch user details with access role and accessible areas
                DateTime now = DateTime.Now;
                using (branchlvitsposdbEntities db = new UniversDbContext().branchDbContext(false, false))
                {
                    task_get = db.branches.Where(a => a.isActive == true).Select(a => new branchDCM()
                    {
                        branchCode = a.branchCode,
                        branchName = a.branchName,
                        branchType = a.branchType,
                        //address = a.address,
                        //branchCodeEntryBranchCode= a.branchCodeEntryBranchCode,
                        //changeByUserName= a.changeByUserName,
                        //city= a.city,
                        //closeDate= a.closeDate,
                        //contactNo1= a.contactNo1,
                        //contactNo2= a.contactNo2,
                        //contactType1= a.contactType1,
                        //contactType2 = a.contactType2,
                        //country = a.country,
                        //CoveringAreas= a.CoveringAreas,
                        //deposit= a.deposit,
                        //description= a.description,
                        //email = a.email,
                        //entryByUserName= a.entryByUserName,
                        //entryDate= a.entryDate,
                        //experiance= a.experiance,
                        //investment = a.investment,
                        //isActive= a.isActive,
                        //isDeleted= a.isDeleted,
                        //lastChangeDate= a.lastChangeDate,
                        //latitudeLoaction= a.logitudeLocation,
                        //logitudeLocation= a.logitudeLocation,
                        //recordByLion= a.recordByLion,
                        //remarks = a.remarks,
                        //openDate= a.openDate,
                        //state= a.state,
                        //staticIpAddress = a.staticIpAddress
                    }).ToListAsync();
                }
                //Run both query parellel
                await Task.WhenAll(task_get);
                dcm.branchDCMList = task_get.Result;
                return new JsonResult() { Data = dcm, MaxJsonLength = Int32.MaxValue };
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
                dcm.errorLogId = logid;
                return new JsonResult() { Data = dcm, MaxJsonLength = Int32.MaxValue };
            }
        }





        /// <summary>
        /// Async
        /// Sign out from account using Secure Session Identifier.
        /// </summary>
        /// <param name="JsonParamString.SessionCM"></param>
        /// <returns>JsonResult.Data.ErrorCM</returns>
        public async Task<JsonResult> LogoutAsync(FormDataCollection JsonParamString)
        {
            try
            {
                //FormDataCollection JsonParamString
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                SessionCM sessionObj = new JavaScriptSerializer().Deserialize<SessionCM>(JsonString);
                Task<user> task_get;
                AESAlgoritham aes = new AESAlgoritham();
                //Fetch user details with access role and accessible areas
                DateTime now = DateTime.Now;
                using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
                {
                    task_get = dbuser.users.Where(ses => ses.sessionID == sessionObj.user.sessionID).SingleOrDefaultAsync();
                    await Task.WhenAll(task_get);
                    user user = task_get.Result;
                    user.sessionID = null;
                    user.sessionValue = null;
                    user.sessionExpireyDateTime = null;
                    user.sessionCreateTime = null;
                    await dbuser.SaveChangesAsync();
                }
                return new JsonResult() { Data = new ErrorCM() { errorLogId = 0 }, MaxJsonLength = Int32.MaxValue };
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
                return new JsonResult() { Data = new ErrorCM() { errorLogId = logid }, MaxJsonLength = Int32.MaxValue };
            }
        }

        /// <summary>
        /// Async
        /// Returns Captcha.Key as Value,Value as Captch Image
        /// </summary>
        /// <returns>JsonResult.Data.CaptchaCCM</returns>
        public async Task<JsonResult> Captcha()
        {
            CaptchaCCM cm = new CaptchaCCM();
            try
            {
                Captcha captcha = new Captcha();
                cm.kvp = await Task.FromResult(captcha.ProcessRequest());
                return new JsonResult() { Data = cm, MaxJsonLength = Int32.MaxValue };
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
                cm.errorLogId = logid;
                return new JsonResult() { Data = cm, MaxJsonLength = Int32.MaxValue };
            }
        }
    }
}