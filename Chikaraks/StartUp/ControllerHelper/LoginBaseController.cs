using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using ChikaraksServiceContractModels;
using ChikaraksServiceOperationLayer.Maintenance;

using Chikaraks.ConstantDictionaryViewModel;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using LionPOSServiceOperationLayer.Login;

namespace StartUp.ControllerHelper
{
    public class LoginBaseController : Controller
    {

        public string actionName { get; set; }
        public string controllerName { get; set; }
        public string BackgroundImageURL { get; set; }
        public SessionCM sessionObj { get; set; }
        protected async override void Initialize(RequestContext requestContext)
        {
            try
            {
                base.Initialize(requestContext);
                string actionName = requestContext.RouteData.Values["action"].ToString();
                string controllerName = requestContext.RouteData.Values["controller"].ToString();
                //Checks Session before calling controller



                //Get session SSID from Cookie
                string SSID = requestContext.HttpContext.Session[ConstantDictionaryVM.MainSession_string] as string;
                if (!string.IsNullOrWhiteSpace(SSID))
                {
                    LoginServices ls = new LoginServices();
                 
                    sessionObj = (JsonConvert.DeserializeObject<SessionCM>(await ls.GetSessionValueBySSIDAsync(SSID)));
                    //Response.Cookies[ConstantDictionaryVM.RememberMe_string].Expires = DateTime.Now.AddDays(-1);
                    //Session.Abandon();
                    if (sessionObj.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sessionObj.errorLogId);
                    }
                    requestContext.HttpContext.Response.Redirect(Url.Action("Index", "Home", null, Request.Url.Scheme));
                }
                else if (Request.Cookies[ConstantDictionaryVM.RememberMe_string] != null && string.IsNullOrWhiteSpace(SSID))
                {
                    HttpCookie cookie = Request.Cookies[ConstantDictionaryVM.RememberMe_string];
                    string roll = cookie.Value;
                    SSID = roll;
                    requestContext.HttpContext.Session[ConstantDictionaryVM.MainSession_string] = roll;
                    LoginServices ls = new LoginServices();
                    sessionObj = (JsonConvert.DeserializeObject<SessionCM>(await ls.GetSessionValueBySSIDAsync(SSID)));
                    Response.Cookies[ConstantDictionaryVM.RememberMe_string].Expires = DateTime.Now.AddDays(-1);
                    Session.Abandon();
                    if (sessionObj.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sessionObj.errorLogId);
                    }
                }

               // if (sessionObj != null && (string.Compare(controllerName, "Login", true) == 0 || string.Compare(controllerName, "NeedHelp", true) == 0) &&
               //    (string.Compare(actionName, "Logout", true) != 0 && (string.Compare(actionName, "RecoveryOptions", true) != 0))
               //)
               // {
               //     if ((ConstantDictionaryVM.access_role_in_access_area.Where(a =>
               //         a.accessRoleTitle == sessionObj.user.accessRoleTitle &&
               //         a.domain == Request.UserHostAddress &&
               //         a.controller == controllerName &&
               //         a.view == actionName && a.canAccess == false).Count() == 1))

               //     {
               //         throw new Exception("Unauthorised Access!! you are not authorised to access this area.contact admin.");
               //     }
               //     else
               //     {
               //         requestContext.HttpContext.Response.Redirect(Url.Action("Index", "Home", null, Request.Url.Scheme));
               //     }
               // }


            }

            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
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


                System.Web.HttpContext.Current.Response.Redirect(Url.Action("Index", "ErrorHandler", new { logid = logid }));
            }
        }


    }
}



