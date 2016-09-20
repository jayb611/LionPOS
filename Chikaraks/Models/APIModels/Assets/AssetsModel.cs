using Chikaraks.Models.WebAPIModels;
using LionPOSServiceContractModels.DomainContractsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChikaraksDbContracts.DomainModels.Chikaraks;


namespace Chikaraks.Models.APIModels.Assets
{
    public class AssetsModel
    {
        public int id { get; set; }
        public string url { get; set; }
    }
    public class RequestModel
    {
        public string deviceType { get; set; }
    }
    public class ResponseModel : BaseResponse
    {
        public string status { get; set; }
        public List<AssetsModel> AssetsDCMList { get; set; }
        public List<content> ContentDCMList { get; set; }
        public List<story_category> StoryCategoryDCMList { get; set; }
        public List<story_scene_pages> StoryScenePagesDCMList { get; set; }
    }
}