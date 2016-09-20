using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChikaraksDbContracts.DomainModels;
using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM;
using ChikaraksServiceContractModels.DomainContractsModel;
using Newtonsoft.Json;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Transactions;
using System.Data.Entity;
using Newtonsoft.Json.Serialization;
using ChikaraksServiceContractModels.ConstantDictionaryContractModel;
using System.IO;

namespace ChikaraksServiceOperationLayer.StoryCategoryServices
{
    public class GetAllDataServices
    {
        public GetAllDataServices()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        public async Task<string> getStoryCategoryAsync(string JsonParamString)
        {
            GetStoryCategoryResultCCM model = new GetStoryCategoryResultCCM();

            try
            {
                BasicQueryContractModel cm = JsonConvert.DeserializeObject<BasicQueryContractModel>(JsonParamString);
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    model.StoryCategoryDCMList = JsonConvert.DeserializeObject<List<StoryCategoryDCM>>(JsonConvert.SerializeObject(await db.story_category.Include(a => a.story_scene_pages).ToListAsync()));
                }
                model.errorLogId = 0;
                return JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                model.errorLogId = 1;
                model.error = ex.InnerException.ToString();
                model.errorDetails = ex.Message;
                return JsonConvert.SerializeObject(model);
            }
        }
        public async Task<string> GetDataByTypeApi(string JsonParamString)
        {
            KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.ResponseModel ResultCCM = new KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.ResponseModel();
            try
            {
                KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.RequestModel RequestCCM = JsonConvert.DeserializeObject<KakubhaiAndSonsServiceContractModels.ControllerContractModel.WebAPIModels.GetDataByType.RequestModel>(JsonParamString);
                Task<List<int>> removeids_t3 = null;
                List<int> removeids_t4 = null;
                Task<List<story_category>> category_t2 = null;
                
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    removeids_t3 = db.story_category.Where(a => a.storyType == RequestCCM.type).Select(a => a.idStoryCategory).ToListAsync();
                    await Task.WhenAll( removeids_t3);
                    int count = 0;
                    if (removeids_t3 != null)
                    {
                        ResultCCM.count = removeids_t3.Result.Count;
                        count = ResultCCM.count;
                    }
                    if (count > RequestCCM.pageRequest)
                    {
                        category_t2 = db.story_category.Where(a => a.storyType == RequestCCM.type).Include(a => a.story_scene_pages).OrderBy(a=>a.idStoryCategory).Skip(RequestCCM.pageRequest).Take(RequestCCM.pageSize).ToListAsync();
                        await Task.WhenAll(category_t2);
                        ResultCCM.pageRequest = RequestCCM.pageRequest;
                    }
                    else
                    {
                        ResultCCM.pageRequest = RequestCCM.pageRequest * -1;
                    }
                }

                ResultCCM.errorLogId = 0;
                if (category_t2 != null)
                {
                    ResultCCM.StoryCategoryDCMList = JsonConvert.DeserializeObject<List<StoryCategoryDCM>>(JsonConvert.SerializeObject(category_t2.Result));
                }
                if (removeids_t3 != null)
                {
                    removeids_t4 = RequestCCM.idExists.Where(a => !removeids_t3.Result.Contains(a)).ToList();
                    ResultCCM.idRemove = removeids_t4;
                }
                
                
                return JsonConvert.SerializeObject(ResultCCM);
            }
            catch (Exception ex)
            {
                ResultCCM.errorLogId = 1;
                ResultCCM.error = ex.InnerException.ToString();
                ResultCCM.errorDetails = ex.Message;
                return JsonConvert.SerializeObject(ResultCCM);
            }
        }
    }
}
