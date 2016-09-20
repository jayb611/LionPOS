using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksDbContracts.DomainModels
{

    [MetadataType(typeof(story_categorySerializeModel))]
    public partial class story_category
    {
    }
    [DataContract]

    class story_categorySerializeModel
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
        public virtual ICollection<story_scene_pages> story_scene_pages { get; set; }

    }
}
