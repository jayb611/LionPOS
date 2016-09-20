using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class UpdateStoryCategoryResultCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
        [DataMember]
        public StoryCategoryDCM StoryCategoryDCM { get; set; }
        public UpdateStoryCategoryResultCCM()
        {
            StoryCategoryDCM = new StoryCategoryDCM();
        }
    }
}
