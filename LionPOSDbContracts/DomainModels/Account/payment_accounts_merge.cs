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
    
    public partial class payment_accounts_merge
    {
        public string paymentAccountsBankName { get; set; }
        public string paymentAccountsAccountNumber { get; set; }
        public string paymentAccountsEntryGroupCode { get; set; }
        public string paymentAccountsBankName_copy { get; set; }
        public string paymentAccountsAccountNumber_copy { get; set; }
        public string paymentAccountsEntryGroupCode_copy { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public System.DateTime entryDate { get; set; }
        public System.DateTime lastChangeDate { get; set; }
        public bool recordByLion { get; set; }
        public string entryByUserName { get; set; }
        public string changeByUserName { get; set; }
        public string paymentAccountsMergeEntryBranchCode { get; set; }
        public string insertRoutePoint { get; set; }
        public string updateRoutePoint { get; set; }
        public string syncGUID { get; set; }
    
        public virtual payment_accounts payment_accounts { get; set; }
        public virtual payment_accounts payment_accounts1 { get; set; }
    }
}