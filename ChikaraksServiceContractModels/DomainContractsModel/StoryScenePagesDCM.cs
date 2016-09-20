using ChikaraksServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.DomainContractsModel
{
    [DataContract]
    public class StoryScenePagesDCM : ErrorCM
    {
        [DataMember]
        public int idStoryScenePages { get; set; }
        [DataMember]
        public int idStoryCategory { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string imageUrl { get; set; }
        [DataMember]
        public string imageUrlStoreLocation { get; set; }
        [DataMember]
        public string audioUrl { get; set; }
        [DataMember]
        public string audioUrlStoreLocation { get; set; }
        [DataMember]
        public string youtubeUrl { get; set; }
        [DataMember]
        public string youtubeStoreLocation { get; set; }
        [DataMember]
        public int? indexOrder { get; set; }
    }
}
