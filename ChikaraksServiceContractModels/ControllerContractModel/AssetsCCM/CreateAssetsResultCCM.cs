using ChikaraksServiceContractModels.DomainContractsModel;
using ChikaraksServiceContractModels.ErrorContactModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChikaraksServiceContractModels.ControllerContractModel.AssetsCCM
{
    [DataContract]
    public class CreateAssetsResultCCM : ErrorCM
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public AssetsDCM AssetsDCM { get; set; }
        public CreateAssetsResultCCM()
        {
            AssetsDCM = new AssetsDCM();
        }
    }
}
