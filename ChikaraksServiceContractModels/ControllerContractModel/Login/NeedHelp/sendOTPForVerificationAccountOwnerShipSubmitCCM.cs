using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.Login.NeedHelp
{
    [DataContract]
    public class sendOTPForVerificationAccountOwnerShipSubmitCCM : ErrorCM
    {
        [DataMember]
        public string RecoveryOptionSelected { get; set; }
        [DataMember]
        public string employeeCode { get; set; }
        [DataMember]
        public string branchCode { get; set; }
        [DataMember]
        public string employeeEntryBranchCode { get; set; }
        [DataMember]
        public string PasswordResetTemplatePath { get; set; }
        [DataMember]
        public string recoveryMode { get; set; }
    }
}