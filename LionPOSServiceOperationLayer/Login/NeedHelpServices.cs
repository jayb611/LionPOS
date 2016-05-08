using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Data;
using System.Net.Mail;
using DomainModels;
using System.Data.Entity;
using System.Threading.Tasks;
using LionUtilities;
using LionUtilities.SecurityPkg;
using LionPOSDbContracts.DomainModels.User;
using LionPOSServiceContractModels;
using LionPOSDbContracts.DomainModels.Employee;
using LionPOSDbContracts.DomainModels.Configuration;
using System.Web;
using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels.Login.NeedHelp;
using LionPOSServiceContractModels.ControllerContractModel.Login.NeedHelp;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using System.Web.Script.Serialization;
using System.Net.Http.Formatting;
using System.Web.Mvc;
using LionPOSServiceContractModels.ErrorContactModel;

namespace LionPOSServiceOperationLayer.Login
{

    /// <summary>
    /// 04-03-2016 Jb created GetSessionDetailsAsync
    /// </summary>
    public class NeedHelpServices
    {
        public NeedHelpServices()
        { }




        /// <summary>
        /// Gets user account details by employee email address
        /// </summary>
        /// <param name="JsonParamString.Data.GetListOfAccountsByEamilSubmitCCM"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetListOfAccountsByEamilAsync(FormDataCollection JsonParamString)
        {
            NeedHelpCCM sm = new NeedHelpCCM();
            try
            {

                //FormDataCollection JsonParamString
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                GetListOfAccountsByEamilSubmitCCM cm = new JavaScriptSerializer().Deserialize<GetListOfAccountsByEamilSubmitCCM>(JsonString);

                Task<employeeSCM> taskEmployee_t1_p2;
                //Fetch Employee Details
                using (employeelvitsposdbEntities dbemp = new UniversDbContext().employeeDbContext(false, false))
                {
                    taskEmployee_t1_p2 = dbemp.employees.
                        Where(
                        employee => employee.emialAddress == cm.email &&
                        employee.employeeEntryBranchCode == cm.branchCode &&
                        employee.employeeEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode
                        ).Select(employee => new employeeSCM
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
                //Run both query parellel
                await Task.WhenAll(taskEmployee_t1_p2);
                //Store Parellel thread results
                sm.employee = taskEmployee_t1_p2.Result;

                //Check is not null or not found
                if (sm.employee != null)
                {
                    Task<List<userSCM>> taskUser_t2_p1;

                    using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
                    {
                        taskUser_t2_p1 = dbuser.users.Where(
                                    user => user.employeeCode == sm.employee.employeeCode &&
                                    user.employeeEntryBranchCode == sm.employee.employeeEntryBranchCode &&
                                    user.employeeEntryGroupCode == sm.employee.employeeEntryGroupCode
                                    ).Select(user => new userSCM
                                    {
                                        accessRoleTitle = user.accessRoleTitle,
                                        blockExpiry = user.blockExpiry,
                                        employeeCode = user.employeeCode,
                                        employeeEntryBranchCode = user.employeeEntryBranchCode,
                                        employeeEntryGroupCode = user.employeeEntryGroupCode,
                                        lastLogin = user.lastLogin,
                                        lastPasswordResetDate = user.lastPasswordResetDate,
                                        sessionCreateTime = user.sessionCreateTime,
                                        sessionExpireyDateTime = user.sessionExpireyDateTime,
                                        sessionID = user.sessionID,
                                        sessionValue = user.sessionValue,
                                        userAccountStatus = user.userAccountStatus,
                                        userEntryBranchCode = user.userEntryBranchCode,
                                        userEntryGroupCode = user.userEntryGroupCode,
                                        userName = user.userName,
                                        userStatus = user.userStatus,
                                        password = user.password,
                                        passwordEncryptionKey = user.passwordEncryptionKey
                                    }
                                    )
                                    .ToListAsync();
                        //Fetch all Access Areas
                    }

                    await Task.WhenAll(taskUser_t2_p1);
                    sm.userList = taskUser_t2_p1.Result;
                    foreach (var user in sm.userList)
                    {
                        user.password = "";
                        user.passwordEncryptionKey = "";

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
        /// Get user detail asynchronusly bu username
        /// </summary>
        /// <param name="JsonParamString.Data.GetUserDetailsSubmitCCM"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetUserDetailsAsync(FormDataCollection JsonParamString)
        {
            NeedHelpCCM sm = new NeedHelpCCM();
            try
            {
                //FormDataCollection JsonParamString
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                GetUserDetailsSubmitCCM cm = new JavaScriptSerializer().Deserialize<GetUserDetailsSubmitCCM>(JsonString);

                Task<user> taskUser_t1_p2;
                //Fetch user details with access role and accessible areas
                using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
                {
                    taskUser_t1_p2 = dbuser.users.
                        Where(
                        user => user.userName == cm.username &&
                        user.userEntryBranchCode == cm.branchCode &&
                        user.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode
                        )
                        .SingleOrDefaultAsync();

                }
                //Run both query parellel
                await Task.WhenAll(taskUser_t1_p2);
                //Store Parellel thread results
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

                }


                //Check is not null or not found
                if (sm.user != null)
                {
                    Task<employeeSCM> taskEmployee_t2_p1;

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
                            emialAddress = employee.emialAddress,
                            contactNo = employee.contactNo1
                        }).SingleOrDefaultAsync();
                    }

                    await Task.WhenAll(taskEmployee_t2_p1);
                    sm.employee = taskEmployee_t2_p1.Result;
                    sm.user.password = "";
                    sm.user.passwordEncryptionKey = "";
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
        /// Send OTP to employee by its recovery options selection
        /// </summary>
        /// <param name="JsonParamString.Data.sendOTPForVerificationAccountOwnerShipSubmitCCM"></param>
        /// <returns></returns>
        public async Task<JsonResult> sendOTPVerificationForAccountOwnershipAsync(FormDataCollection JsonParamString)
        {
            OTPSessionCCM otp = new OTPSessionCCM();

            try
            {
                //FormDataCollection JsonParamString
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                sendOTPForVerificationAccountOwnerShipSubmitCCM cm = new JavaScriptSerializer().Deserialize<sendOTPForVerificationAccountOwnerShipSubmitCCM>(JsonString);

                Utilities util = new Utilities();
                employee employee;
                List<setting> settings;
                using (configurationlvitsposdbEntities dbconf = new UniversDbContext().configurationDbContext(false, false))
                {
                    settings = dbconf.settings.Where(conf => conf.branchCode == cm.branchCode).ToList();
                }

                using (employeelvitsposdbEntities dbemp = new UniversDbContext().employeeDbContext())
                {
                    employee = dbemp.employees.Where(
                                    u => u.employeeCode == cm.employeeCode &&
                                    u.employeeEntryBranchCode == cm.employeeEntryBranchCode &&
                                    u.employeeEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode
                                    ).SingleOrDefault();
                    if (employee != null)
                    {
                        employee.isActiveOneTimePassword = true;
                        employee.oneTimePassword = util.generateOTP(false, false, true, false, 4);
                        employee.oneTimePasswordTimeOut = DateTime.Now.AddMinutes(10);
                        dbemp.SaveChanges();
                        otp.isActiveOneTimePassword = employee.isActiveOneTimePassword;
                        otp.oneTimePassword = employee.oneTimePassword;
                        otp.oneTimePasswordTimeOut = employee.oneTimePasswordTimeOut;
                    }
                }
                if (employee != null)
                {
                    if (
                        (cm.recoveryMode == ConstantDictionaryCM.AccountOTPRecoveryTypes.Mobile && (employee.contactType1 == ConstantDictionaryCM.contact_type.HomeMobile || employee.contactType1 == ConstantDictionaryCM.contact_type.OfficeMobile || employee.contactType1 == ConstantDictionaryCM.contact_type.PrimaryMobile || employee.contactType1 == ConstantDictionaryCM.contact_type.NeighboursMobile || employee.contactType1 == ConstantDictionaryCM.contact_type.WhatsApp)) ||
                        (!string.IsNullOrWhiteSpace(employee.emialAddress) && cm.recoveryMode == ConstantDictionaryCM.AccountOTPRecoveryTypes.Email))
                    {

                        if (cm.recoveryMode == ConstantDictionaryCM.AccountOTPRecoveryTypes.Mobile)
                        {
                            string SMSusername = "jayb611";
                            string SMSpassword = "emineme12";
                            string SMSsenderID = "LVnITS";
                            string SMSto = employee.contactNo1;
                            string SMSmessage = HttpUtility.UrlEncode("Your OTP is : " + otp.oneTimePassword + "\n OTP will expire in 10 min.");
                            string SMSType = "3";
                            string strUrl = "http://login.bulksmsgateway.in/sendmessage.php?user=" + SMSusername + "&password=" + SMSpassword + "&message=" + SMSmessage + "&sender=" + SMSsenderID + "& mobile=" + SMSto + "&type=" + SMSType;
                            WebRequest request = HttpWebRequest.Create(strUrl);
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            Stream s = (Stream)response.GetResponseStream();
                            StreamReader readStream = new StreamReader(s);
                            string dataString = await Task.FromResult( readStream.ReadToEnd());
                            response.Close();
                            s.Close();
                            readStream.Close();
                        }
                        else
                        {

                            /***************** Following is for sending OTP  to the user email Id *******************/

                            string subject = "OTP For Verification From Lion Vision ITS";
                            string Emailbody = File.ReadAllText(cm.PasswordResetTemplatePath);
                            string MessageBody = Emailbody.Replace("123", otp.oneTimePassword);
                            string senderID = settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Support_Email_Address.title).Select(a => a.values).SingleOrDefault();// use sender’s email id here..
                            string senderPassword = settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Support_Email_Password.title).Select(a => a.values).SingleOrDefault(); // sender password here…
                            SmtpClient smtp = new SmtpClient();

                            smtp.Host = settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Outgoing_Server_SMTP.title).Select(a => a.values).SingleOrDefault();
                            smtp.Port = Convert.ToInt32(settings.Where(a => a.title == ConstantRecordsDictionaryCM.Setting_Seeds.Outgoing_Server_SMTP_PORT.title).Select(a => a.values).SingleOrDefault());
                            smtp.EnableSsl = false;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.Credentials = new NetworkCredential(senderID, senderPassword);
                            smtp.Timeout = 30000;

                            MailMessage message = new MailMessage(senderID, employee.emialAddress, subject, MessageBody);
                            message.IsBodyHtml = true;
                            await Task.Run(()=> smtp.Send(message));
                        }
                    }
                }
                return new JsonResult() { Data = otp, MaxJsonLength = Int32.MaxValue };

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
                otp.errorLogId = logid;
                return new JsonResult() { Data = otp, MaxJsonLength = Int32.MaxValue };
            }
        }

        /// <summary>
        /// Reset Passowrd of User by username
        /// </summary>
        /// <param name="JsonParamString.Data.ResetPasswordSubmitCCM"></param>
        /// <returns></returns>
        public async Task<JsonResult> resetPasswordAsync(FormDataCollection JsonParamString)
        {
            try
            {
                //FormDataCollection JsonParamString
                string JsonString = JsonParamString.Get(ConstantDictionaryCM.JsonParamString_string);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                ResetPasswordSubmitCCM cm = new JavaScriptSerializer().Deserialize<ResetPasswordSubmitCCM>(JsonString);

                Task<user> taskUser_t1_p2;
                //Fetch user details with access role and accessible areas

                using (userlvitsposdbEntities dbuser = new UniversDbContext().userDbContext(false, false))
                {
                    taskUser_t1_p2 = dbuser.users.
                        Where(
                        user => user.userName == cm.username &&
                        user.userEntryBranchCode == cm.branchCode &&
                        user.userEntryGroupCode == ConstantDictionaryCM.ApplicationGroupCode
                        )
                        .SingleOrDefaultAsync();
                    await Task.WhenAll(taskUser_t1_p2);

                    AESAlgoritham aesEnc = new AESAlgoritham(cm.password);
                    taskUser_t1_p2.Result.passwordEncryptionKey = aesEnc.key;
                    taskUser_t1_p2.Result.password = aesEnc.encryptedPwd;
                    dbuser.SaveChanges();
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