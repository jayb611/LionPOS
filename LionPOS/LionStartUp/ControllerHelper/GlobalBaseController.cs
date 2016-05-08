using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LionPOS.Models.ViewModels.Maintenance;
using UtilitiesForAll;
using LionPOSServiceContractModels;
using LionPOSServiceOperationLayer.Login;

using LionPOSServiceOperationLayer.Maintenance;
using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using System.Net.Http.Formatting;
using System.Collections.Generic;
using LionPOSServiceContractModels.ErrorContactModel;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;

namespace LionStartUp
{
    public class GlobalBaseController : Controller
    {
        //
        // GET: /Base/
        //HC 27-02-2016 Created
        public bool flag = true;

        public SessionCM sessionObj { get; set; }
        public string actionName;
        public string controllerName;
        public bool showBranchWise { get; set; }

        public string createFormFields { get; set; }

        //public BaseController(string ControllerName)
        //{
        //    this.ControllerName = ControllerName;
        //}

        protected async override void Initialize(RequestContext requestContext)
        {

            try
            {
                base.Initialize(requestContext);
                actionName = requestContext.RouteData.Values["action"].ToString();
                controllerName = requestContext.RouteData.Values["controller"].ToString();

                //Get session SSID from Cookie
                string SSID = requestContext.HttpContext.Session[ConstantDictionaryVM.MainSession_string] as string;
                if (!string.IsNullOrWhiteSpace(SSID))
                {
                    LoginServices ls = new LoginServices();
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>("JsonParamString", SSID));
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
                    requestContext.HttpContext.Session[ConstantDictionaryVM.MainSession_string] = roll;
                    LoginServices ls = new LoginServices();
                    List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>();
                    lkvp.Add(new KeyValuePair<string, string>("JsonParamString", SSID));
                    sessionObj = (await ls.GetSessionValueBySSIDAsync(new FormDataCollection(lkvp))).Data as SessionCM;
                    if (sessionObj.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sessionObj.errorLogId);
                    }
                }
                //Code to uncomment
                // if (sessionObj != null && (string.Compare(controllerName, "Login", true) == 0 || string.Compare(controllerName, "NeedHelp", true) == 0) &&
                //    (string.Compare(actionName, "Logout", true) != 0)
                //)
                // {
                //     if (new AccressRestrictionProcess().isAuthorised(requestContext.HttpContext.Request.UserHostAddress, "Home", "Index", sessionObj) == false)
                //     {
                //         throw new Exception("Unauthorised Access!! you are not authorised to access this area.contact admin.");

                //     }
                //     else
                //     {
                //         requestContext.HttpContext.Response.Redirect(Url.Action("Index", "Home", null, Request.Url.Scheme));
                //     }
                // }
                //showBranchWise = new AccressRestrictionProcess().isAuthorised(requestContext.HttpContext.Request.UserHostAddress, controllerName, actionName, sessionObj).showBranchWise;


            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(            "",
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
