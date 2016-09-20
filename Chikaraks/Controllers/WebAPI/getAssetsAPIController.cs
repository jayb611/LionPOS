using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using Chikaraks.Models;
using System.Web.Script.Serialization;
using Chikaraks.Models.APIModels.Assets;
using ChikaraksDbContracts.DomainModels.Chikaraks;

namespace Chikaraks.Controllers.WebAPI
{
    public class getAssetsAPIController : ApiController
    {
        // GET: getAssetsAPI
        public JsonResult Get()
        {
            RequestModel s = new RequestModel();
            s.deviceType = "Phone";
            return new JsonResult() { Data = s };
        }
        public JsonResult Post(FormDataCollection value)
        {
            try
            {
                string JsonString = value.Get("JsonString");

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                RequestModel data = new JavaScriptSerializer().Deserialize<RequestModel>(JsonString);

                ResponseModel lr = new ResponseModel();
                List<AssetsModel> asset = new List<AssetsModel>();
                chikaraksEntities db = new chikaraksEntities();
          

                var tmp = db.assets.FirstOrDefault();
                int id = 0;
                if (data.deviceType == "Phone")
                {
                    if (tmp != null)
                    {
                        if (tmp.homeBackground != null && tmp.homeBackground != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = tmp.homeBackground });
                        }
                        if (tmp.homeAudio != null && tmp.homeAudio != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = tmp.homeAudio });
                        }
                        if (tmp.title != null && tmp.title != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = tmp.title });
                        }
                    }
                    var lst = db.story_category.Select(a => a).ToList();

                    foreach (var a in lst)
                    {
                        if (a.storyCategoryTitle != null && a.storyCategoryTitle != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.storyCategoryTitle });
                        }
                        if (a.storyCategoryImageUrl != null && a.storyCategoryImageUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.storyCategoryImageUrl });
                        }
                        if (a.storyCategoryImageUrlStoreLocation != null && a.storyCategoryImageUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.storyCategoryImageUrlStoreLocation });
                        }
                        if (a.storyType != null && a.storyType != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.storyType });
                        }
                        if (a.categoryLogoUrl != null && a.categoryLogoUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.categoryLogoUrl });
                        }
                        if (a.categoryLogoUrlStoreLocation != null && a.categoryLogoUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.categoryLogoUrlStoreLocation });
                        }
                        if (a.backgroundImageUrl != null && a.backgroundImageUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.backgroundImageUrl });
                        }
                        if (a.backgroundImageUrlStoreLocation != null && a.backgroundImageUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.backgroundImageUrlStoreLocation });
                        }
                    }
                    var lst2 = db.story_scene_pages.Select(a => a).ToList();

                    foreach (var a in lst2)
                    {
                        if (a.imageUrl != null && a.imageUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.imageUrl });
                        }
                        if (a.imageUrlStoreLocation != null && a.imageUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.imageUrlStoreLocation });
                        }
                        if (a.audioUrl != null && a.audioUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.audioUrl });
                        }
                        if (a.audioUrlStoreLocation != null && a.audioUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.audioUrlStoreLocation });
                        }
                        if (a.youtubeUrl != null && a.youtubeUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.youtubeUrl });
                        }
                        if (a.youtubeStoreLocation != null && a.youtubeStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.youtubeStoreLocation });
                        }
                    }


                    lr.status = "success";
                    lr.StoryCategoryDCMList = lst;
                    lr.StoryScenePagesDCMList = lst2;
                    lr.AssetsDCMList = asset;

                }
                else
                {
                    if (tmp != null)
                    {
                        if (tmp.homeBackground != null && tmp.homeBackground != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = tmp.homeBackground });
                        }
                        if (tmp.homeAudio != null && tmp.homeAudio != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = tmp.homeAudio });
                        }
                        if (tmp.title != null && tmp.title != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = tmp.title });
                        }
                    }
                    var lst = db.story_category.Select(a => a).ToList();

                    foreach (var a in lst)
                    {
                        if (a.storyCategoryTitle != null && a.storyCategoryTitle != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.storyCategoryTitle });
                        }
                        if (a.storyCategoryImageUrl != null && a.storyCategoryImageUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.storyCategoryImageUrl });
                        }
                        if (a.storyCategoryImageUrlStoreLocation != null && a.storyCategoryImageUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.storyCategoryImageUrlStoreLocation });
                        }
                        if (a.storyType != null && a.storyType != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.storyType });
                        }
                        if (a.categoryLogoUrl != null && a.categoryLogoUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.categoryLogoUrl });
                        }
                        if (a.categoryLogoUrlStoreLocation != null && a.categoryLogoUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.categoryLogoUrlStoreLocation });
                        }
                        if (a.backgroundImageUrl != null && a.backgroundImageUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.backgroundImageUrl });
                        }
                        if (a.backgroundImageUrlStoreLocation != null && a.backgroundImageUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.backgroundImageUrlStoreLocation });
                        }
                    }
                    var lst2 = db.story_scene_pages.Select(a => a).ToList();

                    foreach (var a in lst2)
                    {
                        if (a.imageUrl != null && a.imageUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.imageUrl });
                        }
                        if (a.imageUrlStoreLocation != null && a.imageUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.imageUrlStoreLocation });
                        }
                        if (a.audioUrl != null && a.audioUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.audioUrl });
                        }
                        if (a.audioUrlStoreLocation != null && a.audioUrlStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.audioUrlStoreLocation });
                        }
                        if (a.youtubeUrl != null && a.youtubeUrl != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.youtubeUrl });
                        }
                        if (a.youtubeStoreLocation != null && a.youtubeStoreLocation != "")
                        {
                            asset.Add(new AssetsModel() { id = ++id, url = a.youtubeStoreLocation });
                        }
                    }


                    lr.status = "success";
                    lr.StoryCategoryDCMList = lst;
                    lr.StoryScenePagesDCMList = lst2;
                    lr.AssetsDCMList = asset;

                }


                return new JsonResult() { Data = lr };
            }
            catch (Exception )
            {
                ResponseModel lr = new ResponseModel();
                lr.Error = "Unauthorized";
                lr.HttpResponse = HttpStatusCode.Unauthorized.ToString();
                return new JsonResult() { Data = lr };
            }
        }

    }
}
