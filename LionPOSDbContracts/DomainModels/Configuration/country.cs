//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LionPOSDbContracts.DomainModels.Configuration
{
    using System;
    using System.Collections.Generic;
    
    public partial class country
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public country()
        {
            this.states = new HashSet<state>();
        }
    
        public string countryName { get; set; }
        public string a2 { get; set; }
        public string a3 { get; set; }
        public Nullable<int> phoneCode { get; set; }
        public string capitols { get; set; }
        public string latitude { get; set; }
        public string logitude { get; set; }
        public string isoUnm49Code { get; set; }
        public string currencyCode { get; set; }
        public string currencyName { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public System.DateTime entryDate { get; set; }
        public System.DateTime lastChangeDate { get; set; }
        public bool recordByLion { get; set; }
        public string entryByUserName { get; set; }
        public string changeByUserName { get; set; }
        public string insertRoutePoint { get; set; }
        public string updateRoutePoint { get; set; }
        public string syncGUID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<state> states { get; set; }
    }
}