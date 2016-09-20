using Chikaraks.ConstantDictionaryViewModel;
using Chikaraks.Models.BackEndModels.Login.Login;
using Chikaraks.Models.ViewModels.Login.Login;
using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using ChikaraksServiceContractModels.ControllerContractModel.Login.Login;
using ChikaraksServiceContractModels.ErrorContactModel;

using ChikaraksServiceOperationLayer.Maintenance;
using LionPOSServiceOperationLayer.Login;
using Newtonsoft.Json;
using StartUp.ControllerHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Chikaraks.Controllers.LoginControllers
{
    public class LoginController : LoginBaseController
    {

        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult> Index(string message = "")
        {


            LoginViewModel loginViewModel = new LoginViewModel();
            //Set BranchCode

            try
            {
                if (ConstantDictionaryCM.StartUpErrorLog_int > 0)
                {
                    ConstantDictionaryCM.StartUpErrorLog_int = 0;
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ConstantDictionaryCM.StartUpErrorLog_int);
                }
                //Set to do not show login window directly,instead it will show previous logged in persons selection
                //It is used in LionPOS\Views\Login\Login\Index.cshtml
                loginViewModel.showLoginWindowDirect = false;

                //create session from cookie for previous account
                if (Request.Cookies.Get(ConstantDictionaryVM.PreviousAccountsLogin_string) != null)
                {
                    HttpCookie cookie = Request.Cookies[ConstantDictionaryVM.PreviousAccountsLogin_string];
                    string sessionJsonString = cookie.Value;
                    if (sessionJsonString != null)
                    {
                        List<PreviousAccountsCookiesBackEndModel> sml = JsonConvert.DeserializeObject<List<PreviousAccountsCookiesBackEndModel>>(HttpUtility.UrlDecode(sessionJsonString));
                        loginViewModel.previousAccountsLogins = sml;
                    }
                }

                

                if (!string.IsNullOrEmpty(message))
                {
                    if (loginViewModel.AlertList_string == null)
                    {
                        loginViewModel.AlertList_string = new List<string>();
                    }
                    loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.success, message, true));
                }

                // Following Code generate Random background images for login page
                loginViewModel.RandomImageStyleURL_string = await Task.Run(() => getRandomImageURL());

                return View("~/Views/Login/Login/Index.cshtml", loginViewModel);
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessage,
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

        public string getRandomImageURL()
        {
            try
            {
                if (Directory.Exists(Server.MapPath(ConstantDictionaryVM.LoginBackgroundServerMapPath_string)))
                {
                    FileInfo[] images = new DirectoryInfo(Server.MapPath(ConstantDictionaryVM.LoginBackgroundServerMapPath_string)).GetFiles();
                    int min = 0;
                    int max = images.Length - 1;
                    Random r = new Random();
                    int index = r.Next(min, max);
                    string tmp = images[index].FullName.Split('\\').LastOrDefault().ToString();
                    if (!String.IsNullOrWhiteSpace(tmp))
                    {

                        string str = ("style=\"background:url('" + ConstantDictionaryVM.LoginBackgroundViewPath_string + tmp + "') no-repeat fixed center center;    background-size: cover;\"");
                        return str;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessage,
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

                throw ex;
            }
        }



        //Check lists To-Do in Login
        //Check list before submiting to services
        //1)Create page session and add user if do not exist in login page session
        //2)Call Login Process Service
        //Check list to do if user is authorised from service.
        //3)Remove user from login page session
        //4)If remember me ticked then set cookies
        //5)Add user to previous login page ,if it is not in session page.
        //6)Call Dashboard View
        //Check list to do if user is not authorised.
        //7)set page to load direct login dialog
        //8)Call Login View
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {

                LoginPageSessionBackEndModel lpsm;

                //1)Create page session and add user if do not exist in login page session
                if (Session[ConstantDictionaryVM.LoginPageSession_string] == null)
                {
                    //Create List of username for trail session
                    List<LoginPageSessionBackEndModel> lpsml = new List<LoginPageSessionBackEndModel>();
                    lpsm = new LoginPageSessionBackEndModel() { requestCount = 1, username = loginViewModel.username };
                    lpsml.Add(lpsm);
                    Session[ConstantDictionaryVM.LoginPageSession_string] = lpsml;

                }
                else
                {
                    //Add username is it do not exist in trials
                    List<LoginPageSessionBackEndModel> lpsml = Session[ConstantDictionaryVM.LoginPageSession_string] as List<LoginPageSessionBackEndModel>;
                    lpsm = lpsml.Where(a => a.username == loginViewModel.username).SingleOrDefault();
                    if (lpsm == null)
                    {
                        lpsm = new LoginPageSessionBackEndModel() { requestCount = 1, username = loginViewModel.username};
                        lpsml.Add(lpsm);
                    }
                    else
                    {
                        //Increament trails
                        lpsm.requestCount++;
                    }
                    Session[ConstantDictionaryVM.LoginPageSession_string] = lpsml;
                }
                //2)Call Login Process Service
                string captchaValue = ((Session[ConstantDictionaryVM.CaptchaValueSession_string]) != null) ? (Session[ConstantDictionaryVM.CaptchaValueSession_string]).ToString() : "";

                //Either captcha value matcha when captcha is active or captcha is not match will go in
                if ((lpsm.isCaptchaActive == true && loginViewModel.captcha == captchaValue) || lpsm.isCaptchaActive == false)
                {

                    LoginServices hs = new LoginServices();
                    GetSessionDetailsSubmitCCM cm = new GetSessionDetailsSubmitCCM();
                    cm.username = loginViewModel.username;
                    cm.password = loginViewModel.password;
                    cm.requestCount = lpsm.requestCount;

                    //Check user is authorised or not
                    SessionCM sm = JsonConvert.DeserializeObject<SessionCM>(await hs.GetSessionDetailsAsync(JsonConvert.SerializeObject(cm)));

                    if (sm.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sm.errorLogId);
                    }
                    if (sm.isAuthorised == false)
                    {

                        //Get captcha and block attempts values from settings Not from Database
                        bool captch = true;
                        bool block = true;
                        int captachCount = ConstantDictionaryCM.captachCount;
                        int blockCount = ConstantDictionaryCM.blockCount;

                        lpsm.block = block;
                        lpsm.captch = captch;
                        lpsm.captachCount = captachCount;
                        lpsm.blockCount = blockCount;
                        //Captcha attpts exceeds then captcha gets activated
                        if (lpsm.requestCount >= lpsm.captachCount && lpsm.captch == true)
                        {
                            loginViewModel.CaptchaActivated_Null_bool = true;
                            lpsm.isCaptchaActive = true;
                        }
                        //As count exceeds for block it gose and store account status to block mode
                        if (lpsm.blockCount == lpsm.requestCount && lpsm.block == true)
                        {

                            SessionCM ssm = JsonConvert.DeserializeObject<SessionCM>(await hs.GetSessionDetailsAsync(JsonConvert.SerializeObject(cm)));
                            if (ssm.errorLogId > 0)
                            {
                                throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ssm.errorLogId);
                            }


                            loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                                 "Account is blocked due to too many failure attempt! Contact Admin.", true));

                        }
                        else if (lpsm.requestCount >= lpsm.blockCount && lpsm.block == true)
                        {
                            loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                                 "Account is blocked due to too many failure attempt! Contact Admin.", true));
                        }


                        //if found then user will not be empty
                        if (sm.user.userName != null)
                        {
                            //Check is account is blocked then show preoper message
                            if (sm.user.userAccountStatus == ConstantDictionaryCM.AccountStatus.Blocked)
                            {
                                string blockedDuration = "";
                                if (sm.user.blockExpiry != null)
                                {
                                    blockedDuration = "Account blocked till " + sm.user.blockExpiry.ToString() + "!";
                                }
                                else
                                {
                                    blockedDuration = "Account blocked for permanently,Ple.contact your admin for unlock it!";
                                }
                                if (loginViewModel.AlertList_string == null)
                                {
                                    loginViewModel.AlertList_string = new List<string>();
                                }
                                loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                                 blockedDuration, true));
                            }
                            else
                            {
                                //user found but was not authorised as password was not matching
                                if (loginViewModel.AlertList_string == null)
                                {
                                    loginViewModel.AlertList_string = new List<string>();
                                }
                                loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                                 "Authentication failed! Username or password incorrect!", true));
                            }
                        }
                        else
                        {
                            //Account not found error
                            if (loginViewModel.AlertList_string == null)
                            {
                                loginViewModel.AlertList_string = new List<string>();
                            }
                            loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                             "Account not found!!", true));
                        }

                    }
                    else
                    {
                        //3)Remove user from login page session
                        List<LoginPageSessionBackEndModel> lpsml = Session[ConstantDictionaryVM.LoginPageSession_string] as List<LoginPageSessionBackEndModel>;
                        lpsm = lpsml.Where(a => a.username == loginViewModel.username).SingleOrDefault();
                        lpsml.Remove(lpsm);
                        Session[ConstantDictionaryVM.LoginPageSession_string] = lpsml;

                        //4)If remember me ticked then set cookies
                        //Add Session to Remember me cookie so next time they open webpage,if there logged in expiry is not expired then they are logged in directly.
                        if (loginViewModel.remember == true)
                        {
                            HttpCookie Cookies = new HttpCookie(ConstantDictionaryVM.RememberMe_string);
                            Cookies.Value = sm.user.sessionID;
                            Cookies.Expires = sm.user.sessionExpireyDateTime.Value;
                            Response.Cookies.Add(Cookies);
                        }
                        //Set same expiry to Server Session
                        TimeSpan span = sm.user.sessionExpireyDateTime.Value - DateTime.Now;
                        Session.Timeout = Convert.ToInt32(span.TotalMinutes);
                        Session[ConstantDictionaryVM.MainSession_string] = sm.user.sessionID;
                        int iSizesm = System.Text.UTF8Encoding.UTF8.GetByteCount(sm.user.sessionID);
                        //5)Add user to previous login page ,if it is not in session page.
                        //Check that any previous logged in account exists,if not then create and add current user as previous logged in.
                        if (Request.Cookies[ConstantDictionaryVM.PreviousAccountsLogin_string] != null)
                        {
                            //Modefy old cookies
                            HttpCookie cookie = Request.Cookies[ConstantDictionaryVM.PreviousAccountsLogin_string];
                            string sessionJsonString = cookie.Value;
                            if (sessionJsonString != null)
                            {
                                List<PreviousAccountsCookiesBackEndModel> sml = JsonConvert.DeserializeObject<List<PreviousAccountsCookiesBackEndModel>>(HttpUtility.UrlDecode(sessionJsonString));
                                if (sml.Where(a => a.username == sm.user.userName).SingleOrDefault() == null)
                                {
                                    sml.Add(new PreviousAccountsCookiesBackEndModel { username = sm.user.userName});
                                }
                                sessionJsonString = JsonConvert.SerializeObject(sml);
                                cookie.Value = HttpUtility.UrlEncode(sessionJsonString);
                                cookie.Expires = DateTime.Now.AddYears(1);
                                Response.Cookies[ConstantDictionaryVM.PreviousAccountsLogin_string].Value = cookie.Value;
                                Response.Cookies[ConstantDictionaryVM.PreviousAccountsLogin_string].Expires = cookie.Expires;

                            }
                        }
                        else
                        {
                            //Add cookies
                            List<PreviousAccountsCookiesBackEndModel> cml = new List<PreviousAccountsCookiesBackEndModel>();
                            cml.Add(new PreviousAccountsCookiesBackEndModel { username = sm.user.userName});
                            HttpCookie Cookies = new HttpCookie(ConstantDictionaryVM.PreviousAccountsLogin_string);
                            string sessionJsonString = JsonConvert.SerializeObject(cml);
                            //int iSize = System.Text.UTF8Encoding.UTF8.GetByteCount(sessionJsonString);
                            //if (iSize > 4096)
                            //{
                            //}
                            Cookies.Value = HttpUtility.UrlEncode(sessionJsonString);
                            Cookies.Expires = DateTime.Now.AddYears(1);
                            Response.Cookies.Add(Cookies);


                        }

                        //6)Call Dashboard View
                        return RedirectToAction("Index", "Home");

                    }
                }
                else
                {

                    //Captcha was active but its value did not matched 
                    if (loginViewModel.AlertList_string == null)
                    {
                        loginViewModel.AlertList_string = new List<string>();
                    }
                    if (lpsm.requestCount >= lpsm.captachCount && lpsm.captch == true)
                    {
                        loginViewModel.CaptchaActivated_Null_bool = true;
                        lpsm.isCaptchaActive = true;
                        loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.warning,
                             "Captcha do not match! Please enter text same as shown in image.", true));
                    }
                    //As count exceeds for block it gose and store account status to block mode
                    if (lpsm.blockCount == lpsm.requestCount && lpsm.block == true)
                    {
                        LoginServices hs = new LoginServices();
                        GetSessionDetailsSubmitCCM cm = new GetSessionDetailsSubmitCCM();
                        
                        cm.username = loginViewModel.username;
                        cm.password = loginViewModel.password;
                        cm.requestCount = lpsm.requestCount;


                        SessionCM ssm = JsonConvert.DeserializeObject<SessionCM>(await hs.GetSessionDetailsAsync(JsonConvert.SerializeObject(cm)));
                        if (ssm.errorLogId > 0)
                        {
                            throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ssm.errorLogId);
                        }

                        loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                             "Account is blocked due to too many failure attempt! Contact Admin.", true));

                    }
                    else if (lpsm.requestCount > lpsm.blockCount && lpsm.block == true)
                    {
                        loginViewModel.AlertList_string.Add(new ConstantDictionaryVM.AlertCssModel().buildAlertString(ConstantDictionaryVM.AlertCssModel.AlertTypesCssClass.danger,
                             "Account is blocked due to too many failure attempt! Contact Admin.", true));
                    }



                }

                //7)set page to load direct login dialog
                //Set this to true to show login dialog direct
                loginViewModel.showLoginWindowDirect = true;
                //8)Call Login View
                return View("~/Views/Login/Login/Index.cshtml", loginViewModel);
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessage,
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
        public async Task<string> Captcha()
        {
            try
            {
                LoginServices hs = new LoginServices();
                CaptchaCCM cm = (JsonConvert.DeserializeObject<CaptchaCCM>(await hs.Captcha()));
                if (cm.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + cm.errorLogId);
                }
                string base64 = cm.kvp.Key;
                Session[ConstantDictionaryVM.CaptchaValueSession_string] = cm.kvp.Value;
                return base64;
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessage,
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


                throw ex;
            }
        }



        public async Task<ActionResult> Logout(Object sender, EventArgs e)
        {
            try
            {

                LoginServices ls = new LoginServices();
                ErrorCM ssm = (JsonConvert.DeserializeObject<SessionCM>(await ls.LogoutAsync(JsonConvert.SerializeObject(sessionObj))));
                if (ssm.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ssm.errorLogId);
                }
                Response.Cookies[ConstantDictionaryVM.RememberMe_string].Expires = DateTime.Now.AddDays(-1);
                Session.Abandon();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
                                                    "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    ConstantDictionaryVM.ErrorMessage,
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
            finally
            {
                Session.Abandon();
            }
        }
    }
}



