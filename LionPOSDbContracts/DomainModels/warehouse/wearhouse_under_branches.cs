//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LionPOSDbContracts.DomainModels.warehouse
{
    using System;
    using System.Collections.Generic;
    
    public partial class wearhouse_under_branches
    {
        public string warehousesCode { get; set; }
        public string warehousesEntryBranchCode { get; set; }
        public string warehousesEntryGroupCode { get; set; }
        public string branchCode { get; set; }
        public string wearhouseUnderBranchesEntryBranchCode { get; set; }
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
    
        public virtual warehouse warehouse { get; set; }
    }
}
