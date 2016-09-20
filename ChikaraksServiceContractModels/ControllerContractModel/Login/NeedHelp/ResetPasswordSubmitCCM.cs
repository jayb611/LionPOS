using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.Login.NeedHelp
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