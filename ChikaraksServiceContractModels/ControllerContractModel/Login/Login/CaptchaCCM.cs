using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.Login.Login
{
    [DataContract]
    public class CaptchaCCM : ErrorCM
    {
        [DataMember]
        public KeyValuePair<string, string> kvp { get; set; }
    }
}