//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LionPOSDbContracts.DomainModels.Supplier
{
    using System;
    using System.Collections.Generic;
    
    public partial class suppliers_merge
    {
        public string suppliersMergeEntryBranchCode { get; set; }
        public string suppliersEntryBranchCode { get; set; }
        public string suppliersCode { get; set; }
        public string suppliersEntryGroupCode { get; set; }
        public string suppliersEntryBranchCode_copy { get; set; }
        public string suppliersCode_copy { get; set; }
        public string suppliersEntryGroupCode_copy { get; set; }
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
    
        public virtual supplier supplier { get; set; }
        public virtual supplier supplier1 { get; set; }
    }
}