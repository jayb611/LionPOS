using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChikaraksDbContracts.DomainModels;
using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ControllerContractModel.AssetsCCM;
using ChikaraksServiceContractModels.DomainContractsModel;
using Newtonsoft.Json;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Transactions;
using System.Data.Entity;
using Newtonsoft.Json.Serialization;

namespace ChikaraksServiceOperationLayer.AssetsServices
{
    public class AssetsServices
    {

        public AssetsServices()
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

        public int getAssetMaxID()
        {
            int Max = 0;

            try
            {
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    Max = db.assets.Max(u => u.idAssets) + 1;
                }

                return Max;
            }
            catch (Exception)
            {

                return 1;
            }
        }


        public async Task<string> getAssetsAsync(string JsonParamString)
        {
            GetAssetsResultCCM model = new GetAssetsResultCCM();

            try
            {
                BasicQueryContractModel cm = JsonConvert.DeserializeObject<BasicQueryContractModel>(JsonParamString);
                using (chikaraksEntities db = new chikaraksEntities())
                {
                    model.AssetsList = JsonConvert.DeserializeObject<List<AssetsDCM>>(JsonConvert.SerializeObject(await db.assets.ToListAsync()));
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

        public async Task<string> CreateAssetsAsync(string JsonParamString)
        {
            CreateAssetsResultCCM ResultCM = new CreateAssetsResultCCM();
            try
            {
                CreateAssetsRequestCCM RequestModel = (JsonConvert.DeserializeObject<CreateAssetsRequestCCM>(JsonParamString));
                asset Asset = (JsonConvert.DeserializeObject<asset>(JsonConvert.SerializeObject(RequestModel.AssetsDCM)));

                using (TransactionScope sc = new TransactionScope())
                {
                    using (chikaraksEntities cdb = new UniversDbContext().chikaraksDbContext(false, false))
                    {
                        cdb.assets.Add(Asset);
                        await cdb.SaveChangesAsync();
                        ResultCM.AssetsDCM.idAssets = cdb.assets.Max(u => u.idAssets);
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

        public async Task<string> getAssetsByAssetsIDAsync(string JsonParamString)
        {
            GetAssetsByAssetsIDResultCCM ResultCM = new GetAssetsByAssetsIDResultCCM();

            try
            {
                GetAssetsByAssetsIDRequestCCM RequestCM = JsonConvert.DeserializeObject<GetAssetsByAssetsIDRequestCCM>(JsonParamString);
                Task<asset> amodel;
                using (chikaraksEntities adb = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    amodel = (from a in adb.assets
                              where a.idAssets == RequestCM.idAssets
                              select a).SingleOrDefaultAsync();
                }
                await Task.WhenAll(amodel);
                ResultCM.AssetsDCM = JsonConvert.DeserializeObject<AssetsDCM>(JsonConvert.SerializeObject(amodel.Result)); ;
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

        public async Task<string> UpdateAssetsAsync(string JsonParamString)
        {
            try
            {
                UpdateAssetsRequestCCM RequestModel = (JsonConvert.DeserializeObject<UpdateAssetsRequestCCM>(JsonParamString));
                asset Assets = (JsonConvert.DeserializeObject<asset>(JsonConvert.SerializeObject(RequestModel.Assets)));
                using (chikaraksEntities cdb = new UniversDbContext().chikaraksDbContext())
                {
                    cdb.Entry(Assets).State = System.Data.Entity.EntityState.Modified;
                    await cdb.SaveChangesAsync();
                }
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 4, error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }

        }

        public async Task<string> DeleteAssetsAsync(string JsonParamString)
        {
            try
            {
                DeleteAssetsRequestCCM RequestModel = (JsonConvert.DeserializeObject<DeleteAssetsRequestCCM>(JsonParamString));
                asset model = new asset();
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                {
                    model = (from a in db.assets where a.idAssets == RequestModel.idAssets select a).SingleOrDefault();
                    db.assets.Remove(model);
                    await db.SaveChangesAsync();
                }
                return JsonConvert.SerializeObject(new ErrorCM { errorLogId = 0 });
            }

            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new ErrorCM { error = ex.InnerException.ToString(), errorDetails = ex.Message });
            }
        }

        public async Task<string> deleteMultipleAssetsAsync(string JsonParamString)
        {
            try
            {
                SelectedAssetsActionRequestCCM RequestModel = (JsonConvert.DeserializeObject<SelectedAssetsActionRequestCCM>(JsonParamString));

                for (int i = 0; i < RequestModel.idAssets.Count; i++)
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                    {
                        int ID = RequestModel.idAssets[i];
                        asset mod = (from a in db.assets where a.idAssets == ID select a).SingleOrDefault();
                        db.assets.Remove(mod);
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
