using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.Login.Login
{
    [DataContract]
    public class GetSessionDetailsSubmitCCM : ErrorCM
    {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string branchCode { get; set; }
        [DataMember]
        public string groupCode { get; set; }
        [DataMember]
        public int requestCount { get; set; }
    }
}