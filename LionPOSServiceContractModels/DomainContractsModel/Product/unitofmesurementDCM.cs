using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.Product
{
    [DataContract]
    public class unitofmesurementDCM : ErrorCM
    {
        [DataMember]
        public SessionCM SessionCM { get; set; }
        [DataMember]
        public string unitOfMesurementEntryBranchCode { get; set; }
        [DataMember]
        public string unit_of_mesurement_title { get; set; }
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
        public int identryByUser { get; set; }
        [DataMember]
        public string idchangeByUser { get; set; }
        [DataMember]
        public string idbrancheEntry { get; set; }
    }
}
