using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksServiceContractModels.ControllerContractModel.ContentCCM
{
    [DataContract]
    public class CreateContentResultCCM : ErrorCM
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public ContentDCM ContentDCM { get; set; }
        public CreateContentResultCCM()
        {
            ContentDCM = new ContentDCM();
        }
    }
}
