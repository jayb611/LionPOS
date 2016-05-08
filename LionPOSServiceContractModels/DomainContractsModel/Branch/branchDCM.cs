using LionPOSServiceContractModels.DomainContractsModel.Configuration;
using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.Branch
{
    [DataContract]
    public class branchDCM : ErrorCM 
    {
        [DataMember]
        public SessionCM SessionCM { get; set; }
        [DataMember]
        public string branchCode { get; set; }
        [DataMember]
        public string branchName { get; set; }
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
        public string branchType { get; set; }
        [DataMember]
        public System.DateTime openDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> closeDate { get; set; }
        [DataMember]
        public string experiance { get; set; }
        [DataMember]
        public Nullable<decimal> investment { get; set; }
        
        [DataMember]
        public Nullable<decimal> deposit { get; set; }
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
        public string city { get; set; }
        [DataMember]
        public string staticIpAddress { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public bool isActive { get; set; }
        [DataMember]
        public bool isDeleted { get; set; }
        [DataMember]
        public string branchCodeEntryBranchCode { get; set; }
        [DataMember]
        public string changeByUserName { get; set; }
        [DataMember]
        public string entryByUserName { get; set; }
        [DataMember]
        public DateTime entryDate { get; set; }
        [DataMember]
        public System.DateTime lastChangeDate { get; set; }
        [DataMember]
        public bool recordByLion { get; set; }


       
    }
}
