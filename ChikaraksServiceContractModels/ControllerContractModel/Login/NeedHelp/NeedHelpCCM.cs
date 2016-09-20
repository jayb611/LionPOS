using ChikaraksServiceContractModels;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.Login.NeedHelp
{
    [DataContract]
    public class NeedHelpCCM : ErrorCM
    {
        [DataMember]
        public userSCM user { get; set; }
        [DataMember]
        public List<userSCM> userList { get; set; }
        
        [DataMember]
        public string RecoveryOptionSelected { get; set; }


    }
}