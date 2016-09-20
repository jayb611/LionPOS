using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.DomainContractsModel
{
    [DataContract]
    public class StoryCategoryDCM : ErrorCM
    {
        [DataMember]
        public int idStoryCategory { get; set; }
        [DataMember]
        public string storyCategoryTitle { get; set; }
        [DataMember]
        public string storyCategoryImageUrl { get; set; }
        [DataMember]
        public string storyCategoryImageUrlStoreLocation { get; set; }
        [DataMember]
        public string storyType { get; set; }
        [DataMember]
        public string categoryLogoUrl { get; set; }
        [DataMember]
        public string categoryLogoUrlStoreLocation { get; set; }
        [DataMember]
        public string backgroundImageUrl { get; set; }
        [DataMember]
        public string backgroundImageUrlStoreLocation { get; set; }


        [DataMember]
        public List<StoryScenePagesDCM> story_scene_pages { get; set; }
    }
}
