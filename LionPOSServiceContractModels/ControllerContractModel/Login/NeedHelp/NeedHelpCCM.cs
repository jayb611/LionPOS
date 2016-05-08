using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.Login.NeedHelp
{
    [DataContract]
    public class NeedHelpCCM : ErrorCM
    {
        [DataMember]
        public userSCM user { get; set; }
        [DataMember]
        public List<userSCM> userList { get; set; }
        [DataMember]
        public employeeSCM employee { get; set; }
        [DataMember]
        public string RecoveryOptionSelected { get; set; }


    }
}