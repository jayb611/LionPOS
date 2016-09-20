using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class GetFormDetailsResultCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
        [DataMember]
        public StoryCategoryDCM StoryCategoryDCM { get; set; }
        public GetFormDetailsResultCCM()
        {
            StoryCategoryDCM = new StoryCategoryDCM();
        }
    }
}
