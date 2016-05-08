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
    
    public partial class delivery_challan_send
    {
        public string deliveryChallanrEntryBranchCode { get; set; }
        public string deliveryChallanEntryGroupCode { get; set; }
        public int deliveryChallanNo { get; set; }
        public string deliveryChallanPrefix { get; set; }
        public System.DateTime deliveryChallanDate { get; set; }
        public string warehouseStockMasterTransactionCode { get; set; }
        public string warehouseStockMasterEntryBranchCode { get; set; }
        public string warehouseStockMasterEntryGroupCode { get; set; }
        public string warehouseStockMasterEntryType { get; set; }
        public string freightPaidBy { get; set; }
        public Nullable<decimal> freightPaidAmoutn { get; set; }
        public string remarks { get; set; }
        public string transportOrCuriorName { get; set; }
        public string transportVehicleNo { get; set; }
        public string liftingNo { get; set; }
        public Nullable<System.DateTime> liftingDate { get; set; }
        public Nullable<System.DateTime> deliveryDate { get; set; }
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
    
        public virtual warehouse_stock_master warehouse_stock_master { get; set; }
    }
}
