using Chikaraks.ConstantDictionaryViewModel;
using ChikaraksAPI.Models.WebAPIModels.GetAllData;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM;
using ChikaraksServiceOperationLayer.Maintenance;
using ChikaraksServiceOperationLayer.StoryCategoryServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Chikaraks.Controllers
{
    public class GetDataByTypeApiController : ApiController
    {
        public JsonResult Get()
        {
            return Post(null).Result;
        }
        public async System.Threading.Tasks.Task<JsonResult> Post(FormDataCollection value)
        {
            KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.ResponseModel ResultCCM = new KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.ResponseModel();
            try
            {

                string JsonString = value.Get("JsonString");


                GetAllDataServices ss = new GetAllDataServices();
                KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.RequestModel RequestCCM = new JavaScriptSerializer().Deserialize<KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.RequestModel>(JsonString);
                ResultCCM =  JsonConvert.DeserializeObject<KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.ResponseModel>(await ss.GetDataByTypeApi(JsonConvert.SerializeObject(RequestCCM)));
                if (ResultCCM.errorLogId > 0)
                {
                    throw new Exception(ConstantDictionaryCM.checkServerErrorLogNo_string + ResultCCM.errorLogId);
                }
                
                return new JsonResult() { Data = ResultCCM };

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
                                                    "API"
                                                    );


                return new JsonResult() { Data = new { errorLogId = logid, errorURL = new System.Web.Mvc.UrlHelper().Action("Index", "ErrorHandler", new { logid = logid }, Request.RequestUri.Scheme), ex = ex.ToString() } };
            }


            //}
            //    catch (Exception ex)
            //    {
            //        ResponseModel lr = new ResponseModel();
            //        lr.Error = "Unauthorized";
            //        lr.HttpResponse = HttpStatusCode.Unauthorized.ToString();
            //        return new JsonResult() { Data = lr };
            //    }
            //}
        }
    }
}