//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LionPOSDbContracts.DomainModels.Branch
{
    using System;
    using System.Collections.Generic;
    
    public partial class employee_underbranch
    {
        public string employeeUnderBranchEntryBranchCode { get; set; }
        public string branchCode { get; set; }
        public string employeeEntryBranchCode { get; set; }
        public string employeeCode { get; set; }
        public string employeeEntryGroupCode { get; set; }
        public System.DateTime effectiveDate { get; set; }
        public System.DateTime expireDate { get; set; }
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
    
        public virtual branch branch { get; set; }
    }
}
