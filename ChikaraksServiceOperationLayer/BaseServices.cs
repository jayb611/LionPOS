using ChikaraksDbContracts.DomainModels;
using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using ChikaraksServiceOperationLayer.Maintenance;
using DomainModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Web;

namespace LionPOSServiceOperationLayer
{
    public class BaseServices
    {

        public SessionCM SessionObj = null;
        public bool isAdmin { get; set; }
        
        public BaseServices()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            
        }

        public void CheckSession(string SSID)
        {

            SessionCM cm = new SessionCM();
            try
            {
                DateTime now = DateTime.Now;
                string sessionValue = null;
                using (chikaraksEntities dbuser = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    sessionValue = dbuser.users.Where(ses => ses.sessionExpireyDateTime != null && ses.sessionExpireyDateTime > now && ses.sessionID == SSID).Select(ses => ses.sessionValue).SingleOrDefault();
                }
                if (!string.IsNullOrWhiteSpace(sessionValue))
                {
                    SessionObj = JsonConvert.DeserializeObject<SessionCM>(HttpUtility.UrlDecode(sessionValue));


                    
                }
                else
                {
                    throw new Exception("Uauthrized Access!");
                }
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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

                throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + logid);
            }
        }
        public void RefreshSessionObject(SessionCM sessionObj)
        {
            try
            {
                using (chikaraksEntities dbuser = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    user User = dbuser.users.Where(
                                user => user.userName == sessionObj.user.userName
                                ).SingleOrDefault();
                    User.sessionValue = HttpUtility.UrlEncode(JsonConvert.SerializeObject(sessionObj));
                    dbuser.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MaintenanceServices m = new MaintenanceServices();
                int logid = m.CreateLog("",
                                                    "Error occured on creating PreConfigurationsServices of " + System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                    "",
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
                throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + logid);
            }
        }

    }
}
