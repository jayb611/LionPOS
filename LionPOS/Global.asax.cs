using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UtilitiesForAll;
using System.Web.Helpers;
using LionPOSServiceOperationLayer.InitiateSystemStartUp;
using LionPOSServiceOperationLayer.Maintenance;

using LionPOSServiceContractModels.ConstantDictionaryViewModel;
using System.Collections.Generic;
using LionPOSServiceContractModels.ErrorContactModel;
using LionPOSServiceContractModels.ConstantDictionaryContractModel;

namespace LionPOS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
  
        protected void Application_Start()
        {
           
            AreaRegistration.RegisterAllAreas();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
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

                ErrorCM error = JsonConvert.DeserializeObject<ErrorCM>(new InitConstantRecordsDictionaryServices().Initiate()) as ErrorCM;
                if (error.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + error.errorLogId);
                }
                ConstantDictionaryCM.StartUpErrorLog_int = 0;
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog(
                                                    "",
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
                                                    "system"
                                                    );
                ConstantDictionaryCM.StartUpErrorLog_int = logid;
            }


        }
    }
}
