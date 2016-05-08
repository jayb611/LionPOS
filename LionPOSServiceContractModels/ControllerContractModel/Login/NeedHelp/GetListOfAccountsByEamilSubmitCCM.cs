using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.Login.NeedHelp
{
    [DataContract]
    public class GetListOfAccountsByEamilSubmitCCM : ErrorCM
    {
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string branchCode { get; set; }
    }
}