using LionPOSServiceContractModels;
using LionPOSServiceContractModels.DomainContractsModel.Branch;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.Login.Login
{
    [DataContract]
    public class BranchLoginListCCM : ErrorCM
    {
        [DataMember]
        public List<branchDCM> branchDCMList { get; set; }
    }
}