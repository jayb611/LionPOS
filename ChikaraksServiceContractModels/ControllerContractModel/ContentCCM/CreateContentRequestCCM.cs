using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System.Runtime.Serialization;

namespace ChikaraksServiceContractModels.ControllerContractModel.ContentCCM
{
    [DataContract]
    public class CreateContentRequestCCM : ErrorCM
    {
        [DataMember]
        public ContentDCM ContentDCM { get; set; }

        public CreateContentRequestCCM()
        {
            ContentDCM = new ContentDCM();
        }
    }
}
