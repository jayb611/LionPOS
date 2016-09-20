using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class CreateStoryCategoryResultCCM : ErrorCM
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public StoryCategoryDCM StoryCategoryDCM { get; set; }
        public CreateStoryCategoryResultCCM()
        {
            StoryCategoryDCM = new StoryCategoryDCM();
        }
    }
}
