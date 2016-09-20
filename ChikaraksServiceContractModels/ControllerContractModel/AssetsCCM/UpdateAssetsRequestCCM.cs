using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.AssetsCCM
{
    [DataContract]
    public class UpdateAssetsRequestCCM : ErrorCM
    {
        [DataMember]
        public string SSID { get; set; }
        [DataMember]
        public AssetsDCM Assets { get; set; }

    }
}
