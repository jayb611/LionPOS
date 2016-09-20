using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class StoryCategoryBulkActionRequestCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
        [DataMember]
        public List<int> idStoryCategory { get; set; }

        public StoryCategoryBulkActionRequestCCM()
        {
            idStoryCategory = new List<int>();
        }
    }
}
