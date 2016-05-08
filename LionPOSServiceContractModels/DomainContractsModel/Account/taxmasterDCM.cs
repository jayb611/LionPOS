using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.Account
{
    [DataContract]
    public class taxmasterDCM : ErrorCM
    {

        [DataMember]
        public SessionCM SessionCM { get; set; }

        [DataMember]
        public string taxMasterEntryBranchCode { get; set; }

        [DataMember]
        public string taxMasterTitle { get; set; }

        [DataMember]
        public string entryGroupCode { get; set; }

        [DataMember]
        public System.DateTime expiryDate { get; set; }

        [DataMember]
        public bool isProductTax { get; set; }

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
