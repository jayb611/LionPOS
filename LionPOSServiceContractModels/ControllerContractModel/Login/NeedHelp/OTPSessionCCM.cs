using LionPOSServiceContractModels.ErrorContactModel;
using System;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.Login.NeedHelp
{
    [DataContract]
    public class OTPSessionCCM : ErrorCM
    {
        [DataMember]
        public bool isActiveOneTimePassword { get; set; }
        [DataMember]
        public string oneTimePassword { get; set; }
        [DataMember]
        public DateTime? oneTimePasswordTimeOut { get; set; }
    }
}