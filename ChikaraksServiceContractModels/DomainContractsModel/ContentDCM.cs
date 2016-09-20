using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.DomainContractsModel
{
    [DataContract]
    public class ContentDCM : ErrorCM
    {
        [DataMember]
        public int idContent { get; set; }
        [DataMember]
        public string contentTag { get; set; }
        [DataMember]
        public string contentValue { get; set; }
    }
}
