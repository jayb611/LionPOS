//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LionPOSDbContracts.DomainModels.Employee
{
    using System;
    using System.Collections.Generic;
    
    public partial class employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public employee()
        {
            this.employee_merge = new HashSet<employee_merge>();
            this.employee_merge1 = new HashSet<employee_merge>();
        }
    
        public string employeeEntryBranchCode { get; set; }
        public string employeeCode { get; set; }
        public string employeeEntryGroupCode { get; set; }
        public string contactNo1 { get; set; }
        public string contactType1 { get; set; }
        public string contactNo2 { get; set; }
        public string contactType2 { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string sureName { get; set; }
        public string profilePicture { get; set; }
        public string idproof { get; set; }
        public string oneTimePassword { get; set; }
        public bool isActiveOneTimePassword { get; set; }
        public System.DateTime oneTimePasswordTimeOut { get; set; }
        public string designation { get; set; }
        public bool gender { get; set; }
        public string emialAddress { get; set; }
        public bool married { get; set; }
        public int employmentStatus { get; set; }
        public System.DateTime joiningdate { get; set; }
        public Nullable<decimal> currentSalary { get; set; }
        public string castCategory { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public Nullable<System.DateTime> dateOfAniversary { get; set; }
        public string address { get; set; }
        public string licenseNo { get; set; }
        public string pancardNo { get; set; }
        public string employeeWorkShift { get; set; }
        public string bankName { get; set; }
        public string bankAcNo { get; set; }
        public string ifsccode { get; set; }
        public string keyResponsibleArea { get; set; }
        public Nullable<decimal> salary { get; set; }
        public string remarks { get; set; }
        public Nullable<System.DateTime> leavingdate { get; set; }
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
        public virtual ICollection<employee_merge> employee_merge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employee_merge> employee_merge1 { get; set; }
    }
}