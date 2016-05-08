using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UtilitiesForAll;
using LionPOS.Models.ViewModels.Maintenance;
using LionPOSServiceContractModels;
using LionPOSServiceOperationLayer.Login;

using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using LionPOS.Models.AccessRestrictionModel;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;

namespace LionStartUp.ControllerHelper
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
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, SSID));
                    sessionObj = (await ls.GetSessionValueBySSIDAsync(new FormDataCollection(lkvp))).Data as SessionCM;
                    if (sessionObj.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sessionObj.errorLogId);
                    }
                    
                }
                else if (Request.Cookies[ConstantDictionaryVM.RememberMe_string] != null && string.IsNullOrWhiteSpace(SSID))
                {
                    HttpCookie cookie = Request.Cookies[ConstantDictionaryVM.RememberMe_string];
                    string roll = cookie.Value;
                    SSID = roll;
                    requestContext.HttpContext.Session[ConstantDictionaryVM.MainSession_string] = roll;
                    LoginServices ls = new LoginServices();
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>(ConstantDictionaryCM.JsonParamString_string, SSID));
                    sessionObj = (await ls.GetSessionValueBySSIDAsync(new FormDataCollection(lkvp))).Data as SessionCM;
                    if (sessionObj.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sessionObj.errorLogId);
                    }
                }

                if (sessionObj != null && (string.Compare(controllerName, "Login", true) == 0 || string.Compare(controllerName, "NeedHelp", true) == 0) &&
                   (string.Compare(actionName, "Logout", true) != 0)
               )
                {
                    access_role_in_access_areaSCM ass = new AccressRestrictionProcess().isAuthorised(requestContext.HttpContext.Request.UserHostAddress, "Home", "Index", sessionObj);
                    if (ass == null)
                    {
                        throw new Exception("Unauthorised Access!! you are not authorised to access this area.contact admin.");
                    }
                    if (ass.canAccess == false)
                    {
                        throw new Exception("Unauthorised Access!! you are not authorised to access this area.contact admin.");
                    }
                    else
                    {
                        requestContext.HttpContext.Response.Redirect(Url.Action("Index", "Home", null, Request.Url.Scheme));
                    }
                }


            }

            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
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

                FormatHelpers rf = new FormatHelpers();
                System.Web.HttpContext.Current.Response.Redirect(Url.Action("Index", "ErrorHandler", new { logid = logid }));
            }
        }


    }
}



