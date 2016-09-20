using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class GetFormDetailsRequestCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
    }
}
