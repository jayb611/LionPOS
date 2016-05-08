using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.Product
{
    [DataContract]
    public class productDCM : ErrorCM
    {
        [DataMember]
        public SessionCM SessionCM { get; set; }

        [DataMember]
        public string sku { get; set; }

        [DataMember]
        public string productsEntryGroupCode { get; set; }

        [DataMember]
        public string productsEntryBranchCode { get; set; }

        [DataMember]
        public string taxMasterTitle { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string barcodeNumber { get; set; }

        [DataMember]
        public string skuParent { get; set; }

        [DataMember]
        public string productsEntryBranchCodeParent { get; set; }

        [DataMember]
        public string productsEntryGroupCodeParent { get; set; }

        [DataMember]
        public Nullable<bool> includeInMenu { get; set; }

        [DataMember]
        public Nullable<bool> canBeSold { get; set; }

        [DataMember]
        public Nullable<bool> isCombo { get; set; }

        [DataMember]
        public Nullable<bool> canPurchase { get; set; }

        [DataMember]
        public Nullable<System.TimeSpan> makeDuration { get; set; }

        [DataMember]
        public string unit_of_mesurement_title { get; set; }

        [DataMember]
        public Nullable<bool> isCategory { get; set; }

        [DataMember]
        public string productDescription { get; set; }

        [DataMember]
        public Nullable<decimal> stockWarningQty { get; set; }

        [DataMember]
        public decimal discount { get; set; }

        [DataMember]
        public string discountInPercentage { get; set; }

        [DataMember]
        public Nullable<bool> availableOnEcommerce { get; set; }

        [DataMember]
        public string categoryOfPOS { get; set; }

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
