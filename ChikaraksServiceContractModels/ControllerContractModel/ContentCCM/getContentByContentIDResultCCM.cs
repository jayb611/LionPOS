using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.ContentCCM
{
    [DataContract]
    public class GetContentByContentIDResultCCM : ErrorCM
    {
        [DataMember]
        public ContentDCM ContentDCM { get; set; }

        public GetContentByContentIDResultCCM()
        {
            ContentDCM = new ContentDCM();
        }
    }
}
