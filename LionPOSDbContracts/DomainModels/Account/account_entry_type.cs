//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LionPOSDbContracts.DomainModels.Account
{
    using System;
    using System.Collections.Generic;
    
    public partial class account_entry_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public account_entry_type()
        {
            this.account_entry_note = new HashSet<account_entry_note>();
        }
    
        public string accountEntryTypeEntryBranchCode { get; set; }
        public string entryGroupCode { get; set; }
        public string accountEntryTypeTitle { get; set; }
        public bool isProductTax { get; set; }
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
        public virtual ICollection<account_entry_note> account_entry_note { get; set; }
    }
}
