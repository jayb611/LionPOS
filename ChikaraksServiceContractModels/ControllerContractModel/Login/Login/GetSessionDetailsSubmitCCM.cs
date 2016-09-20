using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.Login.Login
{
    [DataContract]
    public class GetSessionDetailsSubmitCCM : ErrorCM
    {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }
        
        [DataMember]
        public int requestCount { get; set; }
    }
}