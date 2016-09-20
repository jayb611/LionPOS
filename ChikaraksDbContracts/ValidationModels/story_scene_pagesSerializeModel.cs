using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksDbContracts.DomainModels
{

    [MetadataType(typeof(story_scene_pagesSerializeModel))]
    public partial class story_scene_pages
    {
    }
    [DataContract]
    class story_scene_pagesSerializeModel
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
        public Nullable<int> indexOrder { get; set; }

    }
}
