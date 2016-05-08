using LionPOS.Models.ViewModels.Login.NeedHelp;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using LionPOSServiceContractModels.ControllerContractModel.Login.NeedHelp;
using LionPOSServiceContractModels.ErrorContactModel;
using LionPOSServiceContractModels.Login.NeedHelp;
using LionPOSServiceOperationLayer.Login;
using LionPOSServiceOperationLayer.Maintenance;
using LionStartUp.ControllerHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LionPOS.Controllers.LoginControllers
{
    public class NeedHelpController : LoginBaseController
    {
        // GET: NeedHelp
        public ActionResult RecoveryOptions(string branchCode)
        {
            RecoveryOptionsViewModel viewModel = new RecoveryOptionsViewModel();
            try
            {
                if (string.IsNullOrEmpty(branchCode))
                {
                    throw new Exception("Add Branch code in URL. e.g http://lionvisinits.com/Login/Index/LionVision");
                }

                viewModel.branchCode = branchCode;
                viewModel.AlertList_string = new List<string>();
                return View("~/Views/Login/NeedHelp/RecoveryOptions.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    viewModel.branchCode,
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessageForUser,
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


                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryOptions(RecoveryOptionsViewModel submitModel)
        {
            try
            {
                NeedHelpCCM sm = new NeedHelpCCM();
                AccountProfileViewModel viewModel = new AccountProfileViewModel();
                NeedHelpServices ls = new NeedHelpServices();
                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                switch (submitModel.RecoveryOptions)
                {
                    case ConstantDictionaryVM.RecoveryOption.RecoverLostPassword_string:
                        GetUserDetailsSubmitCCM cm = new GetUserDetailsSubmitCCM();
                        cm.username = submitModel.Username;
                        cm.branchCode = submitModel.branchCode;

                        lkvp = new List<KeyValuePair<string, string>>();
                        lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                        sm = (await ls.GetUserDetailsAsync(new FormDataCollection(lkvp))).Data as NeedHelpCCM;
                        if (sm.errorLogId > 0)
                        {
                            throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sm.errorLogId);
                        }
                        sm.RecoveryOptionSelected = ConstantDictionaryVM.RecoveryOption.RecoverLostPassword_string;
                        break;
                    case ConstantDictionaryVM.RecoveryOption.RecoverLostUsername_string:
                        GetListOfAccountsByEamilSubmitCCM hm = new GetListOfAccountsByEamilSubmitCCM();
                        hm.email = submitModel.email;
                        hm.branchCode = submitModel.branchCode;
                        lkvp = new List<KeyValuePair<string, string>>();
                        lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(hm)));
                        sm = (await ls.GetListOfAccountsByEamilAsync(new FormDataCollection(lkvp))).Data as NeedHelpCCM;
                        if (sm.errorLogId > 0)
                        {
                            throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sm.errorLogId);
                        }
                        sm.RecoveryOptionSelected = ConstantDictionaryVM.RecoveryOption.RecoverLostUsername_string;
                        break;
                    case ConstantDictionaryVM.RecoveryOption.OtherQueries_string:
                        sm.RecoveryOptionSelected = ConstantDictionaryVM.RecoveryOption.OtherQueries_string;
                        return RedirectPermanent(ConstantDictionaryVM.DeveloperContactUsWebsite_string);
                }
                //Send NeedHelp View Model
                // SR 17-MAR-2016 Modified For Mandetory User name or Email address.
                if ((sm.user == null && submitModel.RecoveryOptions == ConstantDictionaryVM.RecoveryOption.RecoverLostPassword_string && submitModel.Username != null) || (submitModel.RecoveryOptions == ConstantDictionaryVM.RecoveryOption.RecoverLostUsername_string && sm.userList == null && submitModel.email != null))
                {
                    if (submitModel.AlertList_string == null)
                    {
                        submitModel.AlertList_string = new List<string>();
                    }
                    submitModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                     " No Valid Account Found! Please try again.", true));
                    submitModel.Username = "";
                    submitModel.email = "";

                }

                else if (sm.user == null && submitModel.RecoveryOptions == ConstantDictionaryVM.RecoveryOption.RecoverLostPassword_string && submitModel.Username == null)
                {
                    if (submitModel.AlertList_string == null)
                    {
                        submitModel.AlertList_string = new List<string>();
                    }
                    submitModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                     " Please Enter the Username. Try again.", true));
                    submitModel.Username = "";
                    submitModel.email = "";
                }

                else if (submitModel.RecoveryOptions == ConstantDictionaryVM.RecoveryOption.RecoverLostUsername_string && sm.userList == null && submitModel.email == null)
                {
                    if (submitModel.AlertList_string == null)
                    {
                        submitModel.AlertList_string = new List<string>();
                    }
                    submitModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                     " Please Enter an Email address. Try again.", true));
                    submitModel.Username = "";
                    submitModel.email = "";
                }
                Session[ConstantDictionaryVM.RecoverySession_string] = sm;
                if ((sm.user == null && submitModel.RecoveryOptions == ConstantDictionaryVM.RecoveryOption.RecoverLostPassword_string) || (submitModel.RecoveryOptions == ConstantDictionaryVM.RecoveryOption.RecoverLostUsername_string && sm.userList == null))
                {
                    return View("~/Views/Login/NeedHelp/RecoveryOptions.cshtml", submitModel);
                }
                else
                {
                    viewModel.profilePicture = sm.employee.profilePicture;
                    viewModel.name = sm.employee.firstName + " " + sm.employee.sureName;

                    var email = sm.employee.emialAddress;
                    string[] semial = email.Split('@');
                    string[] ssemial = semial[1].Split('.');
                    string emailStared = "";
                    emailStared += semial[0].Substring(0, semial[0].Length / 2);

                    for (int i = 0; i <= semial[0].Length / 2; i++)
                    {
                        emailStared += ("*");
                    }
                    emailStared += ("@");
                    emailStared += ssemial[0].Substring(0, ((ssemial[0].Length - 1) / 2));
                    for (int i = 0; i <= ((ssemial[0].Length - 1) / 2); i++)
                    {
                        emailStared += ("*");
                    }
                    emailStared += (".");
                    for (int i = 0; i < ((ssemial[1].Length - 1) / 2); i++)
                    {
                        emailStared += ("*");
                    }
                    emailStared += ssemial[1].Substring((ssemial[1].Length - 1) / 2, ssemial[1].Length - 1);

                    viewModel.emailAddress = emailStared;
                    viewModel.contactNo = "*******" + sm.employee.contactNo.Substring(sm.employee.contactNo.Length - 3, 3);
                    viewModel.branchCode = sm.employee.employeeEntryBranchCode;

                    TempData["AccountProfileViewModel"] = viewModel;
                    return RedirectToAction("AccountProfile");
                }
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    submitModel.branchCode,
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessageForUser,
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

                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }

        public ActionResult AccountProfile()
        {
            AccountProfileViewModel viewModel = new AccountProfileViewModel();
            try
            {

                viewModel = TempData["AccountProfileViewModel"] as AccountProfileViewModel;
                return View("~/Views/Login/NeedHelp/AccountProfile.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                     viewModel.branchCode,
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessageForUser,
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

                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyOTP(AccountProfileViewModel submitModel)
        {
            NeedHelpCCM sm = null;
            try
            {
                NeedHelpServices ls = new NeedHelpServices();
                VerifyOTPViewModel viewModel = new VerifyOTPViewModel();
                sm = Session[ConstantDictionaryVM.RecoverySession_string] as NeedHelpCCM;
                viewModel.branchCode = submitModel.branchCode;
                if (submitModel.recoveryMode == ConstantDictionaryCM.AccountOTPRecoveryTypes.Email)
                {
                    viewModel.recoveryUsed = submitModel.emailAddress;
                }
                else
                {
                    viewModel.recoveryUsed = submitModel.contactNo;
                }
                viewModel.name = submitModel.name;
                viewModel.profilePicture = submitModel.profilePicture;
                sendOTPForVerificationAccountOwnerShipSubmitCCM cm = new sendOTPForVerificationAccountOwnerShipSubmitCCM();
                cm.RecoveryOptionSelected = sm.RecoveryOptionSelected;
                cm.employeeCode = sm.employee.employeeCode;
                cm.employeeEntryBranchCode = sm.employee.employeeEntryBranchCode;
                cm.branchCode = sm.employee.employeeEntryBranchCode;
                cm.PasswordResetTemplatePath = Server.MapPath(ConstantDictionaryVM.PasswordReset_Email_Template_File_Path);
                cm.recoveryMode = submitModel.recoveryMode;

                List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                lkvp = new List<KeyValuePair<string, string>>();
                lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                OTPSessionCCM ssm = (await ls.sendOTPVerificationForAccountOwnershipAsync(new FormDataCollection(lkvp))).Data as OTPSessionCCM;
                if (ssm.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ssm.errorLogId);
                }
                Session[ConstantDictionaryVM.OTPSessionValue_string] = ssm;
                //Send VerifyAccountOwnerShipByOTP View Model
                return View("~/Views/Login/NeedHelp/VerifyOTP.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    (sm != null) ? sm.employee.employeeEntryBranchCode : "",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessageForUser,
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

                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recovery(VerifyOTPViewModel submitModel)
        {
            try
            {
                NeedHelpCCM sm = Session[ConstantDictionaryVM.RecoverySession_string] as NeedHelpCCM;
                if (sm != null)
                {

                    OTPSessionCCM otp = new OTPSessionCCM();
                    otp = Session[ConstantDictionaryVM.OTPSessionValue_string] as OTPSessionCCM;
                    if (submitModel.verifyOTP != otp.oneTimePassword || otp.oneTimePasswordTimeOut < DateTime.Now || otp.isActiveOneTimePassword == false)
                    {
                        if (submitModel.AlertList_string == null)
                        {
                            submitModel.AlertList_string = new List<string>();
                        }
                        submitModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                         "You have entrered wrong OTP Or OTP is expired!", true));
                        return View("~/Views/Login/NeedHelp/VerifyOTP.cshtml", submitModel);
                    }
                    else
                    {


                        RecoveryViewModel viewModel = new RecoveryViewModel();
                        viewModel.branchCode = submitModel.branchCode;
                        if (sm.RecoveryOptionSelected == ConstantDictionaryVM.RecoveryOption.RecoverLostPassword_string)
                        {
                            viewModel.username = sm.user.userName;
                            TempData["ResetPasswordViewModel"] = viewModel;
                            return RedirectToAction("ResetPasswordForm");
                        }
                        else
                        {
                            viewModel.listUsers = sm.userList.Select(a => a.userName).ToList();
                            return View("~/Views/Login/NeedHelp/ListLostUsers.cshtml", viewModel);
                        }
                    }
                }
                return RedirectToAction("Index", "Login", new { branchCode = submitModel.branchCode });
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    submitModel.branchCode,
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessageForUser,
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

                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }

        [HttpGet]
        public async Task<ActionResult> ResetPasswordForm()
        {
            RecoveryViewModel submitModel = new RecoveryViewModel();
            try
            {
                NeedHelpCCM sm = Session[ConstantDictionaryVM.RecoverySession_string] as NeedHelpCCM;
                NeedHelpServices ls = new NeedHelpServices();
                submitModel = TempData["ResetPasswordViewModel"] as RecoveryViewModel;
                if (sm != null)
                {
                    GetUserDetailsSubmitCCM cm = new GetUserDetailsSubmitCCM();
                    cm.username = submitModel.username;
                    cm.branchCode = submitModel.branchCode;

                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                    NeedHelpCCM nhc = (await ls.GetUserDetailsAsync(new FormDataCollection(lkvp))).Data as NeedHelpCCM;
                    if (nhc.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + nhc.errorLogId);
                    }
                    sm.user = nhc.user;
                    Session[ConstantDictionaryVM.RecoverySession_string] = sm;
                    return View("~/Views/Login/NeedHelp/ResetPassword.cshtml", submitModel);
                }
                return RedirectToAction("Index", "Login", new { branchCode = submitModel.branchCode });
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    submitModel.branchCode,
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessageForUser,
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

                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordForm(RecoveryViewModel submitModel)
        {

            try
            {
                NeedHelpCCM sm = Session[ConstantDictionaryVM.RecoverySession_string] as NeedHelpCCM;
                NeedHelpServices ls = new NeedHelpServices();

                if (sm != null)
                {
                    GetUserDetailsSubmitCCM cm = new GetUserDetailsSubmitCCM();
                    cm.username = submitModel.username;
                    cm.branchCode = submitModel.branchCode;
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                    NeedHelpCCM nhc = (await ls.GetUserDetailsAsync(new FormDataCollection(lkvp))).Data as NeedHelpCCM;
                    if (nhc.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + nhc.errorLogId);
                    }
                    sm.user = nhc.user;
                    Session[ConstantDictionaryVM.RecoverySession_string] = sm;
                    return View("~/Views/Login/NeedHelp/ResetPassword.cshtml", submitModel);
                }
                return RedirectToAction("Index", "Login", new { branchCode = submitModel.branchCode });
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    submitModel.branchCode,
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessageForUser,
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

                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(RecoveryViewModel submitModel)
        {
            try
            {
                NeedHelpCCM sm = Session[ConstantDictionaryVM.RecoverySession_string] as NeedHelpCCM;
                if (sm != null)
                {
                    NeedHelpServices ls = new NeedHelpServices();
                    OTPSessionCCM otp = new OTPSessionCCM();
                    otp = Session[ConstantDictionaryVM.OTPSessionValue_string] as OTPSessionCCM;
                    if (submitModel.password != submitModel.confirmPassword)
                    {
                        if (submitModel.AlertList_string == null)
                        {
                            submitModel.AlertList_string = new List<string>();
                        }
                        submitModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                         "New Password and confirm password do not match! Ple. Re-try!", true));
                        return View("~/Views/Login/NeedHelp/ResetPassword.cshtml", submitModel);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(submitModel.password))
                        {
                            ResetPasswordSubmitCCM cm = new ResetPasswordSubmitCCM();
                            cm.username = sm.user.userName;
                            cm.password = submitModel.password;
                            cm.branchCode = submitModel.branchCode;


                            List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                            lkvp = new List<KeyValuePair<string, string>>();
                            lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, JsonConvert.SerializeObject(cm)));
                            ErrorCM nhc = (await ls.resetPasswordAsync(new FormDataCollection(lkvp))).Data as ErrorCM;
                            if (nhc.errorLogId > 0)
                            {
                                throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + nhc.errorLogId);
                            }
                        }
                    }
                }
                return RedirectToAction("Index", "Login", new { message = "Password Reset Sucessfully. :-)" });
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    submitModel.branchCode,
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessageForUser,
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
                return RedirectToAction("Index", "ErrorHandler", new { logid = logid });
            }
        }




    }

}