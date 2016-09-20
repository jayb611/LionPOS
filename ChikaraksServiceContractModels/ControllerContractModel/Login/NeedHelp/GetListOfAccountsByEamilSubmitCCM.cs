using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.Login.NeedHelp
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