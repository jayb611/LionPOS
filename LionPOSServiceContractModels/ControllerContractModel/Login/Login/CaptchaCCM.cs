using LionPOSServiceContractModels;
using LionPOSServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.Login.Login
{
    [DataContract]
    public class CaptchaCCM : ErrorCM
    {
        [DataMember]
        public KeyValuePair<string, string> kvp { get; set; }
    }
}