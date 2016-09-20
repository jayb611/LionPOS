using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.StoryCategoryCCM
{
    [DataContract]
    public class GetStoryCategoryByPrimaryKeysRequestCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
        [DataMember]
        public int idStoryCategory { get; set; }
    }
}
