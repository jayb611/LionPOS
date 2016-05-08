﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class warehouselvitsposdbEntities : DbContext
    {
        public warehouselvitsposdbEntities()
            : base("name=warehouselvitsposdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<delivery_challan_receive> delivery_challan_receive { get; set; }
        public virtual DbSet<delivery_challan_send> delivery_challan_send { get; set; }
        public virtual DbSet<warehouse_stock_items> warehouse_stock_items { get; set; }
        public virtual DbSet<warehouse_stock_master> warehouse_stock_master { get; set; }
        public virtual DbSet<warehouse_structure> warehouse_structure { get; set; }
        public virtual DbSet<warehouse> warehouses { get; set; }
        public virtual DbSet<warehouses_merge> warehouses_merge { get; set; }
        public virtual DbSet<wearhouse_under_branches> wearhouse_under_branches { get; set; }
    
        public virtual int getWarehousesWithDynamicClauses(string dynamicWhereClauses, string dynamicOrderByFields, Nullable<int> skip, Nullable<int> take)
        {
            var dynamicWhereClausesParameter = dynamicWhereClauses != null ?
                new ObjectParameter("dynamicWhereClauses", dynamicWhereClauses) :
                new ObjectParameter("dynamicWhereClauses", typeof(string));
    
            var dynamicOrderByFieldsParameter = dynamicOrderByFields != null ?
                new ObjectParameter("dynamicOrderByFields", dynamicOrderByFields) :
                new ObjectParameter("dynamicOrderByFields", typeof(string));
    
            var skipParameter = skip.HasValue ?
                new ObjectParameter("skip", skip) :
                new ObjectParameter("skip", typeof(int));
    
            var takeParameter = take.HasValue ?
                new ObjectParameter("take", take) :
                new ObjectParameter("take", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("getWarehousesWithDynamicClauses", dynamicWhereClausesParameter, dynamicOrderByFieldsParameter, skipParameter, takeParameter);
        }
    }
}
