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
    
    public partial class account_entry_note
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public account_entry_note()
        {
            this.recipts = new HashSet<recipt>();
        }
    
        public string accountEntryNoteTransactionCode { get; set; }
        public string accountEntryNoteEntryBranchCode { get; set; }
        public string accountEntryNoteEntryGroupCode { get; set; }
        public string reciptReceivedReferenceNumber { get; set; }
        public string accountEntryTypeTitle { get; set; }
        public string paymentAccountsBankName { get; set; }
        public string paymentAccountsAccountNumber { get; set; }
        public string paymentAccountsEntryGroupCode { get; set; }
        public string reciptStatus { get; set; }
        public decimal amount { get; set; }
        public Nullable<decimal> lessAmount { get; set; }
        public string contactPayerNumber { get; set; }
        public string contactPayerAddress { get; set; }
        public string contactPayerName { get; set; }
        public string contactReceiverNumber { get; set; }
        public string contactReceiverAddress { get; set; }
        public string contactReceiverName { get; set; }
        public string transactionType { get; set; }
        public string paymentType { get; set; }
        public Nullable<System.DateTime> nextPaymentDate { get; set; }
        public string chargesOf { get; set; }
        public string chequeNumber { get; set; }
        public string chequeDate { get; set; }
        public string cardNumber { get; set; }
        public Nullable<int> expiryMonth { get; set; }
        public Nullable<int> expiryYear { get; set; }
        public Nullable<System.DateTime> dueNotificationDate { get; set; }
        public string transferBankname { get; set; }
        public string transferAccountName { get; set; }
        public string transferAccountNumber { get; set; }
        public string transferIfscCode { get; set; }
        public string remarks { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public System.DateTime entryDate { get; set; }
        public bool recordByLion { get; set; }
        public System.DateTime lastChangeDate { get; set; }
        public string entryByUserName { get; set; }
        public string changeByUserName { get; set; }
        public string insertRoutePoint { get; set; }
        public string updateRoutePoint { get; set; }
        public string syncGUID { get; set; }
    
        public virtual account_entry_type account_entry_type { get; set; }
        public virtual payment_accounts payment_accounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<recipt> recipts { get; set; }
    }
}
