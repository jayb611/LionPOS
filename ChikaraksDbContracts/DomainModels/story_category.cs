//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChikaraksDbContracts.DomainModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class story_category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public story_category()
        {
            this.story_scene_pages = new HashSet<story_scene_pages>();
        }
    
        public int idStoryCategory { get; set; }
        public string storyCategoryTitle { get; set; }
        public string storyCategoryImageUrl { get; set; }
        public string storyCategoryImageUrlStoreLocation { get; set; }
        public string storyType { get; set; }
        public string categoryLogoUrl { get; set; }
        public string categoryLogoUrlStoreLocation { get; set; }
        public string backgroundImageUrl { get; set; }
        public string backgroundImageUrlStoreLocation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<story_scene_pages> story_scene_pages { get; set; }
    }
}
