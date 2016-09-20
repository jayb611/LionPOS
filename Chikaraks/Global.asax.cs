using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Helpers;

using Chikaraks.ConstantDictionaryViewModel;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;

namespace Chikaraks
{
    public class MvcApplication : System.Web.HttpApplication
    {


        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.None,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore


            };

            try
            {

                AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
                AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                //ErrorCM error = JsonConvert.DeserializeObject<ErrorCM>(new InitConstantRecordsDictionaryServices().Initiate()) as ErrorCM;
                //if (error.errorLogId > 0)
                //{
                //    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + error.errorLogId);
                //}
                ConstantDictionaryCM.StartUpErrorLog_int = 0;
            }
            catch (Exception)
            {

                ConstantDictionaryCM.StartUpErrorLog_int = 1;
            }


        }
    }
}
