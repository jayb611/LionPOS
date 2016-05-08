using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.DomainContractsModel.Configuration
{
    [DataContract]
    public class StateDCM : ErrorCM 
    {
        [DataMember]
        public string countryName { get; set; }
        [DataMember]
        public string countryA2 { get; set; }
        [DataMember]
        public string stateName { get; set; }
        [DataMember]
        public string a2 { get; set; }


    }
}
