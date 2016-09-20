using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ChikaraksServiceContractModels;
using ChikaraksServiceOperationLayer.Maintenance;

using Chikaraks.ConstantDictionaryViewModel;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using Newtonsoft.Json;
using LionPOSServiceOperationLayer.Login;

namespace StartUp
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
                    sessionObj = (JsonConvert.DeserializeObject<SessionCM>(await ls.GetSessionValueBySSIDAsync(SSID)));

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
                    sessionObj = (JsonConvert.DeserializeObject<SessionCM>(await ls.GetSessionValueBySSIDAsync(SSID)));
                    if (sessionObj.errorLogId > 0)
                    {
                        throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + sessionObj.errorLogId);
                    }
                }
                //if (sessionObj != null)
                //{
                //    //if ((ConstantDictionaryVM.access_role_in_access_area.Where(a =>
                //    //     a.accessRoleTitle == sessionObj.user.accessRoleTitle &&
                //    //     a.domain == Request.UserHostAddress &&
                //    //     a.controller == controllerName &&
                //    //     a.view == actionName && a.canAccess == false).Count() == 1))

                //    //{
                //    //    throw new Exception("Unauthorised Access!! you are not authorised to access this area.contact admin.");
                //    //}
                //    //else
                //    //{

                //    //    requestContext.HttpContext.Response.Redirect(Url.Action("Index", "Home", null, Request.Url.Scheme));

                //    //}
                //}
                //showBranchWise = new AccressRe

            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(            "",
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



        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (sessionObj == null)
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Login" }));
        }


        //public bool showLink(string controller, string action_view)
        //{
        //    try
        //    {
        //        access_role_in_access_areaSCM ass = new AccressRestrictionProcess().isAuthorised(Request.UserHostAddress, controller, action_view, sessionObj.user.accessRoleTitle);
        //        if (ass == null)
        //        {
        //            return false;
        //        }
        //        if (ass.canAccess == false)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MaintenanceServices m = new MaintenanceServices();
        //        int logid = m.CreateLog("",
        //                                            "Error occured on " + System.Reflection.MethodBase.GetCurrentMethod().Name,
        //                                            ConstantDictionaryVM.ErrorMessage,
        //                                            new LionUtilities.ConversionUtilitise().ObjectToString(ex),
        //                                            null,
        //                                            "",
        //                                            this.GetType().Name,
        //                                            System.Reflection.MethodBase.GetCurrentMethod().Name,
        //                                            new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(),
        //                                            new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(),
        //                                            true,
        //                                            sessionObj.user.userName
        //                                            );


        //        return false;
        //    }
        //}

    }
}
