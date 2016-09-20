using System;
using System.Web.Mvc;
using System.Web.Routing;
using ChikaraksServiceOperationLayer.Maintenance;
using Chikaraks.ConstantDictionaryViewModel;

namespace StartUp
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        //HC 27-02-2016 Created
        public bool flag = true;


        public string ControllerName;

        public string CreateFormFields { get; set; }

        //public BaseController(string ControllerName)
        //{
        //    this.ControllerName = ControllerName;
        //}


        //public void SetPager(UInt16 PageNumber, UInt16 TotalPages, UInt16 TotalRecords, UInt16 RecordsPerPage = LabelsAutoFill.RecordsPerPage)
        //{
        //    ViewBag.RecordsPerPage = RecordsPerPage;
        //    ViewBag.PageNumber = PageNumber;
        //    ViewBag.TotalPages = TotalPages;
        //    ViewBag.TotalRecords = TotalRecords;

        //}
        protected override void Initialize(RequestContext requestContext)
        {

            try
            {
                //base.Initialize(requestContext);
                //string actionName = requestContext.RouteData.Values["action"].ToString();
                //string controllerName = requestContext.RouteData.Values["controller"].ToString();
                ////Checks Session before calling controller
                //user user = null;
                //string urlrequest = Request.Url.AbsoluteUri;
                ////if (urlrequest.Contains("CrossDomainSessionCheckResult="))
                ////{
                ////    string username = urlrequest.Split(new string[] { "CrossDomainSessionCheckResult=" }, StringSplitOptions.None)[1];
                ////}
                ////else
                ////{
                //if (Request.Cookies[ConstantDictionaryVM.RememberMe_string] != null)
                //    {
                //        string roll = Request.Cookies[ConstantDictionaryVM.RememberMe_string].Value;
                //        user = JsonConvert.DeserializeObject<user>(HttpUtility.UrlDecode(roll));
                //        requestContext.HttpContext.Session[ConstantDictionaryVM.MainSession_string] = user;
                //    }

                //    if (requestContext.HttpContext.Session[ConstantDictionaryVM.MainSession_string] == null)
                //    {
                //        System.Web.HttpContext.Current.Response.Redirect(ConstantDictionaryVM.WebsiteURL_string + "/Login?url=" + HttpUtility.UrlEncode(urlrequest));
                //    }
                ////}


                ////AccressRestrictionProcess restriction = new AccressRestrictionProcess();
                ////if (restriction.isRestrictedAction(controllerName, actionName, requestContext.HttpContext.Session) == false)
                ////{
                ////    flag = false;
                ////    var context = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current),
                ////     new RouteData());
                ////    var urlHelper = new UrlHelper(context);
                ////    var url = urlHelper.Action("Index", "NoPermission");
                ////    System.Web.HttpContext.Current.Response.Redirect(url);
                ////}
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
    }
}