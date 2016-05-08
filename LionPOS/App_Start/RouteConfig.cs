using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LionPOS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //http://localhost:2913/ErrorHandler/?logid=24&TypeOfPage=Static

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Module",
                url: "{Module}/{controller}/{action}",
                defaults: new {  action = "Index" }
            );

            //routes.MapRoute(
            //    name: "Module",
            //    url: "{Module}/{controller}/{action}/{id}",
            //    defaults: new {  controller = "POSLogin", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Maintenance",
                url: "{controller}/{action}/{logid}/{TypeOfPage}",
                defaults: new { controller = "ErrorHandler", action = "Index" }
            );
            routes.MapRoute(
                name: "NeedHelp",
                url: "{controller}/{action}/{branchCode}",
                defaults: new { controller = "Login", action = "Index", branchCode = UrlParameter.Optional }
            );
            

        }
    }
}
