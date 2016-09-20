using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class CreateStoryCategoryRequestCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
        [DataMember]
        public StoryCategoryDCM StoryCategoryDCM { get; set; }
        public CreateStoryCategoryRequestCCM()
        {
            StoryCategoryDCM = new StoryCategoryDCM();
        }
    }
}
