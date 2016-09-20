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
    public class StoryCategoryServices
    {
        public StoryCategoryServices()
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
                    model.StoryCategoryDCMList = JsonConvert.DeserializeObject<List<StoryCategoryDCM>>(JsonConvert.SerializeObject(await db.story_category.ToListAsync()));
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

        public async Task<string> getStoryCategoryByPrimaryKeysAsync(string JsonParamString)
        {
            GetStoryCategoryByPrimaryKeysResultCCM model = new GetStoryCategoryByPrimaryKeysResultCCM();
            try
            {

                GetStoryCategoryByPrimaryKeysRequestCCM RequestModal = JsonConvert.DeserializeObject<GetStoryCategoryByPrimaryKeysRequestCCM>(JsonParamString);
                Task<story_category> story_category;
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    story_category = (from a in db.story_category
                                      where a.idStoryCategory == RequestModal.idStoryCategory
                                      select a).Include(a => a.story_scene_pages).SingleOrDefaultAsync();
                    await Task.WhenAll(story_category);
                }
                model.StoryCategoryDCM = JsonConvert.DeserializeObject<StoryCategoryDCM>(JsonConvert.SerializeObject(story_category.Result));
                return JsonConvert.SerializeObject(model);
            }

            catch (Exception ex)
            {
                model.errorLogId = 2;
                model.error = ex.InnerException.ToString();
                model.errorDetails = ex.Message;
                return JsonConvert.SerializeObject(model);
            }

        }

        public async Task<string> CreateStoryCategoryAsync(string JsonParamString)
        {
            CreateStoryCategoryResultCCM ResultCM = new CreateStoryCategoryResultCCM();
            try
            {
                CreateStoryCategoryRequestCCM RequestCM = (JsonConvert.DeserializeObject<CreateStoryCategoryRequestCCM>(JsonParamString));

                story_category story_categorys = (JsonConvert.DeserializeObject<story_category>(JsonConvert.SerializeObject(RequestCM.StoryCategoryDCM)));


                using (TransactionScope sc = new TransactionScope())
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                    {
                        db.story_category.Add(story_categorys);
                        await db.SaveChangesAsync();
                        ResultCM.StoryCategoryDCM.idStoryCategory = db.story_category.Max(u => u.idStoryCategory);
                        sc.Complete();
                    }
                }

                ResultCM.errorLogId = 0;
                return JsonConvert.SerializeObject(ResultCM);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 3, error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }

        }

        public async Task<string> UpdateStoryCategoryAsync(string JsonParamString)
        {
            UpdateStoryCategoryResultCCM ResultCM = new UpdateStoryCategoryResultCCM();
            try
            {

                UpdateStoryCategoryRequestCCM RequestCM = (JsonConvert.DeserializeObject<UpdateStoryCategoryRequestCCM>(JsonParamString));
                story_category story_category = (JsonConvert.DeserializeObject<story_category>(JsonConvert.SerializeObject(RequestCM.StoryCategoryDCM)));


                using (TransactionScope sc = new TransactionScope())
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                    {

                        db.Entry(story_category).State = System.Data.Entity.EntityState.Modified;
                        await db.SaveChangesAsync();
                        sc.Complete();
                    }

                }


                return JsonConvert.SerializeObject(ResultCM);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 4, error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }

        }

        public async Task<string> DeleteStoryCategoryAsync(string JsonParamString)
        {
            DeleteStoryCategoryResultCCM ResultCM = new DeleteStoryCategoryResultCCM();
            try
            {
                DeleteStoryCategoryRequestCCM RequestModel = (JsonConvert.DeserializeObject<DeleteStoryCategoryRequestCCM>(JsonParamString));
                story_category story_category = new story_category();

                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                {
                    story_category = (from a in db.story_category
                                      where
                                                  a.idStoryCategory == RequestModel.idStoryCategory
                                      select a).Include(a => a.story_scene_pages).SingleOrDefault();
                }
                if (story_category != null)
                {

                    using (chikaraksEntities sdb = new UniversDbContext().chikaraksDbContext())
                    {
                        story_category smodel = new story_category();

                        sdb.Entry(story_category).State = EntityState.Unchanged;
                        sdb.story_category.Remove(story_category);
                        await sdb.SaveChangesAsync();
                    }
                }
                return JsonConvert.SerializeObject(ResultCM);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }
        }

        public async Task<string> deleteMultipleStoryCategoryAsync(string JsonParamString)
        {
            StoryCategoryBulkActionResultCCM ResultCM = new StoryCategoryBulkActionResultCCM();
            try
            {
                StoryCategoryBulkActionRequestCCM RequestModel = JsonConvert.DeserializeObject<StoryCategoryBulkActionRequestCCM>(JsonParamString);

                for (int i = 0; i < RequestModel.idStoryCategory.Count; i++)
                {
                    story_category story_category = new story_category();
                    using (chikaraksEntities sdb = new UniversDbContext().chikaraksDbContext())
                    {
                        int idStoryCategory = RequestModel.idStoryCategory[i];
                        story_category = (from a in sdb.story_category
                                          where a.idStoryCategory == idStoryCategory
                                          select a).Include(a => a.story_scene_pages).SingleOrDefault();
                    }
                    if (story_category != null)
                    {

                        using (chikaraksEntities sdb = new UniversDbContext().chikaraksDbContext())
                        {
                            sdb.Entry(story_category).State = System.Data.Entity.EntityState.Unchanged;
                            sdb.story_category.Remove(story_category);
                            await sdb.SaveChangesAsync();
                        }
                    }
                }

                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }
        }

        public int checkTitle(string StoryCategoryTitle)
        {

            try
            {
                int match = 0;
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    match = db.story_category.Where(a => string.Compare(a.storyCategoryTitle, StoryCategoryTitle, true) == 0).Count();
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
