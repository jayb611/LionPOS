using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using DomainModels;
using System.Data.Entity;
using LionUtilities.SecurityPkg;

using System.Web;
using Newtonsoft.Json;
using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ControllerContractModel.Login.Login;
using ChikaraksDbContracts.DomainModels;
using ChikaraksServiceOperationLayer.Maintenance;
using ChikaraksServiceContractModels.ErrorContactModel;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;


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
        public async Task<string> GetSessionDetailsAsync(string JsonParamString)
        {

            SessionCM sm = new SessionCM();
            try
            {


                GetSessionDetailsSubmitCCM getSessionCM = JsonConvert.DeserializeObject<GetSessionDetailsSubmitCCM>(JsonParamString);

                
                Task<user> taskUser_t1_p2;

              
                //Fetch user details with access role and accessible areas
                using (chikaraksEntities dbuser = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    taskUser_t1_p2 = dbuser.users.
                        Where(
                        user => user.userName == getSessionCM.username 
                        )
                        .SingleOrDefaultAsync();
                    await Task.WhenAll(taskUser_t1_p2);
                }
                
                if (taskUser_t1_p2.Result != null)
                {
                    sm.user = new userSCM();
                    sm.user.sessionCreateTime = taskUser_t1_p2.Result.sessionCreateTime;
                    sm.user.sessionExpireyDateTime = taskUser_t1_p2.Result.sessionExpireyDateTime;
                    sm.user.blockExpiry = taskUser_t1_p2.Result.blockExpiry;
                    sm.user.sessionID = taskUser_t1_p2.Result.sessionID;
                    sm.user.sessionValue = taskUser_t1_p2.Result.sessionValue;
                    sm.user.userName = taskUser_t1_p2.Result.userName;
                    sm.user.password = taskUser_t1_p2.Result.password;
                    sm.user.passwordEncryptionKey = taskUser_t1_p2.Result.passwordEncryptionKey;
                    sm.user.userAccountStatus = taskUser_t1_p2.Result.userAccountStatus;
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
                        DateTime sessionExpiryDateTime = DateTime.Now.AddMinutes(ConstantDictionaryCM.sessionExpiryTimeout);
                        sm.user.sessionExpireyDateTime = sessionExpiryDateTime;
                        AESAlgoritham aes = new AESAlgoritham();
                        
                        sm.user.password = "";
                        sm.user.passwordEncryptionKey = "";
                        sm.isAuthorised = true;
                        sm.user.sessionCreateTime = DateTime.Now;
                        sm.user.userAccountStatus = ConstantDictionaryCM.AccountStatus.Unblocked;
                        using (chikaraksEntities dbuser = new UniversDbContext().chikaraksDbContext(false, false))
                        {
                            user User = dbuser.users.Where(
                                        user => user.userName == getSessionCM.username 
                                        
                                        ).SingleOrDefault();
                            User.userAccountStatus = ConstantDictionaryCM.AccountStatus.Unblocked;
                            User.sessionCreateTime = DateTime.Now;
                            User.sessionExpireyDateTime = sm.user.sessionExpireyDateTime;
                            User.sessionID = User.userName + sm.user.sessionExpireyDateTime.Value.ToShortDateString() + sm.user.sessionExpireyDateTime.Value.ToShortTimeString();
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
                        bool block = true;
                        int blockCount = ConstantDictionaryCM.blockCount;
                        int UnblockSecound = ConstantDictionaryCM.UnblockSecound;
                        if (blockCount <= getSessionCM.requestCount && block == true)
                        {
                            using (chikaraksEntities dbuser = new UniversDbContext().chikaraksDbContext())
                            {
                                user User = dbuser.users.Where(
                                            user => user.userName == getSessionCM.username
                                            ).SingleOrDefault();
                                if (UnblockSecound > 0)
                                {
                                    User.blockExpiry = DateTime.Now.AddSeconds(UnblockSecound);
                                }
                                else
                                {
                                    User.blockExpiry = null;
                                }
                                User.userAccountStatus = ConstantDictionaryCM.AccountStatus.Blocked;
                                dbuser.SaveChanges();
                            }
                        }
                        sm.isAuthorised = false;
                    }

                }
                return JsonConvert.SerializeObject(sm);
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
                return JsonConvert.SerializeObject(sm);
            }
        }
        /// <summary>
        /// Async
        /// Retrives Session Object by Secure Session identifier.
        /// </summary>
        /// <param name="JsonParamString.string"></param>
        /// <returns>JsonResult.Data.SessionCM</returns>
        public async Task<string> GetSessionValueBySSIDAsync(string JsonParamString)
        {

            SessionCM cm = new SessionCM();
            try
            {
                //string JsonParamString
                string ssid = JsonParamString;

                Task<string> task_get;
                AESAlgoritham aes = new AESAlgoritham();
                //Fetch user details with access role and accessible areas
                DateTime now = DateTime.Now;
                using (chikaraksEntities dbuser = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    task_get = dbuser.users.Where(ses => ses.sessionExpireyDateTime != null && ses.sessionExpireyDateTime > now && ses.sessionID == ssid).Select(ses => ses.sessionValue).SingleOrDefaultAsync();
                }
                //Run both query parellel
                await Task.WhenAll(task_get);
                string sessionValue = task_get.Result;

                if (!string.IsNullOrWhiteSpace(sessionValue))
                {
                    cm = JsonConvert.DeserializeObject<SessionCM>(HttpUtility.UrlDecode(sessionValue));
                    return JsonConvert.SerializeObject(cm);
                }
                else
                {
                    return JsonConvert.SerializeObject(null);
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
                return JsonConvert.SerializeObject(cm);
            }
        }

        /// <summary>
        /// Async
        /// Sign out from account using Secure Session Identifier.
        /// </summary>
        /// <param name="JsonParamString.SessionCM"></param>
        /// <returns>JsonResult.Data.ErrorCM</returns>
        public async Task<string> LogoutAsync(string JsonParamString)
        {
            try
            {
                //string JsonParamString


                SessionCM sessionObj = JsonConvert.DeserializeObject<SessionCM>(JsonParamString);
                Task<user> task_get;
                AESAlgoritham aes = new AESAlgoritham();
                //Fetch user details with access role and accessible areas
                DateTime now = DateTime.Now;
                using (chikaraksEntities dbuser = new UniversDbContext().chikaraksDbContext(false, false))
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
                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = 0 });
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
                return JsonConvert.SerializeObject(new ErrorCM() { errorLogId = logid });
            }
        }
        /// <summary>
        /// Async
        /// Returns Captcha.Key as Value,Value as Captch Image
        /// </summary>
        /// <returns>JsonResult.Data.CaptchaCCM</returns>
        public async Task<string> Captcha()
        {
            CaptchaCCM cm = new CaptchaCCM();
            try
            {
                Captcha captcha = new Captcha();
                cm.kvp = await Task.FromResult(captcha.ProcessRequest());
                return JsonConvert.SerializeObject(cm);
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
                return JsonConvert.SerializeObject(cm);
            }
        }
    }
}