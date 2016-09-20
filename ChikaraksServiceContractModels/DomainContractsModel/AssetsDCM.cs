using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.DomainContractsModel
{
    [DataContract]
    public class AssetsDCM : ErrorCM
    {
        [DataMember]
        public int idAssets { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string homeBackground { get; set; }
        [DataMember]
        public string homeAudio { get; set; }
        [DataMember]
        public string url { get; set; }
    }
}
