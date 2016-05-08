﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class branchlvitsposdbEntities : DbContext
    {
        public branchlvitsposdbEntities()
            : base("name=branchlvitsposdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<branch> branches { get; set; }
        public virtual DbSet<branches_payments> branches_payments { get; set; }
        public virtual DbSet<employee_underbranch> employee_underbranch { get; set; }
    
        public virtual int getBranchesWithDynamicClauses(Nullable<bool> showBranchWise, string branchCode, string dynamicWhereClauses, string dynamicOrderByFields, Nullable<int> skip, Nullable<int> take)
        {
            var showBranchWiseParameter = showBranchWise.HasValue ?
                new ObjectParameter("showBranchWise", showBranchWise) :
                new ObjectParameter("showBranchWise", typeof(bool));
    
            var branchCodeParameter = branchCode != null ?
                new ObjectParameter("branchCode", branchCode) :
                new ObjectParameter("branchCode", typeof(string));
    
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
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("getBranchesWithDynamicClauses", showBranchWiseParameter, branchCodeParameter, dynamicWhereClausesParameter, dynamicOrderByFieldsParameter, skipParameter, takeParameter);
        }
    }
}