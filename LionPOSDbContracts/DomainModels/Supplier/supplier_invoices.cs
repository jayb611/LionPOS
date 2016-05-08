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
    
    public partial class supplier_invoices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supplier_invoices()
        {
            this.supplier_invoices_items = new HashSet<supplier_invoices_items>();
            this.supplier_payments = new HashSet<supplier_payments>();
            this.supplier_purchase_return = new HashSet<supplier_purchase_return>();
        }
    
        public string supplierInvoicesEntryBranchCode { get; set; }
        public string supplierInvoiceNumber { get; set; }
        public string suppliersEntryBranchCode { get; set; }
        public string suppliersCode { get; set; }
        public string suppliersEntryGroupCode { get; set; }
        public System.DateTime supplierInvoicesDate { get; set; }
        public string supplierPurchaseOrdersEntryBranchCode { get; set; }
        public Nullable<int> supplierPoNo { get; set; }
        public string supplierPoNoPrefix { get; set; }
        public string supplierPoEntryGroupCode { get; set; }
        public string taxMasterTitle { get; set; }
        public string remarks { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<decimal> discountInPercentage { get; set; }
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
        public virtual ICollection<supplier_invoices_items> supplier_invoices_items { get; set; }
        public virtual supplier_purchase_orders supplier_purchase_orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplier_payments> supplier_payments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplier_purchase_return> supplier_purchase_return { get; set; }
    }
}