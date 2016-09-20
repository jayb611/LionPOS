using ChikaraksServiceContractModels.ErrorContactModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.AssetsCCM
{
    [DataContract]
    public class SelectedAssetsActionRequestCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
        [DataMember]
        public List<int> idAssets { get; set; }
        [DataMember]
        public List<string> title { get; set; }

        public SelectedAssetsActionRequestCCM()
        {
            idAssets = new List<int>();
            title = new List<string>();
        }
    }
}
