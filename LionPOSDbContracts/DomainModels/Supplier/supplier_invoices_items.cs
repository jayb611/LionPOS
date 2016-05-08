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
    
    public partial class supplier_invoices_items
    {
        public string supplierInvoicesItemsEntryBranchCode { get; set; }
        public string supplierInvoicesEntryBranchCode { get; set; }
        public string supplierInvoiceNumber { get; set; }
        public string suppliersEntryBranchCode { get; set; }
        public string suppliersCode { get; set; }
        public string suppliersEntryGroupCode { get; set; }
        public string sku { get; set; }
        public string productsEntryGroupCode { get; set; }
        public string taxMasterTitle { get; set; }
        public decimal qty { get; set; }
        public string Remarks { get; set; }
        public decimal unitCost { get; set; }
        public decimal discount { get; set; }
        public decimal discountInPercentage { get; set; }
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
    
        public virtual supplier_invoices supplier_invoices { get; set; }
    }
}
