using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.Login.NeedHelp
{
    [DataContract]
    public class ResetPasswordSubmitCCM : ErrorCM
    {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string branchCode { get; set; }
        [DataMember]
        public string password { get; set; }
    }
}