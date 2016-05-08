using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.Supplier
{
    [DataContract]

    public class supplierDCM : ErrorCM
    {

        [DataMember]
        public SessionCM SessionCM { get; set; }

        [DataMember]
        public string suppliersEntryBranchCode { get; set; }
        [DataMember]
        public string suppliersCode { get; set; }
        [DataMember]
        public string suppliersEntryGroupCode { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string shortTitle { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public decimal creditLimitDays { get; set; }
        [DataMember]
        public string creditLimitAmount { get; set; }
        [DataMember]
        public Nullable<System.DateTime> permanentAccountNumberDate { get; set; }
        [DataMember]
        public string contactPersonName { get; set; }
        [DataMember]
        public string contactPersonDesignation { get; set; }
        [DataMember]
        public string pincode { get; set; }
        [DataMember]
        public string mobileNo { get; set; }
        [DataMember]
        public string telephoneNo { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string registreredAddress { get; set; }
        [DataMember]
        public string correspondenceAddress { get; set; }
        [DataMember]
        public string billingAddress { get; set; }
        [DataMember]
        public string serviceTaxNo { get; set; }
        [DataMember]
        public string valueAddedTaxNo { get; set; }
        [DataMember]
        public DateTime? valueAddedTaxDate { get; set; }
        [DataMember]
        public string permanentAccountNumber { get; set; }
        [DataMember]
        public string centralSalesTaxNo { get; set; }
        [DataMember]
        public Nullable<System.DateTime> centralSalesTaxDate { get; set; }
        [DataMember]
        public string taxpayerIdentificationNumber { get; set; }
        [DataMember]
        public DateTime? taxpayerIdentificationNumberDate { get; set; }
        [DataMember]
        public string corporateIdentityNumber { get; set; }
        [DataMember]
        public DateTime? corporateIdentityNumberDate { get; set; }
        [DataMember]
        public string bstNo { get; set; }
        [DataMember]
        public DateTime? bstDate { get; set; }
        [DataMember]
        public string registrationNo { get; set; }
        [DataMember]
        public DateTime? registrationDate { get; set; }
        [DataMember]
        public string legalStatus { get; set; }
        [DataMember]
        public string bankName { get; set; }
        [DataMember]
        public string bankAcNo { get; set; }
        [DataMember]
        public string ifsccode { get; set; }
        [DataMember]
        public int? overDueLockingDays { get; set; }
        [DataMember]
        public bool isActive { get; set; }
        [DataMember]
        public bool isDeleted { get; set; }
        [DataMember]
        public System.DateTime entryDate { get; set; }
        [DataMember]
        public System.DateTime lastChangeDate { get; set; }
        [DataMember]
        public bool recordByLion { get; set; }
        [DataMember]
        public string entryByUserName { get; set; }
        [DataMember]
        public string changeByUserName { get; set; }

    }
}
