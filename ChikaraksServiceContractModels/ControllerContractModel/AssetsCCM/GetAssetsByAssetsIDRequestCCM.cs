using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.AssetsCCM
{
    [DataContract]
    public class GetAssetsByAssetsIDRequestCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
        [DataMember]
        public int idAssets { get; set; }
        [DataMember]
        public string title { get; set; }
    }
}
