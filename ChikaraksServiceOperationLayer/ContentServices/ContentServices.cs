using DomainModels;
using ChikaraksDbContracts.DomainModels;
using ChikaraksServiceContractModels.ControllerContractModel.ContentCCM;
using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ChikaraksServiceOperationLayer.ContentServices
{
    public class ContentServices
    {
        public ContentServices()
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


        public async Task<string> getContentsAsync(string JsonParamString)
        {
            GetContentResultCCM model = new GetContentResultCCM();
            try
            {


                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    model.ContentDCMList = JsonConvert.DeserializeObject<List<ContentDCM>>(JsonConvert.SerializeObject(await db.contents.ToListAsync()));
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




        public async Task<string> getContentByContentIDAsync(string JsonParamString)
        {
            GetContentByContentIDResultCCM ResultCM = new GetContentByContentIDResultCCM();
            try
            {
                GetContentByContentIDRequestCCM RequestCM = JsonConvert.DeserializeObject<GetContentByContentIDRequestCCM>(JsonParamString);
                Task<content> amodel;

                using (chikaraksEntities adb = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    amodel = (from a in adb.contents
                              where a.idContent == RequestCM.ID
                              select a).SingleOrDefaultAsync();
                }
                await Task.WhenAll(amodel);
                ResultCM.ContentDCM = JsonConvert.DeserializeObject<ContentDCM>(JsonConvert.SerializeObject(amodel.Result));
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




        public async Task<string> CreateContentAsync(string JsonParamString)
        {
            CreateContentResultCCM ResultCM = new CreateContentResultCCM();
            try
            {
                CreateContentRequestCCM RequestModel = JsonConvert.DeserializeObject<CreateContentRequestCCM>(JsonParamString);
                content Contents = JsonConvert.DeserializeObject<content>(JsonConvert.SerializeObject(RequestModel.ContentDCM));
                using (TransactionScope sc = new TransactionScope())
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                    {
                        db.contents.Add(Contents);
                        await db.SaveChangesAsync();
                        ResultCM.ContentDCM.idContent = db.contents.Max(u => u.idContent);
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




        public async Task<string> UpdateContentAsync(string JsonParamString)
        {

            try
            {
                UpdateContentRequestCCM RequestModel = (JsonConvert.DeserializeObject<UpdateContentRequestCCM>(JsonParamString));
                content contentModel = (JsonConvert.DeserializeObject<content>(JsonConvert.SerializeObject(RequestModel.ContetntDCM)));

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




        public async Task<string> DeleteContentAsync(string JsonParamString)
        {
            try
            {
                DeleteContentRequestCCM RequestModel = (JsonConvert.DeserializeObject<DeleteContentRequestCCM>(JsonParamString));
                content model = new content();
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                {
                    model = (from a in db.contents where a.idContent == RequestModel.ID select a).SingleOrDefault();
                    db.contents.Remove(model);
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
                SelectedContentActionRequestCCM RequestModel = (JsonConvert.DeserializeObject<SelectedContentActionRequestCCM>(JsonParamString));

                for (int i = 0; i < RequestModel.IDs.Count; i++)
                {
                    using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext())
                    {
                        int ID = RequestModel.IDs[i];
                        content mod = (from a in db.contents where a.idContent == ID select a).SingleOrDefault();
                        db.contents.Remove(mod);
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



        public int checkTitle(string ContentTitle)
        {

            try
            {
                int match = 0;
                using (chikaraksEntities db = new UniversDbContext().chikaraksDbContext(false, false))
                {
                    match = db.contents.Where(a => string.Compare(a.contentTag, ContentTitle, true) == 0).Count();
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
