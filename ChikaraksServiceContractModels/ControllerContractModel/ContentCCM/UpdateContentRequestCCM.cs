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
    public class UpdateContentRequestCCM : ErrorCM
    {
        [DataMember]
        public ContentDCM ContetntDCM { get; set; }

        public UpdateContentRequestCCM()
        {
            ContetntDCM = new ContentDCM();
        }
    }
}
