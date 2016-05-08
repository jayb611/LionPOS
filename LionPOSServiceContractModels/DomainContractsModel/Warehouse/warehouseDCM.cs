using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.warehouse
{
    [DataContract]
    public class warehouseDCM : ErrorCM
    {
        [DataMember]
        public SessionCM SessionCM { get; set; }
        [DataMember]
        public string warehousesCode { get; set; }
        [DataMember]
        public string warehousesEntryBranchCode { get; set; }
        [DataMember]
        public string warehousesEntryGroupCode { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string CoveringAreas { get; set; }
        [DataMember]
        public string logitudeLocation { get; set; }
        [DataMember]
        public string latitudeLoaction { get; set; }
        [DataMember]
        public string warehouseType { get; set; }
        [DataMember]
        public string remarks { get; set; }
        [DataMember]
        public string contactType2 { get; set; }
        [DataMember]
        public string contactNo2 { get; set; }
        [DataMember]
        public string contactType1 { get; set; }
        [DataMember]
        public string contactNo1 { get; set; }
        [DataMember]
        public string email { get; set; }
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
