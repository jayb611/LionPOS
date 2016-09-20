using DomainModels;
using ChikaraksDbContracts.DomainModels;
using ChikaraksServiceContractModels.ControllerContractModel;
using ChikaraksServiceContractModels.ControllerContractModel.StoryScenePagesCCM;
using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksServiceOperationLayer.StoryScenePageServices
{
    public class StoryScenePageServices
    {
        public StoryScenePageServices()
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


        public async Task<string> getStoryScenePagesAsync(string JsonParamString)
        {
            GetStoryScenePagesResultCCM ResultCCM = new GetStoryScenePagesResultCCM();
            try
            {
                GetStoryScenePagesRequestCCM RqstCCM = JsonConvert.DeserializeObject<GetStoryScenePagesRequestCCM>(JsonParamString);
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    ResultCCM.StoryScenePagesDCMList = JsonConvert.DeserializeObject<List<StoryScenePagesDCM>>(JsonConvert.SerializeObject(await (from a in db.story_scene_pages where a.idStoryCategory == RqstCCM.idstorycategory select a).ToListAsync()));
                }
                ResultCCM.errorLogId = 0;
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

        public int GetMaxIDStoryScenePageAsync()
        {
            int Max = 0;

            try
            {
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    Max = db.story_scene_pages.Max(u => u.idStoryScenePages) + 1;
                }

                return Max;
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public async Task<string> getStoryScenePageByPrimaryKeysAsync(string JsonParamString)
        {
            GetStoryScenePageByPrimaryKeysResultCCM ResultModel = new GetStoryScenePageByPrimaryKeysResultCCM();
            try
            {
                GetStoryScenePageByPrimaryKeysRequestCCM RequestCCM = JsonConvert.DeserializeObject<GetStoryScenePageByPrimaryKeysRequestCCM>(JsonParamString);
                Task<story_scene_pages> story_scene_page;
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                {
                    story_scene_page = (from x in db.story_scene_pages
                                        where
                                           x.idStoryCategory == RequestCCM.idStoryCategory &&
                                           x.idStoryScenePages == RequestCCM.idStoryScenePage
                                        select x
                                         ).Include(x => x.story_category).SingleOrDefaultAsync();
                }
                await Task.WhenAll(story_scene_page);
                //ResultModel.StoryScenePagesDCM = JsonConvert.DeserializeObject<StoryScenePagesDCM>(JsonConvert.SerializeObject(story_scene_page.Result));
                ResultModel.StoryScenePagesDCM.audioUrl = story_scene_page.Result.audioUrl;
                ResultModel.StoryScenePagesDCM.audioUrlStoreLocation = story_scene_page.Result.audioUrlStoreLocation;
                ResultModel.StoryScenePagesDCM.idStoryCategory = story_scene_page.Result.idStoryCategory;
                ResultModel.StoryScenePagesDCM.idStoryScenePages = story_scene_page.Result.idStoryScenePages;
                ResultModel.StoryScenePagesDCM.imageUrl = story_scene_page.Result.imageUrl;
                ResultModel.StoryScenePagesDCM.indexOrder = story_scene_page.Result.indexOrder;
                ResultModel.StoryScenePagesDCM.imageUrlStoreLocation = story_scene_page.Result.imageUrlStoreLocation;
                ResultModel.StoryScenePagesDCM.youtubeStoreLocation = story_scene_page.Result.youtubeStoreLocation;
                ResultModel.StoryScenePagesDCM.youtubeUrl = story_scene_page.Result.youtubeUrl;
                ResultModel.StoryScenePagesDCM.title = story_scene_page.Result.title;
                ResultModel.errorLogId = 0;
                return JsonConvert.SerializeObject(ResultModel);
            }
            catch (Exception ex)
            {
                ResultModel.errorLogId = 2;
                ResultModel.errorDetails = ex.Message;
                ResultModel.error = ex.InnerException.ToString();
                return JsonConvert.SerializeObject(ResultModel);
            }
        }



        public async Task<string> CreateStoryScenePageAsync(string JsonParamString)
        {
            CreateStoryScenePageResultCCM ResultCCM = new CreateStoryScenePageResultCCM();
            try
            {
                CreateStoryScenePageRequestCCM RequestModel = JsonConvert.DeserializeObject<CreateStoryScenePageRequestCCM>(JsonParamString);

                story_scene_pages ssp = JsonConvert.DeserializeObject<story_scene_pages>(JsonConvert.SerializeObject(RequestModel.StoryScenePagesDCM));

                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    db.story_scene_pages.Add(ssp);
                    await db.SaveChangesAsync();
                    ResultCCM.idStoryScenePages = (from a in db.story_scene_pages where a.audioUrl == ssp.audioUrl && a.idStoryCategory == ssp.idStoryCategory && a.indexOrder == ssp.indexOrder select a.idStoryScenePages).SingleOrDefault();
                }
                ResultCCM.errorLogId = 0;
                return JsonConvert.SerializeObject(ResultCCM);
            }
            catch (Exception ex)
            {
                ResultCCM.errorLogId = 3;
                ResultCCM.error = ex.InnerException.ToString();
                ResultCCM.errorDetails = ex.Message;
                return JsonConvert.SerializeObject(ResultCCM);
            }


        }



        public async Task<string> UpdateStoryScenePageAsync(string JsonParamString)
        {

            try
            {
                UpdateStoryScenePageRequestCCM RequestModel = (JsonConvert.DeserializeObject<UpdateStoryScenePageRequestCCM>(JsonParamString));
                story_scene_pages contentModel = (JsonConvert.DeserializeObject<story_scene_pages>(JsonConvert.SerializeObject(RequestModel.StoryScenePagesDCM)));

                using (chikaraksEntities adb = new UniversDbContext().chikaraksDbContext())
                {
                    adb.Entry(contentModel).State = EntityState.Modified;
                    await adb.SaveChangesAsync();
                }
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 4, error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }

        }




        public async Task<string> DeleteStoryScenePageAsync(string JsonParamString)
        {
            try
            {
                DeleteStoryScenePageRequestCCM RequestModel = (JsonConvert.DeserializeObject<DeleteStoryScenePageRequestCCM>(JsonParamString));
                story_scene_pages model = new story_scene_pages();
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                {
                    model = (from a in db.story_scene_pages where a.idStoryScenePages == RequestModel.IDStoryScenePage select a).SingleOrDefault();
                    db.story_scene_pages.Remove(model);
                    await db.SaveChangesAsync();
                }
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }

            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new ErrorCM { error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }
        }



        public async Task<string> deleteMultipleContentsAsync(string JsonParamString)
        {
            try
            {
                SelectedStoryScenePageActionRequestCCM RequestModel = (JsonConvert.DeserializeObject<SelectedStoryScenePageActionRequestCCM>(JsonParamString));

                for (int i = 0; i < RequestModel.IDStoryCategoryScene.Count; i++)
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                    {
                        int ID = RequestModel.IDStoryCategoryScene[i];
                        story_scene_pages mod = (from a in db.story_scene_pages where a.idStoryScenePages == ID select a).SingleOrDefault();
                        db.story_scene_pages.Remove(mod);
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


    }
}
