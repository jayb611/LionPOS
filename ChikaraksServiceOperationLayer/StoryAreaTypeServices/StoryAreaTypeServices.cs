using DomainModels;
using LionPOSDbContracts.DomainModels.Chikaraks;
using LionPOSServiceContractModels.ControllerContractModel.StoryAreaTypeCCM;
using LionPOSServiceContractModels.DomainContractsModel.Chikaraks;
using LionPOSServiceContractModels.ErrorContactModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq.Expressions;
using System.Linq;

namespace LionPOSServiceOperationLayer.StoryAreaTypeServices
{
    public class StoryAreaTypeServices
    {
        public StoryAreaTypeServices()
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

        public async Task<string> getStoryAreaTypesAsync(string JsonParamString)
        {
            GetStoryAreaTypeResultCCM model = new GetStoryAreaTypeResultCCM();
            try
            {

                using (TransactionScope tc = new TransactionScope())
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                    {
                        model.StoryAreaTypeDCMList = JsonConvert.DeserializeObject<List<StoryAreaTypeDCM>>(JsonConvert.SerializeObject(await db.story_area_type.ToListAsync()));
                        tc.Complete();
                    }
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

        public async Task<string> getStoryAreaTypebyIDStoryAreaType(string JsonParamString)
        {
            GetStoryAreaTypeByStoryAreaTypeIDResultCCM ResultCM = new GetStoryAreaTypeByStoryAreaTypeIDResultCCM();

            try
            {
                GetStoryAreaTypeByStoryAreaTypeIDRequestCCM RequestCM = JsonConvert.DeserializeObject<GetStoryAreaTypeByStoryAreaTypeIDRequestCCM>(JsonParamString);
                Task<story_area_type> storyAreaType;

                using (chikaraksEntities cdb = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    storyAreaType = (from a in cdb.story_area_type
                              where a.idStoryAreaType == RequestCM.idStoryAreaType
                              select a).SingleOrDefaultAsync();
                }
                await Task.WhenAll(storyAreaType);
                ResultCM.StoryAreaTypeDCM = JsonConvert.DeserializeObject<StoryAreaTypeDCM>(JsonConvert.SerializeObject(storyAreaType.Result));
                return JsonConvert.SerializeObject(ResultCM);
            }
            catch (Exception ex)
            {
                ResultCM.errorLogId = 2;
                ResultCM.error = ex.InnerException.ToString();
                ResultCM.errorDetails = ex.Message;
                return JsonConvert.SerializeObject(ResultCM);
            }
        }


        public async Task<string> CreateStoryAreaTypeAsync(string JsonParamString)
        {
            try
            {
                CreateStoryAreaTypeRequestCCM RequestCCM = JsonConvert.DeserializeObject<CreateStoryAreaTypeRequestCCM>(JsonParamString);
                story_area_type SAT = JsonConvert.DeserializeObject<story_area_type>(JsonConvert.SerializeObject(RequestCCM.StoryAreaTypeDCM));
                using (TransactionScope sc = new TransactionScope())
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                    {
                        db.story_area_type.Add(SAT);
                        await db.SaveChangesAsync();
                        sc.Complete();
                    }
                }
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 3, error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }
        }


        public async Task<string> UpdateStoryAreaType(string JsonParamString)
        {
            try
            {
                UpdateStoryAreaTypeRequestCCM RequestCM = JsonConvert.DeserializeObject<UpdateStoryAreaTypeRequestCCM>(JsonParamString);
                story_area_type SATModel = JsonConvert.DeserializeObject<story_area_type>(JsonConvert.SerializeObject(RequestCM.StoryAreaTypeDCM));
                using (chikaraksEntities adb = new UniversDbContext().chikaraksDbContext())
                {
                    adb.Entry(SATModel).State = EntityState.Modified;
                    await adb.SaveChangesAsync();
                }
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 4, error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }
        }

        public async Task<string> DeleteStoryAreaType(string JsonParamString)
        {
            try
            {
                DeleteSToryAreaTypeRequestCCM RequestModel = (JsonConvert.DeserializeObject<DeleteSToryAreaTypeRequestCCM>(JsonParamString));
                story_area_type model = new story_area_type();

                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                {
                    model = (from a in db.story_area_type where a.idStoryAreaType == RequestModel.IDStoryAreaType select a).SingleOrDefault();
                    db.story_area_type.Remove(model);
                    await db.SaveChangesAsync();
                }
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }
        }

        public async Task<string> deleteMultipleStoryAreaTypeAsync(string JsonParamString)
        {
            try
            {
                SelectedStoryAreaTypeActionRequestCCM RequestModel = (JsonConvert.DeserializeObject<SelectedStoryAreaTypeActionRequestCCM>(JsonParamString));

                for (int i = 0; i < RequestModel.idStoryAreaType.Count; i++)
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                    {
                        int ID = RequestModel.idStoryAreaType[i];
                        story_area_type mod = (from a in db.story_area_type where a.idStoryAreaType == ID select a).SingleOrDefault();
                        db.story_area_type.Remove(mod);
                        await db.SaveChangesAsync();
                    }
                }
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new ErrorCM { error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }
        }

        public int CheckTitle(string Title)
        {

            try
            {
                int match = 0;
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    match = db.story_area_type.Where(a => string.Compare(a.title, Title, true) == 0).Count();
                }

                return match;
            }
            catch (Exception)
            {

                return -1;
            }
        }
    }
}
